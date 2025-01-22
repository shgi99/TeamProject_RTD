using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    public bool isOccupied = false; // Ÿ���� ��ġ�Ǿ����� ����

    public void PlaceTower(GameObject towerPrefab)
    {
        if (isOccupied)
        {
            Debug.Log("This object is already occupied");
            return;
        }

        // Ÿ�� ��ġ
        GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        tower.transform.SetParent(transform); // Ÿ���� �� ������Ʈ�� �ڽ����� ����

        isOccupied = true; // ���� ������Ʈ
        Debug.Log("Tower placed successfully");
    }
}