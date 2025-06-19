using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{ 
    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _backButton;

    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _optionsPanel;

    void Start()
    {
        _startButton.onClick.AddListener(StartGame);
        _optionsButton.onClick.AddListener(OpenOptions);
        _quitButton.onClick.AddListener(QuitGame);
        if (_backButton != null)
            _backButton.onClick.AddListener(BackToMainMenu);
    }

    void StartGame()
    {
        _mainMenuPanel.SetActive(false);
        SceneManager.LoadScene("Level1Scene");
    }

    void OpenOptions()
    {
        _mainMenuPanel.SetActive(false);
        _optionsPanel.SetActive(true);
        Debug.Log("Options geopend");
    }

    void BackToMainMenu()
    {
        _optionsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
        Debug.Log("Terug naar hoofdmenu");
    }

    void QuitGame()
    {
        Debug.Log("Quit Game clicked!");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}