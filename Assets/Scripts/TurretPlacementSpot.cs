using UnityEngine;

public class TurretPlacementSpot : MonoBehaviour {
    [SerializeField]private bool isOccupied = false;

    public bool IsAvailable() {
        return !isOccupied;
    }

    public void SetOccupied(bool occupied) {
        isOccupied = occupied;
        if (occupied) {
            Debug.Log($"Spot occupied: {gameObject.name}"); // Logging when a spot is occupied

        }else {
            Debug.Log($"Spot vacated: {gameObject.name}"); // Logging when a spot is vacated

        }
    }
}
