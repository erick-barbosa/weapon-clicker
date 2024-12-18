using System.Collections;
using UnityEngine;

//INHERITANCE
public class Bow : Weapon {
    public Bow(int resourceAmount) {
        MaxResource = resourceAmount;
        SpendResourceValue = 2;
    }

    //POLYMORPHISM
    public override void Attack() {
        SpendResource(SpendResourceValue);
    }

    public override void RecoverResource(int amout) {
        if (Resource + amout <= MaxResource) {
            Resource += amout;
        }
        else {
            Resource = MaxResource;
        }
    }

    protected override IEnumerator RecoverResourceOverTime() {
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

    public override void SpendResource(int amount) {
        if (Resource - amount >= 0) {
            Resource -= amount;
        }
        else {
            return;
        }

        WeaponUIController.Instance.UpdateResourceExhibition();
        CoroutineManager.Instance.StartResourceRecovering(RecoverResourceOverTime());
    }
    public override void VerifyShouldRecoverOverTime() {
        if (Resource < MaxResource) {
            CoroutineManager.Instance.StartResourceRecovering(RecoverResourceOverTime());
        }
    }
}
