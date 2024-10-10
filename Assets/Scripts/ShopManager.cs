using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
    public GameObject[] turretPrefabs;  // Turret prefabs for placement
    public Button[] turretButtons;  // UI buttons for each turret
    public int[] turretCosts;  // Costs of each turret
    private int selectedTurretIndex = -1;

    public AcornManager acornManager;

    void Update() {
        // Update the shop to enable/disable buttons based on acorn count
        for (int i = 0; i < turretButtons.Length; i++) {
            if (acornManager.GetCurrentAcorns() >= turretCosts[i]) {
                turretButtons[i].interactable = true;
                turretButtons[i].image.color = Color.white;  // Active
            }
            else {
                turretButtons[i].interactable = false;
                turretButtons[i].image.color = Color.grey;  // Greyed out
            }
        }
    }

    // Called when the player selects a turret from the shop
    public void SelectTurret(int index) {
        if (acornManager.GetCurrentAcorns() >= turretCosts[index]) {
            selectedTurretIndex = index;

            Debug.Log($"Selected turret: {turretPrefabs[index].name}, Cost: {turretCosts[index]} acorns."); // Logging selection

            // Start dragging the turret UI element
            FindObjectOfType<TurretPlacer>().StartPlacingTurret();
            
        } else {
            Debug.LogWarning($"Not enough acorns to select turret: {turretPrefabs[index].name}"); // Logging insufficient acorns

        }
    }

    

    public GameObject GetSelectedTurretPrefab() {
        if (selectedTurretIndex >= 0) {
            return turretPrefabs[selectedTurretIndex];
        }
        return null;
    }

    public int GetSelectedTurretCost() {
        if (selectedTurretIndex >= 0) {
            return turretCosts[selectedTurretIndex];
        }
        return 0;
    }

    public void DeselectTurret() {
        Debug.Log("Turret deselected."); // Logging deselection

        selectedTurretIndex = -1;
    }
}
