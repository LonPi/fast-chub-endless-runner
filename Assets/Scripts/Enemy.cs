using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float moveSpeed, maxHitpoints, damage;
    public float minAttackInterval, maxAttackInterval;
    public GameObject Bullet;
    float relativeSpeedToGround;
    float direction = -1;
    Rigidbody2D _rb2D;
    bool isFacingRight;
    float randomIdleInterval;
    float timer;
    float curHitpoints;
    Animator _animator;

    void Start () {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        isFacingRight = true;
        timer = 0f;
        relativeSpeedToGround = direction * (Player.Instance.relativeSpeedToGround + moveSpeed);
        curHitpoints = maxHitpoints;
        GenerateRandomIdleInterval();
    }

    void Update ()
    {
        timer += Time.deltaTime;
        if (timer >= randomIdleInterval)
        {
            Attack();
            timer = 0f;
            GenerateRandomIdleInterval();
        }
    }

    void Attack()
    {
        _animator.SetTrigger("attack");
        GameObject obj = Instantiate(Bullet, transform.position, Quaternion.identity);
    }

    public void SetParams(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void ApplyDamage(float _damage)
    {
        this.curHitpoints -= _damage;
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
