using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyAttack : MonoBehaviour {

    [SerializeField]
    [Tooltip("How much damage does this enemy do.")]
    private int damage = 20;
    [SerializeField]
    [Tooltip("How fast will this enemy attack (seconds).")]
    private float attackSpeed = 1f;

    EnemyMovement enemyMovement;
    Transform nexus;

    void Start() {
        enemyMovement = gameObject.GetComponent<EnemyMovement>();
        nexus = GameObject.FindWithTag("Nexus").transform;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Hit", other.gameObject);
        if (other.gameObject.CompareTag("Turret")) {
            float enemyToNexus = Vector2.Distance(transform.position, nexus.position);
            float towerToNexus = Vector2.Distance(other.transform.position, nexus.position);
            if (enemyToNexus > towerToNexus) {
                // enemyMovement.shouldMove = false;
                // Attack continuously.
                TurretHealth th = other.GetComponent<TurretHealth>();
                StartCoroutine(Attack(th));
            }
        }
    }

    IEnumerator Attack(TurretHealth th) {
        // while (!enemyMovement.shouldMove) {
        // th.TakeDamage(damage);
        yield return new WaitForSeconds(attackSpeed);
        // }
    }

    void OnTriggerExit2D(Collider2D other) {
        Debug.Log("Exit", other.gameObject);
        if (other.gameObject.CompareTag("Turret")) {
            // enemyMovement.shouldMove = true;
        }
    }
}
