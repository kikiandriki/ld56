using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerv2 : MonoBehaviour {
    [SerializeField]
    [Tooltip("Should the game auto start? Debug.")]
    private bool autoStartGame = false;

    [Header("Spawning Settings")]
    [SerializeField]
    private Wave[] waves;  // Add an array of Wave objects for different rounds
    
    [SerializeField]
    private Transform[] spawnPoints; // An array of scene spawn points

    private Dictionary<SpawnPointID, Transform> spawnPointMap;



    [SerializeField]
    [Tooltip("Locations where turrets can be placed.")]
    private Transform[] turretPlacePoints;

    [Header("Round Settings")]
    [SerializeField]
    private float timeBetweenRounds = 10f;
    [SerializeField]
    private int numberOfRounds = 0;
    [SerializeField]
    private bool roundActive = false;
    [SerializeField]
    private bool autoEndRound = true;

    private int currentRound = 0;  // Keep track of the current round index

    void Start() {

        // Initialize the spawn point dictionary, mapping IDs to actual scene spawn points

        spawnPointMap = new Dictionary<SpawnPointID, Transform>
      {
            { SpawnPointID.SpawnPoint1, spawnPoints[0] },
            { SpawnPointID.SpawnPoint2, spawnPoints[1] },
            { SpawnPointID.SpawnPoint3, spawnPoints[2] },
            { SpawnPointID.SpawnPoint4, spawnPoints[3] },
            { SpawnPointID.SpawnPoint5, spawnPoints[4] }
        };




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
        currentRound = 1;

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
                // Wait for post-round timer.
                yield return new WaitForSeconds(timeBetweenRounds);
            }
        }
    }

    IEnumerator StartRound() {
        roundActive = true;

        // Choose the appropriate wave for this round
        Wave currentWave = waves[Mathf.Min(currentRound - 1, waves.Length - 1)];

        // Spawn enemies from the wave's configuration
        foreach (var enemyWave in currentWave.enemyWaves) {
            yield return new WaitForSeconds(enemyWave.spawnDelay);

            Transform spawnPoint = spawnPointMap[enemyWave.spawnPointID];


            // Spawn enemy at the designated spawn point
            Instantiate(enemyWave.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
