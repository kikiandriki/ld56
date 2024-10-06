using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable {
    [SerializeField]
    [Tooltip("How many hit points does this enemy have?")]
    private int hitPoints = 20;

    public void TakeDamage(int damage) {
        hitPoints -= damage;
        if (hitPoints <= 0) {
            Die();
        }
    }

    public void Die() {
        GameController controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        if (controller) {
            controller.DestroyEnemy(gameObject);
        }
    }
}
