using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AcornManager : MonoBehaviour {
    [SerializeField]
    private int currentAcorns = 12;

    [SerializeField]
    private TMP_Text acornUIText; // UI element to display the acorn count

    public void CollectAcorn(int amount) {
        currentAcorns += amount;
        UpdateAcornUI();
    }

    private void UpdateAcornUI() {
        if (acornUIText != null) {
            acornUIText.text = "Acorns: " + currentAcorns;
        }
    }
}
