using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject turretShopPanel;
    [SerializeField] private GameObject turretUIPanel;
    [SerializeField] private GameObject achievementPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject shopButtonPanel;

    [Header("Text")] 
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI turretLevelText;
    [SerializeField] private TextMeshProUGUI totalCoinsText;
    [SerializeField] private TextMeshProUGUI lifesText;
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI gameOverTotalCoinsText;
    
    private Turret currentTurretSelected;

    private void Update()
    {
        totalCoinsText.text = CurrencySystem.Instance.TotalCoins.ToString();
        lifesText.text = LevelManager.Instance.TotalLives.ToString();
        currentWaveText.text = "Wave "+LevelManager.Instance.CurrentWave;
    }

    public void SlowTime()
    {
        Time.timeScale = 0.5f;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    public void FastTime()
    {
        Time.timeScale = 2f;
    }

    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameOverTotalCoinsText.text = CurrencySystem.Instance.TotalCoins.ToString();
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void OpenAchievementPanel(bool status)
    {
        achievementPanel.SetActive(status);
    }

    public void CloseNodeUIPanel()
    {
        currentTurretSelected.CloseAttackRangeSprite();
        turretUIPanel.SetActive(false);
    }
    
    public void UpgradeTurret()
    {
        currentTurretSelected.TurretUpgrade.UpgradeTurret();
        UpdateUpgradeText();
        UpdateTurretLevel();
        UpdateSellValue();
    }

    public void SellTurret()
    {
        currentTurretSelected.SellTurret();
        currentTurretSelected = null;
        turretUIPanel.SetActive(false);
    }
    
    private void ShowTurretUI()
    {
        turretUIPanel.SetActive(true);
        UpdateUpgradeText();
        UpdateTurretLevel();
        UpdateSellValue();
    }

    private void UpdateUpgradeText()
    {
        upgradeText.text = currentTurretSelected.TurretUpgrade.UpgradeCost.ToString();
    }

    private void UpdateTurretLevel()
    {
        turretLevelText.text = $"Level {currentTurretSelected.TurretUpgrade.Level}";
    }

    private void UpdateSellValue()
    {
        int sellAmount = currentTurretSelected.TurretUpgrade.GetSellValue();
        sellText.text = sellAmount.ToString();
    }
    
    public void ToggleTurretShopView()
    {
        turretShopPanel.SetActive(!(turretShopPanel.activeSelf));
        shopButtonPanel.SetActive(!(shopButtonPanel.activeSelf));
    }

    private void TurretSelected(Turret turretSelected)
    {
        currentTurretSelected = turretSelected;

        ShowTurretUI();
    }
    
    private void OnEnable()
    {
        Turret.OnTurretSelected += TurretSelected;
    }

    private void OnDisable()
    {
        Turret.OnTurretSelected -= TurretSelected;
    }
}
