using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemySpawnPoint> spawners;

    void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        if (Bear5.spawned)
        {
            spawners[0].canSpawn = false;
        }

        foreach (EnemySpawnPoint esp in spawners)
        {
            if (!time_manager.instance.isDay && esp.canSpawn)
            {
                StartCoroutine(SpawnEnemies(esp));
            }
        }
    }

    public IEnumerator SpawnEnemies(EnemySpawnPoint esp)
    {
        esp.canSpawn = false;

        for (int i = 0; i < esp.spawnPoints.Count; i++)
        {
            Instantiate(esp.enemy, esp.spawnPoints[i].position, Quaternion.identity);
        }

        yield return new WaitForSeconds(esp.respawnTime);

        esp.canSpawn = true;
    }

    void ChangeRespawnTime(int enemyID)
    {
        foreach (EnemySpawnPoint esp in spawners)
        {
            float newCooldown = esp.respawnTime * 0.8f;
            esp.respawnTime = newCooldown;
            break;
        }
    }
}

[System.Serializable]
public class EnemySpawnPoint
{
    public GameObject enemy;
    public List<Transform> spawnPoints;
    public float respawnTime;
    public bool canSpawn = true;
}
