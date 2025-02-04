using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildableObject : MonoBehaviour
{
    public bool isOccupied = false;

    public GameObject towerUIPanel;
    public GameObject towerInfoPanel;
    public GameObject displayRange;
    public Button mergeButton;
    public Button sellButton;

    public GameObject arrowUI; 
    public Tower currentTower {  get; private set; }
    private void Start()
    {
        if (towerInfoPanel != null && towerInfoPanel.GetComponent<Billboard>() == null)
        {
            towerInfoPanel.gameObject.AddComponent<Billboard>();
        }
        HideUI();
    }
    public void ShowUI()
    {
        if (currentTower == null) return;

        displayRange.transform.localScale = Vector3.one * currentTower.attackRange * 2;
        towerUIPanel.SetActive(true);
        towerInfoPanel.GetComponent<UITowerInfo>().SetTowerInfo(currentTower);

        mergeButton.onClick.RemoveAllListeners();
        mergeButton.onClick.AddListener(() => MergeTower());

        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => SellTower());
    }
    public void ShowArrow()
    {
        if (!isOccupied && arrowUI != null)
        {
            arrowUI.SetActive(true);
        }
    }
    public void HideUI()
    {
        towerUIPanel.SetActive(false);
    }
    public void HideArrow()
    {
        if (arrowUI != null)
        {
            arrowUI.SetActive(false);
        }
    }
    public void PlaceTower(GameObject towerPrefab, TowerData towerData)
    {
        if (isOccupied)
        {
            Debug.Log("This object is occupied");
            return;
        }

        GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        tower.transform.SetParent(transform);

        currentTower = tower.GetComponent<Tower>();
        if (currentTower != null)
        {
            currentTower.InitTower(towerData);
        }

        isOccupied = true;
        HideArrow();
        Debug.Log($"Tower '{towerData.Tower_Name}' placed successfully.");
    }
    public void RemoveTower()
    {
        if(currentTower != null)
        {
            currentTower.ClearBeforeDestroy();
            Destroy(currentTower.gameObject);
            currentTower = null;
            isOccupied = false;
        }
    }
    private void MergeTower()
    {
        TowerBuildManager buildManager = FindObjectOfType<TowerBuildManager>();
        if (buildManager != null)
        {
            buildManager.MergingTower(this);
        }
        HideUI();
    }

    private void SellTower()
    {
        TowerBuildManager buildManager = FindObjectOfType<TowerBuildManager>();
        if (buildManager != null)
        {
            buildManager.SellTower(currentTower);
        }
        HideUI();
    }
    
}