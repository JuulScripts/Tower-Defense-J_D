using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    private GameUIManager uiManager;
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
    private bool allWavesSpawned = false;
    private bool transitioningToNextLevel = false;

    private int currentWave = 0;

    private void Start()
    {
        uiManager = FindFirstObjectByType<GameUIManager>();
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
        allWavesSpawned = true;
    }
    private void Update()
    {
        if (allWavesSpawned && !transitioningToNextLevel)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                transitioningToNextLevel = true;
                if (uiManager != null)
                {
                    uiManager.ShowLevelCompleteMessage();
                }
                StartCoroutine(LoadNextLevelDelayed(3f)); 
            }
        }
    }
    private IEnumerator LoadNextLevelDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Level1Scene")
        {
            PlayerHandling.ResetMoney(400);
            SceneManager.LoadScene("Level2Scene");
        }
        else if (currentScene == "Level2Scene")
        {
            GameResult.SetResult(true);
            SoundManager.Instance.PlaySFX(SoundManager.Instance.victorySound);
            SceneManager.LoadScene("StartScene");
        }
    }
}
