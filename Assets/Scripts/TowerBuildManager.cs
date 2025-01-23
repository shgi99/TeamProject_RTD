using System.Collections.Generic;
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    public GameObject towerPrefab;
    public LayerMask buildableLayer;

    private bool isBuildingMode = false;
    private bool isMergingMode = false;
    private List<Tower> buildedTowers = new List<Tower>();
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
        if ((!isBuildingMode && !isMergingMode) || Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log($"{touch.position}");
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildableLayer))
            {
                BuildableObject buildable = hit.collider.GetComponent<BuildableObject>();
                if (buildable != null)
                {
                    if (isBuildingMode)
                    {
                        buildable.PlaceTower(towerPrefab, DataTableManager.TowerTable.GetRandomByRarity(1));
                        Tower buildedTower = buildable.currentTower;
                        buildedTowers.Add(buildedTower);
                    }
                    else if(isMergingMode)
                    {
                        MergingTower(buildable);
                    }
                }
            }
        }
    }
    public void MergingTower(BuildableObject buildableObject)
    {
        Tower selectedTower = buildableObject.currentTower;

        List<Tower> matchingTowers = GetMatchingTowers(selectedTower);

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
            if(tower == selectedTower)
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
}
