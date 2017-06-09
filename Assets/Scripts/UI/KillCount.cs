using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCount : MonoBehaviour {
    Text killCountText;
    float killCount;

	void Start () {
        killCountText = GetComponent<Text>();
	}
	
	void Update () {
        killCount += Mathf.RoundToInt(Player.Instance.GetStats("killCount"));
        killCountText.text = "Kill Count: " + killCount.ToString();
    }
}
