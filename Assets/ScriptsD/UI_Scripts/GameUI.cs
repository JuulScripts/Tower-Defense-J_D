using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

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
    
    [Header("Level Complete UI")]
    [SerializeField] private GameObject _levelCompleteText;

    [Header("Units to Place")]
    [SerializeField] private GameObject[] _unitPrefabs;

    [Header("Upgrade System")]
    [SerializeField] private Transform _upgradePanel;
    [SerializeField] private GameObject _upgradeButtonPrefab;

    [Header("Unit Upgrade Sprites")]
    [SerializeField] private Sprite[] unitSprites;
    [SerializeField] private string[] unitSpriteNames;
    [SerializeField] private int[] _upgradeCosts;

    [Header("Endpoint UI")]
    [SerializeField] private TextMeshProUGUI _endpointHpText;

    private Dictionary<string, Sprite> unitSpriteDict;
    private int _playerMoney = 15;
    private int _kills = 0;
    private float _timer = 0f;
    private Endpoint _endpoint;

    void Awake()
    {
        unitSpriteDict = new Dictionary<string, Sprite>();
        for (int i = 0; i < unitSpriteNames.Length; i++)
        {
            if (!unitSpriteDict.ContainsKey(unitSpriteNames[i]))
            {
         
                unitSpriteDict.Add(unitSpriteNames[i], unitSprites[i]);
        
            }
        }
    }

    void Start()
    {
        _endpoint = FindFirstObjectByType<Endpoint>();

        UpdateTopBar();
        UpdateTowerButtons();
        PlacementSystem.OnUnitPlaced += OnUnitPlaced;
        Unit.OnUnitUpgraded += OnUnitUpgraded;
    }

    void Update()
    {
        UpdateTopBar();
        UpdateTowerButtons();
        RefreshUpgradeButtons();
    }

    void UpdateTopBar()
    {
        _playerMoney = PlayerHandling.Player.money;
        _moneyText.text = $"[${_playerMoney}]";
        _killsText.text = $"[Kills:{_kills}]";
        _timer += Time.deltaTime;
        _timerText.text = "Time: " + FormatTime(_timer);
        
        if (_endpoint != null)
        {
            _endpointHpText.text = $"Endpoint HP: {_endpoint.CurrentHealth}";
        }

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

    public void TryPlaceUnit(int index)
    {
        if (index < 0 || index >= _towerCosts.Length) return;

        int cost = _towerCosts[index];
        if (_playerMoney >= cost)
        {
            _playerMoney -= cost;
            UpdateTopBar();
            UpdateTowerButtons();

            GameObject prefabToPlace = _unitPrefabs[index];
            PlacementSystem.start_placing(prefabToPlace);
            PlayerHandling.DecreaseMoney(cost);
            SoundManager.Instance.PlaySFX(SoundManager.Instance.purchaseSound);
        }
        else
        {
            Debug.Log("Niet genoeg geld!");
        }
    }

    private void OnUnitPlaced(GameObject unitGO)
    {
        Unit unit = unitGO.GetComponent<Unit>();
        if (unit != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.placementSound);
            RegisterUpgradeButton(unit);
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
        UpdateTopBar();
        UpdateTowerButtons();
        RefreshUpgradeButtons();
        SoundManager.Instance.PlaySFX(SoundManager.Instance.coinSound);
    }

    public void AddKill()
    {
        _kills++;
        UpdateTopBar();
    }

    private void RegisterUpgradeButton(Unit unit)
    {

        if (unit == null || unit.upgradedunit == null)
        {
            Debug.Log($"{unit?.unitName ?? "Unknown unit"} has no further upgrades.");
            return;
        }

        GameObject newButtonGO = Instantiate(_upgradeButtonPrefab, _upgradePanel);
        Button btn = newButtonGO.GetComponent<Button>();
        TextMeshProUGUI btnText = newButtonGO.GetComponentInChildren<TextMeshProUGUI>();
        Image btnImage = newButtonGO.GetComponent<Image>();

        string nameKey = unit.unitName;
       
        if (unitSpriteDict.TryGetValue(nameKey, out Sprite sprite))
        {
            btnImage.sprite = sprite;
        }
        else
        {
            Debug.LogError($"No sprite found for unit: {nameKey} unit:{unit}");
        }

        int index = Array.IndexOf(unitSpriteNames, nameKey);
        int upgradeCost = (index >= 0 && index < _upgradeCosts.Length) ? _upgradeCosts[index] : 0;

        btnText.text = $"Upgrade {nameKey} (${upgradeCost})";
        btn.interactable = PlayerHandling.Player.money >= upgradeCost;

        btn.onClick.AddListener(() =>
        {
            if (unit != null && PlayerHandling.Player.money >= upgradeCost)
            {
                PlayerHandling.DecreaseMoney(upgradeCost);
                SoundManager.Instance.PlaySFX(SoundManager.Instance.upgradeSound);
                unit.Upgrade();
                Destroy(newButtonGO);
            }
            else
            {
                Debug.Log("Not enough money to upgrade!");
            }
        });
    }

    public void RefreshUpgradeButtons()
    {
        foreach (Transform child in _upgradePanel)
        {
            Button btn = child.GetComponent<Button>();
            TextMeshProUGUI btnText = child.GetComponentInChildren<TextMeshProUGUI>();

            if (btn != null && btnText != null)
            {
                string text = btnText.text;
                int costStart = text.IndexOf("($");
                int costEnd = text.IndexOf(")", costStart);
                if (costStart >= 0 && costEnd > costStart)
                {
                    string costString = text.Substring(costStart + 2, costEnd - costStart - 2);
                    if (int.TryParse(costString, out int cost))
                    {
                        btn.interactable = PlayerHandling.Player.money >= cost;
                    }
                }
            }
        }
    }
    private void OnUnitUpgraded(GameObject upgradedUnitGO)
    {
        Unit upgradedUnit = upgradedUnitGO.GetComponent<Unit>();
        if (upgradedUnit != null)
        {
            print(upgradedUnit);
            RegisterUpgradeButton(upgradedUnit);
        }
    }
    private void OnDestroy()
    {
        Unit.OnUnitUpgraded -= OnUnitUpgraded;
    }
    public void ShowLevelCompleteMessage()
    {
        if (_levelCompleteText != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.levelCompleteSound);
            _levelCompleteText.SetActive(true);
        }
    }
}