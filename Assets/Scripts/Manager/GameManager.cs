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
    public int mineral { get; private set; } = 40000;
    public int gas { get; private set; } = 0;
    public int terazin { get; private set; } = 0;
    private int currentRound = 1;
    private EnemySpawner enemySpawner;
    public int costMineralToGas { get; private set; } = 100;
    public int costBuildTower { get; private set; } = 100;
    public float clearTime = 0f;
    private bool isGamePause = false;
    private float playTime = 0f;
    private int finalRound;
    private float nextRoundTimeLeft = 10f;
    private void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
    }
    private void Start()
    {
        Application.targetFrameRate = int.MaxValue;
        finalRound = DataTableManager.WaveTable.GetWaveCount();
        uiManager.SetRoundText(currentRound);
        uiManager.UpdateResources();
        StartCoroutine(SpawnNextRound());
    }
    private void Update()
    {
        if(!isGameOver && !isGamePause)
        {
            playTime += Time.deltaTime;
            uiManager.UpdatePlayTime(playTime);
        }
        if(isGameOver && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetGameClear();
        }
    }
    public void TogglePause()
    {
        isGamePause = !isGamePause;
        if(isGamePause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public IEnumerator SpawnNextRound()
    {
        nextRoundTimeLeft = 10f;
        while (nextRoundTimeLeft > 0)
        {
            FindObjectOfType<UILogPanel>().AddLog($"다음 라운드까지 {nextRoundTimeLeft}초");
            yield return new WaitForSeconds(1f);
            nextRoundTimeLeft -= 1f;
        }

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
            if (currentRound % 10 == 0)
            {
                uiManager.HideBossHpBar();
            }
            if(currentRound == finalRound)
            {
                SetGameClear();
            }
            else
            {
                currentRound++;
                uiManager.SetRoundText(currentRound);
                StartCoroutine(SpawnNextRound());
            }
        }
    }
    private void SetGameClear()
    {
        clearTime = playTime;
        TogglePause();
        uiManager.ShowGameClearPanel();
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
