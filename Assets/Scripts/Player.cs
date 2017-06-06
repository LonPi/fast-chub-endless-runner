using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour {

    public float jumpHeight;
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<Player>();
            return instance;
        }
    }

    public bool Jump { get; set; }
    public bool Attack { get; set; }
    public bool OnGround { get { return _playerController.collisionInfo.below; }  }
    public Vector2 velocity { get { return _velocity; } }
    public float relativeSpeedToGround { get; private set; }
    public Dictionary<int, Collider2D> damagedEnemyList = new Dictionary<int, Collider2D>();
    public Dictionary<int, Collider2D> incomingBulletList = new Dictionary<int, Collider2D>();

    // private variables
    int horizontalRayCount = 7, verticalRayCount = 7;
    float skinWidth = .015f;
    float verticalRaySpacing, horizontalRaySpacing;
    Animator _animator;
    Vector2 _velocity;
    PlayerController _playerController;
    BoxCollider2D hitbox;
    float gravity = -20f;
    bool secondJumpAvailable;
    bool canJump;
    int _layerMask;
    

	void Start ()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
        relativeSpeedToGround = GameObject.Find("Ground").GetComponent<Parallax>().scrollingSpeed;
        _layerMask = 1 << LayerMask.NameToLayer("Enemy");
    }
	
	void Update ()
    {
        var collisionInfo = _playerController.collisionInfo;
        if (collisionInfo.below || collisionInfo.above)
        {
            _velocity.y = 0f;
        }
        HandleInput();
        _velocity.y += gravity * Time.deltaTime;
        Vector2 deltaMovement = _velocity * Time.deltaTime;
        _playerController.Move(ref deltaMovement);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var collisionInfo = _playerController.collisionInfo;
            if (collisionInfo.below)
            {
                secondJumpAvailable = true;
                canJump = true;
            }
            else
            {
                if (secondJumpAvailable)
                {
                    canJump = true;
                    secondJumpAvailable = false;
                }
                else
                    canJump = false;

            }
            if (canJump)
            {
                _animator.SetBool("jump", true);
                _animator.SetBool("onGround", false);
                transform.position = new Vector2(transform.position.x, transform.position.y + jumpHeight);
            }
        }
    }

    // called by player animator state machine
    public void LaunchAttack()
    {
        // Get the collider at this specific frame
        Transform hitboxTransform = transform.Find("Hitbox");
        int childCount = hitboxTransform.childCount;
        

        for (int i = 0; i < childCount; i++)
        {
            if (hitboxTransform.GetChild(i).gameObject.activeInHierarchy)
            {
                hitbox = hitboxTransform.GetChild(i).gameObject.GetComponent<BoxCollider2D>();
                break;
            }
        }

        Assert.IsNotNull(hitbox);

        Bounds bounds = hitbox.bounds;
        Vector2 bottomLeft = transform.TransformPoint(bounds.min);
        Vector2 topRight = transform.TransformPoint(bounds.max);
        // register hits on a bunch of bullets
        Collider2D[] hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max, _layerMask);

        foreach(Collider2D _collider in hits)
        {
            int instanceID = _collider.transform.root.GetInstanceID();
            
            if (_collider.transform.root.tag == "Enemy" && !damagedEnemyList.ContainsKey(instanceID)) {
                damagedEnemyList.Add(instanceID, _collider);
            }
            if (_collider.transform.root.tag == "Bullet" && !incomingBulletList.ContainsKey(instanceID))
            {
                incomingBulletList.Add(instanceID, _collider);
                Debug.LogError("bullet hit");
            }
        }
    }

    public void BounceOffBullet()
    {
        foreach (KeyValuePair<int, Collider2D> kvp in incomingBulletList)
        {
            Collider2D collider = kvp.Value;
            float raycastDistance = Vector2.Distance(hitbox.bounds.center, collider.bounds.center);
            Vector2 raycastDirection = (hitbox.bounds.center - collider.bounds.center).normalized;
            RaycastHit2D hit = Physics2D.Raycast(collider.bounds.center, raycastDirection, raycastDistance, _layerMask);
            //Debug.LogError("Player.LaunchAttack() Bullet center: " + _collider.bounds.center  + " hitbox center: " + hitbox.bounds.center + " intersect point: " + hit.point + " distance= " + raycastDistance);
            Vector2 contactPoint = new Vector2(hitbox.bounds.center.x + hitbox.bounds.extents.x, hit.point.y);
            Assert.IsTrue(hitbox.bounds.Contains(contactPoint));

            Bullet bullet = collider.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                float bouncingAngle = CalculateBounceAngle(contactPoint, hitbox);
                bullet.SetVelocity(bouncingAngle);
            }
        }
        incomingBulletList.Clear();
    }

    void CalculateRaySpacing(Collider2D hitbox)
    {
        Bounds bounds = hitbox.bounds;
        verticalRaySpacing = (bounds.size.x - 2 * skinWidth) / (verticalRayCount - 1);
        horizontalRaySpacing = (bounds.size.y - 2 * skinWidth) / (horizontalRayCount - 1);
    }

    float CalculateBounceAngle(Vector2 contactPoint, Collider2D hitbox)
    {
        float bounceAngle = Random.Range(22.5f, 70f);
        Debug.Log("original bounce angle: " + bounceAngle);
        bool a = false;
        if (contactPoint.y >= hitbox.bounds.center.y)
        {
            bounceAngle += 90f;
            a = true;
        }
        Debug.Log("returned bounce angle: " + bounceAngle + " hit point below half: " + a);
        return bounceAngle;
    }
}
