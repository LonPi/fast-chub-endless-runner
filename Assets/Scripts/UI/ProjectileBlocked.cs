using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileBlocked : MonoBehaviour {
    Text projectileBlockedText;
    float projectileBlockedCount;

	void Start () {
        projectileBlockedText = GetComponent<Text>();
	}
	
	void Update () {
        projectileBlockedCount = Mathf.RoundToInt(Player.Instance.GetStats("projectileBlock"));
        projectileBlockedText.text = "Deflected: " + projectileBlockedCount.ToString();
    }
}
