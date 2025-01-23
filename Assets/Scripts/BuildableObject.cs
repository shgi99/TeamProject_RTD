using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    public bool isOccupied = false; // Ÿ���� ��ġ�Ǿ����� ����
    public Tower currentTower {  get; private set; }
    public void PlaceTower(GameObject towerPrefab, TowerData towerData)
    {
        if (isOccupied)
        {
            Debug.Log("This object is already occupied");
            return;
        }

        GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        tower.transform.SetParent(transform); // Ÿ���� �� ������Ʈ�� �ڽ����� ����

        currentTower = tower.GetComponent<Tower>();
        if (currentTower != null)
        {
            currentTower.InitTower(towerData);
        }

        isOccupied = true; // ���� ������Ʈ
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