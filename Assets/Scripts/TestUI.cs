using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public List<GameObject> towers;
    public List<GameObject> patricles;
    public List<GameObject> projectiles;
    
    public void OnClickTower()
    {
        int randomIdx = Random.Range(0, towers.Count);
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-4f, 2f));
        Instantiate(towers[randomIdx], randomPos, Quaternion.identity);
    }
    public void OnClickParticle()
    {
        int randomIdx = Random.Range(0, patricles.Count);
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-4f, 2f));
        Instantiate(patricles[randomIdx], randomPos, Quaternion.identity);
    }
    public void OnClickProjectiles()
    {
        int randomIdx = Random.Range(0, projectiles.Count);
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-4f, 2f));
        Instantiate(projectiles[randomIdx], randomPos, Quaternion.identity);
    }
}
