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
            PoolManager.instance.ReturnObjectToPool(gameObject);

        relativeSpeedToGround = -1 * (Player.Instance.relativeSpeedToGround + moveSpeed);
        if (Player.Instance.PlayerDeath())
            relativeSpeedToGround = 0f;
        transform.Translate(new Vector2(relativeSpeedToGround, 0f) * Time.deltaTime);
        if (Player.Instance.transform.position.x > transform.position.x && !dodged)
        {
            Player.Instance.StatTracker("obstacleDodge");
            dodged = true;
        }
    }

    public void SetParams(Vector2 position)
    {
        transform.position = position;
        if (_boxCollider != null)
        {
            _boxCollider.enabled = true;
        }

    }

    bool InCameraView()
    {
        Vector3 screenPoint = GameManager.instance._cameraRef.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.collider is BoxCollider2D)
        {
            Vector3 contactPoint = collision.contacts[0].point;

            if (collision.collider.bounds.center.y < this.GetComponent<Collider2D>().bounds.max.y)
            {
                Player.Instance.KnockObstacle();
                GetComponent<BoxCollider2D>().enabled = false;
            }
            
        }
    }
}
