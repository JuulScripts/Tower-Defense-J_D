using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        _volumeSlider.value = savedVolume;

        _volumeSlider.onValueChanged.AddListener(SetVolume);
        SoundManager.Instance.SetVolume(savedVolume);
    }

    private void SetVolume(float value)
    {
        SoundManager.Instance.SetVolume(value);
    }

    private void OnDestroy()
    {
        _volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }
}