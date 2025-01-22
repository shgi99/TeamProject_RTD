using System.Collections.Generic;
using UnityEngine;
public class WaveData
{
    public string Round_ID { get; set; }
    public int Round { get; set; }
    public string Enemy_ID { get; set; }
    public int NumofEnemy { get; set; }
    public int DmgToLife { get; set; }
}

public class WaveTable : DataTable
{
    private readonly Dictionary<string, WaveData> dictionary = new Dictionary<string, WaveData>();
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<WaveData>(textAsset.text);
        dictionary.Clear();

        foreach (var wave in list)
        {
            if (!dictionary.ContainsKey(wave.Round_ID))
            {
                dictionary.Add(wave.Round_ID, wave);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {wave.Round_ID}");
            }
        }
    }
    public WaveData Get(string id)
    {
        if (!dictionary.ContainsKey(id))
        {
            return null;
        }
        return dictionary[id];
    }
}
