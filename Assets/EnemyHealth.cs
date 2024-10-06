using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    [Tooltip("How many hit points does this enemy have?")]
    private int hitPoints = 20;

    public void TakeDamage(int damage) {
        hitPoints -= damage;
    }

    void Update() {
        if (hitPoints <= 0) {
            // TODO: destroy and animate death.
            gameObject.SetActive(false);
        }
    }
}
