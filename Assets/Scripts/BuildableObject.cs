using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    public bool isOccupied = false; // 타워가 설치되었는지 여부

    public void PlaceTower(GameObject towerPrefab)
    {
        if (isOccupied)
        {
            Debug.Log("This object is already occupied");
            return;
        }

        // 타워 배치
        GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        tower.transform.SetParent(transform); // 타워를 이 오브젝트의 자식으로 설정

        isOccupied = true; // 상태 업데이트
        Debug.Log("Tower placed successfully");
    }
}