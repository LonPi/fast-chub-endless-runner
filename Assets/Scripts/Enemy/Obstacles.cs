using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {

    public float moveSpeed;
    float direction = -1f;
    float relativeSpeedToGround;

    Rigidbody2D _rb2D;

	void Start () {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    void Update ()
    {
        relativeSpeedToGround = direction * (Player.Instance.relativeSpeedToGround + moveSpeed);
        if (Player.Instance.PlayerDeath())
            relativeSpeedToGround = 0;
        transform.Translate(new Vector2(relativeSpeedToGround, 0f) * Time.deltaTime);
    }

    public void SetParams(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 contactPoint = collision.contacts[0].point;

            if (collision.collider.bounds.min.y < this.GetComponent<Collider2D>().bounds.max.y)
                Player.Instance.KnockObstacle();
        }
    }
}
