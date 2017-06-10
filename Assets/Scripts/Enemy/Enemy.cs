using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHitpoints;
    public float minAttackInterval, maxAttackInterval;
    public GameObject ProjectilePrefab;
    public float relativeSpeedToGround { get; private set; }
    Rigidbody2D _rb2D;
    bool isFacingRight;
    float randomIdleInterval;
    float timer;
    float curHitpoints;
    float gravity = -20f;
    Animator _animator;
    EnemyController _enemyController;
    Vector2 _velocity;
    bool _isDead;
    bool wasInCameraView;
    bool dodged;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemyController = GetComponent<EnemyController>();
        isFacingRight = true;
        timer = 0f;
        curHitpoints = maxHitpoints;
        GenerateRandomIdleInterval();
        _isDead = false;
        wasInCameraView = false;
        dodged = false;
    }

    void Update()
    {
        if (!wasInCameraView && InCameraView())
        {
            wasInCameraView = true;
        }
        // return this object back to pool
        if (wasInCameraView && !InCameraView())
            PoolManager.instance.ReturnObjectToPool(gameObject);

        if (_isDead)
        {
            _animator.SetBool("dead", true);
        }

        // attack only if player is on the left side and in camera view
        if (!_isDead && Player.Instance.transform.position.x < transform.position.x && InCameraView())
            Attack();

        var collisionInfo = _enemyController.collisionInfo;
        if (collisionInfo.below)
        {
            _velocity.y = 0f;
        }

        _velocity.y += gravity;
        Vector2 deltaMovement = _velocity * Time.deltaTime;
        relativeSpeedToGround = GameManager.instance._parallaxRef.scrollingSpeed;
        // move the same speed with scrolling ground
        transform.Translate(new Vector2(relativeSpeedToGround * -1, 0f) * Time.deltaTime);
        _enemyController.CollideOnGround(ref deltaMovement);

        if (Player.Instance.transform.position.x > transform.position.x && !dodged)
        {
            Player.Instance.StatTracker("enemyDodge");
            dodged = true;
        }
    }

    void Attack()
    {
        timer += Time.deltaTime;

        if (timer >= randomIdleInterval)
        {
            _animator.SetTrigger("attack");
            //GameObject obj = Instantiate(Bullet, transform.position, Quaternion.identity);
            GameObject projectileObject = PoolManager.instance.GetObjectfromPool(ProjectilePrefab);
            projectileObject.GetComponent<Projectile>().SetParams(transform.position);
            timer = 0f;
            GenerateRandomIdleInterval();
        }
    }

    bool InCameraView()
    {
        Vector3 screenPoint = GameManager.instance._cameraRef.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    public void TakeDamage(float _damage)
    {
        this.curHitpoints -= _damage;

        if (curHitpoints <= 0 && !_isDead)
        {
            curHitpoints = 0;
            _isDead = true;
            Player.Instance.StatTracker("killCount");
        }
    }

    public void SetParams(Vector2 position)
    {
        transform.position = position;
        _isDead = false;
        curHitpoints = maxHitpoints;
        if (_animator)
            _animator.SetBool("dead", false);
        wasInCameraView = false;
    }

    void GenerateRandomIdleInterval()
    {
        randomIdleInterval = Random.Range(minAttackInterval - 0.01f, maxAttackInterval - 0.01f);
    }

    void Flip()
    {
        if (isFacingRight && _rb2D.velocity.x < 0 || !isFacingRight && _rb2D.velocity.x > 0)
        {
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            isFacingRight = !isFacingRight;
        }
    }
}
