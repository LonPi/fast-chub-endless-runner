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
        relativeSpeedToGround = direction * (Player.Instance.relativeSpeedToGround + moveSpeed);
	}
	
	void Update ()
    {
        _rb2D.velocity = new Vector2(relativeSpeedToGround, _rb2D.velocity.y);
    }

    public void SetParams(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
}
