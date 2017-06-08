using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class volumeMute : MonoBehaviour {
    public float setVolume;
    public Sprite mute;
    public Sprite unmute;
    private Button btn;
    private bool muted;

    // Use this for initialization
    void Start () {
        muted = false;
        btn = gameObject.GetComponent<Button>();
        //btn.onClick.AddListener(TaskOnClick);
        setVolume = 1;
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TaskOnClick();
        }
    }
    
    public void TaskOnClick()
    {
        if (muted)
        {
            AudioListener.volume = setVolume;
            btn.image.overrideSprite = unmute;
            muted = false;
        }
        else
        {
            AudioListener.volume = 0;
            btn.image.overrideSprite = mute;
            muted = true;
        }
    }
}
