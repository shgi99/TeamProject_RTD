using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StringData
{
    public int String_ID { get; set; }
    public string Content { get; set; }
}
public class StringTable : DataTable
{
    private readonly Dictionary<int, StringData> dictionary = new Dictionary<int, StringData>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<StringData>(textAsset.text);
        dictionary.Clear();


        foreach (var str in list)
        {
            if (!dictionary.ContainsKey(str.String_ID))
            {
                dictionary.Add(str.String_ID, str);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {str.String_ID}");
            }
        }
    }
    public StringData Get(int id)
    {
        if (!dictionary.ContainsKey(id))
        {
            return null;
        }
        return dictionary[id];
    }
}
