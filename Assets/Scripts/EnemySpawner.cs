using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform startPoint;
    public List<Transform> movePoints;
    public GameObject enemyPrefab;
    public float spawnInterval = 0.3f;

    public IEnumerator SpawnEnemies(WaveData waveData)
    {
        for (int i = 0; i < waveData.NumofEnemy; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            enemyMovement.Init(startPoint, movePoints, waveData.DmgToLife);
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.Init(DataTableManager.EnemyTable.Get(waveData.Enemy_ID).Enemy_HP);
            GameManager.instance.enemies.Add(enemy);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
