using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : MonoBehaviour
{
    public Image rewardImage;
    public TextMeshProUGUI rewardAmount;
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questProgress;
    public List<Image> rewardIcons;
    public GameObject clearPanel;

    public void SetQuestData(QuestData quest, string progress)
    {
        rewardImage.sprite = rewardIcons[quest.Reward_Type - 1].sprite;
        rewardImage.color = rewardIcons[quest.Reward_Type - 1].color;
        rewardAmount.text = $"{quest.Reward_Amt}";
        questName.text = quest.Quest_Name;
        questDescription.text = GetDescriptionText(quest);
        questProgress.text = progress;
    }

    public string GetDescriptionText(QuestData quest)
    {
        string descText = "";
        string[] requiredTowers = quest.Req.Split('_');
        for(int i = 0; i < requiredTowers.Length; i++)
        {
            var towerName = DataTableManager.TowerTable.Get(int.Parse(requiredTowers[i])).Tower_Name;
            descText += towerName;
            if(i ==  requiredTowers.Length - 1)
            {
                descText += " 설치";
                break;
            }
            else
            {
                descText += ", ";
            }
        }
        return descText;
    }
    public void SetClear()
    {
        clearPanel.SetActive(true);
        questProgress.text = "클리어";
    }
}
