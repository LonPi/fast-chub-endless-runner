using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdDodged : MonoBehaviour {
    Text enemyDodgedText;
    float enemyDodgedCount;

	void Start () {
        enemyDodgedText = GetComponent<Text>();
	}
	
	void Update () {
        enemyDodgedCount = Mathf.RoundToInt(Player.Instance.GetStats("birdDodge"));
        enemyDodgedText.text = "Bird Dodge: " + enemyDodgedCount.ToString();
    }
}
