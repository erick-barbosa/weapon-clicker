using UnityEngine;

public class SoundManager : MonoBehaviour {
    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip hoverSound;
    private static SoundManager instance_;

    public static SoundManager Instance {
        get {
            if (instance_ == null) {
                GameObject obj = new GameObject("CoroutineManager");
                instance_ = obj.AddComponent<SoundManager>();
                DontDestroyOnLoad(obj);
            }
            return instance_;
        }
    }


    private void Awake() {
        instance_ = this;
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlayHoverEffect() {
        m_AudioSource.PlayOneShot(hoverSound);
    }
}
