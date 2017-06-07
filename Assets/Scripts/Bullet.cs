using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float moveSpeed, upwardThrust, damage;
    public bool bounced { get; private set; }
    Vector2 targetPosition, direction, velocity;
    CircleCollider2D _circleCollider;
    float gravity = -20f;
    int _layerMask;
    bool hitPlayer;

    void Start () {
        targetPosition = Player.Instance.transform.position;
        direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
        _circleCollider = GetComponent<CircleCollider2D>();
        direction = direction.normalized;
        velocity = new Vector2(direction.x * moveSpeed, upwardThrust);
        hitPlayer = false;
        _layerMask = 1 << LayerMask.NameToLayer("Player");
    }
    
    void FixedUpdate()
    {
        if (!hitPlayer)
        {
            DetectHit();
        }
        if (!bounced)
        {
            gravity = -10f;
        }

        velocity.y += gravity * Time.deltaTime;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    void DetectHit()
    {
        Bounds bounds = _circleCollider.bounds;
        Collider2D hit = Physics2D.OverlapCircle(bounds.center, _circleCollider.radius, _layerMask);
        // make sure player's hurtbox is hit and not their feet collider for example
        if (hit && hit.transform.parent.name == "Hurtbox" && !bounced)
        {
            hitPlayer = true;
            Player.Instance.TakeDamage(this.damage, gameObject.GetInstanceID());
        }
    }

    public void Bounce(float bouncingAngle)
    {
        float velocityMagnitude = velocity.magnitude;
        transform.rotation = Quaternion.Euler(0, 0, bouncingAngle - 180f);
        this.velocity.y = Mathf.Sin(bouncingAngle * Mathf.Deg2Rad) * velocityMagnitude;
        this.velocity.x = Mathf.Cos(bouncingAngle * Mathf.Deg2Rad) * velocityMagnitude;
        this.bounced = true;
    }
}
