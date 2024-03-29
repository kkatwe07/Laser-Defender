﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;


        IEnumerator Start()
        {
            do
            {
                yield return StartCoroutine(SpawnAllWaves());
            }
            while (looping);
        }

        IEnumerator SpawnAllWaves()
        {
            for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
            {
                var currentWave = waveConfigs[waveIndex];
                yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            }
        }
        IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
        {
            for (int enemyCount = 0; enemyCount < waveConfig.getNoOfEnemies(); enemyCount++)
            {
                var newEnemy = Instantiate(
                    waveConfig.getEnemyPrefab(),
                    waveConfig.getWaypoints()[0].transform.position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                yield return new WaitForSeconds(waveConfig.getTimeBetweenSpawns());
            }
        }
    }

