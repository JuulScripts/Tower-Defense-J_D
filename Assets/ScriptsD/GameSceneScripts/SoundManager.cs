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

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    public void SetVolume(float value)
    {
        volume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("Volume", volume);
        ApplyVolume();
    }

    public float GetVolume()
    {
        return volume;
    }

    private void ApplyVolume()
    {
        if (sfxSource != null)
            sfxSource.volume = volume;
    }
}
