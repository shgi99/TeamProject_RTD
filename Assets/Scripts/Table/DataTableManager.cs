using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
        var enemyTable = new EnemyTable();
        enemyTable.Load(DataTableIds.Enemy);
        tables.Add(DataTableIds.Enemy, enemyTable);
        var waveTable = new WaveTable();
        waveTable.Load(DataTableIds.Wave);
        tables.Add(DataTableIds.Wave, waveTable);
        var towerTable = new TowerTable();
        towerTable.Load(DataTableIds.Tower);
        tables.Add(DataTableIds.Tower, towerTable);
        var upgradeTable = new UpgradeTable();
        upgradeTable.Load(DataTableIds.Upgrade);
        tables.Add(DataTableIds.Upgrade, upgradeTable);
        var skillTable = new SkillTable();
        skillTable.Load(DataTableIds.Skill);
        tables.Add(DataTableIds.Skill, skillTable);
        var questTable = new QuestTable();
        questTable.Load(DataTableIds.Quest);
        tables.Add(DataTableIds.Quest, questTable);
        var stringTable = new StringTable();
        stringTable.Load(DataTableIds.String);
        tables.Add(DataTableIds.String, stringTable);
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

    public static UpgradeTable UpgradeTable
    {
        get
        {
            return Get<UpgradeTable>(DataTableIds.Upgrade);
        }
    }
    public static SkillTable SkillTable
    {
        get
        {
            return Get<SkillTable>(DataTableIds.Skill);
        }
    }
    public static QuestTable QuestTable
    {
        get
        {
            return Get<QuestTable>(DataTableIds.Quest);
        }
    }
    public static StringTable StringTable
    {
        get
        {
            return Get<StringTable>(DataTableIds.String);
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
