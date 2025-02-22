using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        startButton.onClick.AddListener(() =>
            {
            SoundManager.Instance.PlayButtonTouch();
                SceneManager.LoadScene("GameScene");
            }
        );
        quitButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonTouch();
            Application.Quit();
            }
        );
    }
    
}
