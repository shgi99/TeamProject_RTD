using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI roundText;

    public TextMeshProUGUI mineralText;
    public TextMeshProUGUI gasText;
    public TextMeshProUGUI terazinText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI buildTowerCostText;
    public TextMeshProUGUI buyGasCostText;
    public TextMeshProUGUI playTimerText;

    public GameObject gameClearPanel;
    public GameObject gameOverPanel;
    public GameObject useTerazinPanel;
    public GameObject upgradePanel;
    public GameObject questPanel;
    public GameObject bossHpBarUI;
    public GameObject pausePanel;
    public GameObject guidePanel;
    public GameObject cheatPanel;
    public GameObject optionPanel;
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void SetRoundText(int currentRound)
    {
        roundText.text = $"{currentRound} 라운드";
    }
    public void SetGameOver()
    {
        gameOverPanel.SetActive(true);
    }
    public void ShowUpgradePanel()
    {
        SoundManager.Instance.PlayButtonTouch();
        upgradePanel.SetActive(true);
    }
    public void ShowUsingTerazinPanel()
    {
        SoundManager.Instance.PlayButtonTouch();
        useTerazinPanel.SetActive(true);
    }
    public void ShowGuidePanel()
    {
        guidePanel.SetActive(true);
    }
    internal void UpdateResources()
    {
        mineralText.text = gameManager.mineral.ToString();
        gasText.text = gameManager.gas.ToString();
        terazinText.text = gameManager.terazin.ToString();
        lifeText.text = gameManager.life.ToString();

        buildTowerCostText.color = gameManager.mineral < gameManager.costBuildTower ? Color.red : Color.white;
        buyGasCostText.color = gameManager.mineral < gameManager.costMineralToGas ? Color.red : Color.white;

        // useTerazinPanel.GetComponent<UIUsingTerazin>().UpdateResource();
        // upgradePanel.GetComponent<UIUpgrade>().UpdateUpgradeUI();
    }
    public void ShowBossHpBar()
    {
        bossHpBarUI.SetActive(true);
    }
    public void HideBossHpBar()
    {
        bossHpBarUI.SetActive(false);
    }
    public void OnClickPauseButton()
    {
        SoundManager.Instance.PlayButtonTouch();
        pausePanel.SetActive(true);
        gameManager.TogglePause();
    }
    public void OnClickResumeButton()
    {
        SoundManager.Instance.PlayButtonTouch();
        pausePanel.SetActive(false);
        gameManager.TogglePause();
    }
    public void OnClickCheatButton()
    {
        SoundManager.Instance.PlayButtonTouch();
        cheatPanel.SetActive(true);
    }
    public void UpdatePlayTime(float playTime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(playTime);
        playTimerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
    public void ShowGameClearPanel()
    {
        gameClearPanel.SetActive(true);
    }
    public void ShowQuestPanel()
    {
        questPanel.SetActive(true);
    }

    public void ShowOptionPanel()
    {
        optionPanel.SetActive(true);
    }
}
