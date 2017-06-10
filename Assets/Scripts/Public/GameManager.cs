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
    float fadeTime = 2.0f;

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
    }

    public void ReloadLevel()
    {
        StartCoroutine(_ReloadLevel());
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
}
