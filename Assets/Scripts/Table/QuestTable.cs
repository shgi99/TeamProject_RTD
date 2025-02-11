using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QuestData
{
    public int Quest_ID { get; set; }
    public string Quest_Name { get; set; }
    public string Req { get; set; }
    public int Reward_Type { get; set; }
    public int Reward_Amt { get; set; }
    public int Result_String { get; set; }
}
public class QuestTable : DataTable
{
    private readonly Dictionary<int, QuestData> dictionary = new Dictionary<int, QuestData>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<QuestData>(textAsset.text);
        dictionary.Clear();


        foreach (var quest in list)
        {
            if (!dictionary.ContainsKey(quest.Quest_ID))
            {
                dictionary.Add(quest.Quest_ID, quest);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {quest.Quest_ID}");
            }
        }
    }
    public QuestData Get(int id)
    {
        if (!dictionary.ContainsKey(id))
        {
            return null;
        }
        return dictionary[id];
    }
    public Dictionary<int, QuestData> GetDictionary() 
    {  
        return dictionary; 
    }
}
