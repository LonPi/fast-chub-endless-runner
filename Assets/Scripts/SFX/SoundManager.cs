using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource 
        playerFxSource,
        enemyFxSource,
        musicSource;
    public AudioClip
        jump,
        defMagicBall,
        defShuriken,
        defArrow,
        attack,
        deadProjectile,
        deadObstacle,
        deadMage,
        deadNinja,
        deadArcher;

    public static SoundManager instance = null;       
    public float lowPitchRange = 0.95f;              
    public float highPitchRange = 1.05f;            


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayerPlaySingle(AudioClip clip)
    {
        playerFxSource.clip = clip;
        playerFxSource.Play();
    }

    public void PlayerPlayOneShot(AudioClip clip)
    {
        playerFxSource.PlayOneShot(clip);
    }

    public void EnemyPlaySingle(AudioClip clip)
    {
        enemyFxSource.clip = clip;
        enemyFxSource.Play();
    }

    public void EnemyPlayOneShot(AudioClip clip)
    {
        enemyFxSource.PlayOneShot(clip);
    }
}