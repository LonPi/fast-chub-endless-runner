using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {

    public float moveSpeed;

    Rigidbody2D _rb2D;

	void Start () {
        _rb2D = GetComponent<Rigidbody2D>();	
	}
	
	void Update ()
    {
        _rb2D.velocity = new Vector2(-1* moveSpeed, 0f);
    }

    public void SetParams(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
}
