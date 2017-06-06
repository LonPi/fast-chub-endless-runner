using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    int verticalRayCount = 6;
    int horizontalRayCount = 6;
    float skinWidth = 0.015f;
    float verticalRaySpacing, horizontalRaySpacing;
    BoxCollider2D myCollider;
    Vector2 raycastTopLeft, raycastBottomLeft, raycastBottomRight;
    int layerMask;
    public CollisionInfo collisionInfo;
    public State state;

    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        layerMask = 1 << LayerMask.NameToLayer("Ground");
        CalculateRaySpacing();
        collisionInfo.Reset();
        state.Reset();
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = myCollider.bounds;
        verticalRaySpacing = (bounds.size.x - 2 * skinWidth) / (verticalRayCount - 1);
        horizontalRaySpacing = (bounds.size.y - 2 * skinWidth) / (horizontalRayCount - 1);
    }

    private void Update()
    {
        CalculateRaySpacing();
        CalculateRaycastOrigins();

        
    }

    void CalculateRaycastOrigins()
    {
        Vector2 _localScale = transform.localScale;
        
        Vector2 size = new Vector2(myCollider.size.x * Mathf.Abs(_localScale.x), myCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
        Vector2 center = new Vector2(myCollider.offset.x * _localScale.x, myCollider.offset.y * _localScale.y);

        raycastTopLeft = transform.position + new Vector3(center.x - size.x + skinWidth, center.y + size.y - skinWidth);
        raycastBottomRight = transform.position + new Vector3(center.x + size.x - skinWidth, center.y - size.y + skinWidth);
        raycastBottomLeft = transform.position + new Vector3(center.x - size.x + skinWidth, center.y - size.y + skinWidth);
    }

    public void Move(ref Vector2 deltaMovement)
    {
        collisionInfo.Reset();
        if (deltaMovement.y != 0)
            HandleVerticalMovement(ref deltaMovement);
        if (deltaMovement.x != 0)
            HandleHorizontalMovement(ref deltaMovement);
        transform.Translate(deltaMovement);

    }

    void HandleVerticalMovement(ref Vector2 deltaMovement)
    {
        state.isFalling = deltaMovement.y < 0;
        float raycastDistance = Mathf.Abs(deltaMovement.y) + skinWidth;
        Vector2 rayOrigin = state.isFalling ? raycastBottomLeft : raycastTopLeft;
        Vector2 direction = state.isFalling ? Vector2.down : Vector2.up;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayVector = new Vector2(rayOrigin.x + i * verticalRaySpacing, rayOrigin.y);
            Debug.DrawRay(rayVector, direction * raycastDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rayVector, direction, raycastDistance, layerMask);
            if (hit)
            {
                deltaMovement.y = hit.point.y - rayVector.y;
                raycastDistance = Mathf.Abs(deltaMovement.y) + skinWidth;
                if (state.isFalling)
                {
                    deltaMovement.y += skinWidth;
                    collisionInfo.below = true;
                }
                else
                {
                    deltaMovement.y -= skinWidth;
                    collisionInfo.above = true;
                }
            }
        }
    }

    void HandleHorizontalMovement(ref Vector2 deltaMovement)
    {
        state.isMovingRight = deltaMovement.x > 0;
        float raycastDistance = Mathf.Abs(deltaMovement.x) + skinWidth;
        Vector2 rayOrigin = state.isMovingRight ? raycastBottomRight : raycastBottomLeft;
        Vector2 direction = state.isMovingRight ? Vector2.right : Vector2.left;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayVector = new Vector2(rayOrigin.x, rayOrigin.y + i * horizontalRaySpacing);
            Debug.DrawRay(rayVector, direction * raycastDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rayVector, direction, raycastDistance, layerMask);

            if (hit)
            {
                deltaMovement.x = hit.point.x - rayVector.x;
                raycastDistance = Mathf.Abs(deltaMovement.x) + skinWidth;
                if (state.isMovingRight)
                {
                    deltaMovement.x -= skinWidth;
                    collisionInfo.right = true;
                }
                else
                {
                    deltaMovement.x += skinWidth;
                    collisionInfo.left = true;
                }
            }
        }
    }

    public struct State
    {
        public bool isMovingRight, isFalling;
        public void Reset()
        {
            isMovingRight = isFalling = false;
        }
    }

    public struct CollisionInfo
    {
        public bool above, below, left, right;
        public void Reset()
        {
            above = below = left = right = false;
        }
    }
}