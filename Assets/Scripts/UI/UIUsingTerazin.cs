using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUsingTerazin : MonoBehaviour
{
    public GameObject usingTerazinPanel;
    public Button quitButton;
    public Button getMineralButton;
    public Button getGasButton;
    public Button buildHeroTowerButton;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            return;
        }

        quitButton.onClick.AddListener(() => usingTerazinPanel.SetActive(false));
        getMineralButton.onClick.AddListener(() =>
        {
            gameManager.MinusResource(ResourceType.Terazin, 1);
            gameManager.AddResource(ResourceType.Mineral, 400);
        });
        getGasButton.onClick.AddListener(() =>
        {
            gameManager.MinusResource(ResourceType.Terazin, 1);
            gameManager.AddResource(ResourceType.Gas, 300);
        });

    }
   
}
