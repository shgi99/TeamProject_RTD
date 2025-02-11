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
        normalAttackIconButton.onClick.AddListener(() => attackDescText.text = "공격력만큼 단일 적에게 데미지");
        if(currentTower.skillData != null)
        {
            skillAttackIconButton.gameObject.SetActive(true);
            skillAttackIconButton.onClick.RemoveAllListeners();
            skillAttackIconButton.onClick.AddListener(() => attackDescText.text = $"공격력의 {currentTower.skillData.SkillDmgMul}배만큼 적에게 {GetAttackDescString(tower.skillType)}");
        }
        else
        {
            skillAttackIconButton.gameObject.SetActive(false);
        }
        statText.text = $"이름: {currentTower.towerName}\n종족: {DataTableManager.TowerTable.GetTowerTypeString(currentTower.towerType)}\n공격력: {currentTower.currentDamage}";
        attackDescText.text = "공격력만큼 단일 적에게 데미지";
    }
    public string GetAttackTypeString(AttackType attackType)
    {
        return attackType == AttackType.Single ? "단일" : "광역";
    }
    public string GetAttackDescString(EffectState effectState)
    {
        string attackDescString = "";
        switch(effectState)
        {
            case EffectState.StrongAttack:
                attackDescString = $"강력한 {GetAttackTypeString(currentTower.skillAttackType)} 데미지";
                break;
            case EffectState.Slow:
                attackDescString = $"{GetAttackTypeString(currentTower.skillAttackType)} 데미지 + 슬로우";
                break;
            case EffectState.Stun:
                attackDescString = $"{GetAttackTypeString(currentTower.skillAttackType)} 데미지 + 스턴";
                break;
        }
        return attackDescString;
    }
}
