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
        statText.text = $"이름: {tower.towerName}\n종족: {DataTableManager.TowerTable.GetTowerTypeString(tower.towerType)}\n공격력: {tower.currentDamage}";
        attackDescText.text = "공격력만큼 단일 적에게 데미지";
    }

    private void OnEnable()
    {
        Tower currentTower = GetComponentInParent<Tower>();
        SetTowerInfo(currentTower);
    }
}
