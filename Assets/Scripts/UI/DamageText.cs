using UnityEngine;
using TMPro; // Certifique-se de ter o TextMeshPro importado

public class DamageText : MonoBehaviour {
    public GameObject damageTextPrefab; // Arraste seu prefab aqui pelo Inspector
    public Transform canvasTransform; // Arraste o Canvas aqui pelo Inspector

    void Update() {
        if (Input.GetMouseButtonDown(0)) // Clique do mouse
        {
            Vector3 mousePosition = Input.mousePosition;
        }
    }

    public void ShowDamageText(Vector3 position, int damage) {
        // Converter a posi��o do clique da tela para a posi��o do mundo
        Vector3 mousePos = CursorFollower.Instance.transform.position;
        // Instanciar o texto na posi��o do mundo
        GameObject instance = Instantiate(damageTextPrefab, mousePos, Quaternion.identity, canvasTransform);
        TextMeshProUGUI tmp = instance.GetComponent<TextMeshProUGUI>();
        tmp.text = damage.ToString();
        Destroy(instance, 1.5f); // Destroi o texto depois de 1.5 segundos
    }
}
