using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonChildColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
    private Button button;
    private Image[] childImages;
    private Text[] childTexts;

    void Start() {
        button = GetComponent<Button>();
        childImages = GetComponentsInChildren<Image>();
        childTexts = GetComponentsInChildren<Text>();

        // Set the default color when the game starts
        SetChildColors(button.colors.normalColor);
    }

    // Trigger when the pointer enters the button (hover)
    public void OnPointerEnter(PointerEventData eventData) {
        if (button.interactable) {
            SetChildColors(button.colors.highlightedColor);
        }
    }

    // Trigger when the pointer exits the button (stop hover)
    public void OnPointerExit(PointerEventData eventData) {
        if (button.interactable) {
            SetChildColors(button.colors.normalColor);
        }
    }

    // Trigger when the pointer clicks down on the button
    public void OnPointerDown(PointerEventData eventData) {
        if (button.interactable) {
            SetChildColors(button.colors.pressedColor);
        }
    }

    // Trigger when the pointer releases the button click
    public void OnPointerUp(PointerEventData eventData) {
        if (button.interactable) {
            SetChildColors(button.colors.highlightedColor); // Back to highlighted if still hovering
        }
    }

    // Method to set the color for all child components
    private void SetChildColors(Color color) {
        foreach (Image img in childImages) {
            img.color = color;
        }

        foreach (Text txt in childTexts) {
            txt.color = color;
        }
    }
}
