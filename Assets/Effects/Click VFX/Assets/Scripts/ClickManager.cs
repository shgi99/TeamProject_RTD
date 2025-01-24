using UnityEngine.VFX;
using UnityEngine;
public class ClickManager : MonoBehaviour
{
    //This is a simple script that does a raycast and plays the VFX on a prefab
    public GameObject VFXPrefab;
    VisualEffect[] visualEffects;
    void Start()
    {
        visualEffects = VFXPrefab.GetComponentsInChildren<VisualEffect>();
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                VFXPrefab.transform.position = hit.point;
                for (int i = 0; i < visualEffects.Length; i++)
                {
                    visualEffects[i].SendEvent("Click");
                }
            }
        }
    }
}
