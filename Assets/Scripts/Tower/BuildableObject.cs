using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildableObject : MonoBehaviour
{
    public bool isOccupied = false;

    public GameObject arrowUI; 
    public Tower currentTower {  get; private set; }
    private ObjectPoolingManager poolManager;
    private void Awake()
    {
        poolManager = FindObjectOfType<ObjectPoolingManager>();
    }
    public void ShowArrow()
    {
        if (!isOccupied && arrowUI != null)
        {
            arrowUI.SetActive(true);
        }
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
            return;
        }

        GameObject tower = poolManager.GetObject("Tower");
        tower.transform.position = transform.position;
        tower.transform.rotation = Quaternion.identity;
        tower.transform.SetParent(transform);

        currentTower = tower.GetComponent<Tower>();
        if (currentTower != null)
        {
            currentTower.InitTower(towerData);
        }

        isOccupied = true;
        HideArrow();
    }
    public void RemoveTower()
    {
        if(currentTower != null)
        {
            currentTower.ClearBeforeDestroy(); 
            Transform model = currentTower.transform.GetChild(1);
            model.SetParent(null);
            var modelAssetPath = DataTableManager.TowerTable.Get(currentTower.towerId).Asset_Path;
            poolManager.ReturnObject(modelAssetPath, model.gameObject);

            currentTower.transform.SetParent(null);
            poolManager.ReturnObject("Tower", currentTower.gameObject);
            currentTower = null;
            isOccupied = false;
        }
    }
}