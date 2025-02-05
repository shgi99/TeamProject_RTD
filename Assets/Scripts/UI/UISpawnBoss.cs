using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISpawnBoss : MonoBehaviour
{
    public Button SpawnBossButtonOne;
    public Button SpawnBossButtonTwo;
    public Button SpawnBossButtonThree;

    public TextMeshProUGUI TimerTextOne;
    public TextMeshProUGUI TimerTextTwo;
    public TextMeshProUGUI TimerTextThree;

    private EnemySpawner enemySpawner;

    private float cooldownTime = 120f;

    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();

        SpawnBossButtonOne.onClick.AddListener(() => StartCoolDown(SpawnBossButtonOne, TimerTextOne, 1));
        SpawnBossButtonTwo.onClick.AddListener(() => StartCoolDown(SpawnBossButtonTwo, TimerTextTwo, 2));
        SpawnBossButtonThree.onClick.AddListener(() => StartCoolDown(SpawnBossButtonThree, TimerTextThree, 3));
    }

    public void StartCoolDown(Button button, TextMeshProUGUI timertext, int stage)
    {
        enemySpawner.SpawnMissionBoss(stage);
        button.interactable = false;
        SetButtonTextAlpha(button, 0.01f);
        StartCoroutine(CooldownRoutine(button, timertext));
    }
    public IEnumerator CooldownRoutine(Button button, TextMeshProUGUI timerText)
    {
        float remainingTime = cooldownTime;
        
        while (remainingTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = $"{minutes:D2}:{seconds:D2}";

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        timerText.text = "";
        button.interactable = true;
        SetButtonTextAlpha(button, 1f);
    }
    private void SetButtonTextAlpha(Button button, float alpha)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            Color color = buttonText.color;
            color.a = alpha;
            buttonText.color = color;
        }
    }
}
