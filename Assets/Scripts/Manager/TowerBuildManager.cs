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
    private List<Tower> buildedTowers = new List<Tower>();

    private GameManager gameManager;
    private TowerUIManager towerUIManager;
    private void Start()
    {
    }
    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        towerUIManager = FindObjectOfType<TowerUIManager>();
    }
    public void ToggleBuildingMode()
    {
        isBuildingMode = !isBuildingMode;
        buildModeOffButton.SetActive(isBuildingMode);
        if(isBuildingMode)
        {
            towerUIManager.ShowBuildableTiles();
        }
        else
        {
            towerUIManager.HideBuildableTiles();
        }
        Debug.Log($"Building mode: {(isBuildingMode ? "ON" : "OFF")}");
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
            if(IsPointerOverUIObject())
            {
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildableLayer))
            {
                BuildableObject buildable = hit.collider.GetComponent<BuildableObject>();
                if (buildable != null)
                {
                    if (isBuildingMode)
                    {
                        if (!buildable.isOccupied && gameManager.MinusResource(ResourceType.Mineral, buildCost))
                        {
                            buildable.PlaceTower(towerPrefab, DataTableManager.TowerTable.GetRandomByRarity(1));
                            Tower buildedTower = buildable.currentTower;
                            buildedTowers.Add(buildedTower);
                        }
                        else
                        {
                            Debug.Log("미네랄이 부족합니다.");
                        }
                    }
                    else
                    {
                        if (buildable.isOccupied)
                        {
                            towerUIManager.DisplayTowerUI(buildable);
                        }
                        else
                        {
                            towerUIManager.HideUI();
                        }
                    }
                }
            }
            else
            {
                towerUIManager.HideUI();
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

    internal void SellTower(Tower selectedTower)
    {
        gameManager.AddResource(ResourceType.Mineral, selectedTower.sellPrice);
        RemoveTower(selectedTower);
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.GetTouch(0).position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
