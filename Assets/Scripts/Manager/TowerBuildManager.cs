using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerBuildManager : MonoBehaviour
{
    public GameObject towerPrefab;
    public LayerMask buildableLayer;
    public GameObject buildModeOffButton;

    public bool isBuildingMode = false;
    private int buildCost = 100;
    public List<Tower> buildedTowers { get; private set; } = new List<Tower>();

    private GameManager gameManager;
    private QuestManager questManager;
    private TowerUIManager towerUIManager;
    private TowerData selectedHeroTower;
    private void Start()
    {
    }
    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        towerUIManager = FindObjectOfType<TowerUIManager>();
        questManager = GetComponent<QuestManager>();
    }
    public void SelectTower(TowerData towerData)
    {
        selectedHeroTower = towerData;
        isBuildingMode = true;
        towerUIManager.ShowBuildableTiles();
    }
    public void ToggleBuildingMode()
    {
        if(!isBuildingMode)
        {
            isBuildingMode = true;
            towerUIManager.ShowBuildableTiles();
            towerUIManager.HideTowerUI();
        }
        else
        {
            isBuildingMode = false;
            towerUIManager.HideBuildableTiles();
        }
    }

    private void Update()
    {
        if(gameManager.isGameOver)
        {
            if (buildedTowers.Count > 0)
            {
                foreach (var tower in buildedTowers)
                {
                    tower.ClearBeforeDestroy();
                }
            }
            return;
        }
        if (Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            bool isBuildButton = false;
            bool isMergeButton = false;
            bool isSellButton = false;
            if (IsPointerOverUIObject(out isBuildButton, out isMergeButton, out isSellButton))
            {
                if (isBuildButton)
                {
                    return;
                }

                if (isMergeButton || isSellButton)
                {
                    if (isBuildingMode)
                    {
                        ToggleBuildingMode();
                    }
                    return;
                }

                if (isBuildingMode)
                {
                    ToggleBuildingMode();
                }
                towerUIManager.HideTowerUI();
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildableLayer))
            {
                if (isBuildingMode)
                {
                    ToggleBuildingMode();
                }
                towerUIManager.HideTowerUI();
                return;
            }

            BuildableObject buildable = hit.collider.GetComponent<BuildableObject>();
            if (buildable != null)
            {
                if (isBuildingMode)
                {
                    if (!buildable.isOccupied && gameManager.canUseResource(ResourceType.Mineral, buildCost))
                    {
                        if (selectedHeroTower != null && gameManager.canUseResource(ResourceType.Terazin, 1))
                        {
                            gameManager.MinusResource(ResourceType.Mineral, buildCost);
                            gameManager.MinusResource(ResourceType.Terazin, 1);
                            buildable.PlaceTower(towerPrefab, selectedHeroTower);
                            Tower buildedTower = buildable.currentTower;
                            var towerRarityText = DataTableManager.TowerTable.GetColoredRarityText(buildedTower.towerName, buildedTower.towerRarity);
                            FindObjectOfType<UILogPanel>().AddLog($"{towerRarityText} 설치!");
                            buildedTowers.Add(buildedTower);
                            questManager.CheckQuests();

                            selectedHeroTower = null;
                            isBuildingMode = false;
                            towerUIManager.HideBuildableTiles();
                        }
                        else
                        {
                            gameManager.MinusResource(ResourceType.Mineral, buildCost);
                            buildable.PlaceTower(towerPrefab, DataTableManager.TowerTable.GetRandomByRarity(1));
                            Tower buildedTower = buildable.currentTower;
                            var towerRarityText = DataTableManager.TowerTable.GetColoredRarityText(buildedTower.towerName, buildedTower.towerRarity);
                            FindObjectOfType<UILogPanel>().AddLog($"{towerRarityText} 설치!");
                            buildedTowers.Add(buildedTower);
                            questManager.CheckQuests();
                        }
                    }
                    else if(buildable.isOccupied)
                    {
                        ToggleBuildingMode();
                        towerUIManager.DisplayTowerUI(buildable.currentTower, GetMatchingTowers(buildable.currentTower).Count > 1);
                    }
                }
                else if (buildable.isOccupied)
                {
                    towerUIManager.DisplayTowerUI(buildable.currentTower, GetMatchingTowers(buildable.currentTower).Count > 1);
                }
                else
                {
                    towerUIManager.HideTowerUI();
                }
            }
        }
    }
    public void MergingTower(BuildableObject buildableObject)
    {
        Tower selectedTower = buildableObject.currentTower;
        TowerRarity currentRarity = selectedTower.towerRarity;
        if (currentRarity == TowerRarity.God)
        {
            return;
        }

        List<Tower> matchingTowers = GetMatchingTowers(selectedTower);
        matchingTowers.RemoveAll(tower => tower == null || !tower.gameObject.activeInHierarchy);
        if (matchingTowers.Count < 2)
        {
            Debug.Log("No Same Tower.");
            return;
        }

        Tower closestTower = FindClosestTower(selectedTower, matchingTowers);
        if(closestTower == null)
        {
            return;
        }

        RemoveTower(selectedTower);
        RemoveTower(closestTower);

        buildableObject.PlaceTower(towerPrefab, DataTableManager.TowerTable.GetUpgradeRarity((int)currentRarity));  

        Tower newTower = buildableObject.currentTower;
        var towerRarityText = DataTableManager.TowerTable.GetColoredRarityText(newTower.towerName, newTower.towerRarity);
        FindObjectOfType<UILogPanel>().AddLog($"{towerRarityText} 합성 완료!");
        buildedTowers.Add(newTower);
        questManager.CheckQuests();
    }

    public List<Tower> GetMatchingTowers(Tower selectedTower)
    {
        List<Tower> matchingTowers = new List<Tower>();
        
        foreach(var tower in buildedTowers)
        {
            if(tower.towerId == selectedTower.towerId)
            {
                matchingTowers.Add(tower);
            }
        }

        return matchingTowers;
    }

    public Tower FindClosestTower(Tower selectedTower, List<Tower> matchingTowers)
    {
        Tower closestTower = null;
        float closestDistance = float.MaxValue;

        foreach(var tower in matchingTowers)
        {
            if (tower == null || !tower.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (tower == selectedTower)
            {
                continue;
            }

            float distance = Vector3.Distance(selectedTower.transform.position, tower.transform.position);
            if(distance < closestDistance)
            {
                closestTower = tower;
                closestDistance = distance;
            }
        }

        return closestTower;
    }
    public void RemoveTower(Tower tower)
    {
        if (tower != null)
        {
            BuildableObject buildable = tower.GetComponentInParent<BuildableObject>();
            if (buildable != null)
            {
                buildable.RemoveTower();
            }
            buildedTowers.Remove(tower);
        }
    }

    public List<Tower> GetTowersByType(TowerType type)
    {
        return buildedTowers.FindAll(tower => tower.towerType == type);
    }

    internal void SellTower(Tower selectedTower)
    {
        gameManager.AddResource(ResourceType.Mineral, selectedTower.sellPrice);
        RemoveTower(selectedTower);
    }
    private bool IsPointerOverUIObject(out bool isBuildButton, out bool isMergeButton, out bool isSellButton)
    {
        isBuildButton = false;
        isMergeButton = false;
        isSellButton = false;
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.GetTouch(0).position
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name == "BuildButton")
            {
                isBuildButton = true;
                return true;
            }
            else if(result.gameObject.name == "MergeButton")
            {
                isMergeButton = true;
                return true;
            }
            else if (result.gameObject.name == "SellButton")
            {
                isSellButton = true;
                return true;
            }
        }
        return results.Count > 0;
    }
}
