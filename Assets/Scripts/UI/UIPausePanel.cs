using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPausePanel : MonoBehaviour
{
    public Button resumeButton;
    public Button quitButton;

    private UIManager uiManager;
    // Start is called before the first frame update
    void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            return;
        }
        quitButton.onClick.AddListener(() => Application.Quit());
        resumeButton.onClick.AddListener(() => uiManager.OnClickResumeButton());
    }
}
