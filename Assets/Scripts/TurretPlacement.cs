using UnityEngine;

public class TurretPlacer : MonoBehaviour {
    public LayerMask placementLayerMask;  // Layer for turret placement points
    private ShopManager shopManager;
    private AcornManager acornManager;
    private bool isPlacingTurret = false;
    private GameObject turretVisual; // Reference to the currently active turret visual


    void Start() {
        shopManager = FindObjectOfType<ShopManager>();
        acornManager = FindObjectOfType<AcornManager>();
    }

    void Update() {
        if (isPlacingTurret) {
            HandleTurretPlacement();
            ShowTurretVisual();
        }
    }

    public void StartPlacingTurret() {
        Debug.Log("TurretPlacement.cs Started placing turret. "); // Logging placement start

        isPlacingTurret = true;
        InstantiateTurretVisual(); // Create the turret visual when starting placement

    }
    void InstantiateTurretVisual() {
        GameObject selectedTurretPrefab = shopManager.GetSelectedTurretPrefab();
        if (selectedTurretPrefab != null) {
            turretVisual = Instantiate(selectedTurretPrefab, Vector3.zero, Quaternion.identity); // Instantiate at a dummy position
            turretVisual.GetComponent<Collider2D>().enabled = false; // Disable collider if necessary
        }
    }

    void ShowTurretVisual() {
        if (turretVisual != null) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            turretVisual.transform.position = mousePos; // Update turret visual position
        }
    }

    void HandleTurretPlacement() {
        // Get mouse position in world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Update the visual of the selected turret (dragging effect)
        // Display the turret sprite under the cursor while it's being placed

        if (Input.GetMouseButtonDown(0)) // Left-click to place
        {
            // Check if the position is valid 
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos, placementLayerMask);

            if (hitCollider != null && hitCollider.GetComponent<TurretPlacementSpot>().IsAvailable()) {
                // Place the turret if valid
                
                Debug.Log($"Placing turret at position: {mousePos}"); 

                PlaceTurret(mousePos);
            }
            else {
                Debug.LogWarning("Invalid placement location or spot occupied."); 

            }
        }
        else if (Input.GetMouseButtonDown(1)) //right click to deselect 
            {
            
            DeselectTurret();


        }


    }

    void DeselectTurret() {
        //stop placing turret and destroy sprite
        if (turretVisual != null) {
            Destroy(turretVisual);
            turretVisual = null;
        }
        shopManager.DeselectTurret();
        isPlacingTurret=false;

    }


    void PlaceTurret(Vector2 position) {
        GameObject selectedTurretPrefab = shopManager.GetSelectedTurretPrefab();
        int turretCost = shopManager.GetSelectedTurretCost();

        if (selectedTurretPrefab != null && acornManager.GetCurrentAcorns() >= turretCost) {
            // Instantiate the turret at the position
            Instantiate(selectedTurretPrefab, position, Quaternion.identity);

            // Deduct the cost from acorns
            acornManager.SpendAcorns(turretCost);
            Debug.Log($"Placed turret: {selectedTurretPrefab.name} at {position}. Cost deducted: {turretCost} acorns."); // Logging successful placement


            // Mark the placement spot as occupied
            Collider2D hitCollider = Physics2D.OverlapPoint(position, placementLayerMask);
            if (hitCollider != null) {
                hitCollider.GetComponent<TurretPlacementSpot>().SetOccupied(true);
            }

            // Stop placing the turret
            shopManager.DeselectTurret();
            isPlacingTurret = false;
        }else {
            Debug.LogWarning("Unable to place turret: either prefab is null or not enough acorns."); // Logging unsuccessful placement

        }
    }
}
