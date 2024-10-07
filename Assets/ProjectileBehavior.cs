using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ProjectileBehavior : MonoBehaviour {

    [Tooltip("Target to move towards.")]
    public Transform target;

    [SerializeField]
    [Tooltip("How fast should the projectile move.")]
    private float projectileSpeed = 2f;

    [Tooltip("The action to run when the target is hit.")]
    public Action OnHitCallback;

    void Update() {
        if (OnHitCallback != null) {
            if (target != null) {
                transform.position = Vector3.MoveTowards(transform.position, target.position, projectileSpeed * Time.deltaTime);

                Vector3 direction = (target.position - transform.position).normalized;
                float distance = gameObject.GetComponent<CircleCollider2D>().radius * transform.localScale.x;
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distance);
                Debug.DrawRay(transform.position, direction * distance, Color.cyan);

                foreach (var hit in hits) {
                    if (hit.collider.gameObject.Equals(target.gameObject)) {
                        OnHitCallback.Invoke();
                        OnHitCallback = null;
                        Destroy(gameObject);
                    }
                }
            }
            else {
                OnHitCallback = null;
                Destroy(gameObject);
            }
        }
    }
}
