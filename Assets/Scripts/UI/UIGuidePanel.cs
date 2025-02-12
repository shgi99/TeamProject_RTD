using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuidePanel : MonoBehaviour
{
    public GameObject guidePanel;
    public List<GameObject> pages = new List<GameObject>();
    private int pageIndex = 0;
    private void OnEnable()
    {
        pages[pageIndex].SetActive(true);
    }
    public void OnClickQuitButton()
    {
        foreach(var page in pages)
        {
            page.SetActive(false);
        }
        guidePanel.SetActive(false);
    }
    public void OnClickNextButton()
    {
        if(pageIndex + 1 < pages.Count)
        {
            pages[pageIndex++].SetActive(false);
            pages[pageIndex].SetActive(true);
        }
    }
    public void OnClickPrevButton()
    {
        if (pageIndex - 1 >= 0)
        {
            pages[pageIndex--].SetActive(false);
            pages[pageIndex].SetActive(true);
        }
    }
}
