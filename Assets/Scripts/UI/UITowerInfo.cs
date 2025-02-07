using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITowerInfo : MonoBehaviour
{
    public TextMeshProUGUI statText;
    public TextMeshProUGUI attackDescText;
    public Button normalAttackIconButton;
    public Button skillAttackIconButton;
    public List<Sprite> Icons;
    public Tower currentTower;
    public void SetTowerInfo(Tower tower)
    {
        if (tower == null)
        {
            return;
        }
        currentTower = tower;
        skillAttackIconButton.GetComponent<Image>().sprite = Icons[(int)currentTower.skillType];
        normalAttackIconButton.onClick.RemoveAllListeners();
        normalAttackIconButton.onClick.AddListener(() => attackDescText.text = "���ݷ¸�ŭ ���� ������ ������");
        if(currentTower.skillData != null)
        {
            skillAttackIconButton.gameObject.SetActive(true);
            skillAttackIconButton.onClick.RemoveAllListeners();
            skillAttackIconButton.onClick.AddListener(() => attackDescText.text = $"���ݷ��� {currentTower.skillData.SkillDmgMul}�踸ŭ ������ {GetAttackDescString(tower.skillType)}");
        }
        else
        {
            skillAttackIconButton.gameObject.SetActive(false);
        }
        statText.text = $"�̸�: {currentTower.towerName}\n����: {DataTableManager.TowerTable.GetTowerTypeString(currentTower.towerType)}\n���ݷ�: {currentTower.currentDamage}";
        attackDescText.text = "���ݷ¸�ŭ ���� ������ ������";
    }
    public string GetAttackTypeString(AttackType attackType)
    {
        return attackType == AttackType.Single ? "����" : "����";
    }
    public string GetAttackDescString(EffectState effectState)
    {
        string attackDescString = "";
        switch(effectState)
        {
            case EffectState.StrongAttack:
                attackDescString = $"������ {GetAttackTypeString(currentTower.skillAttackType)} ������";
                break;
            case EffectState.Slow:
                attackDescString = $"{GetAttackTypeString(currentTower.skillAttackType)} ������ + ���ο�";
                break;
            case EffectState.Stun:
                attackDescString = $"{GetAttackTypeString(currentTower.skillAttackType)} ������ + ����";
                break;
        }
        return attackDescString;
    }
}
