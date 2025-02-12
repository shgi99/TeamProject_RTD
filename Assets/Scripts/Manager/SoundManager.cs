using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("오디오 소스")]
    public AudioSource bgmSource;      // BGM을 재생할 AudioSource
    public AudioSource sfxSource;      // 효과음을 재생할 AudioSource

    [Header("오디오 클립")]
    public AudioClip bgmClip;          // 배경음악
    public AudioClip buttonClickSound; // 버튼 클릭 사운드

    private Dictionary<string, AudioClip> towerAttackSounds = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}

