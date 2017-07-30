using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPooler : MonoBehaviour
{

    public static GameObjectPooler current;
    public GameObject pooledObject;
    public int poolSize = 100;
    public bool allowGrow = true;

    List<GameObject> objectPool;

    private void Awake()
    {
        current = this;
		objectPool = new List<GameObject>();

		for (int i = 0; i < poolSize; i++)
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.SetActive(false);
			objectPool.Add(obj);
		}
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }

        if (allowGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            return obj;
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
