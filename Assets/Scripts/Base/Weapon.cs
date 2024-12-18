using System.Collections;
using UnityEngine;

//ABSTRACTION
public abstract class Weapon {
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

    public abstract void Attack();
    public abstract void SpendResource(int amount);
    public abstract void RecoverResource(int amout);
    protected abstract IEnumerator RecoverResourceOverTime();
    public abstract void VerifyShouldRecoverOverTime();
}
