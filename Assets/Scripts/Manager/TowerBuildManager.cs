using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    public GameObject towerPrefab;
    public LayerMask buildableLayer;

    private bool isBuildingMode = false;
    private bool isMergingMode = false;
    private int buildCost = 100;
    private List<Tower> buildedTowers = new List<Tower>();
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }
    public void ToggleBuildingMode()
    {
        isBuildingMode = !isBuildingMode;
        isMergingMode = false;
        Debug.Log($"Building mode: {(isBuildingMode ? "ON" : "OFF")}");
    }
    public void ToggleMergingMode()
    {
        isMergingMode = !isMergingMode;
        isBuildingMode = false;
        Debug.Log($"Merging mode: {(isMergingMode ? "ON" : "OFF")}");
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
        if ((!isBuildingMode && !isMergingMode) || Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildableLayer))
            {
                BuildableObject buildable = hit.collider.GetComponent<BuildableObject>();
                if (buildable != null)
                {
                    if (isBuildingMode)
                    {
                        if(gameManager.MinusResource(ResourceType.Mineral, buildCost))
                        {
                            buildable.PlaceTower(towerPrefab, DataTableManager.TowerTable.GetRandomByRarity(1));
                            Tower buildedTower = buildable.currentTower;
                            buildedTowers.Add(buildedTower);
                            ToggleBuildingMode();
                        }
                        else
                        {
                            Debug.Log("미네랄이 부족합니다.");
                        }
                    }
                    else if(isMergingMode)
                    {
                        if(buildable.currentTower != null)
                        {
                            MergingTower(buildable);
                            ToggleMergingMode();
                        }
                    }
                }
            }
        }
    }
    public void MergingTower(BuildableObject buildableObject)
    {
        Tower selectedTower = buildableObject.currentTower;

        List<Tower> matchingTowers = GetMatchingTowers(selectedTower);
        matchingTowers.RemoveAll(tower => tower == null || !tower.gameObject.activeInHierarchy);
        if (GetMatchingTowers(selectedTower).Count < 2)
        {
            Debug.Log("No Same Tower.");
            return;
        }

        Tower closestTower = FindClosestTower(selectedTower, matchingTowers);
        if(closestTower == null)
        {
            return;
        }

        TowerRarity currentRarity = selectedTower.towerRarity;

        RemoveTower(selectedTower);
        RemoveTower(closestTower);

        buildableObject.PlaceTower(towerPrefab, DataTableManager.TowerTable.GetUpgradeRarity((int)currentRarity));

        Tower newTower = buildableObject.currentTower;
        buildedTowers.Add(newTower);
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
}
