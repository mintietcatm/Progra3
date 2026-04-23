using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Sounds")]
    public AudioClip walk;
    public AudioClip run;
    public AudioClip collect;
    public AudioClip powerup;
    public AudioClip powerupEnd;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}