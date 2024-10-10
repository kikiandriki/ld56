using UnityEngine;
using System.Collections;

public class AcornDrop : MonoBehaviour {
    [SerializeField]
    private int acornValue = 12; // Value of acorns when collected

    [SerializeField]
    private float arcHeight = 1f; // Height of the arc
    [SerializeField]
    private float arcDuration = 1.5f; // How long the arc lasts before the acorn stops
    [SerializeField]
    private float gravity = -9.8f; // Simulated gravity force

    private AcornManager acornManager;
    private Vector3 initialVelocity; // Velocity for the arc
    private bool isLanded = false; // To stop movement after landing

    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start() {


        acornManager = FindObjectOfType<AcornManager>();

        // Add 2D Collider if not already attached
        if (GetComponent<Collider2D>() == null) {
            gameObject.AddComponent<CircleCollider2D>();
        }

        // Set initial random direction for the arc
        startPosition = transform.position;
        float randomX = Random.Range(-1f, 1f); // Random horizontal direction
        float randomZ = Random.Range(-1f, 1f); // Random horizontal direction

        targetPosition = startPosition + new Vector3(randomX, 0, randomZ); // Where the acorn should land

        // Calculate initial velocity for the arc
        float distance = Vector3.Distance(startPosition, targetPosition);
        float velocityY = Mathf.Sqrt(-2 * gravity * arcHeight); // Vertical speed to reach peak of the arc
        float timeToPeak = Mathf.Abs(velocityY / gravity); // Time to reach peak
        float totalTime = timeToPeak * 2; // Time to complete the arc

        float horizontalSpeed = distance / totalTime;
        initialVelocity = new Vector3(randomX * horizontalSpeed, velocityY, randomZ * horizontalSpeed);

        StartCoroutine(MoveInArc());
    }

    void OnMouseDown() {
        if (isLanded) {
            acornManager.CollectAcorn(acornValue);
            Destroy(gameObject); // Remove the drop after collecting
        }
    }

    private IEnumerator MoveInArc() {
        float elapsedTime = 0f;

        while (elapsedTime < arcDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / arcDuration;

            // Calculate position over time for a parabolic arc
            Vector3 horizontalMovement = Vector3.Lerp(startPosition, targetPosition, t);
            float verticalMovement = startPosition.y + initialVelocity.y * t + 0.5f * gravity * t * t;

            // Update acorn position
            transform.position = new Vector3(horizontalMovement.x, verticalMovement, horizontalMovement.z);

            yield return null;
        }

        // Once landed, stop the movement
        isLanded = true;
       // transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z); // Snap to ground level
    }
}
