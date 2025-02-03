using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UpgradeData
{
    public int Upgrade_ID { get; set; }
    public int Upgrade_Lv { get; set; }
    public int Gas_Amount { get; set; }
    public float Change_Stat { get; set; }
}
public class UpgradeTable : DataTable
{
    private readonly Dictionary<int, UpgradeData> dictionary = new Dictionary<int, UpgradeData>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<UpgradeData>(textAsset.text);
        dictionary.Clear();

        foreach (var upgrade in list)
        {
            if (!dictionary.ContainsKey(upgrade.Upgrade_ID))
            {
                dictionary.Add(upgrade.Upgrade_ID, upgrade);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {upgrade.Upgrade_ID}");
            }
        }
    }
    public UpgradeData Get(int id)
    {
        if (!dictionary.ContainsKey(id))
        {
            return null;
        }
        return dictionary[id];
    }
    public int GetDataCount()
    {
        return dictionary.Count;
    }
}
