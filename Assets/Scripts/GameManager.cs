using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<GameObject> enemies = new List<GameObject>();
    public bool isGameOver = false;
    public UIManager uiManager;
    private int life = 10;
    private int currentRound = 1;
    private EnemySpawner enemySpawner;
    private void Awake()
    {
        instance = this;
        enemySpawner = GetComponent<EnemySpawner>();
    }
    void Start()
    {
        uiManager.SetRoundText(currentRound);
        uiManager.SetLifeText(life);
        StartCoroutine(SpawnNextRound());
    }
    public IEnumerator SpawnNextRound()
    {
        yield return new WaitForSeconds(5f);
        var currentRoundData = DataTableManager.WaveTable.Get(200 + currentRound);
        StartCoroutine(enemySpawner.SpawnEnemies(currentRoundData));
    }
    public void CheckClear(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count <= 0)
        {
            Debug.Log($"{currentRound} Clear!!");
            currentRound++;
            uiManager.SetRoundText(currentRound);
            StartCoroutine(SpawnNextRound());
        }
    }
    public void DamageToLife(int damage)
    {
        life -= damage;

        if(life <= 0)
        {
            life = 0;
            isGameOver = true;
            uiManager.SetGameOver();
        }
        uiManager.SetLifeText(life);
    }
}
