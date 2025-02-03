using System.Collections.Generic;
using UnityEngine;
public class WaveData
{
    public int Round_ID { get; set; }
    public int Round { get; set; }
    public int Enemy_ID { get; set; }
    public int NumofEnemy { get; set; }
}

public class WaveTable : DataTable
{
    private readonly Dictionary<int, WaveData> dictionary = new Dictionary<int, WaveData>();
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
    public WaveData Get(int id)
    {
        if (!dictionary.ContainsKey(id))
        {
            return null;
        }
        return dictionary[id];
    }
}
