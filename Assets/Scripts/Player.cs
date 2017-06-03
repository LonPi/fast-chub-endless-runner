using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float jumpSpeed;
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
    bool secondJumpAvail;
    bool canJump;
    bool isJumping;
    public bool Attack { get; set; }
    public bool HasJumpedTwice { get; set; }
    public bool OnGround { get { return IsGrounded(); } }
    Animator _animator;
    public Rigidbody2D _rb2D { get; set; }
    BoxCollider2D _boxCollider;
    public Dictionary<int, GameObject> damagedEnemies = new Dictionary<int, GameObject>();

	void Start ()
    {
        _animator = GetComponent<Animator>();
        _rb2D = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }
	
	void Update ()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetTrigger("attack");
            LaunchAttack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                secondJumpAvail = true;
                canJump = true;
            }
            else
            {
                if (secondJumpAvail)
                {
                    canJump = true;
                    secondJumpAvail = false;
                }
                else
                    canJump = false;

            }
            if (canJump)
            {
                _animator.SetBool("jump", true);
            }
        }
    }

    bool IsGrounded()
    {
        Bounds bounds = _boxCollider.bounds;
        float raycastLength = bounds.size.y;
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        Debug.DrawRay(bounds.center, Vector2.down * raycastLength, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(bounds.center, Vector2.down, raycastLength, layerMask);
        return hit;
    }

    public void LaunchAttack()
    {
        // Get the collider at this specific frame
        Transform hitboxTransform = transform.Find("Hitbox");
        int childCount = hitboxTransform.childCount;
        BoxCollider2D hitbox = null;

        for (int i = 0; i < childCount; i++)
        {
            if (hitboxTransform.GetChild(i).gameObject.activeInHierarchy)
            {
                hitbox = hitboxTransform.GetChild(i).gameObject.GetComponent<BoxCollider2D>();
                break;
            }
        }

        if (hitbox == null)
        {
            return;
        }

        Bounds bounds = hitbox.bounds;
        Vector2 bottomLeft = transform.TransformPoint(bounds.min);
        Vector2 topRight = transform.TransformPoint(bounds.max);
        //Debug.Log("world: " + bottomLeft + " " + topRight + " local: " + bounds.min + " " + bounds.max);
        Collider2D[] hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max, 1 << LayerMask.NameToLayer("Enemy"));
        foreach(Collider2D _collider in hits)
        {
            int instanceID = _collider.transform.root.GetInstanceID();
            //Debug.LogError("hit: " + instanceID + " " + _collider.transform.gameObject.name);
            if (!damagedEnemies.ContainsKey(instanceID)) {
                damagedEnemies.Add(instanceID, _collider.transform.root.gameObject);
            }
        }
        
        
    }

}
