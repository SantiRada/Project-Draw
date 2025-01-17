using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [Header("Stats")]
    public Transform initialPos;
    public GameObject[] spawnpointEnemies;
    public Vector2 minPosCamera, maxPosCamera;

    [Header("Data Room")]
    public Vector2 timeBetweenEnemies;
    public Vector2 timeToCreateEnemies;
    [HideInInspector] public bool canAdvance = false;
    private int totalWeight = 0;
    private List<Enemy> enemiesToCreate = new List<Enemy>();
    private List<Enemy> enemiesAlive = new List<Enemy>();
    [Space]
    [HideInInspector] public ProgressManager _progressManager;

    private void Start()
    {
        DetectorRoom detector = GetComponentInChildren<DetectorRoom>();
        detector._currentRoom = this;

        _progressManager = FindAnyObjectByType<ProgressManager>();
        ProceduralMap pMap = _progressManager.GetMap();

        if (pMap != null) {
            enemiesToCreate = pMap.typeEnemies;
            totalWeight = pMap.totalWeight;
        }

        StartCoroutine("CreateWaves");
    }
    private void Update()
    {
        if (enemiesAlive.Count <= 0) { if (totalWeight <= 0) { canAdvance = true; } }
    }
    private IEnumerator CreateWaves()
    {
        while (totalWeight > 0)
        {
            StartCoroutine("SpawnEnemies");

            float timeAwait = Random.Range(timeToCreateEnemies.x, timeToCreateEnemies.y);
            yield return new WaitForSeconds(timeAwait);
        }
    }
    private IEnumerator SpawnEnemies()
    {
        int value = CalculateCountToCreated();

        for (int i = 0; i < value; i++)
        {
            if (totalWeight <= 0) break;

            int numEnemy = Random.Range(0, enemiesToCreate.Count);

            Enemy enemy = Instantiate(enemiesToCreate[numEnemy], spawnpointEnemies[Random.Range(0, spawnpointEnemies.Length)].transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy._myRoom = this;
            totalWeight -= enemy.weight;

            enemiesAlive.Add(enemy);

            float timeBetween = Random.Range(timeBetweenEnemies.x, timeBetweenEnemies.y);
            yield return new WaitForSeconds(timeBetween);
        }
    }
    private int CalculateCountToCreated()
    {
        if (totalWeight <= 0) return 0;

        int count = Random.Range(2, 5);

        return count;
    }
    public void DestroyEnemy(Enemy enemy)
    {
        enemiesAlive.Remove(enemy);

        if (enemiesAlive.Count <= 0) StartCoroutine("SpawnEnemies");
    }
}
