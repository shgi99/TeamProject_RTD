using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUsingTerazin : MonoBehaviour
{
    public GameObject usingTerazinPanel;
    public GameObject chooseHeroTowerPanel;
    public Button quitButton;
    public Button getMineralButton;
    public Button getGasButton;
    public Button buildHeroTowerButton;

    public TextMeshProUGUI terazinToMineralCostText;
    public TextMeshProUGUI terazinToGasCostText;
    public TextMeshProUGUI terazinToBuildHeroCostText;
    public TextMeshProUGUI mineralToBuildHeroCostText;

    private int costTerazinToMineral = 1;
    private int costTerazinToGas = 1;
    private int costTerazinToBuildHero = 1;
    private int costMineralToBuildHero = 100;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            return;
        }

        quitButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonTouch();
            usingTerazinPanel.SetActive(false);
            });
        getMineralButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonTouch();
            if (gameManager.canUseResource(ResourceType.Terazin, 1))
            {
                gameManager.MinusResource(ResourceType.Terazin, 1);
                gameManager.AddResource(ResourceType.Mineral, 400);
            }
        });
        getGasButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonTouch();
            if (gameManager.canUseResource(ResourceType.Terazin, 1))
            {
                gameManager.MinusResource(ResourceType.Terazin, 1);
                gameManager.AddResource(ResourceType.Gas, 300);
            }
        });
        buildHeroTowerButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayButtonTouch();
            if (gameManager.canUseResource(ResourceType.Terazin, 1) && gameManager.canUseResource(ResourceType.Mineral, 100))
            {
                chooseHeroTowerPanel.SetActive(true);
            }
        });
    }
    private void OnEnable()
    {
        SetTextColor();
    }
    public void SetTextColor()
    {
        terazinToMineralCostText.color = gameManager.terazin < costTerazinToMineral ? Color.red : Color.black;
        terazinToGasCostText.color = gameManager.terazin < costTerazinToGas ? Color.red : Color.black;
        terazinToBuildHeroCostText.color = gameManager.terazin < costTerazinToBuildHero ? Color.red : Color.black;
        mineralToBuildHeroCostText.color = gameManager.mineral < costMineralToBuildHero ? Color.red : Color.black;
    }
}
