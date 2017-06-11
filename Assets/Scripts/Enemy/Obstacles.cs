using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {

    public float moveSpeed;
    float relativeSpeedToGround;
    bool dodged;
    BoxCollider2D _boxCollider;

	void Start ()
    {
        dodged = false;
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update ()
    {
        if (!InCameraView())
        {
            PoolManager.instance.ReturnObjectToPool(gameObject);
        }

        if (Player.Instance.PlayerDeath())
            relativeSpeedToGround = 0f;

        if (Player.Instance.transform.position.x > transform.position.x && !dodged)
        {
            Player.Instance.StatTracker("obstacleDodge");
            dodged = true;
        }
        relativeSpeedToGround = -1 * (Player.Instance.relativeSpeedToGround + moveSpeed);
        transform.Translate(new Vector2(relativeSpeedToGround, 0f) * Time.deltaTime);
    }

    public void SetParams(Vector2 position)
    {
        transform.position = position;
        dodged = false;
        if (_boxCollider != null)
        {
            _boxCollider.enabled = true;
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
        // collider at the feet
        if (collision.collider.gameObject.tag == "Player" && collision.collider is BoxCollider2D)
        {
            if (collision.collider.bounds.center.y < this.GetComponent<Collider2D>().bounds.max.y)
            {
                Player.Instance.KnockObstacle();
                // disable so that player will not get pushed up on top of the obstacle when they died
                _boxCollider.enabled = false;
                SoundManager.Instance.EnemyPlayOneShot(SoundManager.Instance.knockTree);
            }
            
        }
    }
}
