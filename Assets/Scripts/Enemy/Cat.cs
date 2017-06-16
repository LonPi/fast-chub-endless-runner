using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {

    public float moveSpeed;
    float _relativeSpeedToGround;
    float _jumpHeight;
    bool _dodged;
    bool _dead;
    BoxCollider2D _boxCollider;
    Animator _animator;

    void Start ()
    {
        _dodged = false;
        _dead = false;
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _jumpHeight = 0f;
    }

    void Update ()
    {

        if (!InCameraView())
        {
            PoolManager.instance.ReturnObjectToPool(gameObject);
        }
        if (Player.Instance.PlayerDeath())
        {
            _relativeSpeedToGround = 0f;
        }
        if (_dead && _animator.isActiveAndEnabled && _animator.GetBool("jump"))
        {
            _relativeSpeedToGround = 10f;
            _jumpHeight = 30f;
        }
        if (Player.Instance.transform.position.x > (transform.position.x + _boxCollider.bounds.extents.x * 3f) && !_dodged && !_dead)
        {
            Player.Instance.StatTracker("catDodge");
            _dodged = true;
        }
        
    }

    private void LateUpdate()
    {
        _relativeSpeedToGround = -1 * (Player.Instance.relativeSpeedToGround + moveSpeed);
        transform.Translate(new Vector2(_relativeSpeedToGround, _jumpHeight) * Time.smoothDeltaTime);
    }
    public void SetParams(Vector2 position)
    {
        transform.position = position;
        _dodged = false;
        _dead = false;
        _jumpHeight = 0f;
        if (_boxCollider != null)
        {
            _boxCollider.enabled = true;
        }
        if (_animator)
        {
            // reset animator to entry state
            _animator.enabled = true;
            _animator.Play("Idle", 0, 0f);
            _animator.SetBool("jump", false);
            _animator.SetBool("dead", false);
        }
    }

    bool InCameraView()
    {
        Vector3 screenPoint = GameManager.instance._cameraRef.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > -0.2f && screenPoint.y > 0f && screenPoint.y < 1f;
        return onScreen;
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.collider.gameObject.transform.parent != null && collision.collider.gameObject.transform.parent.name == "Hurtbox")
            {
                Player.Instance.KnockObstacle("cat");
            }
            _animator.SetBool("dead", true);
            _boxCollider.enabled = false;
            _dead = true;
            Player.Instance.StatTracker("catKill");
            SoundManager.Instance.EnemyPlayOneShot(SoundManager.Instance.explodeCat);
        }
    }
}
