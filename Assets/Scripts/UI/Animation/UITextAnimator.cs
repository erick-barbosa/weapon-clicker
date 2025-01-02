using System.Collections;
using TMPro;
using UnityEngine;

public class UITextAnimator : MonoBehaviour {
    [SerializeField] private float timeToDisappear = 5;
    [SerializeField] private float maxY = 150;
    [SerializeField] private float minY = 80;
    [SerializeField] private float maxX = 50;
    [SerializeField] private float minX = -50;

    private TextMeshProUGUI text;

    private Vector3 randomTargetPos;
    private Vector3 initialPosition;

    private bool willBeDestroyed = false;

    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        var rangeX = Random.Range(transform.localPosition.x + minX, transform.localPosition.x + maxX);
        var rangeY = Random.Range(transform.localPosition.y + minY, transform.localPosition.y + maxY);
        randomTargetPos = new Vector3(rangeX, rangeY, 1);
        initialPosition = transform.localPosition;

        StartCoroutine(Animate());
        StartCoroutine(StartLifetime());
    }

    private IEnumerator StartLifetime() {
        yield return new WaitForSeconds(timeToDisappear);
        willBeDestroyed = true;
        StopAllCoroutines();
        Destroy(gameObject);
    }
    private IEnumerator Animate() {
        float elapsedTime = 0f;
        while (!willBeDestroyed) {
            yield return null;
            
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / timeToDisappear; // Atualiza a posição e o alpha
            transform.localPosition = Vector3.Lerp(initialPosition, randomTargetPos, t); 
            text.alpha = Mathf.Lerp(1f, 0f, t); 
            
            if (elapsedTime >= timeToDisappear) { 
                willBeDestroyed = true; Destroy(gameObject); 
            }
        } 
    }        
}
