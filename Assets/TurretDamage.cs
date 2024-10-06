using System.Collections;
using UnityEngine;

public class TurretDamage : MonoBehaviour {

    private Transform nexus;

    [SerializeField]
    [Tooltip("How much damage does this enemy do.")]
    private int damage = 20;
    [SerializeField]
    [Tooltip("How fast will this enemy attack (seconds).")]
    private float attackSpeed = 1f;

    [SerializeField]
    [Tooltip("Is this turret currently attacking.")]
    private bool isAttacking = false;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            if (!isAttacking) {
                EnemyHealth targetEnemyHealth = other.GetComponent<EnemyHealth>();
                if (targetEnemyHealth) {
                    float td = Vector3.Distance(transform.position, nexus.position);
                    float ed = Vector3.Distance(targetEnemyHealth.transform.position, nexus.position);
                    if (ed > td) {
                        StartCoroutine(Attack(targetEnemyHealth));
                    }
                }
            }
        }
    }

    public void CancelAttack() {
        isAttacking = false;
    }

    void Start() {
        nexus = GameObject.FindWithTag("Nexus").transform;
    }

    IEnumerator Attack(EnemyHealth eh) {
        while (isAttacking) {
            float td = Vector3.Distance(transform.position, nexus.position);
            float ed = Vector3.Distance(eh.transform.position, nexus.position);
            if (ed > td) {
                eh.TakeDamage(damage);
                yield return new WaitForSeconds(attackSpeed);
            }
            else {
                isAttacking = false;
            }
        }
    }
}
