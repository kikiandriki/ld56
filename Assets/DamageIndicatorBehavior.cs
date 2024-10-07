using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class DamageIndicatorBehavior : MonoBehaviour {

    [SerializeField]
    private float lerpSpeed = 1f;
    private float lerpProgress = 0f;

    [ReadOnly]
    [SerializeField]
    private float initialOpacity;
    [SerializeField]
    private float desiredOpacity;

    [ReadOnly]
    [SerializeField]
    private float initialY;
    [SerializeField]
    private float desiredLift;
    private float desiredY;

    private TextMeshPro tmp;

    void Awake() {
        tmp = gameObject.GetComponent<TextMeshPro>();
        Renderer r = tmp.GetComponent<Renderer>();
        r.sortingLayerName = "Indicators";
        r.sortingOrder = 10;
        initialOpacity = tmp.alpha;
        initialY = transform.localPosition.y;
        desiredY = initialY + desiredLift;
        StartCoroutine(KillSelf());
    }

    public void SetText(string text) {
        tmp.text = text;
    }

    IEnumerator KillSelf() {
        yield return new WaitForSeconds(lerpSpeed + 1f);
        Destroy(gameObject);
    }

    void Update() {
        if (lerpProgress < 1f) {
            lerpProgress += lerpSpeed * Time.deltaTime;

            tmp.alpha = Mathf.Lerp(initialOpacity, desiredOpacity, lerpProgress);
            float newY = Mathf.Lerp(initialY, desiredY, lerpProgress);
            transform.localPosition = new Vector2(transform.localPosition.x, newY);
        }
    }
}
