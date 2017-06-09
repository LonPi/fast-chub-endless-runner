using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleDodged : MonoBehaviour {
    Text obstacleDodgedText;
    float obstacleDodgeCount;

	void Start () {
        obstacleDodgedText = GetComponent<Text>();
	}
	
	void Update () {
        obstacleDodgeCount = Mathf.RoundToInt(Player.Instance.GetStats("obstacleDodge"));
        obstacleDodgedText.text = "Obstacles: " + obstacleDodgeCount.ToString();
    }
}
