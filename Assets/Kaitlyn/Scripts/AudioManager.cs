using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip dayMusic;
    public AudioClip nightMusic;
    private time_manager tm; //ill get rid of ts latr dont wrry
    [Space(5f)]
    //public AudioSource enemySfxSource;
    //we gon want enemies to have their own personal source for directional audio right 
    public AudioClip enemyMovement;
    public AudioClip enemyAttack;
    [Space(3f)]
    public AudioSource playerSfxSource;
    public AudioClip playerWalk;
    public AudioClip playerAttack;
    public AudioClip playerHurt;
    [Space(3f)]
    public AudioSource environmentSfxSource;
    [Space(3f)]
    public AudioClip Bear5Walk;
    public AudioClip Bear5Punch;
    public AudioClip Bear5SW;

    public static AudioManager instance;

    void Start()
    {
        tm = GameObject.FindAnyObjectByType<time_manager>();

        if(instance == null)
        {
            instance = this;
        }
    }


    void Update()
    {
        BackgroundMusic();
    }

    private void BackgroundMusic()
    {
        if(musicSource == null) return;
        if(tm == null) return;

        if (tm.isDay)
        {
            if(musicSource.clip == dayMusic) return;

            musicSource.clip = dayMusic;

            musicSource.Play();
        }
        else
        {
            if (musicSource.clip == nightMusic) return;

            musicSource.clip = nightMusic;

            musicSource.Play();
        }
    }

    public void PlayPlayerSfx(AudioClip clip)
    {
        if(clip == null) return;

        playerSfxSource.clip = clip;

        if(clip == playerWalk)
        {
            playerSfxSource.loop = true;
        }
        else
        {
            playerSfxSource.loop = false; 
        }

        playerSfxSource.Play();
    }

    public void PlayEnvironmentSfx(AudioClip clip)
    {
        if (clip == null) return;

        environmentSfxSource.clip = clip;
        environmentSfxSource.Play();
    }
}
