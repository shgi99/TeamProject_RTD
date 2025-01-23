using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    public bool isOccupied = false; // 타워가 설치되었는지 여부
    public Tower currentTower {  get; private set; }
    public void PlaceTower(GameObject towerPrefab, TowerData towerData)
    {
        if (isOccupied)
        {
            Debug.Log("This object is already occupied");
            return;
        }

        GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        tower.transform.SetParent(transform); // 타워를 이 오브젝트의 자식으로 설정

        currentTower = tower.GetComponent<Tower>();
        if (currentTower != null)
        {
            currentTower.InitTower(towerData);
        }

        isOccupied = true; // 상태 업데이트
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