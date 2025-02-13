using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private TowerAudioDatabase towerAudioDatabase;
    private Dictionary<string, AudioClip> audioClipDict;

    private int maxConcurrentSFX = 10;
    private int currentConcurrentSFX = 0;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioClipDict = towerAudioDatabase.audioDictionary;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayTowerSFX(string TowerID)
    {
        if (audioClipDict.ContainsKey(TowerID))
        {
            AudioClip clip = audioClipDict[TowerID];
            if (currentConcurrentSFX < maxConcurrentSFX)
            {
                currentConcurrentSFX++;
                sfxSource.PlayOneShot(clip);
                StartCoroutine(WaitAndDecrement(clip.length));
            }
        }
    }
    public void PlayButtonTouch()
    {
        if (audioClipDict.ContainsKey("Button"))
        {
            sfxSource.PlayOneShot(audioClipDict["Button"]);
        }
    }
    public void PlayBGM()
    {
        if (audioClipDict.ContainsKey("BGM"))
        {
            bgmSource.clip = audioClipDict["BGM"];
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    public void PauseBGM()
    {
        bgmSource.Pause();
    }
    public IEnumerator WaitAndDecrement(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentConcurrentSFX--;
    }
}

