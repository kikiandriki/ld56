using System.Collections;
using UnityEngine;


[System.Serializable]
public class HotPoints {
    public Transform a1;
    public Transform a2;
    public Transform a3;
    public Transform aEnemy;

    public Transform b1;
    public Transform b2;
    public Transform b3;
    public Transform bEnemy;

    public Transform c1;
    public Transform c2;
    public Transform c3;
    public Transform cEnemy;

    public Transform d1;
    public Transform d2;
    public Transform d3;
    public Transform dEnemy;

    public Transform e1;
    public Transform e2;
    public Transform e3;
    public Transform eEnemy;

    public Transform[] EnemySpawns {
        get {
            return new Transform[] {
                aEnemy,
                bEnemy,
                cEnemy,
                dEnemy,
                eEnemy
            };
        }
    }
}


public class GameController : MonoBehaviour {

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
    private HotPoints hotPoints;

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
        StartCoroutine(StartGame());
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
        int randomIndex = Random.Range(0, hotPoints.EnemySpawns.Length);
        Transform randomSpawn = hotPoints.EnemySpawns[randomIndex];
        // Create the new enemy.
        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawn.position, randomSpawn.rotation);
    }
}
