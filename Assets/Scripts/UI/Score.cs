using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    Text scoreText;
    float score;

	void Start () {
        scoreText = GetComponent<Text>();
	}
	
	void Update () {
        score = Mathf.RoundToInt(Player.Instance.GetStats("score"));
        scoreText.text = "Score: " + score.ToString();
    }
}
