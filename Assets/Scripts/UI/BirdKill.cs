using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdKill : MonoBehaviour {
    Text killCountText;
    float killCount;

	void Start () {
        killCountText = GetComponent<Text>();
	}
	
	void Update () {
        killCount = Mathf.RoundToInt(Player.Instance.GetStats("birdKill"));
        killCountText.text = "Bird Kill: " + killCount.ToString();
    }
}
