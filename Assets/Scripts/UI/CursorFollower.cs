using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollower : MonoBehaviour {
    private static CursorFollower instance_;

    public static CursorFollower Instance {
        get {
            if (instance_ == null) {
                GameObject obj = new GameObject("CursorFollower");
                instance_ = obj.AddComponent<CursorFollower>();
                DontDestroyOnLoad(obj);
            }
            return instance_;
        }
    }

    private void Awake() {
        instance_ = this;
    }

    private void Update() {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 1);
    }
}
