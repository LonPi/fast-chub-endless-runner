using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDodged : MonoBehaviour {
    Text enemyDodgedText;
    float enemyDodgedCount;

	void Start () {
        enemyDodgedText = GetComponent<Text>();
	}
	
	void Update () {
        enemyDodgedCount = Mathf.RoundToInt(Player.Instance.GetStats("enemyDodge"));
        enemyDodgedText.text = "Enemies: " + enemyDodgedCount.ToString();
    }
}
