using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform startPoint;
    public List<Transform> movePoints;
    public GameObject enemyBasePrefab;
    public float spawnInterval = 0.3f;
    private UIManager uiManager;
    private ObjectPoolingManager poolManager;
    private GameManager gameManager;
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        poolManager = FindObjectOfType<ObjectPoolingManager>();
        gameManager = GetComponent<GameManager>();  
    }
    public IEnumerator SpawnEnemies(WaveData waveData)
    {
        for (int i = 0; i < waveData.NumofEnemy; i++)
        {
            EnemyData currentEnemyData = DataTableManager.EnemyTable.Get(waveData.Enemy_ID);

            GameObject enemyInstance = poolManager.GetObject("Enemy");
            enemyInstance.transform.position = startPoint.position;
            enemyInstance.transform.rotation = Quaternion.identity;
            enemyInstance.SetActive(true);

            Transform visualParent = enemyInstance.transform.Find("VisualParent");
            if (visualParent == null)
            {
                visualParent = new GameObject("VisualParent").transform;
                visualParent.SetParent(enemyInstance.transform);
                visualParent.localPosition = Vector3.zero;
            }

            foreach (Transform child in visualParent)
            {
                poolManager.ReturnObject(child.gameObject.name, child.gameObject);
            }

            string assetPath = currentEnemyData.AssetPath;
            GameObject visualPrefab = poolManager.GetObject(assetPath);
            if (visualPrefab == null)
            {
                Debug.LogError($"Enemy VisualPrefab not found{assetPath}");
            }
            else
            {
                visualPrefab.transform.SetParent(visualParent);
                visualPrefab.transform.localPosition = Vector3.zero;
                visualPrefab.transform.localRotation = Quaternion.identity;
                visualPrefab.SetActive(true);
            }

            EnemyMovement enemyMovement = enemyInstance.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.Init(startPoint, movePoints, currentEnemyData.DmgToLife);
            }

            EnemyHealth enemyHealth = enemyInstance.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                EnemyType enemyType = waveData.Round % 10 == 0 ? EnemyType.Boss : EnemyType.Common;
                if(enemyType == EnemyType.Boss)
                {
                    uiManager.ShowBossHpBar();
                }
                enemyHealth.Init(currentEnemyData, enemyType);
            }

            if (gameManager != null)
            {
                gameManager.enemies.Add(enemyInstance);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    public void SpawnMissionBoss(int stage)
    {
        EnemyData currentEnemyData = DataTableManager.EnemyTable.Get(150 + stage);

        GameObject enemyInstance = poolManager.GetObject("Enemy");
        enemyInstance.transform.position = startPoint.position;
        enemyInstance.transform.rotation = Quaternion.identity;
        enemyInstance.SetActive(true);

        Transform visualParent = enemyInstance.transform.Find("VisualParent");
        if (visualParent == null)
        {
            visualParent = new GameObject("VisualParent").transform;
            visualParent.SetParent(enemyInstance.transform);
            visualParent.localPosition = Vector3.zero;
        }

        foreach (Transform child in visualParent)
        {
            poolManager.ReturnObject(child.gameObject.name, child.gameObject);
        }

        string assetPath = currentEnemyData.AssetPath;
        GameObject visualPrefab = poolManager.GetObject(assetPath);
        if (visualPrefab == null)
        {
            Debug.LogError($"Enemy VisualPrefab not found{assetPath}");
        }
        else
        {
            visualPrefab.transform.SetParent(visualParent);
            visualPrefab.transform.localPosition = Vector3.zero;
            visualPrefab.transform.localRotation = Quaternion.identity;
            visualPrefab.SetActive(true);
        }

        EnemyMovement enemyMovement = enemyInstance.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.Init(startPoint, movePoints, currentEnemyData.DmgToLife);
        }

        EnemyHealth enemyHealth = enemyInstance.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.Init(currentEnemyData, EnemyType.MissionBoss);
        }
    }
}
