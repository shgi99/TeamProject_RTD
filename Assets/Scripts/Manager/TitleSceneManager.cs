using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject loadingPanel;
    public Slider loadingSlider;
    public TextMeshProUGUI loadingPct;
    public Button startButton;
    public Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        ResourceManager.instance.LoadAllResourcesAsync(
            progress =>
            {
                loadingSlider.value = progress;
                loadingPct.text = $"{(progress * 100f):F0}%";
            },
            () =>
            {
                loadingPanel.SetActive(false);
            });

        startButton.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
        quitButton.onClick.AddListener(() => Application.Quit());
    }
    
}
