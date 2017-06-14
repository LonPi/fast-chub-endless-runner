using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Player _playerRef { get; private set; }
    public Camera _cameraRef { get; private set; }
    public Parallax _parallaxRef { get; private set; }
    public GameplayCanvas _gameplayCanvasRef { get; private set; }
    float fadeTime = 2.0f;
    public HighScore highScore;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void InitReferences()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _cameraRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _parallaxRef = GameObject.Find("Ground").GetComponent<Parallax>();
        _gameplayCanvasRef = GameObject.Find("Canvas").GetComponent<GameplayCanvas>();
    }

    void RecordHighScore()
    {
        if (Player.Instance.GetStats("score") > highScore.highestScore)
            highScore.highestScore = Player.Instance.GetStats("score");
        if (Player.Instance.GetStats("distance") > highScore.longestDistance)
            highScore.longestDistance = Player.Instance.GetStats("distance");
        if (Player.Instance.GetStats("birdKill") > highScore.highestBirdKill)
            highScore.highestBirdKill = Player.Instance.GetStats("birdKill");
        if (Player.Instance.GetStats("catKill") > highScore.highestCatKill)
            highScore.highestCatKill = Player.Instance.GetStats("catKill");
        if (Player.Instance.GetStats("birdDodge") > highScore.highestBirdDodged)
            highScore.highestBirdDodged = Player.Instance.GetStats("birdDodge");
        if (Player.Instance.GetStats("catDodge") > highScore.highestCatDodged)
            highScore.highestCatDodged = Player.Instance.GetStats("catDodge");
        if (Player.Instance.GetStats("obstacleDodge") > highScore.highestObstacleDodged)
            highScore.highestObstacleDodged = Player.Instance.GetStats("obstacleDodge");
    }

    public void ReloadLevel()
    {
        StopAllCoroutines();
        StartCoroutine(_ReloadLevel());
    }

    public void OnGameOver()
    {
        RecordHighScore();
        _gameplayCanvasRef.OnGameOver();
    }

    IEnumerator _ReloadLevel()
    {
        fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitReferences();
        PoolManager.instance.OnSceneLoaded();
    }

    public struct HighScore
    {
        public float
            highestScore,
            longestDistance,
            highestBirdKill,
            highestCatKill,
            highestObstacleDodged,
            highestBirdDodged,
            highestCatDodged;
    }

}
