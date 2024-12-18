using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponUIController : MonoBehaviour {
    private static WeaponUIController instance_;

    public static WeaponUIController Instance {
        get {
            if (instance_ == null) {
                GameObject obj = new GameObject("CoroutineManager");
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

    [SerializeField] private TextMeshProUGUI resourceText; 
    [SerializeField] private Transform[] weaponBorders;
    [SerializeField] private Sprite[] weaponSprites;
    [SerializeField] private Transform activeBorder;
    [SerializeField] private Image weaponImage;

    void Awake() {
        instance_ = this;

        weaponImage = GetComponent<Image>();

        sword = new Sword(10);
        staff = new Staff(3);
        bow = new Bow(5);

        ChangeWeapon(1);
    }

    public void ChangeWeapon(int weaponTypeIndex) {
        WeaponType weaponType = (WeaponType)weaponTypeIndex;

        currentWeapon = weaponType switch {
            WeaponType.Bow => bow,
            WeaponType.Sword => sword,
            WeaponType.Staff => staff,
            _ => sword,
        };

        weaponImage.sprite = weaponSprites[weaponTypeIndex];
        UpdateActivatedButton(weaponTypeIndex);
        UpdateResourceExhibition();

        currentWeapon.VerifyShouldRecoverOverTime();
    }

    public void OnWeaponClick() {
        if (currentWeapon != null) {
            currentWeapon.Attack();
        }
        else {
            Debug.LogError("CurrentWeapon is not set.");
        }
    }

    public void UpdateResourceExhibition() {
        if (currentWeapon != null) {
            ResourceBarManager.Instance.UpdateResourceBar(CurrentWeapon.SpendResourceValue);
        }
        else {
            Debug.LogError("CurrentWeapon is not set when updating resource text.");
        }
    }

    private void UpdateActivatedButton(int index) {
        if (activeBorder != null) {
            activeBorder.transform.parent.GetComponent<UISpriteAnimator>().ShouldAnimateExternal = true;
            activeBorder.gameObject.SetActive(false);
        }
        activeBorder = weaponBorders[index].transform;
        activeBorder.gameObject.SetActive(true);
        activeBorder.transform.parent.GetComponentInParent<UISpriteAnimator>().ShouldAnimateExternal = false;
        activeBorder.transform.parent.GetComponent<UISpriteAnimator>().ResetSpriteAnimation();
    }

    public enum WeaponType {
        Bow,
        Sword,
        Staff
    }
}
