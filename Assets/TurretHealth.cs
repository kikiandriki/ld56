using UnityEngine;

public class TurretHealth : MonoBehaviour {

    [SerializeField]
    [Tooltip("How many hit points does this turret have?")]
    private int hitPoints = 100;

    public void TakeDamage(int damage) {
        hitPoints -= damage;
    }

    void Update() {
        if (hitPoints <= 0) {
            gameObject.SetActive(false);
        }
    }
}
