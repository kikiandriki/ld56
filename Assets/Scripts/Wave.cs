using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewWave", menuName = "Wave System/Wave")]
public class Wave : ScriptableObject {
    
    [System.Serializable]
    public class EnemyWave {
        public GameObject enemyPrefab;
        public SpawnPointID spawnPointID; // Use the Enum instead of Transform
        public float spawnDelay;
    }

    public EnemyWave[] enemyWaves;
}
public enum SpawnPointID {
    SpawnPoint1,
    SpawnPoint2,
    SpawnPoint3,
    SpawnPoint4,
    SpawnPoint5
}
