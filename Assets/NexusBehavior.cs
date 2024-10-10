using System.Collections;
using System.Linq;
using UnityEngine;

public class NexusBehavior : MonoBehaviour, IDamageable {

    private Transform nexus;
    public LayerMask placementLayerMask;  // Layer for turret placement points


    [Header("Health")]
    [SerializeField]
    [Tooltip("How many hit points does this enemy have?")]
    private int hitPoints = 20;
    [SerializeField]
    [Tooltip("Where should damage indicators spawn.")]
    private Transform damageIndicatorSpawn;
    [SerializeField]
    [Tooltip("Prefab for damage indicator.")]
    private GameObject damageIndicatorPrefab;

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
    [Tooltip("Whether or not this entity can attack.")]
    private bool canAttack = true;
    [ReadOnly]
    [SerializeField]
    [Tooltip("The target that this enemy is attacking.")]
    private IDamageable attackTarget;
    [SerializeField]
    [Tooltip("Layers to target when checking for attacks.")]
    private LayerMask targetLayer;
    [SerializeField]
    [Tooltip("Projectile prefab to spawn when attacking.")]
    private GameObject projectilePrefab;


    SceneGuy sg;

    void Start() {
        nexus = GameObject.FindWithTag("Nexus").transform;
        sg = GameObject.FindWithTag("SceneGuy").GetComponent<SceneGuy>();
    }

    public bool TakeDamage(int damage) {
        // Already dead.
        if (gameObject.layer == 9) return true;

        hitPoints -= damage;

        GameObject damageIndicator = Instantiate(
            damageIndicatorPrefab,
            damageIndicatorSpawn.position,
            damageIndicatorSpawn.rotation,
            damageIndicatorSpawn
        );

        if (damageIndicator.GetComponent<DamageIndicatorBehavior>() is DamageIndicatorBehavior dib) {
            dib.SetText("-" + damage);
        }

        if (hitPoints <= 0) {
            Die();
            return true;
        }
        return false;
    }

    public void Die() {
        sg.LoadQuit();
    }

    public void Attack(IDamageable target) {
        // Check that we're in range and attack is not on cooldown.
        MonoBehaviour dob = target as MonoBehaviour;

        Vector3 direction = (dob.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, targetLayer);
        Debug.DrawRay(transform.position, direction * attackRange, Color.yellow);

        if (canAttack && hit.collider && dob.gameObject.Equals(hit.collider.gameObject)) {

            // Spawn a projectile that will deal damage once it hits the target.
            ProjectileBehavior projectile = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<ProjectileBehavior>();
            projectile.target = dob.transform;
            projectile.OnHitCallback = () => {
                target.TakeDamage(attackDamage);
            };

            // Start attack cooldown.
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }

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

        Collider2D[] targetsInAttackRange = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayer);

        // If there is a target in attack range.
        if (targetsInAttackRange != null && targetsInAttackRange.Length > 0) {

            // Prioritize the target closest to the nexus.
            Collider2D targetInAttackRange = targetsInAttackRange.OrderBy(collider => Vector2.Distance(collider.transform.position, nexus.position)).FirstOrDefault();

            // If the target is damageable.
            if (targetInAttackRange.GetComponent<IDamageable>() is IDamageable damageable) {
                // Attack the target.
                Attack(damageable);
            }
        }
    }
}
