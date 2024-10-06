using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamageable {

    private Transform nexus;
    private Transform originPoint;
    private bool spawned = false;

    [Header("Movement")]
    [ReadOnly]
    [SerializeField]
    [Tooltip("What the enemy should move towards.")]
    private Transform moveTarget;
    [SerializeField]
    [Tooltip("How fast can this enemy move.")]
    private float moveSpeed = 5f;
    [ReadOnly]
    [SerializeField]
    [Tooltip("Whether or not this enemy should be moving.")]
    private bool shouldMove = true;

    [Header("Health")]
    [SerializeField]
    [Tooltip("How many hit points does this enemy have?")]
    private int hitPoints = 20;

    [Header("Attacking")]
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

    public void Terrorize(Transform origin) {
        originPoint = origin;
        spawned = true;
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
            controller.DestroyEnemy(gameObject);
        }
    }

    public void StopAttacking() {
        // Stop attacking.
        attackTarget = null;
        // Start moving.
        shouldMove = true;
    }

    public void StartAttacking(IDamageable target) {
        // Set the target.
        attackTarget = target;
        // Stop moving.
        shouldMove = false;
        // Start attacking.
        StartCoroutine(Attack());
    }

    IEnumerator Attack() {
        // As long as we have a target...
        while (attackTarget != null && !attackTarget.Equals(null)) {
            // Deal damage.
            attackTarget.TakeDamage(attackDamage);
            // Wait for attack speed cooldown.
            yield return new WaitForSeconds(attackSpeed);
        }
        shouldMove = true;
    }

    void Update() {
        // If we should be moving...
        if (shouldMove && spawned) {
            // If we haven't reached the end of the lane...
            if (transform.position != nexus.position) {
                // Calculate the direction to the nexus.
                Vector3 direction = (nexus.position - transform.position).normalized;

                // Determine the target to hit.
                RaycastHit2D h1 = Physics2D.Raycast(originPoint.position, direction, Mathf.Infinity, ~ignoreLayer);
                Debug.DrawRay(originPoint.position, direction * 1000, Color.yellow);
                if (h1.collider) {
                    if (h1.collider.TryGetComponent<IDamageable>(out var damageable)) {
                        moveTarget = h1.transform;
                    }
                }

                if (!moveTarget.Equals(null)) {
                    // Move towards the target.
                    transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, moveSpeed * Time.deltaTime);
                    // Cast a ray to determine if anything is in attack range.
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, ~ignoreLayer);
                    Debug.DrawRay(transform.position, direction * attackRange, Color.red);
                    // If something is in our range...
                    if (hit.collider != null) {
                        Debug.Log("Enemy Hit: " + hit.collider.name);
                        // If the target is damageable...
                        if (hit.collider.TryGetComponent<IDamageable>(out var damageable)) {
                            // Start attacking.
                            StartAttacking(damageable);
                        }
                    }
                }
            }
        }
    }
}
