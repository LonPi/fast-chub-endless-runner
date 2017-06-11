using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    public float moveSpeed;
    float _relativeSpeedToGround;
    bool _dodged;
    bool _dead;
    BoxCollider2D _boxCollider;
    Animator _animator;
    float _minHeight = -2f;
    float _maxHeight = 2f;

    void Start ()
    {
        _dodged = false;
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        transform.position = new Vector2(transform.position.x, transform.position.y + Random.Range(_minHeight, _maxHeight));
    }

    void Update ()
    {
        if (!InCameraView())
        {
            PoolManager.instance.ReturnObjectToPool(gameObject);
        }
        if (Player.Instance.transform.position.x > (transform.position.x + _boxCollider.bounds.extents.x * 3f) && !_dodged && !_dead )
        {
            Player.Instance.StatTracker("birdDodge");
            _dodged = true;
        }
        _relativeSpeedToGround = -1 * (Player.Instance.relativeSpeedToGround + moveSpeed);
        transform.Translate(new Vector2(_relativeSpeedToGround, 0f) * Time.deltaTime);
    }

    public void SetParams(Vector2 position)
    {
        transform.position = position;
        _dodged = false;
        _dead = false;
        if (_boxCollider != null)
        {
            _boxCollider.enabled = true;
        }
        if (_animator)
        {
            _animator.enabled = true;
            _animator.Play("Idle", 0, 0f);
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
            if (collision.collider.transform.parent != null &&  collision.collider.transform.parent.name == "Hurtbox")
            {
                Player.Instance.KnockObstacle();
            }
            _animator.SetBool("dead", true);
            _boxCollider.enabled = false;
            _dead = true;
            Player.Instance.StatTracker("birdKill");
            SoundManager.Instance.EnemyPlayOneShot(SoundManager.Instance.explodeBird);
        }
    }
}
