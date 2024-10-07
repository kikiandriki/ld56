using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamageable {

    private Transform nexus;

    [Header("Movement")]
    [ReadOnly]
    [SerializeField]
    [Tooltip("What the enemy should move towards.")]
    private Transform moveTarget;
    [SerializeField]
    [Tooltip("How fast can this enemy move.")]
    private float moveSpeed = 5f;

    [Header("Health")]
    [SerializeField]
    [Tooltip("How many hit points does this enemy have?")]
    private int hitPoints = 20;

    [Header("Attacking")]
    [SerializeField]
    [Tooltip("Vision range.")]
    private float visionRange = 4f;
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
    [Tooltip("Whether or not this entity can attack.")]
    private bool canAttack = true;
    [ReadOnly]
    [SerializeField]
    [Tooltip("The target that this enemy is attacking.")]
    private IDamageable attackTarget;
    [SerializeField]
    [Tooltip("Layers to target when checking for attacks.")]
    private LayerMask targetLayer;

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
        StopAllCoroutines();
        GameController controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        if (controller) {
            controller.DestroyEnemy(gameObject);
        }
    }

    public void Attack(IDamageable target) {
        // Check that we're in range and attack is not on cooldown.
        MonoBehaviour dob = target as MonoBehaviour;

        Vector3 direction = (dob.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, targetLayer);
        Debug.DrawRay(transform.position, direction * attackRange, Color.yellow);

        if (canAttack && hit.collider && dob.gameObject.Equals(hit.collider.gameObject)) {
            // Deal damage.
            target.TakeDamage(attackDamage);
            // Start attack cooldown.
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }


    /**

    Enemy behavior

    - Default state
        Run at nexus
    
    - Target in vision
        Run at target
    
    - Target in attack range
        Attack target

     */

    void DrawDebugCircle(Vector3 center, float radius, Color color) {
        int segments = 40; // Higher values make a smoother circle
        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++) {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float nextAngle = (i + 1) * angleStep * Mathf.Deg2Rad;

            Vector3 pointA = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius + center;
            Vector3 pointB = new Vector3(Mathf.Cos(nextAngle), Mathf.Sin(nextAngle), 0) * radius + center;

            Debug.DrawLine(pointA, pointB, color);
        }
    }

    void Update() {

        DrawDebugCircle(transform.position, attackRange, Color.red);
        DrawDebugCircle(transform.position, visionRange, Color.blue);

        // If there is a target in attack range.
        if (Physics2D.OverlapCircle(transform.position, attackRange, targetLayer) is Collider2D targetInAttackRange) {
            // If the target is damageable.
            if (targetInAttackRange.GetComponent<IDamageable>() is IDamageable damageable) {
                // Attack the target.
                Attack(damageable);
            }
        }
        // If there is a target in vision.
        else if (Physics2D.OverlapCircle(transform.position, visionRange, targetLayer) is Collider2D targetInVision) {
            // Move towards the target.
            transform.position = Vector3.MoveTowards(transform.position, targetInVision.transform.position, moveSpeed * Time.deltaTime);
        }
        // Default state.
        else {
            // Move towards the nexus.
            transform.position = Vector3.MoveTowards(transform.position, nexus.position, moveSpeed * Time.deltaTime);
        }
    }
}
