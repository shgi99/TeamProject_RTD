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
    public float AtkDmg { get; set; }
    public float Pct_1 { get; set; }
    public string Pjt_1 { get; set; }
    public int SkillAtk_ID { get; set; }
    public float Pct_2 { get; set; }
    public int Sell_Price { get; set; }
    public string Asset_Path { get; set; }
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
                Debug.LogError($"키 중복: {tower.Tower_ID}");
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
    public Dictionary<int, TowerData> GetAll()
    {
        return dictionary;
    }
    public List<TowerData> GetListRarity(TowerRarity towerRarity)
    {
        return rarityDictionary[(int)towerRarity];
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

    public string GetTowerTypeString(TowerType towerType)
    {
        string towerTypeString = "";
        switch (towerType)
        {
            case TowerType.Human:
                towerTypeString = "인간";
                break;
            case TowerType.Machine:
                towerTypeString = "기계";
                break;
            case TowerType.Monster:
                towerTypeString = "괴물";
                break;
        }
        return towerTypeString;
    }
    public Color GetRarityColor(TowerRarity towerRarity)
    {
        Color rarityColor = Color.white;
        switch (towerRarity)
        {
            case TowerRarity.Common:
                break;
            case TowerRarity.Rare:
                rarityColor = new Color(0, 1, 1, 1);
                break;
            case TowerRarity.Hero:
                rarityColor = new Color(0.6f, 0, 1, 1);
                break;
            case TowerRarity.Legendary:
                rarityColor = new Color(1, 0.2f, 0, 1);
                break;
            case TowerRarity.God:
                rarityColor = new Color(0.25f, 1, 0.35f, 1);
                break;
        }
        return rarityColor;
    }
    public string GetColoredRarityText(string baseText, TowerRarity rarity)
    {
        Color rarityColor = GetRarityColor(rarity);
        string hexColor = ColorUtility.ToHtmlStringRGB(rarityColor);

        return $"<color=#{hexColor}>{baseText}</color>";
    }
}
