using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public bool isGameOver = false;
    public UIManager uiManager;
    public int life { get; private set; } = 10;
    public int mineral { get; private set; } = 500;
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
    private void Update()
    {
        if(isGameOver && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
    public IEnumerator SpawnNextRound()
    {
        yield return new WaitForSeconds(10f);
        var currentRoundData = DataTableManager.WaveTable.Get(200 + currentRound);
        StartCoroutine(enemySpawner.SpawnEnemies(currentRoundData));
    }
    public void CheckClear(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count <= 0)
        {
            Debug.Log($"{currentRound} Clear!!");
            AddResource(ResourceType.Mineral, 200);
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
    public bool MinusResource(ResourceType resourceType, int amount) 
    {
        switch (resourceType)
        {
            case ResourceType.Terazin:
                if(terazin - amount < 0)
                {
                    return false;
                }
                terazin -= amount;
                break;
            case ResourceType.Mineral:
                if (mineral - amount < 0)
                {
                    return false;
                }
                mineral -= amount;
                break;
            case ResourceType.Gas:
                if (gas - amount < 0)
                {
                    return false;
                }
                gas -= amount;
                break;
        }
        uiManager.UpdateResources();
        return true;
    }

    public void MineralToGas()
    {
        
        if(!MinusResource(ResourceType.Mineral, costMineralToGas))
        {
            return;
        }
        int addGas = Random.Range(20, 129);
        addGas = addGas - addGas % 10;
        AddResource(ResourceType.Gas, addGas);
    }
    
}
