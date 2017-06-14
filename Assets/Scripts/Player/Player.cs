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
    static bool tutorial = true;
    bool _isDead,
        _inputAtk,
        _inputJump,
        _gotShield,
        _gotHorn;
    float
        _distance = 0f,
        _score = 0f,
        _birdKill = 0f,
        _catKill = 0f,
        _obstacleDodge = 0f,
        _birdDodge = 0f,
        _catDodge = 0f,
        _shieldPick = 0f,
        _hornPick = 0f,
        _shieldDuration = 10f,
        _shieldTimer = 0f,
        _hornDuration = 3f,
        _hornTimer = 0f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
        if (!tutorial)
        {
            GameObject.Find("KeyA").SetActive(false);
            GameObject.Find("KeySpace").SetActive(false);
        }
        
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
            GetInput();

        HandleMovement();
        _distance += relativeSpeedToGround * Time.deltaTime / 2;
        _score += relativeSpeedToGround * Time.deltaTime / 2;
        if (_gotShield)
        {
            _shieldTimer += Time.deltaTime;
            if (_shieldTimer >= _shieldDuration)
            {
                _gotShield = false;
            }
        }
        if (_gotHorn)
        {
            _hornTimer += Time.deltaTime;
            if (_hornTimer >= _hornDuration)
            {
                _gotHorn = false;
            }
        }
    }

    void GetInput()
    {
        _inputAtk = false;
        _inputJump = false;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            if (touchDeltaPosition.x == 0f && touchDeltaPosition.y == 0f)
                _inputJump = true;
            else
                _inputAtk = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
            _inputAtk = true;
        if (Input.GetKeyDown(KeyCode.Space))
            _inputJump = true;  
        HandleInput();
    }

    void HandleMovement()
    {
        _velocity.y += gravity * Time.deltaTime;
        Vector2 deltaMovement = _velocity * Time.deltaTime;
        _playerController.Move(ref deltaMovement);
    }

    void HandleInput()
    {
        if (_inputAtk)
        {
            _animator.SetTrigger("attack");
            if (GameObject.Find("KeyA"))
            {
                GameObject.Find("KeyA").SetActive(false);
                tutorial = false;
            }
        }

        if (_inputJump)
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
            if (GameObject.Find("KeySpace"))
            {
                GameObject.Find("KeySpace").SetActive(false);
                tutorial = false;
            }
        }
    }

    public void KnockObstacle(string source)
    {
        if (!_gotShield || source == "tree")
        {
            _isDead = true;
            _animator.SetTrigger("dead");
            SoundManager.Instance.PlayerPlayOneShot(SoundManager.Instance.playerDead);
            GameManager.instance.OnGameOver();
        }
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
        if (statType == "shieldPick")
        {
            _shieldPick++;
            _gotShield = true;
            transform.Find("Bubble").gameObject.SetActive(true);
            _shieldTimer = 0f;
            //SoundManager.Instance.MiscPlayOneShot(SoundManager.Instance.deadCat);
        }
        if (statType == "hornPick")
        {
            _hornPick++;
            _gotHorn = true;
            _hornTimer = 0f;
            //SoundManager.Instance.MiscPlayOneShot(SoundManager.Instance.deadCat);
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

    public bool BirdHorde()
    {
        return _gotHorn;
    }

    public bool ShieldOn()
    {
        if (_gotShield && _shieldTimer <= _shieldDuration * 0.9f)
            return true;
        else
            return false;
    }
}
