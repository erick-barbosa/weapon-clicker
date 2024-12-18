using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpriteAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private Image targetImage; // O componente Image que exibe o sprite
    [SerializeField] private Sprite[] animationFrames; // Array de sprites para a animação
    [SerializeField] private Sprite starterFrameSprite; // Array de sprites para a animação
    [SerializeField] private float frameRate = 20f; // Taxa de quadros por segundo
    [SerializeField] private float scaleMultiplier = 1.1f; // Taxa de quadros por segundo
    [SerializeField] private bool hasNoConditions = true; 
    private bool shouldAnimate = false;
    [SerializeField] private bool shouldAnimateExternal = true;
    public bool ShouldAnimateExternal { 
        get => shouldAnimateExternal;
        set {
            shouldAnimateExternal = value;
        }
    }

    private int currentFrame;
    private float timer;

    private void Start() {
        starterFrameSprite = GetComponent<Image>().sprite;
    }

    private void Update() {
        if (hasNoConditions) {
            Animate();
        }
        else {
            if (shouldAnimate) {
                Animate();
            }
        }
    }

    private void Animate() {
        if (animationFrames.Length == 0 || targetImage == null)
            return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate) {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            targetImage.sprite = animationFrames[currentFrame];
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (ShouldAnimateExternal) {
            shouldAnimate = true;
            gameObject.transform.localScale = Vector3.one * scaleMultiplier;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        ResetSpriteAnimation();
    }

    public void ResetSpriteAnimation() {
        shouldAnimate = false;
        gameObject.transform.localScale = Vector3.one;
        currentFrame = 0;
        targetImage.sprite = starterFrameSprite;
    }
}
