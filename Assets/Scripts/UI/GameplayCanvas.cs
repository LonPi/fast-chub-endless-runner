using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour {

    Text timerText, distanceText, scoreText, birdKillText, hiScoreText,
        catKillText, obstacleDodgedText, birdDodgedText, catDodgedText;
    Text oopsText, currentScoreText, bestScoreText,
        currentDistanceText, bestDistanceText,
        currentTreeDodgeText, bestTreeDodgeText,
        currentBirdDodgeText, bestBirdDodgeText,
        currentCatDodgeText, bestCatDodgeText,
        currentBirdKillText, bestBirdKillText,
        currentCatKillText, bestCatKillText, highScoreText;
    GameObject retryButton, highScoreGrid;

    void Start ()
    {
        _InGameUiRef();
        _EndGameUiRef();
        // disable end-game texts and buttons
        oopsText.enabled = false;
        highScoreGrid.SetActive(false);
        retryButton.SetActive(false);
    }

    public void OnGameOver()
    {
        // disable in-game texts
        distanceText.enabled = false;
        scoreText.enabled = false;
        birdKillText.enabled = false;
        catKillText.enabled = false;
        obstacleDodgedText.enabled = false;
        birdDodgedText.enabled = false;
        catDodgedText.enabled = false;
        hiScoreText.enabled = false;
        // get end-game stats
        currentScoreText.text = Mathf.RoundToInt(Player.Instance.GetStats("score")).ToString();
        bestScoreText.text = Mathf.RoundToInt(GameManager.instance.highScore.highestScore).ToString();
        currentDistanceText.text = Mathf.RoundToInt(Player.Instance.GetStats("distance")).ToString();
        bestDistanceText.text = Mathf.RoundToInt(GameManager.instance.highScore.longestDistance).ToString();
        currentTreeDodgeText.text = Mathf.RoundToInt(Player.Instance.GetStats("obstacleDodge")).ToString();
        bestTreeDodgeText.text = Mathf.RoundToInt(GameManager.instance.highScore.highestObstacleDodged).ToString();
        currentBirdDodgeText.text = Mathf.RoundToInt(Player.Instance.GetStats("birdDodge")).ToString();
        bestBirdDodgeText.text = Mathf.RoundToInt(GameManager.instance.highScore.highestBirdDodged).ToString();
        currentCatDodgeText.text = Mathf.RoundToInt(Player.Instance.GetStats("catDodge")).ToString();
        bestCatDodgeText.text = Mathf.RoundToInt(GameManager.instance.highScore.highestCatDodged).ToString();
        currentBirdKillText.text = Mathf.RoundToInt(Player.Instance.GetStats("birdKill")).ToString();
        bestBirdKillText.text = Mathf.RoundToInt(GameManager.instance.highScore.highestBirdKill).ToString();
        currentCatKillText.text = Mathf.RoundToInt(Player.Instance.GetStats("catKill")).ToString();
        bestCatKillText.text = Mathf.RoundToInt(GameManager.instance.highScore.highestCatKill).ToString();
        StartCoroutine(_OnDisplayHighScore());
    }

    public void PressRestartLevel()
    {
        GameManager.instance.ReloadLevel();
    }

    IEnumerator _OnDisplayHighScore()
    {
        oopsText.gameObject.GetComponent<OnScreenMessage>().ShowText();
        yield return new WaitForSeconds(oopsText.gameObject.GetComponent<OnScreenMessage>().lifeSpan);
        highScoreGrid.SetActive(true);
        retryButton.SetActive(true);
    }

    void _InGameUiRef()
    {
        timerText = transform.Find("Timer").GetComponent<Text>();
        distanceText = transform.Find("Distance").GetComponent<Text>();
        scoreText = transform.Find("Score").GetComponent<Text>();
        birdKillText = transform.Find("BirdKill").GetComponent<Text>();
        catKillText = transform.Find("CatKill").GetComponent<Text>();
        obstacleDodgedText = transform.Find("ObstacleDodged").GetComponent<Text>();
        birdDodgedText = transform.Find("BirdDodged").GetComponent<Text>();
        catDodgedText = transform.Find("CatDodged").GetComponent<Text>();
        hiScoreText = transform.Find("BestScore").GetComponent<Text>();
    }

    void _EndGameUiRef()
    {
        oopsText = transform.Find("Oops").GetComponent<Text>();
        retryButton = transform.Find("Retry").transform.gameObject;
        highScoreGrid = transform.Find("HighScore").transform.gameObject;
        currentScoreText = transform.Find("HighScore").Find("ScoreTitle").Find("Current").GetComponent<Text>();
        bestScoreText = transform.Find("HighScore").Find("ScoreTitle").Find("Best").GetComponent<Text>();
        currentDistanceText = transform.Find("HighScore").Find("DistanceTitle").Find("Current").GetComponent<Text>();
        bestDistanceText = transform.Find("HighScore").Find("DistanceTitle").Find("Best").GetComponent<Text>();
        currentTreeDodgeText = transform.Find("HighScore").Find("TreeTitle").Find("Current").GetComponent<Text>();
        bestTreeDodgeText = transform.Find("HighScore").Find("TreeTitle").Find("Best").GetComponent<Text>();
        currentBirdDodgeText = transform.Find("HighScore").Find("BirdDodgeTitle").Find("Current").GetComponent<Text>();
        bestBirdDodgeText = transform.Find("HighScore").Find("BirdDodgeTitle").Find("Best").GetComponent<Text>();
        currentCatDodgeText = transform.Find("HighScore").Find("CatDodgeTitle").Find("Current").GetComponent<Text>();
        bestCatDodgeText = transform.Find("HighScore").Find("CatDodgeTitle").Find("Best").GetComponent<Text>();
        currentBirdKillText = transform.Find("HighScore").Find("BirdKillTitle").Find("Current").GetComponent<Text>();
        bestBirdKillText = transform.Find("HighScore").Find("BirdKillTitle").Find("Best").GetComponent<Text>();
        currentCatKillText = transform.Find("HighScore").Find("CatKillTitle").Find("Current").GetComponent<Text>();
        bestCatKillText = transform.Find("HighScore").Find("CatKillTitle").Find("Best").GetComponent<Text>();
    }
}
