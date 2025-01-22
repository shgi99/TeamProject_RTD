using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int enemiesPerRound;
    private int currentRound = 1;
    public List<GameObject> enemies = new List<GameObject>();
    private EnemySpawner enemySpawner;
    private void Awake()
    {
        instance = this;
        enemySpawner = GetComponent<EnemySpawner>();
    }
    void Start()
    {
        StartCoroutine(SpawnNextRound());
    }
    public IEnumerator SpawnNextRound()
    {
        yield return new WaitForSeconds(5f);
        var currentRoundData = DataTableManager.WaveTable.Get((200 + currentRound).ToString());
        StartCoroutine(enemySpawner.SpawnEnemies(currentRoundData));
    }
    public void CheckClear(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count <= 0)
        {
            Debug.Log($"{currentRound} Clear!!");
            currentRound++;
            StartCoroutine(SpawnNextRound());
        }
    }
}
