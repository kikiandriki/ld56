using System.Collections;
using UnityEngine;

public class TurretBehavior : MonoBehaviour, IDamageable {

    private Transform nexus;

    [Header("Health")]
    [SerializeField]
    [Tooltip("How many hit points does this enemy have?")]
    private int hitPoints = 20;

    [Header("Attacking")]
    [ReadOnly]
    [SerializeField]
    [Tooltip("Should this turret be looking for targets.")]
    private bool shouldAttack = true;
    [SerializeField]
    [Tooltip("Attack range.")]
    private float attackRange = 2f;
    [SerializeField]
    [Tooltip("Attack damage.")]
    private int attackDamage = 10;
    [SerializeField]
    [Tooltip("Attack speed.")]
    private float attackSpeed = 1f;
    [ReadOnly]
    [SerializeField]
    [Tooltip("The target that this enemy is attacking.")]
    private IDamageable attackTarget;
    [SerializeField]
    [Tooltip("Layers to ignore when checking for attacks.")]
    private LayerMask ignoreLayer;

    void Start() {
        nexus = GameObject.FindWithTag("Nexus").transform;
    }

    public bool TakeDamage(int damage) {
        hitPoints -= damage;
        if (hitPoints <= 0) {
            Die();
            return true;
        }
        return false;
    }

    public void Die() {
        StopAttacking();
        StopAllCoroutines();
        GameController controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        if (controller) {
            controller.DestroyTurret(gameObject);
        }
    }

    public void StopAttacking() {
        // Stop attacking.
        attackTarget = null;
        // Start looking for targets.
        shouldAttack = true;
    }

    public void StartAttacking(IDamageable target) {
        // Set the target.
        attackTarget = target;
        // Stop looking for new targets.
        shouldAttack = false;
        // Start attacking.
        StartCoroutine(Attack());
    }

    IEnumerator Attack() {
        // As long as we have a target...
        while (attackTarget != null && !attackTarget.Equals(null)) {
            Debug.Log("Turret Target" + attackTarget);
            // Deal damage.
            attackTarget.TakeDamage(attackDamage);
            // Wait for attack speed cooldown.
            yield return new WaitForSeconds(attackSpeed);
        }
        // Otherwise... look for targets.
        shouldAttack = true;
    }

    void Update() {
        // If we should be looking for new targets...
        if (shouldAttack) {
            // Calculate the direction opposite the nexus.
            Vector3 direction = (nexus.position - transform.position).normalized * -1;
            // Cast a ray to determine if anything is in attack range.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, ~ignoreLayer);
            Debug.DrawRay(transform.position, direction * attackRange, Color.blue);
            // If something is in our range...
            if (hit.collider != null) {
                Debug.Log("Turret Hit: " + hit.collider.name);
                // If the target is damageable...
                if (hit.collider.TryGetComponent<IDamageable>(out var damageable)) {
                    // Start attacking.
                    StartAttacking(damageable);
                }
            }
        }
    }
}
