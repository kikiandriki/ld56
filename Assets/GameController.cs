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
}


public class GameController : MonoBehaviour {

    [Header("Spawning Settings")]
    [SerializeField]
    [Tooltip("How many seconds between spawn.")]
    private float spawnRate = 5f;
    [SerializeField]
    [Tooltip("Multiplier of spawn frequency, for stage difficulty.")]
    private float difficultyMultiplier = 1f;
    [SerializeField]
    [Tooltip("Enemy to spawn.")]
    private GameObject enemyPrefab;
    [SerializeField]
    private HotPoints hotPoints;


    void Start() {
        // Start the game immediately.
        StartGame();
    }

    void StartGame() {
        // Start a round.
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound() {
        // Wait for pre-round time.
        yield return new WaitForSeconds(spawnRate);
        // Spawn an enemy.
        SpawnEnemy();
    }

    void SpawnEnemy() {
        // Create the new enemy.
        GameObject newEnemy = Instantiate(enemyPrefab, hotPoints.cEnemy.position, hotPoints.cEnemy.rotation);
        // Log.
        Debug.Log("Spawned enemy", newEnemy);
    }
}
