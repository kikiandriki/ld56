using System.Collections;
using UnityEngine;


public class GameController : MonoBehaviour {

    [SerializeField]
    [Tooltip("Should the game auto start? Debug.")]
    private bool autoStartGame = false;

    [Header("Spawning Settings")]
    [SerializeField]
    [Tooltip("How many seconds between spawn.")]
    private float spawnRate = 1f;
    [SerializeField]
    [Tooltip("Multiplier of spawn frequency, for stage difficulty.")]
    private float difficultyMultiplier = 1f;
    [SerializeField]
    [Tooltip("Enemy to spawn.")]
    private GameObject enemyPrefab;
    [SerializeField]
    [Tooltip("Base amount of enemies to spawn in a round.")]
    private int baseEnemiesToSpawn = 2;
    [SerializeField]
    [Tooltip("Locations where enemies can spawn from.")]
    private Transform[] enemySpawnPoints;
    [SerializeField]
    [Tooltip("Locations where turrets can be placed.")]
    private Transform[] turretPlacePoints;

    [Header("Round Settings")]
    [SerializeField]
    [Tooltip("How much time to wait between rounds.")]
    private float timeBetweenRounds = 10f;
    [SerializeField]
    [Tooltip("How many rounds will be played. 0 for infinite.")]
    private int numberOfRounds = 0;
    [SerializeField]
    [Tooltip("Whether or not a round is currently active.")]
    private bool roundActive = false;
    [SerializeField]
    [Tooltip("Whether or not to automatically end the round when the last enemy has spawned.")]
    private bool autoEndRound = true;

    void Start() {
        // Start the game immediately.
        if (autoStartGame) {
            StartCoroutine(StartGame());
        }
    }

    public void EndRound() {
        roundActive = false;
    }


    IEnumerator WaitForRoundToEnd() {
        while (roundActive) {
            yield return null;
        }
    }

    IEnumerator StartGame() {

        bool doNextRound = true;
        int currentRound = 1;

        while (doNextRound) {
            // Start a round.
            yield return StartCoroutine(StartRound());
            if (autoEndRound) EndRound();
            // Wait for the round to fully end.
            yield return StartCoroutine(WaitForRoundToEnd());
            // Increment the round counter.
            currentRound++;
            // Check if we should run another round.
            doNextRound = numberOfRounds < 1 || currentRound <= numberOfRounds;
            if (doNextRound) {
                // Wait for post round timer.
                yield return new WaitForSeconds(timeBetweenRounds);
                // Increase the difficulty multiplier.
                difficultyMultiplier += currentRound * 0.2f;
            }
        }
    }

    IEnumerator StartRound() {
        roundActive = true;

        int enemiesToSpawn = Mathf.RoundToInt(baseEnemiesToSpawn * difficultyMultiplier);

        while (enemiesToSpawn > 0) {
            // Spawn an enemy.
            SpawnEnemy();
            enemiesToSpawn -= 1;
            // Wait for spawn rate.
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnEnemy() {
        // Pick a random spawn point.
        int randomIndex = Random.Range(0, enemySpawnPoints.Length);
        Transform randomSpawn = enemySpawnPoints[randomIndex];
        // Create the new enemy.
        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawn.position, randomSpawn.rotation);
    }
}
