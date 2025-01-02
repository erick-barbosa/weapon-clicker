using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpriteAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private Image targetImage; 
    [SerializeField] private Sprite[] animationFrames; 
    [SerializeField] private Sprite mainFrameSprite; 

    [SerializeField] private float frameRate = 20f; 
    [SerializeField] private float scaleMultiplier = 1.1f; 
    [SerializeField] private bool hasNoConditions = true; 
    [SerializeField] private bool shouldAnimateExternal = true;
    public bool ShouldAnimateExternal { 
        get => shouldAnimateExternal;
        set {
            shouldAnimateExternal = value;
        }
    }

    private int currentFrame;
    private float timer;
    private bool justActivated = true;
    private bool shouldAnimate = false;

    private void OnEnable() {
        justActivated = true;
        Invoke(nameof(ResetActivationFlag), 0.1f); 
    }

    private void ResetActivationFlag() {
        justActivated = false;
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
            if (!justActivated) {
                SoundManager.Instance.PlayHoverEffect();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        ResetSpriteAnimation();
    }

    public void ResetSpriteAnimation() {
        shouldAnimate = false;
        gameObject.transform.localScale = Vector3.one;
        currentFrame = 0;
        targetImage.sprite = mainFrameSprite;
    }
}
