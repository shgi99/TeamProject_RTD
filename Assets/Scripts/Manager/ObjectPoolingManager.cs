using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();
    public void PreloadObjects(string key, GameObject prefab, int amount)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
            prefabDictionary[key] = prefab;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            poolDictionary[key].Enqueue(obj);
        }
    }
    public GameObject GetObject(string key)
    {
        if (poolDictionary.ContainsKey(key))
        {
            if (poolDictionary[key].Count == 0)
            {
                if (prefabDictionary.ContainsKey(key))
                {
                    GameObject newObj = Instantiate(prefabDictionary[key]);
                    newObj.SetActive(true);
                    return newObj;
                }
                else
                {
                    return null;
                }
            }

            GameObject obj = poolDictionary[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }
    public void ReturnObject(string key, GameObject obj)
    {
        obj.SetActive(false);
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }
        poolDictionary[key].Enqueue(obj);
    }
}
