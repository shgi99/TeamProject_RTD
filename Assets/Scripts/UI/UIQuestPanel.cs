using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestPanel : MonoBehaviour
{
    public GameObject questPanel;
    public Transform questListParent;
    public GameObject questItemPrefab;
    public ScrollRect scrollRect;
    public Button quitButton;

    private Dictionary<int, GameObject> activeQuestItems = new Dictionary<int, GameObject>();
    private QuestManager questManager;
    private TowerBuildManager towerBuildManager;

    private void Awake()
    {
        questManager = FindObjectOfType<QuestManager>();
        towerBuildManager = FindObjectOfType<TowerBuildManager>();
        quitButton.onClick.AddListener(() => questPanel.SetActive(false));
    }
    private void OnEnable() {
        PopulateQuests();
    }
    public void PopulateQuests()
    {
        ClearQuestUI();

        foreach (var quest in DataTableManager.QuestTable.GetDictionary().Values)
        {
            CreateQuestUI(quest);
        }
    }

    private void CreateQuestUI(QuestData quest)
    {
        GameObject questItem = Instantiate(questItemPrefab, questListParent);
        UIQuestItem questUI = questItem.GetComponent<UIQuestItem>();

        questUI.SetQuestData(quest, GetQuestProgress(quest));
        if (questManager.IsCompleted(quest.Quest_ID))
        {
            questUI.SetClear();
        }
        activeQuestItems[quest.Quest_ID] = questItem;
    }

    private string GetQuestProgress(QuestData quest)
    {
        string[] requiredTowers = quest.Req.Split('_');
        int currentProgress = 0;
        HashSet<int> builtTowers = new HashSet<int>();

        foreach (Tower tower in towerBuildManager.buildedTowers)
        {
            builtTowers.Add(tower.towerId);
        }

        foreach (string towerId in requiredTowers)
        {
            if (builtTowers.Contains(int.Parse(towerId)))
            {
                currentProgress++;
            }
        }

        return $"{currentProgress} / {requiredTowers.Length}";
    }

    private void ClearQuestUI()
    {
        foreach (Transform child in questListParent)
        {
            Destroy(child.gameObject);
        }
        activeQuestItems.Clear();
    }
}
