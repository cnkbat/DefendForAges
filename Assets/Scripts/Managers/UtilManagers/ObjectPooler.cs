using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }



    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public bool isObjPoolingActive;

    #region Singleton

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

    public GameObject SpawnFromPool(string tag, Vector3 spawnPos)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("object spawn error with " + tag);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);

        objectToSpawn.transform.position = spawnPos;

        if (objectToSpawn.TryGetComponent(out IPoolableObject pooled))
        {
            pooled.OnObjectPooled();
        }


        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    #region Enemy

    public GameObject SpawnEnemyFromPool(string tag, Vector3 spawnPos, Transform newParent)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("object spawn error with " + tag);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.parent = newParent;
        objectToSpawn.SetActive(true);

        objectToSpawn.transform.position = spawnPos;

        if (objectToSpawn.TryGetComponent(out IPoolableObject pooled))
        {
            pooled.OnObjectPooled();
        }


        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    #endregion

    #region Bullet

    public GameObject SpawnBulletFromPool(string tag, Vector3 spawnPos, Transform fireTarget = null, float damage = 0, bool isPlayersBullet = false)
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
            bullet.SetTarget(fireTarget);
            bullet.SetDamage(damage);
            bullet.SetIsPlayerBullet(isPlayersBullet);
        }

        if (objectToSpawn.TryGetComponent(out IPoolableObject pooled))
        {
            pooled.OnObjectPooled();
        }


        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    #endregion

    #region Visuals

    public GameObject SpawnFloatingTextFromPool(string tag, Vector3 spawnPos, float damageValue, float fontSize, Color textColor)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("object spawn error with " + tag);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);

        objectToSpawn.transform.position = spawnPos;

        if (objectToSpawn.TryGetComponent(out IPoolableObject pooled))
        {
            pooled.OnObjectPooled();
        }


        if (objectToSpawn.TryGetComponent(out FloatingTextAnimation floatingText))
        {
            floatingText.SetText(damageValue.ToString());
            floatingText.SetTextColor(textColor);
        }


        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    #endregion
}
