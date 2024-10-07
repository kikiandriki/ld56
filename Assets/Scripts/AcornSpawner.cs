using System.Collections;
using UnityEngine;

public class AcornDropSpawner : MonoBehaviour {
    [SerializeField]
    private GameObject acornDropPrefab; // Acorn drop prefab

    [SerializeField]
    private Transform treePosition; // The tree position to spawn near

    [SerializeField]
    private float spawnRadius = 2f; // The radius around the tree for drops to appear

    [SerializeField]
    private float spawnInterval = 5f; // Time between drops

    void Start() {
        StartCoroutine(SpawnAcorns());
    }

    private IEnumerator SpawnAcorns() {
        while (true) {
            yield return new WaitForSeconds(spawnInterval);
            SpawnAcornDrop();
        }
    }

    private void SpawnAcornDrop() {
        Vector3 randomPosition = treePosition.position + (Vector3)(Random.insideUnitCircle * spawnRadius);
        Instantiate(acornDropPrefab, randomPosition, Quaternion.identity);
    }
}