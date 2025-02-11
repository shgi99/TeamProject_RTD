using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
#if UNITY_EDITOR
        var enemyTable = new EnemyTable();
        enemyTable.Load(DataTableIds.Enemy);
        tables.Add(DataTableIds.Enemy, enemyTable);
        var waveTable = new WaveTable();
        waveTable.Load(DataTableIds.Wave);
        tables.Add(DataTableIds.Wave, waveTable);
        var towerTable = new TowerTable();
        towerTable.Load(DataTableIds.Tower);
        tables.Add(DataTableIds.Tower, towerTable);
#else
        var enemyTable = new EnemyTable();
        enemyTable.Load(DataTableIds.Enemy);
        tables.Add(DataTableIds.Enemy, enemyTable);
        var waveTable = new WaveTable();
        waveTable.Load(DataTableIds.Wave);
        tables.Add(DataTableIds.Wave, waveTable);
        var towerTable = new TowerTable();
        towerTable.Load(DataTableIds.Tower);
        tables.Add(DataTableIds.Tower, towerTable);
#endif
    }
    public static EnemyTable EnemyTable
    {
        get
        {
            return Get<EnemyTable>(DataTableIds.Enemy);
        }
    }
    public static WaveTable WaveTable
    {
        get
        {
            return Get<WaveTable>(DataTableIds.Wave);
        }
    }
    public static TowerTable TowerTable
    {
        get
        {
            return Get<TowerTable>(DataTableIds.Tower);
        }
    }
    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("None Table");
            return null;
        }
        return tables[id] as T;
    }
}
