using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed, damage;
    public bool bounced { get; private set; }
    Vector2 targetPosition;
    int _layerMask;
    bool hitPlayer;

    // kinematic equation variables
    //float gravity = -3f;
    public float gravity = -20f;
    Vector2 _velocity;
    float initialVelocity_y;

    void Start()
    {
        targetPosition = Player.Instance.transform.position;
        hitPlayer = false;
        _layerMask = 1 << LayerMask.NameToLayer("Player");
        CalculateInitialVelocity();
    }

    void Update()
    {
        if (!InCameraView())
        {
            PoolManager.instance.ReturnObjectToPool(gameObject);
            if (Player.Instance.tookDamageFromProjectileList.ContainsKey(gameObject.GetInstanceID()))
            {
                Player.Instance.tookDamageFromProjectileList.Remove(gameObject.GetInstanceID());
            }
        }
    }

    private void FixedUpdate()
    {
        if (!hitPlayer)
        {
            DetectHit();
        }
        _velocity.y += gravity * Time.deltaTime;
        transform.position += (Vector3)_velocity * Time.deltaTime;
    }

    bool InCameraView()
    {
        Vector3 screenPoint = Player.Instance._cameraRef.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    public void SetParams(Vector2 position)
    {
        transform.position = position;
        targetPosition = Player.Instance.transform.position;
        CalculateInitialVelocity();
        hitPlayer = false;
        transform.rotation = Quaternion.identity;
        bounced = false;
    }

    void CalculateInitialVelocity()
    {
        float distanceToTravel, timeToTravel, height;
        float resultantHorizontalVelocity = moveSpeed + Player.Instance.relativeSpeedToGround;
        distanceToTravel = Mathf.Abs(targetPosition.x - transform.position.x);
        timeToTravel = distanceToTravel / resultantHorizontalVelocity;
        height = Mathf.Abs(targetPosition.y - transform.position.y);
        initialVelocity_y = (height - 0.5f * gravity * Mathf.Pow(timeToTravel, 2)) / timeToTravel;

        // set initial velocity
        // initial velocity has to be scaled according to ground speed
        _velocity = new Vector2(-1 * resultantHorizontalVelocity, initialVelocity_y);
    }

    void DetectHit()
    {
        Collider2D hit = null;
        if (GetComponent<CircleCollider2D>() != null)
        {
            CircleCollider2D _circleCollider = GetComponent<CircleCollider2D>();
            Bounds bounds = _circleCollider.bounds;
            float radius = Mathf.Abs(_circleCollider.radius * transform.localScale.x);
            hit = Physics2D.OverlapCircle(bounds.center, radius, _layerMask);
            Debug.DrawLine(bounds.center, new Vector2(bounds.center.x + radius, bounds.center.y + radius), Color.magenta);
        }

        else if (GetComponent<BoxCollider2D>() != null)
        {
            BoxCollider2D _boxCollider = GetComponent<BoxCollider2D>();
            Bounds bounds = _boxCollider.bounds;
            hit = Physics2D.OverlapArea(bounds.min, bounds.max, _layerMask);
            Debug.DrawLine(bounds.max, bounds.min, Color.magenta);
        }
        
        // make sure player's hurtbox is hit and not their feet collider for example
        if (hit && hit.gameObject.tag != "Player")
            return;

        if (hit && hit.gameObject.transform.parent != null && hit.gameObject.transform.parent.name == "Hurtbox" && !bounced)
        {
            hitPlayer = true;
            Player.Instance.TakeDamage(this.damage, gameObject.GetInstanceID());
        }
    }

    public void Bounce(float bouncingAngle)
    {
        float velocityMagnitude = _velocity.magnitude;
        transform.rotation = Quaternion.Euler(0, 0, bouncingAngle - 180f);
        Debug.Log("before bounce: " + _velocity);
        this._velocity.y = initialVelocity_y;
        this._velocity.x *= -1;
        this._velocity.x -= Player.Instance.relativeSpeedToGround;
        //this._velocity.y = Mathf.Sin(bouncingAngle * Mathf.Deg2Rad) * velocityMagnitude;
        //this._velocity.x = Mathf.Cos(bouncingAngle * Mathf.Deg2Rad) * velocityMagnitude;
        this.bounced = true;
    }
}
