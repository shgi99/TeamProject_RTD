using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : MonoBehaviour
{
    public GameObject upgradePanel;
    public TextMeshProUGUI humanLevelText;
    public TextMeshProUGUI machineLevelText;
    public TextMeshProUGUI monsterLevelText;

    public Button quitButton;
    public Button humanUpgradeButton;
    public Button machineUpgradeButton;
    public Button monsterUpgradeButton;

    private UpgradeManager upgradeManager;
    private GameManager gameManager;
    private void OnEnable()
    {
        gameManager = FindObjectOfType<GameManager>();
        upgradeManager = gameManager.GetComponent<UpgradeManager>();
        if (upgradeManager == null)
        {
            return;
        }
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => 
        { 
            SoundManager.Instance.PlayButtonTouch(); 
            upgradePanel.SetActive(false); 
        });
        humanUpgradeButton.onClick.RemoveAllListeners();
        humanUpgradeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonTouch();
            UpgradeTower(TowerType.Human);
        });
        machineUpgradeButton.onClick.RemoveAllListeners();
        machineUpgradeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonTouch();
            UpgradeTower(TowerType.Machine);
        });
        monsterUpgradeButton.onClick.RemoveAllListeners();
        monsterUpgradeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonTouch();
            UpgradeTower(TowerType.Monster);
        });
        UpdateUpgradeUI();
    }
    private void UpgradeTower(TowerType type)
    {
        upgradeManager.UpgradeTower(type);
        UpdateUpgradeUI();
    }
    public void UpdateUpgradeUI()
    {
        int humanLevel = upgradeManager.GetUpgradeLevel(TowerType.Human);
        int machineLevel = upgradeManager.GetUpgradeLevel(TowerType.Machine);
        int monsterLevel = upgradeManager.GetUpgradeLevel(TowerType.Monster);

        int humanCost = upgradeManager.GetNextUpgradeCost(TowerType.Human);
        int machineCost = upgradeManager.GetNextUpgradeCost(TowerType.Machine);
        int monsterCost = upgradeManager.GetNextUpgradeCost(TowerType.Monster);

        humanLevelText.text = $"Lv.{humanLevel}";
        machineLevelText.text = $"Lv.{machineLevel}";
        monsterLevelText.text = $"Lv.{monsterLevel}";

        var humanUpgradeCostText = humanUpgradeButton.GetComponentInChildren<TextMeshProUGUI>();
        var machineUpgradeCostText = machineUpgradeButton.GetComponentInChildren<TextMeshProUGUI>();
        var monsterUpgradeCostText = monsterUpgradeButton.GetComponentInChildren<TextMeshProUGUI>();

        humanUpgradeCostText.color = gameManager.gas < humanCost ? Color.red : Color.black;
        machineUpgradeCostText.color = gameManager.gas < machineCost ? Color.red : Color.black;
        monsterUpgradeCostText.color = gameManager.gas < monsterCost ? Color.red : Color.black;

        humanUpgradeCostText.text = upgradeManager.CanUpgrade(TowerType.Human) ? $"{humanCost}" : "풀강";
        machineUpgradeCostText.text = upgradeManager.CanUpgrade(TowerType.Machine) ? $"{machineCost}" : "풀강";
        monsterUpgradeCostText.text = upgradeManager.CanUpgrade(TowerType.Monster) ? $"{monsterCost}" : "풀강";

        humanUpgradeButton.interactable = upgradeManager.CanUpgrade(TowerType.Human);
        machineUpgradeButton.interactable = upgradeManager.CanUpgrade(TowerType.Machine);
        monsterUpgradeButton.interactable = upgradeManager.CanUpgrade(TowerType.Monster);
    }
}
