using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatKill : MonoBehaviour {
    Text killCountText;
    float killCount;

	void Start () {
        killCountText = GetComponent<Text>();
	}
	
	void Update () {
        killCount = Mathf.RoundToInt(Player.Instance.GetStats("catKill"));
        killCountText.text = "Cat Kill: " + killCount.ToString();
    }
}
