using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{

    public float jumpHeight, maxHitpoints;
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
    int horizontalRayCount = 7, verticalRayCount = 7;
    float skinWidth = .015f;
    float verticalRaySpacing, horizontalRaySpacing;
    Animator _animator;
    Vector2 _velocity;
    PlayerController _playerController;
    BoxCollider2D hitbox;
    SpriteRenderer _spriteRenderer;
    float gravity = 0f;
    float _currentHitpoint;
    bool secondJumpAvailable, canJump;
    bool _isDead;
    int _layerMask;

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
        relativeSpeedToGround = GameObject.Find("Ground").GetComponent<Parallax>().scrollingSpeed;
        var collisionInfo = _playerController.collisionInfo;
        if (collisionInfo.below || collisionInfo.above)
        {
            _velocity.y = 0f;
        }
        HandleInput();
        _velocity.y += gravity * Time.deltaTime;
        Vector2 deltaMovement = _velocity * Time.deltaTime;
        _playerController.Move(ref deltaMovement);
    }

    private void LateUpdate()
    {

    }
    private void FixedUpdate()
    {
        
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetTrigger("attack");
            //GameObject.Find("Main Camera").GetComponent<CameraShake>().Shake(0.1f, 0.1f);
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
                //_playerController.Move(ref deltaMovement);
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
        Vector2 bottomLeft = transform.TransformPoint(bounds.min);
        Vector2 topRight = transform.TransformPoint(bounds.max);
        // register hits on a bunch of bullets
        Collider2D[] hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max, _layerMask);

        // populate list 
        // make sure that each enemy receive one damage instance per attack frame
        // make sure that each bullet only get 
        foreach (Collider2D _collider in hits)
        {
            int instanceID = _collider.transform.root.GetInstanceID();

            if (_collider.transform.root.tag == "Enemy" && !damagedEnemyList.ContainsKey(instanceID))
            {
                damagedEnemyList.Add(instanceID, _collider);
            }
            if (_collider.transform.root.tag == "Projectile" && !incomingProjectileList.ContainsKey(instanceID))
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

            Projectile bullet = collider.gameObject.GetComponent<Projectile>();
            // do not reflect the same bullet that already damaged us
            // instead, destroy it (return the object back to pool)
            if (bullet != null && tookDamageFromProjectileList.ContainsKey(bullet.gameObject.GetInstanceID()))
            {
                // TODO: destroy/return this bullet to pool
                continue;
            }
            // do not bounce the same bullet more than once if we managed to reflect it back
            if (bullet != null && !bullet.bounced)
            {
                float bouncingAngle = GetBounceAngle(contactPoint, hitbox);
                bullet.Bounce(bouncingAngle);
                deflectedProjectileList.Add(bullet.gameObject.GetInstanceID(), 1);
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
        Debug.Break();
        StartCoroutine(_IndicateBeingDamaged());

        if (_currentHitpoint <= 0 && !_isDead)
        {
            _isDead = true;
            _currentHitpoint = 0f;
        }
        deflectedProjectileList.Clear();
    }

    void CalculateRaySpacing(Collider2D hitbox)
    {
        Bounds bounds = hitbox.bounds;
        verticalRaySpacing = (bounds.size.x - 2 * skinWidth) / (verticalRayCount - 1);
        horizontalRaySpacing = (bounds.size.y - 2 * skinWidth) / (horizontalRayCount - 1);
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
    }

    public bool PlayerDeath()
    {
        return _isDead;
    }
}
