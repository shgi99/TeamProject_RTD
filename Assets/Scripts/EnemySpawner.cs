using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform startPoint;
    public List<Transform> movePoints;
    public GameObject enemyPrefab;              
    public float spawnInterval = 0.3f;   
    public int enemiesPerRound = 20;     
    private int currentRound = 1;        

    public void StartRound()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerRound; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
        Debug.Log($"Round {currentRound} completed!");
        currentRound++;
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.SetMovePoints(startPoint, movePoints);
    }
}
