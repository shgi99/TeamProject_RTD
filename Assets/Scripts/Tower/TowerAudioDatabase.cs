using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerAudioDatabase", menuName = "ScriptableObjects/TowerAudioDatabase", order = 1)]
public class TowerAudioDatabase : ScriptableObject
{
    [SerializedDictionary("AudioID", "AudioClip")]
    public SerializedDictionary<string, AudioClip> audioDictionary;
}