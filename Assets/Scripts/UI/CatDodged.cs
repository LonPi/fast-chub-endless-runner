using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatDodged : MonoBehaviour {
    Text enemyDodgedText;
    float enemyDodgedCount;

	void Start () {
        enemyDodgedText = GetComponent<Text>();
	}
	
	void Update () {
        enemyDodgedCount = Mathf.RoundToInt(Player.Instance.GetStats("catDodge"));
        enemyDodgedText.text = "Cat Dodge: " + enemyDodgedCount.ToString();
    }
}
