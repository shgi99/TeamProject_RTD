using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    private Dictionary<string, GameObject> resourceDict = new Dictionary<string, GameObject>();
    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadAllResources()
    {
        foreach (var towerData in DataTableManager.TowerTable.GetAll().Values)
        {
            if (!string.IsNullOrEmpty(towerData.Asset_Path))
            {
                resourceDict[towerData.Asset_Path] = Resources.Load<GameObject>(towerData.Asset_Path);
            }
        }

        foreach (var enemyData in DataTableManager.EnemyTable.GetAll().Values)
        {
            if (!string.IsNullOrEmpty(enemyData.AssetPath))
            {
                resourceDict[enemyData.AssetPath] = Resources.Load<GameObject>(enemyData.AssetPath);
            }
        }

        foreach (var towerData in DataTableManager.TowerTable.GetAll().Values)
        {
            if (!string.IsNullOrEmpty(towerData.Pjt_1))
            {
                resourceDict[towerData.Pjt_1] = Resources.Load<GameObject>(towerData.Pjt_1);
            }
        }
        foreach (var skillData in DataTableManager.SkillTable.GetAll().Values)
        {
            if (!string.IsNullOrEmpty(skillData.Pjt))
            {
                resourceDict[skillData.Pjt] = Resources.Load<GameObject>(skillData.Pjt);
            }
        }
    }
    public GameObject GetResource(string key)
    {
        return resourceDict.ContainsKey(key) ? resourceDict[key] : null;
    }
}
