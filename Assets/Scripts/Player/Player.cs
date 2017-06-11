using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
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

    public bool OnGround { get { return _playerController.collisionInfo.below; } }
    public Vector2 velocity { get { return _velocity; } }
    public float relativeSpeedToGround { get; private set; }
    // private variables
    Animator _animator;
    Vector2 _velocity;
    PlayerController _playerController;
    float gravity = -20f;
    bool secondJumpAvailable, canJump;
    bool _isDead;
    float 
        _distance = 0f,
        _score = 0f,
        _birdKill = 0f,
        _catKill = 0f,
        _obstacleDodge = 0f,
        _birdDodge = 0f,
        _catDodge = 0f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        relativeSpeedToGround = GameManager.instance._parallaxRef.scrollingSpeed;
        var collisionInfo = _playerController.collisionInfo;
        if (collisionInfo.below || collisionInfo.above)
        {
            _velocity.y = 0f;
        }

        if (!_isDead)
            HandleInput();

        HandleMovement();
        _distance += relativeSpeedToGround * Time.deltaTime / 2;
        _score += relativeSpeedToGround * Time.deltaTime / 2;
    }

    void HandleMovement()
    {
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
                _velocity.y = jumpHeight;
            }
        }
    }

    public void KnockObstacle()
    {
        _isDead = true;
        _animator.SetTrigger("dead");
        SoundManager.Instance.PlayerPlayOneShot(SoundManager.Instance.playerDead);
        GameManager.instance.OnGameOver();
    }

    public bool PlayerDeath()
    {
        return _isDead;
    }

    public void StatTracker(string statType)
    {
        if (statType == "birdKill")
        {
            _birdKill++;
            _score += 10f;
        }
        if (statType == "catKill")
        {
            _catKill++;
            _score += 10f;
        }
        if (statType == "obstacleDodge")
        {
            _obstacleDodge++;
            _score += 10f;
        }
        if (statType == "birdDodge")
        {
            _birdDodge++;
            _score += 10f;
            SoundManager.Instance.MiscPlayOneShot(SoundManager.Instance.deadBird);
        }
        if (statType == "catDodge")
        {
            _catDodge++;
            _score += 10f;
            SoundManager.Instance.MiscPlayOneShot(SoundManager.Instance.deadCat);
        }
    }

    public float GetStats(string statType)
    {
        if (statType == "score") return _score;
        else if (statType == "distance") return _distance;
        else if (statType == "birdKill") return _birdKill;
        else if (statType == "catKill") return _catKill;
        else if (statType == "obstacleDodge") return _obstacleDodge;
        else if (statType == "birdDodge") return _birdDodge;
        else if (statType == "catDodge") return _catDodge;
        else return 0;
    }
}
