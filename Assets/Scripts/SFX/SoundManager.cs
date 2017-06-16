using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource 
        playerFxSource,
        enemyFxSource,
        uiFxSource,
        miscFxSource,
        buffFxSource,
        musicSource;
    public AudioClip
        jumpAtk,
        groundAtk,
        deadCat,
        deadBird,
        explodeCat,
        explodeBird,
        knockTree,
        highScore,
        fastPace,
        playerDead,
        jump,
        bubbleStart,
        bubbleEnd,
        summonHorde;

    public static SoundManager Instance = null;       
    public float lowPitchRange = 0.95f;              
    public float highPitchRange = 1.05f;            


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayerPlayOneShot(AudioClip clip)
    {
        playerFxSource.PlayOneShot(clip);
    }

    public void EnemyPlayOneShot(AudioClip clip)
    {
        enemyFxSource.PlayOneShot(clip);
    }

    public void UiPlayOneShot(AudioClip clip)
    {
        uiFxSource.PlayOneShot(clip);
    }

    public void MiscPlayOneShot(AudioClip clip)
    {
        miscFxSource.PlayOneShot(clip);
    }

    public void BuffPlayOneShot(AudioClip clip)
    {
        buffFxSource.PlayOneShot(clip);
    }
}