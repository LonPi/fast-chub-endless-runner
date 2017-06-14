using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    Text scoreText;
    float score;
    float highscore;
    bool newhighscore;

	void Start () {
        scoreText = GetComponent<Text>();
        highscore = GameManager.instance.highScore.highestScore;
        newhighscore = false;
    }
	
	void Update () {
        score = Mathf.RoundToInt(Player.Instance.GetStats("score"));
        scoreText.text = "Score: " + score.ToString();
        if (score > highscore && highscore > 100 && !newhighscore)
        {
            SoundManager.Instance.UiPlayOneShot(SoundManager.Instance.highScore);
            newhighscore = true;
        }
    }
}
