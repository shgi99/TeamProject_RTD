using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TowerData
{
    public int Tower_ID { get; set; }
    public string Tower_Name { get; set; }
    public int Tower_Rarity { get; set; }
    public int Tower_Type { get; set; }
    public float AtkRng { get; set; }
    public float AtkSpd { get; set; }
    public int AtkDmg { get; set; }
    public float Pct_1 { get; set; }
    public int SkillAtk_ID { get; set; }
    public float Pct_2 { get; set; }
    public int Sell_Price { get; set; }
    public string Asset_Path { get; set; }
    public int Pjt {  get; set; }

}
public class TowerTable : DataTable
{
    private readonly Dictionary<int, TowerData> dictionary = new Dictionary<int, TowerData>();
    private readonly Dictionary<int, List<TowerData>> rarityDictionary = new Dictionary<int, List<TowerData>>();
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<TowerData>(textAsset.text);
        dictionary.Clear();
        rarityDictionary.Clear();

        foreach (var tower in list)
        {
            if (!dictionary.ContainsKey(tower.Tower_ID))
            {
                dictionary.Add(tower.Tower_ID, tower);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {tower.Tower_ID}");
            }

            if (!rarityDictionary.ContainsKey(tower.Tower_Rarity))
            {
                rarityDictionary[tower.Tower_Rarity] = new List<TowerData>();
            }
            rarityDictionary[tower.Tower_Rarity].Add(tower);
        }
    }
    public TowerData Get(int id)
    {
        if (!dictionary.ContainsKey(id))
        {
            return null;
        }
        return dictionary[id];
    }

    public TowerData GetRandomByRarity(int rarity)
    {
        if (rarityDictionary.TryGetValue(rarity, out List<TowerData> towers) && towers.Count > 0)
        {
            int randomIndex = Random.Range(0, towers.Count);
            return towers[randomIndex];
        }
        return null;
    }
    public TowerData GetUpgradeRarity(int currentRarity)
    {
        return GetRandomByRarity(currentRarity + 1);
    }
}
