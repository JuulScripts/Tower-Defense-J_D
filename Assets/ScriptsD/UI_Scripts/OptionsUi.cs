using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;

    private void Start()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = _volumeSlider.value;

        _volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    private void OnDestroy()
    {
        _volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }
}