using System.Linq;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class GameController : MonoBehaviour {

    [SerializeField]
    [Tooltip("Should the game auto start? Debug.")]
    private bool autoStartGame = false;

    [Header("Enemy Settings")]
    [SerializeField]
    [Tooltip("Waves to spawn.")]
    private Wave[] waves;
    [SerializeField]
    [Tooltip("Locations where enemies can spawn from.")]
    private Transform[] enemySpawnPoints;
    [ReadOnly]
    [SerializeField]
    [Tooltip("Enemies that have been spawned.")]
    private List<GameObject> enemies;
    // Map for IDs to spawn points.
    private Dictionary<SpawnPointID, Transform> enemySpawnPointMap;

    [Header("Turret Settings")]
    [ReadOnly]
    [SerializeField]
    [Tooltip("Locations where turrets can be placed.")]
    private Transform[] turretPlacePoints;
    [ReadOnly]
    [SerializeField]
    [Tooltip("Turrets that have been placed.")]
    private List<GameObject> turrets;

    [Header("Round Settings")]
    [ReadOnly]
    [SerializeField]
    [Tooltip("The current round being played.")]
    private int currentRound = 0;
    [SerializeField]
    [Tooltip("How much time to wait between rounds.")]
    private float timeBetweenRounds = 10f;
    [ReadOnly]
    [SerializeField]
    [Tooltip("Whether or not a round is currently active.")]
    private bool roundActive = false;
    [SerializeField]
    [Tooltip("Whether or not to automatically end the round when the last enemy has spawned.")]
    private bool autoEndRound = true;

    void Start() {
        // Create the spawn point map.
        enemySpawnPointMap = new Dictionary<SpawnPointID, Transform> {
            { SpawnPointID.SpawnPoint1, enemySpawnPoints[0] },
            { SpawnPointID.SpawnPoint2, enemySpawnPoints[1] },
            { SpawnPointID.SpawnPoint3, enemySpawnPoints[2] },
            { SpawnPointID.SpawnPoint4, enemySpawnPoints[3] },
            { SpawnPointID.SpawnPoint5, enemySpawnPoints[4] }
        };

        // Find all turret place points.
        GameObject[] turretPlaceObjects = GameObject.FindGameObjectsWithTag("TurretPlacePoint");
        turretPlacePoints = turretPlaceObjects.Select(o => o.transform).ToArray();

        // Start the game immediately.
        if (autoStartGame) {
            StartCoroutine(StartGame());
        }
    }

    public void EndRound() {
        roundActive = false;
    }


    IEnumerator WaitForRoundToEnd() {
        while (roundActive && enemies.Count > 0) {
            yield return null;
        }
    }

    IEnumerator StartGame() {
        Debug.Log("Starting game");
        while (currentRound < waves.Length) {
            // Start a round.
            yield return StartCoroutine(StartRound());
            if (autoEndRound) EndRound();
            Debug.Log("Waiting for round to end...");
            // Wait for the round to fully end.
            yield return StartCoroutine(WaitForRoundToEnd());
            // Increment the round counter.
            currentRound++;
            if (currentRound < waves.Length) {
                // Wait for post round timer.
                Debug.Log("Waiting for post round timer...");
                yield return new WaitForSeconds(timeBetweenRounds);
            }
        }
    }

    IEnumerator StartRound() {
        Debug.Log("Round " + (currentRound + 1) + " commencing");
        roundActive = true;

        // Choose the appropriate wave for this round
        Wave currentWave = waves[Mathf.Min(currentRound, waves.Length)];

        // Spawn enemies from the wave's configuration
        foreach (var enemyWave in currentWave.enemyWaves) {
            // Wait for spawn delay.
            yield return new WaitForSeconds(enemyWave.spawnDelay);
            // Pick the spawn point.
            Transform spawnPoint = enemySpawnPointMap[enemyWave.spawnPointID];
            // Spawn enemy at the designated spawn point.
            SpawnEnemy(enemyWave.enemyPrefab, spawnPoint);
        }
    }

    void SpawnEnemy(GameObject enemy, Transform origin) {
        // Pick a random spawn point.
        Vector3 offset = new(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        Vector3 newPosition = origin.position + offset;
        // Create the new enemy.
        GameObject newEnemy = Instantiate(enemy, newPosition, origin.rotation);
        enemies.Add(newEnemy);
    }

    public void DestroyEnemy(GameObject enemy) {
        enemies.Remove(enemy);
        Destroy(enemy);
    }

    public void SpawnTurret() {
        Debug.LogError("Spawning turret not implemented.");
    }

    public void DestroyTurret(GameObject turret) {
        turrets.Remove(turret);
        Destroy(turret);
    }
}
