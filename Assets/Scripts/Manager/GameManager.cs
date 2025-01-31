using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public bool isGameOver = false;
    public UIManager uiManager;
    public int life { get; private set; } = 10;
    public int mineral { get; private set; } = 300;
    public int gas { get; private set; } = 0;
    public int terazin { get; private set; } = 0;
    private int currentRound = 1;
    private EnemySpawner enemySpawner;
    private int costMineralToGas = 100;
    private void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
    }
    void Start()
    {
        uiManager.SetRoundText(currentRound);
        uiManager.UpdateResources();
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
            AddResource(ResourceType.Mineral, 300);
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
        uiManager.UpdateResources();
    }
    public void AddResource(ResourceType resourceType, int amount)
    {
        switch(resourceType)
        {
            case ResourceType.Terazin:
                terazin += amount;
                break;
            case ResourceType.Mineral:
                mineral += amount;
                break;
            case ResourceType.Gas:
                gas += amount;
                break;
        }
        uiManager.UpdateResources();
    }
    public void MinusResource(ResourceType resourceType, int amount) 
    {
        switch (resourceType)
        {
            case ResourceType.Terazin:
                terazin -= amount;
                break;
            case ResourceType.Mineral:
                mineral -= amount;
                break;
            case ResourceType.Gas:
                gas -= amount;
                break;
        }
        uiManager.UpdateResources();
    }

    public void MineralToGas()
    {
        MinusResource(ResourceType.Mineral, costMineralToGas);
        int addGas = Random.Range(20, 129);
        addGas = addGas - addGas % 10;
        AddResource(ResourceType.Gas, addGas);
    }
}
