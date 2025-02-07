using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;
[ExecuteInEditMode]
public class PreviewEvent : MonoBehaviour
{
    public string eventName = "Click";
    public float delay = 2f;
    VisualEffect[] visualEffects;

    void OnValidate()
    {
        visualEffects = GetComponentsInChildren<VisualEffect>();
    }
    void Start()
    {
        if (visualEffects.Length > 0)
         {
             InvokeRepeating("TriggerEvent", 0f, delay);
         }
    }

    void TriggerEvent()
    {
        for (int i = 0; i < visualEffects.Length; i++)
        {
            visualEffects[i].GetComponent<VisualEffect>().SendEvent(eventName);
            //Debug.Log("Event");
        }
    }
}
