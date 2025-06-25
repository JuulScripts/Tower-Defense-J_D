using UnityEngine;

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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
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
}
