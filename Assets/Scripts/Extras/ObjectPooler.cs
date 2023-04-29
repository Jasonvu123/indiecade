using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    //[SerializeField] private GameObject enemy2;
    //[SerializeField] private GameObject enemy3;
    [SerializeField] private int poolSize = 10;
    private List<GameObject> _pool;
    private GameObject _poolContainer;

    private void Awake()
    {
        _pool = new List<GameObject>();
        _poolContainer = new GameObject($"Pool - {prefab.name}");
        CreatePooler();
    }
    private void CreatePooler()
    {
        /*
        for (int i = 0; i < poolSize; i++)
        {
            _pool.Add(CreateInstance(enemy1));
        }
        for (int i = 0; i < poolSize; i++)
        {
            _pool.Add(CreateInstance(enemy2));
        }
        for (int i = 0; i < poolSize; i++)
        {
            _pool.Add(CreateInstance(enemy3));
        }
        */
                for (int i = 0; i < poolSize; i++)
        {
            _pool.Add(CreateInstance());
        }
    }
    private GameObject CreateInstance()
    {
        GameObject newInstance = Instantiate(prefab);
        newInstance.transform.SetParent(_poolContainer.transform);
        newInstance.SetActive(false);
        return newInstance;
    }
/*
    private GameObject getPrefab(int i)
    {
        if(_pool[i] == enemy1)
        {
            return enemy1;
        }
        else if(_pool[i] == enemy2)
        {
            return enemy2;
        }
        else 
        {
            return enemy3;
        }
    }
*/
    public GameObject GetInstanceFromPool()
    {
        int i = 0;
        for (i = 0; i < _pool.Count; i++)
        {
            
            if (!_pool[i].activeInHierarchy)
            {
                return _pool[i];
            }
            
           // return CreateInstance(getPrefab(i));
        }
        return CreateInstance();
        
    }
    public static void ReturnToPool(GameObject instance)
    {
        instance.SetActive(false);
    }
    public static IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
    }
}
