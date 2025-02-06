using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Types;
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

        quitButton.onClick.AddListener(() => upgradePanel.SetActive(false));
        humanUpgradeButton.onClick.AddListener(() => UpgradeTower(TowerType.Human));
        machineUpgradeButton.onClick.AddListener(() => UpgradeTower(TowerType.Machine));
        monsterUpgradeButton.onClick.AddListener(() => UpgradeTower(TowerType.Monster));
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

        humanUpgradeCostText.text = $"{humanCost}";
        machineUpgradeCostText.text = $"{machineCost}";
        monsterUpgradeCostText.text = $"{monsterCost}";

        humanUpgradeButton.interactable = upgradeManager.CanUpgrade(TowerType.Human);
        machineUpgradeButton.interactable = upgradeManager.CanUpgrade(TowerType.Machine);
        monsterUpgradeButton.interactable = upgradeManager.CanUpgrade(TowerType.Monster);
    }
}
