using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

//ABSTRACTION
public class Weapon {
    //ENCAPSULATION
    public int MaxResource {
        get => maxResource_;
        protected set {
            maxResource_ = value;
            resource_ = maxResource_;
        }
    }
    private int maxResource_;

    public int Resource {
        get => resource_; 
        protected set {
            resource_ = Mathf.Clamp(value, 0, maxResource_);
        }
    }
    private int resource_;
    public float RecoverTimeSeconds { get; protected set; } = 0.5f;
    public int SpendResourceValue { get; protected set; } = 1;
    public bool IsRecovering{ get; protected set; }

    protected int currentCombo;
    protected float comboResetTime = 1.0f; // Tempo máximo entre ataques do combo
    protected Coroutine resetComboCoroutine;

    public virtual bool Attack() {
        if (SpendResource(SpendResourceValue)) {
            UIStateAnimator animator = WeaponUIController.Instance.stateAnimator;
            CoroutineManager coroutineManager = CoroutineManager.Instance;

            if (currentCombo >= 3) {
                ResetCombo();
            }

            currentCombo++;
            animator.SetNextState();

            if (resetComboCoroutine != null) {
                coroutineManager.StopCoroutine(resetComboCoroutine);
            }
            resetComboCoroutine = coroutineManager.StartCoroutine(ResetComboAfterDelay());
            return true;
        }

        return false;
    }

    public virtual void RecoverResource(int amout) {
        if (Resource + amout <= MaxResource) {
            Resource += amout;
        }
        else {
            Resource = MaxResource;
        }
    }

    protected virtual IEnumerator RecoverResourceOverTime() {
        IsRecovering = true;

        if (Resource == MaxResource) {
            IsRecovering = false;
            CoroutineManager.Instance.StopResourceRecovering(RecoverResourceOverTime());
            yield break;
        }

        while (Resource < MaxResource) {
            yield return new WaitForSeconds(RecoverTimeSeconds);
            RecoverResource(1);
            WeaponUIController.Instance.UpdateResourceExhibition();
        }
    }

    public virtual bool SpendResource(int amount) {
        if (Resource - amount < 0) {
            return false; 
        }

        Resource -= amount;

        WeaponUIController.Instance.UpdateResourceExhibition();
        CoroutineManager.Instance.StartResourceRecovering(RecoverResourceOverTime());

        return true;    
    }
    public virtual void VerifyShouldRecoverOverTime() {
        if (Resource < MaxResource) {
            CoroutineManager.Instance.StartResourceRecovering(RecoverResourceOverTime());
        }
    }

    private IEnumerator ResetComboAfterDelay() {
        yield return new WaitForSeconds(comboResetTime);
        ResetCombo();
    }

    private void ResetCombo() {
        UIStateAnimator animator = WeaponUIController.Instance.stateAnimator;

        animator.ResetState();
        currentCombo = 0;
    }

    public int GetCurrentCombo() {
        return currentCombo;
    }

    public enum Type {
        Instant,
        Charge,
        Constant
    }
}
