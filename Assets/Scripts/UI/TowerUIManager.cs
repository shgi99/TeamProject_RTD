using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    private BuildableObject selectedTile;
    private List<BuildableObject> buildableTiles;
    private TowerBuildManager towerBuildManager;
    // Start is called before the first frame update
    void Start()
    {
        buildableTiles = new List<BuildableObject>(FindObjectsOfType<BuildableObject>());
        towerBuildManager = FindObjectOfType<TowerBuildManager>();
    }

    public void DisplayTowerUI(BuildableObject buildable)
    {
        if (buildable != null)
        {
            if (selectedTile != null && selectedTile != buildable)
            {
                selectedTile.HideUI();
            }
            buildable.ShowUI();
            selectedTile = buildable;
        }
        else if (selectedTile != null)
        {
            selectedTile.HideUI();
            selectedTile = null;
        }
    }
    public void HideUI()
    {
        if (selectedTile != null)
        {
            selectedTile.HideUI();
            selectedTile = null;
        }
    }
    public void ShowBuildableTiles()
    {
        foreach (var tile in buildableTiles)
        {
            tile.ShowArrow();
        }
    }
    public void HideBuildableTiles()
    {
        foreach (var tile in buildableTiles)
        {
            tile.HideArrow();
        }
    }
}
