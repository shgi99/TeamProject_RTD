using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPausePanel : MonoBehaviour
{
    public Button resumeButton;
    public Button questButton;
    public Button guideButton;
    public Button quitButton;
    public Button optionButton;
    private UIManager uiManager;
    // Start is called before the first frame update
    void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            return;
        }
        quitButton.onClick.AddListener(() => 
        { 
            SoundManager.Instance.PlayButtonTouch(); 
            Application.Quit(); 
        });
        questButton.onClick.AddListener(() => 
        { 
            SoundManager.Instance.PlayButtonTouch(); 
            uiManager.ShowQuestPanel();
        });
        guideButton.onClick.AddListener(() => 
        { 
            SoundManager.Instance.PlayButtonTouch(); 
            uiManager.ShowGuidePanel(); 
        });
        optionButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonTouch();
            uiManager.ShowOptionPanel();
        });
        resumeButton.onClick.AddListener(() => 
        { 
            SoundManager.Instance.PlayButtonTouch(); 
            uiManager.OnClickResumeButton();
        });
    }
}
