using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameClear : MonoBehaviour
{
    public TextMeshProUGUI clearTimeText;
    public Button quitButton;

    private void OnEnable()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        TimeSpan timeSpan = TimeSpan.FromSeconds(gameManager.clearTime);
        clearTimeText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }
}
