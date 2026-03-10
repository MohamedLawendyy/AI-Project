using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // This creates a nice clean box in the Unity Inspector to set up each wave
    [System.Serializable]
    public class Wave
    {
        public string waveName;             // Name of the wave (e.g., "Wave 1")
        public GameObject[] enemyPrefabs;   // Put your 3 enemy prefabs here
        public int enemyCount;              // Total enemies to spawn this wave
        public float spawnRate;             // How fast they spawn (e.g., 1 = 1 per second, 0.5 = 1 every 2 seconds)
    }

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [Header("Wave Settings")]
    public Wave[] waves;                    // List of all your waves
    public float timeBetweenWaves = 5f;     // Time to rest before the next wave starts

    [Header("Spawn Locations")]
    public Transform[] spawnPoints;         // Empty GameObjects placed around the map

    private int nextWave = 0;               // Tracks which wave we are on
    private float waveCountdown;            // Timer before next wave
    private float searchCountdown = 1f;     // Timer to check if enemies are dead
    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced! Please assign them in the Inspector.");
        }

        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        // 1. If we are waiting for the player to kill the enemies
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                // All enemies are dead!
                WaveCompleted();
            }
            else
            {
                return; // Enemies still alive, do nothing
            }
        }

        // 2. If it is time to start the next wave
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                // Start spawning the next wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            Debug.Log("ALL WAVES COMPLETE! YOU WIN!");
            this.enabled = false; // Stop the script, the game is over
            // TODO: Call your Win Screen manager here
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        // We only check once per second to save computer memory (better performance)
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;

            // Checks the scene for any GameObject with the tag "Enemy"
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false; // No enemies found, they must be dead
            }
        }
        return true; // Enemies found!
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.waveName);
        state = SpawnState.SPAWNING;

        // Loop to spawn the correct amount of enemies
        for (int i = 0; i < _wave.enemyCount; i++)
        {
            // Pick a random enemy prefab from the list you gave this wave
            GameObject randomEnemy = _wave.enemyPrefabs[Random.Range(0, _wave.enemyPrefabs.Length)];
            SpawnEnemy(randomEnemy);

            // Wait before spawning the next one
            yield return new WaitForSeconds(1f / _wave.spawnRate);
        }

        // Finished spawning all enemies for this wave
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(GameObject _enemy)
    {
        // 1. Pick a random spawn point
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 2. Spawn the enemy
        GameObject newEnemy = Instantiate(_enemy, randomSpawnPoint.position, randomSpawnPoint.rotation);

        // --- NEW: LINK THE TARGET TO BEHAVIOR BRICKS ---
        // We find the player in the scene
        GameObject player = GameObject.FindWithTag("Player");

        // We find the Behavior Executor on the new enemy
        var executor = newEnemy.GetComponent<BehaviorExecutor>();

        if (executor != null && player != null)
        {
            // This line manually sets the "target" variable in the Behavior Tree!
            executor.SetBehaviorParam("target", player);
        }
        // -----------------------------------------------

        // 3. TERRAIN FIX: Force the enemy to snap to the NavMesh
        UnityEngine.AI.NavMeshAgent agent = newEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(newEnemy.transform.position, out hit, 5.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }
    }
}