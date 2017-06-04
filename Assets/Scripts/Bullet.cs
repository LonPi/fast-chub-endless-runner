using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float moveSpeed;
    Vector2 targetPosition;
    Vector2 direction;
    Vector2 oldPosition;
    Vector2 oldVelocity;
    Rigidbody2D _rb2D;

    void Start () {
        targetPosition = Player.Instance.transform.position;
        direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
        direction = direction.normalized;
        _rb2D = GetComponent<Rigidbody2D>();
        _rb2D.velocity = direction * moveSpeed;
    }

	void Update ()
    {
    }

    
    void FixedUpdate()
    {

    }

}
