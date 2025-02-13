using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChooseHeroPanel : MonoBehaviour
{
    public GameObject uiChooseHeroPanel;
    public List<Button> heroTowers;
    public List<GameObject> heroTowersMarker;
    public Button quitButton;
    public TowerData selectedTower;
    private List<TowerData> heroTowerDatas;
    // Start is called before the first frame update
    void OnEnable()
    {
        heroTowerDatas = DataTableManager.TowerTable.GetListRarity(TowerRarity.Hero);
        quitButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayButtonTouch();
            uiChooseHeroPanel.SetActive(false);
            selectedTower = null;

            foreach (var marker in heroTowersMarker)
            {
                marker.SetActive(false);
            }
        });
        for(int i = 0; i < heroTowers.Count; i++)
        {
            int index = i;
            heroTowers[i].onClick.RemoveAllListeners();
            heroTowers[i].onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonTouch();
                selectedTower = heroTowerDatas[index];
                for (int j = 0; j < heroTowersMarker.Count; j++)
                {
                    heroTowersMarker[j].SetActive(j == index);
                }
                FindObjectOfType<TowerBuildManager>().SelectTower(selectedTower);
            });
        }
    }
    private void OnDisable()
    {
        
    }
}
