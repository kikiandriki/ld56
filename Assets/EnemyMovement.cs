using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    private GameObject nexus;

    [SerializeField]
    private float moveSpeed = 5f;

    void Start() {
        nexus = GameObject.FindWithTag("Nexus");
    }

    void Update() {
        if (transform.position != nexus.transform.position) {
            transform.position = Vector3.MoveTowards(transform.position, nexus.transform.position, moveSpeed * Time.deltaTime);
        }
    }
}
