using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour {
    private static CoroutineManager _instance;

    public static CoroutineManager Instance {
        get {
            if (_instance == null) {
                GameObject obj = new GameObject("CoroutineManager");
                _instance = obj.AddComponent<CoroutineManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    private IEnumerator resourceRecoverCoroutine;

    public void StartResourceRecovering(IEnumerator routine) {
        if (resourceRecoverCoroutine == null) {
            resourceRecoverCoroutine = routine;
            StartCoroutine(routine);
        }
        else if (resourceRecoverCoroutine != routine) {
            StopAllCoroutines();
            resourceRecoverCoroutine = routine;
            StartCoroutine(routine);
        }
    }

    public void StopResourceRecovering(IEnumerator routine) {
        if (resourceRecoverCoroutine != null) {
            StopCoroutine(routine);
            resourceRecoverCoroutine = null;
        }
    }
}
