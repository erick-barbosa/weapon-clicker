using System.Collections;
using UnityEngine;

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

    public virtual bool Attack() {
        return SpendResource(SpendResourceValue);
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
}
