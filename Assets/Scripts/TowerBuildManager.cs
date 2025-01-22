using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    public GameObject towerPrefab;
    public LayerMask buildableLayer;

    private bool isBuildingMode = false;

    public void ToggleBuildingMode()
    {
        isBuildingMode = !isBuildingMode;
        Debug.Log($"Building mode: {(isBuildingMode ? "ON" : "OFF")}");
    }

    private void Update()
    {
        if (!isBuildingMode || Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log($"{touch.position}");
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildableLayer))
            {
                BuildableObject buildable = hit.collider.GetComponent<BuildableObject>();
                if (buildable != null)
                {
                    buildable.PlaceTower(towerPrefab);
                }
            }
        }
    }
}
