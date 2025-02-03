using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private TowerBuildManager towerBuildManager;
    private Dictionary<TowerType, int> upgradeLevels = new Dictionary<TowerType, int>();
    void Start()
    {
        towerBuildManager = FindObjectOfType<TowerBuildManager>();
        if (towerBuildManager == null)
        {
            Debug.LogError("TowerBuildManager를 찾을 수 없습니다!");
        }

        foreach (TowerType type in System.Enum.GetValues(typeof(TowerType)))
        {
            upgradeLevels[type] = 0;
        }
    }

    public void UpgradeTower(TowerType targetType)
    {
        if (towerBuildManager == null)
        {
            return;
        }

        int currentLevel = upgradeLevels[targetType];

        UpgradeData upgradeData = DataTableManager.UpgradeTable.Get(currentLevel + 1);
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.MinusResource(ResourceType.Gas, upgradeData.Gas_Amount))
        {
            upgradeLevels[targetType]++;
            List<Tower> matchingTowers = towerBuildManager.GetTowersByType(targetType);

            foreach (Tower tower in matchingTowers)
            {
                tower.ApplyUpgrade(upgradeLevels[targetType]);
            }
        }
        else
        {
            Debug.Log("가스가 부족합니다.");
        }
    }
    public int GetUpgradeLevel(TowerType type)
    {
        return upgradeLevels.ContainsKey(type) ? upgradeLevels[type] : 0;
    }

    public int GetNextUpgradeCost(TowerType type)
    {
        int nextLevel = GetUpgradeLevel(type) + 1;
        UpgradeData upgradeData = DataTableManager.UpgradeTable.Get(nextLevel);
        return upgradeData != null ? upgradeData.Gas_Amount : -1;
    }

    public bool CanUpgrade(TowerType type)
    {
        return upgradeLevels[type] < DataTableManager.UpgradeTable.GetDataCount();
    }
}
