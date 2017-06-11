using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour {

    Text timerText, distanceText, scoreText, birdKillText,
        catKillText, obstacleDodgedText, birdDodgedText, catDodgedText;
    Text oopsText, highScoreText;
    GameObject retryButton;

    void Start ()
    {
        // Set up references
        timerText = transform.Find("Timer").GetComponent<Text>();
        distanceText = transform.Find("Distance").GetComponent<Text>();
        scoreText = transform.Find("Score").GetComponent<Text>();
        birdKillText = transform.Find("BirdKill").GetComponent<Text>();
        catKillText = transform.Find("CatKill").GetComponent<Text>();
        obstacleDodgedText = transform.Find("ObstacleDodged").GetComponent<Text>();
        birdDodgedText = transform.Find("BirdDodged").GetComponent<Text>();
        catDodgedText = transform.Find("CatDodged").GetComponent<Text>();
        oopsText = transform.Find("Oops").GetComponent<Text>();
        highScoreText = transform.Find("HighScore").GetComponent<Text>();
        retryButton = transform.Find("HighScore").Find("Retry").transform.gameObject;
        // disable texts
        oopsText.enabled = highScoreText.enabled = false;
        // disable buttons
        retryButton.SetActive(false);
    }

    public void OnGameOver()
    {
        // disable texts
        timerText.enabled = false;
        distanceText.enabled = false;
        scoreText.enabled = false;
        birdKillText.enabled = false;
        catKillText.enabled = false;
        obstacleDodgedText.enabled = false;
        birdDodgedText.enabled = false;
        catDodgedText.enabled = false;

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
        highScoreText.text =
            "Highest score: " + Mathf.RoundToInt(GameManager.instance.highScore.highestScore) + "\n" +
            "Longest distance: " + Mathf.RoundToInt(GameManager.instance.highScore.longestDistance) + "\n" +
            "Highest bird kill: " + GameManager.instance.highScore.highestBirdKill + "\n" +
            "Highest cat kill: " + GameManager.instance.highScore.highestCatKill + "\n" +
            "Highest obstacle dodged: " + GameManager.instance.highScore.highestObstacleDodged + "\n" +
            "Highest bird dodged: " + GameManager.instance.highScore.highestBirdDodged + "\n" +
            "Highest cat dodged: " + GameManager.instance.highScore.highestCatDodged;
        highScoreText.enabled = true;
        retryButton.SetActive(true);
    }
}
