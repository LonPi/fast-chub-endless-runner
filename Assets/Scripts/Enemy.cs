using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float moveSpeed;
    public float minAttackInterval, maxAttackInterval;
    public GameObject Bullet;
    float relativeSpeedToGround;
    float direction = -1;
    Rigidbody2D _rb2D;
    bool isFacingRight;
    float randomInterval;
    float attackTimer;
    Animator _animator;
    bool _wasVisible;

    void Start () {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        isFacingRight = true;
        attackTimer = 0f;
        relativeSpeedToGround = direction * (Player.Instance.relativeSpeedToGround + moveSpeed);
        GenerateRandomAttackInterval();
	}

    void Update ()
    {
        Move();
        Attack();
    }

    void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= randomInterval)
        {
            _animator.SetTrigger("attack");
            GameObject obj = Instantiate(Bullet, transform.position, Quaternion.identity);
            attackTimer = 0f;
            GenerateRandomAttackInterval();
        }
    }

    void Move()
    {
        _rb2D.velocity = new Vector2(relativeSpeedToGround, _rb2D.velocity.y);
        Flip();
    }
    public void SetParams(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    void GenerateRandomAttackInterval()
    {
        randomInterval = Random.Range(minAttackInterval - 0.01f, maxAttackInterval - 0.01f);
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
