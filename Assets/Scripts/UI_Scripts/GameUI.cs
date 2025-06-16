using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("Tower Buttons")]
    [SerializeField] private Button[] _towerButtons;
    [SerializeField] private TextMeshProUGUI[] _priceTexts;
    [SerializeField] private int[] _towerCosts;

    [Header("Top Bar")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _killsText;

    private int _playerMoney = 10;
    private int _kills = 0;
    private float _timer = 0f;

    void Start()
    {
        UpdateTopBar();
        UpdateTowerButtons();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        _timerText.text = "Time: " + FormatTime(_timer);
    }

    void UpdateTopBar()
    {
        _moneyText.text = $"[${_playerMoney}]";
        _killsText.text = $"[Kills:{_kills}]";
    }

    void UpdateTowerButtons()
    {
        int count = Mathf.Min(_towerButtons.Length, _priceTexts.Length, _towerCosts.Length);
        for (int i = 0; i < count; i++)
        {
            _priceTexts[i].text = $"${_towerCosts[i]}";
            bool canAfford = _playerMoney >= _towerCosts[i];

            var colors = _towerButtons[i].colors;
            colors.normalColor = canAfford ? Color.white : Color.gray;
            _towerButtons[i].colors = colors;

            _towerButtons[i].interactable = canAfford;
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void AddMoney(int amount)
    {
        _playerMoney += amount;
        UpdateTopBar();
        UpdateTowerButtons();
    }

    public void AddKill()
    {
        _kills++;
        UpdateTopBar();
    }
}
