using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("����� �ҽ�")]
    public AudioSource bgmSource;      // BGM�� ����� AudioSource
    public AudioSource sfxSource;      // ȿ������ ����� AudioSource

    [Header("����� Ŭ��")]
    public AudioClip bgmClip;          // �������
    public AudioClip buttonClickSound; // ��ư Ŭ�� ����

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

