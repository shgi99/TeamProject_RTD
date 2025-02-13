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
        SoundManager.Instance.PlayButtonTouch();
        foreach (var page in pages)
        {
            page.SetActive(false);
        }
        guidePanel.SetActive(false);
    }
    public void OnClickNextButton()
    {
        SoundManager.Instance.PlayButtonTouch();
        if (pageIndex + 1 < pages.Count)
        {
            pages[pageIndex++].SetActive(false);
            pages[pageIndex].SetActive(true);
        }
    }
    public void OnClickPrevButton()
    {
        SoundManager.Instance.PlayButtonTouch();
        if (pageIndex - 1 >= 0)
        {
            pages[pageIndex--].SetActive(false);
            pages[pageIndex].SetActive(true);
        }
    }
}
