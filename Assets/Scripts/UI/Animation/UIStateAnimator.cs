using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStateAnimator : MonoBehaviour {
    [SerializeField] private int state = 0;
    private int maxState;
    private Image stateImage;
    [SerializeField] private List<Sprite> stateSprites = new List<Sprite>();
    [SerializeField] private List<AnimationSet> animationFrames = new List<AnimationSet>();
    [SerializeField] private float frameRate;
    private bool isAnimating = false;
    [SerializeField] private bool shouldWaitAnimationFinish = true;
    [SerializeField] private bool shouldAnimateOnStart = true;

    private void OnEnable() {
        stateImage = GetComponent<Image>();
    }

    private void Awake() {
        maxState = stateSprites.Count - 1;
        if (shouldAnimateOnStart) {
            AnimateAll();
        }
    }

    public void SetAnimation(List<AnimationSet> animationSet, List<Sprite> animationSprites) {
        animationFrames = animationSet;
        stateSprites = animationSprites;
        ResetState();
    }

    public void SetNextState() {
        if (shouldWaitAnimationFinish && isAnimating) {
            return;
        }

        if (state <= maxState) {
            OnChangeState();
        }
    }
    
    public void SetStateTemporary() {
        SetNextState();
        StartCoroutine(ResetStateAfterAnimation());
    }

    public void SetPreviousState() {
        if (state - 1 > 0 && !isAnimating) {
            //OnChangeState();
            state--;
        }
    }

    public void ResetState() {
        state = 0;
        StartCoroutine(ChangeState());
        isAnimating = false;        
    }

    private IEnumerator ChangeState() {
        yield return new WaitForEndOfFrame();
        stateImage.sprite = stateSprites[state];
    }

    private IEnumerator ResetStateAfterAnimation() {
        // Aguarde até que a animação termine
        while (isAnimating) {
            yield return null;
        }

        // Reseta o estado
        Debug.Log("Reset");
        ResetState();
    }

    private void OnChangeState() {
        if (animationFrames.Count == 0) {
            Debug.Log("Missing animations for: " + gameObject.name);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(AnimateForward());
    }

    private void AnimateAll() {
        StartCoroutine(AnimateAllCoroutine());
    }

    private IEnumerator AnimateAllCoroutine() {
        isAnimating = true;
        yield return new WaitForEndOfFrame();

        for (int i = state; i < maxState; i++) {
            yield return StartCoroutine(AnimateForward()); // Aguarda o término de Animate antes de continuar
        }

        isAnimating = false;
    }

    private IEnumerator AnimateForward() {
        isAnimating = true;
        int currentFrame = 0;

        while (currentFrame < animationFrames[state].frames.Count) {
            if (currentFrame < 0 || currentFrame >= animationFrames[state].frames.Count) {
                break;
            }

            stateImage.sprite = animationFrames[state].frames[currentFrame];
            currentFrame++;

            yield return new WaitForSeconds(1f / frameRate);
        }

        if (state + 1 < animationFrames.Count) {
            state++;
        }

        isAnimating = false;
        yield break;
    }
}
