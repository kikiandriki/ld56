using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    private Transform nexus;

    [Header("Movement")]
    [SerializeField]
    [Tooltip("How fast can this enemy move.")]
    private float moveSpeed = 5f;
    [ReadOnly]
    [SerializeField]
    [Tooltip("Whether or not this enemy should be moving.")]
    private bool shouldMove = true;

    [Header("Attacking")]
    [SerializeField]
    [Tooltip("Attack range.")]
    private float attackRange = 2f;
    [SerializeField]
    [Tooltip("Layers to ignore when checking for attacks.")]
    private LayerMask ignoreLayer;

    void OnTriggerEnter2D(Collider2D other) { }

    void Start() {
        nexus = GameObject.FindWithTag("Nexus").transform;
    }

    void Update() {

        Vector3 direction = (nexus.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, ~ignoreLayer);
        Debug.DrawRay(transform.position, direction * attackRange, Color.red);

        if (hit.collider != null) {
            Debug.Log("Hit: " + hit.collider.name);
            shouldMove = false;
        }
        else {
            shouldMove = true;
        }

        if (shouldMove) {
            if (transform.position != nexus.position) {
                transform.position = Vector3.MoveTowards(transform.position, nexus.position, moveSpeed * Time.deltaTime);
            }
        }
    }
}
