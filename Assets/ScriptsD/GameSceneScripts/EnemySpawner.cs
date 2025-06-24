using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
    {
        public GameObject enemyPrefab;
        public int amount;
        public float spawnInterval;
    }

    public Transform spawnPoint;
    public GameObject[] waypoints;
    public List<Wave> waves = new List<Wave>();

    private int currentWave = 0;

    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (currentWave < waves.Count)
        {
            Wave wave = waves[currentWave];

            for (int i = 0; i < wave.amount; i++)
            {
                GameObject enemy = Instantiate(wave.enemyPrefab, spawnPoint.position, Quaternion.identity);

                EnemyBehaviour eb = enemy.GetComponent<EnemyBehaviour>();
                if (eb != null)
                {
                    eb.SetWaypoints(waypoints);
                    eb.enabled = true;
                }

                yield return new WaitForSeconds(wave.spawnInterval);
            }

            currentWave++;
            yield return new WaitForSeconds(8f);
        }
    }
}
