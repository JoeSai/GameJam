using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private static MonsterSpawner _instance;
    public static MonsterSpawner Instance { get { return _instance; } }

    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float spawnInterval = 2f;
    private float spawnTimer = 0f;

    private int currentWave = 0;
    private int[] waveSpawnCount = { 1, 2, 4, 6, 8 };
    private bool[] spawnPointUsed;

    private int killedMonsterCount;

    private bool isCanSpawn = true;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
        spawnPointUsed = new bool[spawnPoints.Count];
    }

    void Update()
    {
        if (isCanSpawn)
        {
            spawnTimer += Time.deltaTime;
        }
      
        if (spawnTimer >= spawnInterval && isCanSpawn)
        {
            spawnTimer = 0f;

            SpawnMonster();
            isCanSpawn = false;
        }
    }

    public void AddKilledMonsterCount()
    {
        killedMonsterCount++;
        if(killedMonsterCount == waveSpawnCount[currentWave])
        {
            currentWave += 1;
            if (currentWave < waveSpawnCount.Length)
            {
                isCanSpawn = true;
                killedMonsterCount = 0;
            }
            else
            {
                Debug.Log("game over");
            }
        }
    }

    public void SpawnMonster()
    {
        for (int i = 0; i < waveSpawnCount[currentWave]; i++)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            while (spawnPointUsed[spawnIndex])
            {
                spawnIndex = Random.Range(0, spawnPoints.Count);
            }
            GameObject monster = Instantiate(monsterPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
            spawnPointUsed[spawnIndex] = true;
        }

        for (int i = 0; i < spawnPointUsed.Length; i++)
        {
            spawnPointUsed[i] = false;
        }
    }
}
