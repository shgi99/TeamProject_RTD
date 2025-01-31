using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public abstract class DataTable
{
    public static readonly string FormatPath = "Tables/{0}";
    public abstract void Load(string filename);
    public static List<T> LoadCSV<T>(string csv)
    {
        using (var reader = new StringReader(csv))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csvReader.GetRecords<T>().ToList<T>();
        }
    }
}
