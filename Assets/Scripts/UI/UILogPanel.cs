using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILogPanel : MonoBehaviour
{
    public TextMeshProUGUI[] logTexts; 
    private Queue<string> logQueue = new Queue<string>();
    private Coroutine fadeCoroutine;
    private float fadeDelay = 3f;
    void Start()
    {
        foreach (var log in logTexts)
        { 
            log.text = "\n";
            log.color = new Color(log.color.r, log.color.g, log.color.b, 0);
        }
    }

    public void AddLog(string message)
    {
        if (logQueue.Count >= logTexts.Length)
        {
            logQueue.Dequeue();
        }

        logQueue.Enqueue(message);
        UpdateLogUI();

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutLog());
    }
    private void UpdateLogUI()
    {
        int index = logTexts.Length - logQueue.Count;
        int queueIndex = 0;

        foreach (var log in logQueue)
        {
            logTexts[index + queueIndex].text = log;
            logTexts[index + queueIndex].color = new Color(logTexts[index + queueIndex].color.r, logTexts[index + queueIndex].color.g, logTexts[index + queueIndex].color.b, 1);
            queueIndex++;
        }
    }

    private IEnumerator FadeOutLog()
    {
        yield return new WaitForSeconds(fadeDelay);

        float fadeDuration = 1.5f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timer / fadeDuration);

            foreach (var log in logTexts)
            {
                log.color = new Color(log.color.r, log.color.g, log.color.b, alpha);
            }
            yield return null;
        }

        foreach (var log in logTexts)
        {
            log.text = "\n";
        }

        logQueue.Clear();
    }
}
