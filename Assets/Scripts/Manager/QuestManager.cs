using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<int, QuestData> questDictionary;
    private TowerBuildManager towerBuildManager;
    private HashSet<int> completedQuests = new HashSet<int>();
    void Start()
    {
        questDictionary = DataTableManager.QuestTable.GetDictionary();
        towerBuildManager = GetComponent<TowerBuildManager>();
    }

    public void CheckQuests()
    {
        foreach (var quest in questDictionary.Values)
        {
            if (!completedQuests.Contains(quest.Quest_ID) && IsQuestComplete(quest))
            {
                GrantReward(quest);
                completedQuests.Add(quest.Quest_ID);
            }
        }
    }
    private bool IsQuestComplete(QuestData quest)
    {
        string[] requiredTowers = quest.Req.Split('_');
        HashSet<int> builtTowerIds = new HashSet<int>();

        foreach (Tower tower in towerBuildManager.buildedTowers)
        {
            builtTowerIds.Add(tower.towerId);
        }

        foreach (string towerId in requiredTowers)
        {
            if (!builtTowerIds.Contains(int.Parse(towerId)))
            {
                return false;
            }
        }
        return true;
    }
    private void GrantReward(QuestData quest)
    {
        GetComponent<GameManager>().AddResource((ResourceType)quest.Reward_Type, quest.Reward_Amt);
        var questCompleteText = DataTableManager.StringTable.Get(quest.Result_String).Content;
        FindObjectOfType<UILogPanel>().AddLog(questCompleteText);
    }

    public bool IsCompleted(int quest_ID)
    {
        return completedQuests.Contains(quest_ID);
    }
}
