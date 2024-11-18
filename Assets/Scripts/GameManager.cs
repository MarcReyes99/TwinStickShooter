using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    private int currentRound = 1;
    private int enemiesToSpawn = 10;
    private List<GameObject> activeEnemies = new List<GameObject>();


    void Start()
    {
        StartRound();
    }

    void Update()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
            }
        }

        if (activeEnemies.Count == 0)
        {
            StartNextRound();
        }
    }

    private void StartRound()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            activeEnemies.Add(enemy);
        }
    }

    private void StartNextRound()
    {
        currentRound++;
        enemiesToSpawn = Mathf.CeilToInt(enemiesToSpawn * 1.2f);
        StartRound();
    }
}
