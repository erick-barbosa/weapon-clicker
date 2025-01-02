using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIController : MonoBehaviour {
    private static WeaponUIController instance_;

    public static WeaponUIController Instance {
        get {
            if (instance_ == null) {
                GameObject obj = new GameObject("Weapon");
                instance_ = obj.AddComponent<WeaponUIController>();
                DontDestroyOnLoad(obj);
            }
            return instance_;
        }
    }

    public Weapon CurrentWeapon { get => currentWeapon; set => currentWeapon = value; }
    private Weapon currentWeapon;

    private Sword sword;
    private Bow bow;
    private Staff staff;

    [SerializeField] private Transform[] weaponBorders;
    [SerializeField] private Sprite[] weaponSprites;
    [SerializeField] private Transform activeBorder;
    [SerializeField] private Image weaponImage;
    public AudioSource weaponSoundSource;
    public List<AudioClip> weaponSoundClips;
    public UIStateAnimator stateAnimator;

    void Awake() {
        instance_ = this;

        weaponImage = GetComponent<Image>();
        stateAnimator = GetComponent<UIStateAnimator>();
        weaponSoundSource = GetComponent<AudioSource>();

        sword = new Sword();
        staff = new Staff();
        bow = new Bow();

        ChangeWeapon(1);
    }

    public void ChangeWeapon(int weaponTypeIndex) {
        WeaponName weaponType = (WeaponName)weaponTypeIndex;

        currentWeapon = weaponType switch {
            WeaponName.Bow => bow,
            WeaponName.Sword => sword,
            WeaponName.Staff => staff,
            _ => sword,
        };

        UpdateActivatedButton(weaponTypeIndex);
        UpdateAnimationSets(weaponTypeIndex);
        UpdateResourceExhibition();

        currentWeapon.VerifyShouldRecoverOverTime();
    }

    public void OnWeaponClick() {
        if (currentWeapon != null) {
            if (currentWeapon.Attack()) {
                weaponSoundSource.PlayOneShot(weaponSoundClips[currentWeapon.GetCurrentCombo() - 1]);
                Enemy.Instance.TakeDamage(currentWeapon.SpendResourceValue);
            }
        }
        else {
            Debug.LogError("CurrentWeapon is not set.");
        }
    }

    private void UpdateAnimationSets(int weaponTypeIndex) {
        var animationHolder = weaponBorders[weaponTypeIndex].transform.parent.GetComponent<WeaponAnimationSetHolder>();

        stateAnimator.SetAnimation(animationHolder.animations, animationHolder.states); 
        weaponSoundClips = animationHolder.attackSounds;
    }

    public void UpdateResourceExhibition() {
        if (currentWeapon != null) {
            ResourceBarUIManager.Instance.UpdateResourceBar(CurrentWeapon.SpendResourceValue);
        }
        else {
            Debug.LogError("CurrentWeapon is not set when updating resource text.");
        }
    }

    private void UpdateActivatedButton(int index) {
        if (activeBorder != null) {
            activeBorder.gameObject.SetActive(false);
        }
        activeBorder = weaponBorders[index].transform;
        activeBorder.gameObject.SetActive(true);
    }

    public enum WeaponName {
        Bow,
        Sword,
        Staff
    }
}
