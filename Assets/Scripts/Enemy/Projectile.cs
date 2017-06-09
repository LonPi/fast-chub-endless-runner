using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed, damage;
    public bool bounced { get; private set; }
    Vector2 targetPosition;
    int direction = -1; // always to the left
    CircleCollider2D _circleCollider;
    int _layerMask;
    bool hitPlayer;
    bool debug = false;

    // kinematic equation variables
    //float gravity = -3f;
    public float gravity = -20f;
    Vector2 _velocity;

    void Start()
    {
        targetPosition = Player.Instance.transform.position;
        _circleCollider = GetComponent<CircleCollider2D>();
        hitPlayer = false;
        _layerMask = 1 << LayerMask.NameToLayer("Player");
        CalculateInitialVelocity();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!hitPlayer)
        {
            DetectHit();
        }
        if (debug)
        {
            Debug.Log(_velocity);
        }
        _velocity.y += gravity * Time.deltaTime;
        transform.position += (Vector3)_velocity * Time.deltaTime;
    }

    void CalculateInitialVelocity()
    {
        float distanceToTravel, timeToTravel, initialVelocity_y, height;

        distanceToTravel = Mathf.Abs(targetPosition.x - transform.position.x);
        timeToTravel = distanceToTravel / moveSpeed;
        height = Mathf.Abs(targetPosition.y - transform.position.y);
        initialVelocity_y = (height - 0.5f * gravity * Mathf.Pow(timeToTravel, 2)) / timeToTravel;

        // set initial velocity
        _velocity = new Vector2(direction * moveSpeed, initialVelocity_y);
        Debug.Log("distance: " + distanceToTravel + " height: " + height + "initial velocity: " + initialVelocity_y);
    }

    void DetectHit()
    {
        Bounds bounds = _circleCollider.bounds;
        float radius = Mathf.Abs(_circleCollider.radius * transform.localScale.x);
        Collider2D hit = Physics2D.OverlapCircle(bounds.center, radius, _layerMask);
        Debug.DrawLine(bounds.center, new Vector2(bounds.center.x + radius, bounds.center.y + radius), Color.magenta);
        // make sure player's hurtbox is hit and not their feet collider for example
        if (hit && hit.gameObject.tag != "Player")
            return;

        if (hit && hit.gameObject.transform.parent != null && hit.gameObject.transform.parent.name == "Hurtbox" && !bounced)
        {
            hitPlayer = true;
            Debug.Log("hit registered");
            Debug.Log(_velocity);
            debug = true;
            Player.Instance.TakeDamage(this.damage, gameObject.GetInstanceID());
        }
    }

    public void Bounce(float bouncingAngle)
    {
        float velocityMagnitude = _velocity.magnitude;
        transform.rotation = Quaternion.Euler(0, 0, bouncingAngle - 180f);
        this._velocity.y = Mathf.Sin(bouncingAngle * Mathf.Deg2Rad) * velocityMagnitude;
        this._velocity.x = Mathf.Cos(bouncingAngle * Mathf.Deg2Rad) * velocityMagnitude;
        this.bounced = true;
    }
}
