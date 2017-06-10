using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    Text timerText;
    float time;

	void Start () {
        timerText = GetComponent<Text>();
        time = 0f;
	}
	
	void Update () {
        if (!Player.Instance.PlayerDeath()) time += Time.deltaTime;
        var minutes = Mathf.Floor(time / 60f);
        var seconds = Mathf.Floor(time % 60f);

        //update the label value
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
