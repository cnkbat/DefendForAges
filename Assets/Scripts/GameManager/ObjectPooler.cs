using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler instance { get; private set; }
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public bool isObjPoolingActive;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }
    #endregion

    void Start()
    {
        if (!isObjPoolingActive) return;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {

                GameObject spawnedObj = Instantiate(pool.prefab);

                objectPool.Enqueue(spawnedObj);
                spawnedObj.SetActive(false);

            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 spawnPos, GameObject fireTarget = null)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("object spawn error with " + tag);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);

        objectToSpawn.transform.position = spawnPos;
        
        
        if (objectToSpawn.TryGetComponent(out Bullet bullet))
        {
            bullet.GetComponent<Bullet>().SetTarget(fireTarget);
        }
        
        if (objectToSpawn.TryGetComponent(out IPoolableObject pooled))
        {
            pooled.OnObjectPooled();
        }


        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
