using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] private PoolStruct[] objects;
    [SerializeField] private List<Transform> spawns;
    [SerializeField] private List<Transform> usedSpawns;

    [SerializeField] private int maxObjInScene;
    [SerializeField] private int enabledObjects;

    [SerializeField] private float spawnRate;
    [SerializeField] private float enqueueTime;

    private Queue<GameObject> pool;
    private Coroutine spawnObjects;

    private void Awake()
    {
        pool = new Queue<GameObject>();

        for (int objectIndex = 0; objectIndex < objects.Length; objectIndex++)
        {
            for (int availableObjects =0; availableObjects < objects[objectIndex].cantidad; availableObjects++)
            {
                GameObject instance = Instantiate(objects[objectIndex].prefab, RandomPosition().position, Quaternion.identity);
                instance.transform.SetParent(transform);
                instance.SetActive(false);
                pool.Enqueue(instance);
            }
        }
        spawnObjects = StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        yield return null;
    }

    private Transform RandomPosition()
    {
        int randomSpawnPoint = UnityEngine.Random.Range(0, spawns.Count);
        usedSpawns.Add(spawns[randomSpawnPoint]);
        spawns.RemoveAt(randomSpawnPoint);

        return spawns[randomSpawnPoint];

    }
    public void ReturnToList(Transform spawn)
    {
        spawns.Add(spawn);
        usedSpawns.Remove(spawn);

        StopCoroutine(spawnObjects);
        spawnObjects=StartCoroutine(SpawnObjects());
    }

    
}
