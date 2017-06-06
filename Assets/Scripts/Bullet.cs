using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float moveSpeed;
    Vector2 targetPosition;
    Vector2 direction;
    Vector2 velocity;

    void Start () {
        targetPosition = Player.Instance.transform.position;
        direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
        direction = direction.normalized;
        velocity = direction * moveSpeed;
    }

	void Update ()
    {
        
    }

    
    void FixedUpdate()
    {
        transform.Translate(velocity * Time.deltaTime);
    }

    public void SetVelocity(float bouncingAngle)
    {
        float velocityMagnitude = velocity.magnitude;
        Debug.Log("velocity magnitude: " + velocityMagnitude);
        //this.velocity.x = Mathf.Cos(bouncingAngle * Mathf.Deg2Rad) * velocityMagnitude;
        //this.velocity.y = Mathf.Sin(bouncingAngle * Mathf.Deg2Rad) * velocityMagnitude;
        Debug.Log("final velocity: " + velocity);
        transform.rotation = Quaternion.Euler(0, 0, bouncingAngle - 180f);
    }

}
