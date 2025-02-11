using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    public GameObject towerInfoPanel;
    public Image attackRange;
    public Button mergeButton;
    private BuildableObject selectedTile;
    private List<BuildableObject> buildableTiles;
    private TowerBuildManager towerBuildManager;
    private Tower selectedTower;
    // Start is called before the first frame update
    void Start()
    {
        buildableTiles = new List<BuildableObject>(FindObjectsOfType<BuildableObject>());
        towerBuildManager = FindObjectOfType<TowerBuildManager>();
    }

    public void DisplayTowerUI(Tower tower, bool canMerge = true)
    {
        if (tower == null) return;

        selectedTower = tower;
        float rangeSize = tower.attackRange * 2;  // 크기 조정 필요
        attackRange.rectTransform.sizeDelta = new Vector3(rangeSize, rangeSize);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(tower.transform.position);
        screenPosition.x = Mathf.Clamp(screenPosition.x, 50, Screen.width - 50);
        screenPosition.y = Mathf.Clamp(screenPosition.y, 50, Screen.height - 50);

        if(tower.towerRarity == TowerRarity.God)
        {
            mergeButton.gameObject.SetActive(false);
        }
        else
        {
            mergeButton.gameObject.SetActive(true);
            mergeButton.interactable = canMerge;
            if(canMerge)
            {
                mergeButton.image.color = new Color(mergeButton.image.color.r, mergeButton.image.color.b, mergeButton.image.color.g, 1f);
                var mergeButtonText = mergeButton.GetComponentInChildren<TextMeshProUGUI>();
                mergeButtonText.color = new Color(mergeButtonText.color.r, mergeButtonText.color.g, mergeButtonText.color.b, 0.7f);
            }
            else
            {
                mergeButton.image.color = new Color(mergeButton.image.color.r, mergeButton.image.color.b, mergeButton.image.color.g, 0.3f);
                var mergeButtonText = mergeButton.GetComponentInChildren<TextMeshProUGUI>();
                mergeButtonText.color = new Color(mergeButtonText.color.r, mergeButtonText.color.g, mergeButtonText.color.b, 0.3f);
            }
        }
        towerInfoPanel.transform.position = screenPosition;
        towerInfoPanel.GetComponentInChildren<UITowerInfo>().SetTowerInfo(tower);
        towerInfoPanel.SetActive(true);
    }
    public void HideTowerUI()
    {
        towerInfoPanel.SetActive(false);
        selectedTower = null;
    }
    public void MergeTower()
    {
        if (selectedTower == null) return;

        TowerBuildManager buildManager = FindObjectOfType<TowerBuildManager>();
        buildManager.MergingTower(selectedTower.GetComponentInParent<BuildableObject>());

        HideTowerUI();
    }

    public void SellTower()
    {
        if (selectedTower == null) return;

        TowerBuildManager buildManager = FindObjectOfType<TowerBuildManager>();
        buildManager.SellTower(selectedTower);

        HideTowerUI();
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
