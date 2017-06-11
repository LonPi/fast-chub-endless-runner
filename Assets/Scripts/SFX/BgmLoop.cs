using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmLoop : MonoBehaviour {

    public AudioClip bgmStart;
    public AudioClip bgmLoop;
    AudioSource _audio;

	void Start () {
        _audio = GetComponent<AudioSource>();
        StartCoroutine(playBgm());
	}
	
	IEnumerator playBgm()
    {
        _audio.clip = bgmStart;
        _audio.Play();
        yield return new WaitForSeconds(_audio.clip.length);
        _audio.clip = bgmLoop;
        _audio.Play();
    }
}
