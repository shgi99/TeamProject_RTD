using System.Collections.Generic;
using UnityEngine;
public class EnemyData
{
    public int Enemy_ID { get; set; }
    public int Enemy_HP { get; set; }
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
                Debug.LogError($"Ű �ߺ�: {enemy.Enemy_ID}");
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
}
