using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileDodged : MonoBehaviour {
    Text projectileDodgedText;
    float projectileDodgedCount;

	void Start () {
        projectileDodgedText = GetComponent<Text>();
	}
	
	void Update () {
        projectileDodgedCount = Mathf.RoundToInt(Player.Instance.GetStats("projectileDodge"));
        projectileDodgedText.text = "Projectiles: " + projectileDodgedCount.ToString();
    }
}
