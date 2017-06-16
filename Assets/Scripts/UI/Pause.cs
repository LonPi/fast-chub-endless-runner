using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public Sprite pause, unpause;
    Button pauseButton;    
    bool isPaused;

	void Start () {
        isPaused = false;
        pauseButton = GetComponent<Button>();
        pauseButton.image.overrideSprite = pause;
    }
	
	public void PressPause()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
            pauseButton.image.overrideSprite = unpause;
        }

        else
        {
            isPaused = false;
            Time.timeScale = 1;
            pauseButton.image.overrideSprite = pause;
        }
    }
}
