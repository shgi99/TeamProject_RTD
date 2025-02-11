using System.Collections.Generic;
using UnityEngine;
public class EnemyData
{
    public int Enemy_ID { get; set; }
    public float Enemy_HP { get; set; }
    public int DmgToLife { get; set; }
    public int Drop {  get; set; }
    public int Drop_Amount { get; set; }
    public string AssetPath { get; set; }
}

public class EnemyTable : DataTable
{
    private readonly Dictionary<int, EnemyData> dictionary = new Dictionary<int, EnemyData>();
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<EnemyData>(textAsset.text);
        dictionary.Clear();

        foreach (var enemy in list)
        {
            if (!dictionary.ContainsKey(enemy.Enemy_ID))
            {
                dictionary.Add(enemy.Enemy_ID, enemy);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {enemy.Enemy_ID}");
            }
        }
    }
    public EnemyData Get(int id)
    {
        if (!dictionary.ContainsKey(id))
        {
            return null;
        }
        return dictionary[id];
    }
    public Dictionary<int, EnemyData> GetAll()
    {
        return dictionary;
    }
}
