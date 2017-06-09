using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Distance : MonoBehaviour {
    Text distanceText;
    float distance;

	void Start () {
        distanceText = GetComponent<Text>();
	}
	
	void Update () {
        distance = Mathf.RoundToInt(Player.Instance.GetStats("distance"));
        distanceText.text = "Distance: " + Mathf.RoundToInt(distance).ToString();
    }
}
