using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private static MonsterSpawner _instance;
    public static MonsterSpawner Instance { get { return _instance; } }

    [SerializeField] private List<GameObject> monsterPrefabs;
    [SerializeField] private List<GameObject> bossPrefabs;

    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float spawnInterval = 5f;
    private float spawnTimer = 0f;

    private int currentWave = 0;
    private int[] waveSpawnCount = { 1, 2, 4, 6, 8 };
    private bool[] spawnPointUsed;

    private int killedMonsterCount;

    private bool isCanSpawn = true;
    private bool isInfiniteMode = false;
    private bool isBossMode = false;
    private float spawnIntervalDecrement = 0.1f;  // 每一波生成间隔减少的时间
    [SerializeField] private float spawnIntervalLimit = 1.8f;

    private float spawnBossInterval = 10;
    private float spawnBossDuration = 0;
    private bool isSpawndBoss = false;

    private List<MonsterScript> bossScripts;

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

        if (GameManager.isTEST)
        {
            SpawnTestMonster();
        }

        bossScripts = new List<MonsterScript>(bossPrefabs.Count);
    }

    void Update()
    {
        if (GameManager.isTEST)
        {
            return;
        }
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }

        if (isInfiniteMode)
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer >= spawnInterval)
            {
                spawnTimer = 0;
                SpawnOneMonster();
                if(spawnInterval < 4f)
                {
                    SpawnOneMonster();
                }
                if (spawnInterval < 3f)
                {
                    SpawnOneMonster();
                }
                if (spawnInterval < 2f)
                {
                    SpawnOneMonster();
                }

                spawnInterval -= spawnIntervalDecrement;
                if(spawnInterval < spawnIntervalLimit)
                {
                    isInfiniteMode = false;
                    isBossMode = true;
                }
            }

            return;
        }

        if(isBossMode)
        {
            if (!isSpawndBoss)
            {
                spawnBossDuration += Time.deltaTime;
                if(spawnBossDuration > spawnBossInterval)
                {
                    SpawnBossMonster();
                    isSpawndBoss = true;
                }
                return;
            }
            bool isAllBossDied = true;
            for (int i = 0; i < bossScripts.Count; i++)
            {
                if (bossScripts[i].GetHealth() > 0)
                {
                    isAllBossDied = false;
                    break;
                }
            }
            if (isAllBossDied)
            {
                GameManager.Instance.GameOver();
            }

            return;
        }

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
        GameManager.Instance.UpdateScore();
        if (isInfiniteMode || isBossMode)
        {
            return;
        }

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
                isInfiniteMode = true;
                //
            }

            CreateItem();
        }
    }

    private void CreateItem()
    {
        if(currentWave == 3 || currentWave == 5){
            ItemGenerator.Instance.GenerateItem();
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
            GameObject monster = Instantiate(monsterPrefabs[0], spawnPoints[spawnIndex].position, Quaternion.identity);
            spawnPointUsed[spawnIndex] = true;
        }

        for (int i = 0; i < spawnPointUsed.Length; i++)
        {
            spawnPointUsed[i] = false;
        }
    }

    public void SpawnOneMonster()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Count);
        int prefabIndex = Random.Range(0, monsterPrefabs.Count);
        GameObject monster = Instantiate(monsterPrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
    }

    public void SpawnBossMonster()
    {
        int spawnIndex = 1;

        for (int i = 0; i < bossPrefabs.Count; i++)
        {
            GameObject monster = Instantiate(bossPrefabs[i], spawnPoints[spawnIndex].position, Quaternion.identity);
            MonsterScript monsterScript = monster.GetComponent<MonsterScript>();
            monsterScript.SetHealth(10);
            spawnIndex++;

            bossScripts.Add(monsterScript);
        }
    }

public void SpawnTestMonster()
    {
        GameObject monster = Instantiate(monsterPrefabs[1], spawnPoints[0].position, Quaternion.identity);
        monster.GetComponent<MonsterScript>().SetHealth(12);

        GameObject monster1 = Instantiate(monsterPrefabs[1], spawnPoints[1].position, Quaternion.identity);
        monster.GetComponent<MonsterScript>().SetHealth(120);
    }
}
