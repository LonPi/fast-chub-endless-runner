using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    public float jumpHeight, maxHitpoints, damage;
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<Player>();
            return instance;
        }
    }

    public bool Jump { get; set; }
    public bool Attack { get; set; }
    public bool OnGround { get { return _playerController.collisionInfo.below; } }
    public Vector2 velocity { get { return _velocity; } }
    public float relativeSpeedToGround { get; private set; }
    public Dictionary<int, Collider2D> damagedEnemyList = new Dictionary<int, Collider2D>();
    public Dictionary<int, Collider2D> incomingProjectileList = new Dictionary<int, Collider2D>();
    public Dictionary<int, int> deflectedProjectileList = new Dictionary<int, int>();
    public Dictionary<int, int> tookDamageFromProjectileList = new Dictionary<int, int>();
    // private variables
    Animator _animator;
    Vector2 _velocity;
    PlayerController _playerController;
    BoxCollider2D hitbox;
    SpriteRenderer _spriteRenderer;
    Parallax _parallaxScript;
    float gravity = -20f;
    float _currentHitpoint;
    bool secondJumpAvailable, canJump;
    bool _isDead;
    int _layerMask;
    float _distance = 0f;
    float _score = 0f;
    float _killCount = 0f;
    float _obstacleDodgeCount = 0f;
    float _enemyDodgeCount = 0f;
    float _projectileDodgeCount = 0f;
    float _projectileBlockCount = 0f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _layerMask = 1 << LayerMask.NameToLayer("Enemy");
        _currentHitpoint = maxHitpoints;
    }

    void Update()
    {
        relativeSpeedToGround = GameManager.instance._parallaxRef.scrollingSpeed;
        var collisionInfo = _playerController.collisionInfo;
        if (collisionInfo.below || collisionInfo.above)
        {
            _velocity.y = 0f;
        }
        HandleInput();
        _velocity.y += gravity * Time.deltaTime;
        Vector2 deltaMovement = _velocity * Time.deltaTime;
        _playerController.Move(ref deltaMovement);
        _distance += relativeSpeedToGround * Time.deltaTime / 2;
        _score += relativeSpeedToGround * Time.deltaTime / 2;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var collisionInfo = _playerController.collisionInfo;
            if (collisionInfo.below)
            {
                secondJumpAvailable = true;
                canJump = true;
            }
            else
            {
                if (secondJumpAvailable)
                {
                    canJump = true;
                    secondJumpAvailable = false;
                }
                else
                    canJump = false;

            }
            if (canJump)
            {
                _animator.SetBool("jump", true);
                _animator.SetBool("onGround", false);
                _velocity.y = jumpHeight;
            }
        }
    }

    // called by player animator state machine
    public void LaunchAttack()
    {
        // Get the collider at this specific frame
        Transform hitboxTransform = transform.Find("Hitbox");
        int childCount = hitboxTransform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            if (hitboxTransform.GetChild(i).gameObject.activeInHierarchy)
            {
                hitbox = hitboxTransform.GetChild(i).gameObject.GetComponent<BoxCollider2D>();
                break;
            }
        }

        Assert.IsNotNull(hitbox);
        Bounds bounds = hitbox.bounds;
        // register hits on a bunch of bullets
        Collider2D[] hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max, _layerMask);

        // populate list 
        // make sure that each enemy receive one damage instance per attack frame
        // make sure that each bullet only get 
        foreach (Collider2D _collider in hits)
        {
            int instanceID = _collider.transform.root.GetInstanceID();
            
            if (_collider.transform.parent.parent != null && _collider.transform.parent.parent.tag == "Enemy" && !damagedEnemyList.ContainsKey(instanceID))
            {
                damagedEnemyList.Add(instanceID, _collider);
            }
            if (_collider.gameObject.tag == "Projectile" && !incomingProjectileList.ContainsKey(instanceID))
            {
                incomingProjectileList.Add(instanceID, _collider);
            }
        }
    }

    public void BounceOffBullet()
    {
        foreach (KeyValuePair<int, Collider2D> kvp in incomingProjectileList)
        {
            Collider2D collider = kvp.Value;
            float raycastDistance = Vector2.Distance(hitbox.bounds.center, collider.bounds.center);
            Vector2 raycastDirection = (hitbox.bounds.center - collider.bounds.center).normalized;
            RaycastHit2D hit = Physics2D.Raycast(collider.bounds.center, raycastDirection, raycastDistance, _layerMask);
            Vector2 contactPoint = hit.point;

            Projectile projectile = collider.gameObject.GetComponent<Projectile>();
            // do not reflect the same bullet that already damaged us
            // instead, destroy it (return the object back to pool)
            if (projectile != null && tookDamageFromProjectileList.ContainsKey(projectile.gameObject.GetInstanceID()))
            {
                // TODO: destroy/return this bullet to pool
                continue;
            }
            // do not bounce the same bullet more than once if we managed to reflect it back
            if (projectile != null && !projectile.bounced)
            {
                float bouncingAngle = GetBounceAngle(contactPoint, hitbox);
                projectile.Bounce(bouncingAngle);
                deflectedProjectileList.Add(projectile.gameObject.GetInstanceID(), 1);
            }
        }
    }

    public void TakeDamage(float damage, int bulletId)
    {
        // do not take damage if the bullet has been deflected
        if (deflectedProjectileList.ContainsKey(bulletId))
            return;
        _currentHitpoint -= damage;
        tookDamageFromProjectileList.Add(bulletId, 1);
        StartCoroutine(_IndicateBeingDamaged());

        if (_currentHitpoint <= 0 && !_isDead)
        {
            _isDead = true;
            GameManager.instance.ReloadLevel();
            _currentHitpoint = 0f;
        }
        deflectedProjectileList.Clear();
    }

    float GetBounceAngle(Vector2 contactPoint, Collider2D hitbox)
    {
        float bounceAngle = Random.Range(10.5f, 30f);
        return bounceAngle;
    }

    IEnumerator _IndicateBeingDamaged()
    {
        Color red = new Color32(0xFF, 0x78, 0x78, 0xFF);
        Color zeroAlpha = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        int flashTime = 5;
        for (int i = 0; i < flashTime; i++)
        {
            _spriteRenderer.color = red;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = zeroAlpha;
            yield return new WaitForSeconds(0.1f);

        }
    }

    public void KnockObstacle()
    {
        _isDead = true;
        _animator.SetTrigger("dead");
    }

    public bool PlayerDeath()
    {
        return _isDead;
    }

    public void StatTracker(string statType)
    {
        if (statType == "killCount")
        {
            _killCount++;
            _score += 10f;
        }
        if (statType == "obstacleDodge")
        {
            _obstacleDodgeCount++;
            _score += 10f;
        }
        if (statType == "projectileDodge")
        {
            _projectileDodgeCount++;
            _score += 10f;
        }
        if (statType == "enemyDodge")
        {
            _enemyDodgeCount++;
            _score += 10f;
        }
        if (statType == "projectileBlock")
        {
            _projectileBlockCount++;
            _score += 10f;
        }
    }

    public float GetStats(string statType)
    {
        if (statType == "score") return _score;
        else if (statType == "distance") return _distance;
        else if (statType == "killCount") return _killCount;
        else if (statType == "obstacleDodge") return _obstacleDodgeCount;
        else if (statType == "projectileDodge") return _projectileDodgeCount;
        else if (statType == "enemyDodge") return _enemyDodgeCount;
        else if (statType == "projectileBlock") return _projectileBlockCount;
        else return 0;
    }
}
