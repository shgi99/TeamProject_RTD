using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    public bool isOccupied = false;
    public GameObject arrowUI; 
    public Tower currentTower {  get; private set; }
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
}