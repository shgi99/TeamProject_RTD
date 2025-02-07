using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICheatPanel : MonoBehaviour
{
    public GameObject cheatPanel;
    public Button mineralButton;
    public Button gasButton;
    public Button quitButton;
    public GameObject FPS;
    private bool isSpeedUp = false;
    private bool ShowFPS = true;
    private void Start()
    {
        mineralButton.onClick.RemoveAllListeners();
        mineralButton.onClick.AddListener(() => FindObjectOfType<GameManager>().AddResource(ResourceType.Mineral, 10000));
        gasButton.onClick.RemoveAllListeners();
        gasButton.onClick.AddListener(() => FindObjectOfType<GameManager>().AddResource(ResourceType.Gas, 1000));
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => cheatPanel.SetActive(false));
    }
    public void ToggleSpeedUp()
    {
        isSpeedUp = !isSpeedUp;
        if(isSpeedUp)
        {
            Time.timeScale = 2f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public void ToggleShowFPS()
    {
        ShowFPS = !ShowFPS;
        FPS.SetActive(ShowFPS);
    }
}
