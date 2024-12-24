using System.Collections;
using UnityEngine;

//INHERITANCE
public class Bow : Weapon {
    public Bow() {
        MaxResource = 5;
        SpendResourceValue = 2;
    }

    private int currentCombo;
    private float comboResetTime = 1.0f; // Tempo máximo entre ataques do combo
    private Coroutine resetComboCoroutine;
    public override bool Attack() {
        if (base.Attack()) {

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

    private IEnumerator ResetComboAfterDelay() {
        yield return new WaitForSeconds(comboResetTime);
        ResetCombo();
    }

    private void ResetCombo() {
        UIStateAnimator animator = WeaponUIController.Instance.stateAnimator;

        animator.ResetState();
        currentCombo = 0;
    }
}
