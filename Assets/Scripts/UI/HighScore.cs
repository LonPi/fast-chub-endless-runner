using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
    Text scoreText;
    int highscore;

	void Start () {
        scoreText = GetComponent<Text>();
        highscore = Mathf.RoundToInt(GameManager.instance.highScore.highestScore);
        if (highscore < 1f)
            gameObject.SetActive(false);
        else
            scoreText.text = "   Best: " + highscore.ToString();
    }
}
