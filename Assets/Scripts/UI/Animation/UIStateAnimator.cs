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

    private void Awake() {
        stateImage = GetComponent<Image>();
        maxState = stateSprites.Count - 1;
        if (state == 5) {
            AnimateAll();
        }
    }

    public void SetAnimationFrames(List<AnimationSet> animations) {
        animationFrames = animations;
    }
    public void SetAnimationStates(List<Sprite> animations) {
        stateSprites = animations;
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
    public void SetPreviousState() {
        if (state - 1 > 0 && !isAnimating) {
            //OnChangeState();
            state--;
        }
    }
    public void ResetState() {
        state = 0;
        stateImage.sprite = stateSprites[state];
        isAnimating = false;
    }

    private void OnChangeState() {
        if (animationFrames.Count == 0) {
            Debug.Log("Missing animations for: " + gameObject.name);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(Animate());
    }
    private void AnimateAll() {
        StartCoroutine(AnimateAllCoroutine());
    }

    private IEnumerator AnimateAllCoroutine() {
        isAnimating = true;
        yield return new WaitForEndOfFrame();

        for (int i = state; i < maxState; i++) {
            yield return StartCoroutine(Animate()); // Aguarda o término de Animate antes de continuar
        }

        isAnimating = false;
    }

    private IEnumerator Animate() {
        isAnimating = true;
        int currentFrame = 0;

        Debug.Log("animate");
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
