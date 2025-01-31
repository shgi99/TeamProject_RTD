using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI lifeText;
    public GameObject gameOverPanel;
    public GameObject upgradePanel;

    public void SetRoundText(int currentRound)
    {
        roundText.text = $"{currentRound} ¶ó¿îµå";
    }
    public void SetLifeText(int currentlife)
    {
        lifeText.text = currentlife.ToString();
    }
    public void SetGameOver()
    {
        gameOverPanel.SetActive(true);
    }
    public void ShowUpgradePanel()
    {
        upgradePanel.SetActive(true);
    }
}
