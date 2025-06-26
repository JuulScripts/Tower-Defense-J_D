using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Effects")]
    public AudioClip attackSound;
    public AudioClip coinSound;
    public AudioClip purchaseSound;
    public AudioClip placementSound;
    public AudioClip endpointDamageSound;
    public AudioClip enemyDeathSound;
    public AudioClip upgradeSound;
    public AudioClip levelCompleteSound;
    public AudioClip lossSound;
    public AudioClip victorySound;

    private float volume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            volume = PlayerPrefs.GetFloat("Volume", 1f);
            ApplyVolume();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip) // Plays a sound effect if the clip is not null and the audio source is assigned
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    public void SetVolume(float value) // Sets the volume, clamps it between 0 and 1, saves it to PlayerPrefs, and applies it to the audio source
    {
        volume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("Volume", volume);
        ApplyVolume();
    }

    public float GetVolume() // Returns the current volume level
    {
        return volume;
    }

    private void ApplyVolume() // Applies the saved volume to the audio source
    {
        if (sfxSource != null)
            sfxSource.volume = volume;
    }
}
