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

    public GameObject gameOverPanel;
    public GameObject useTerazinPanel;
    public GameObject upgradePanel;
    public GameObject bossHpBarUI;

    public void SetRoundText(int currentRound)
    {
        roundText.text = $"{currentRound} ¶ó¿îµå";
    }
    public void SetGameOver()
    {
        gameOverPanel.SetActive(true);
    }
    public void ShowUpgradePanel()
    {
        upgradePanel.SetActive(true);
    }
    public void ShowUsingTerazinPanel()
    {
        useTerazinPanel.SetActive(true);
    }
    internal void UpdateResources()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        mineralText.text = gameManager.mineral.ToString();
        gasText.text = gameManager.gas.ToString();
        terazinText.text = gameManager.terazin.ToString();
        lifeText.text = gameManager.life.ToString();

        buildTowerCostText.color = gameManager.mineral < gameManager.costBuildTower ? Color.red : Color.white;
        buyGasCostText.color = gameManager.mineral < gameManager.costMineralToGas ? Color.red : Color.white;
    }
    public void ShowBossHpBar()
    {
        bossHpBarUI.SetActive(true);
    }
    public void HideBossHpBar()
    {
        bossHpBarUI.SetActive(false);
    }
}
