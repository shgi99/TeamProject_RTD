using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITowerInfo : MonoBehaviour
{
    public TextMeshProUGUI statText;
    public TextMeshProUGUI attackDescText;
    
    public void SetTowerInfo(Tower tower)
    {
        if (tower == null)
        {
            return;
        }
        statText.text = $"�̸�: {tower.towerName}\n����: {DataTableManager.TowerTable.GetTowerTypeString(tower.towerType)}\n���ݷ�: {tower.currentDamage}";
        attackDescText.text = "���ݷ¸�ŭ ���� ������ ������";
    }

    private void OnEnable()
    {
        Tower currentTower = GetComponentInParent<Tower>();
        SetTowerInfo(currentTower);
    }
}
