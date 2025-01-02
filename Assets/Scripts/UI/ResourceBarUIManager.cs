using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourceBarUIManager : MonoBehaviour {
    [SerializeField] private Sprite fullSprite; 
    [SerializeField] private Sprite emptySprite; 
    [SerializeField] private Sprite previewSprite; 
    [SerializeField] private Transform barParent;
    [SerializeField] private GameObject divisionPrefab;
    [SerializeField] private int maxBarDivisions = 10;

    private List<GameObject> resourceObjects = new List<GameObject>();

    private static ResourceBarUIManager instance_;
    private bool shouldShowPreview;

    public static ResourceBarUIManager Instance {
        get {
            if (instance_ == null) {
                GameObject obj = new GameObject("CoroutineManager");
                instance_ = obj.AddComponent<ResourceBarUIManager>();
                DontDestroyOnLoad(obj);
            }
            return instance_;
        }
    }

    private void Awake() {
        instance_ = this;
    }

    void Start() {
        InitializeResources();
    }

    private void InitializeResources() {
        for (int i = 0; i < maxBarDivisions; i++) {
            GameObject resource = Instantiate(divisionPrefab, barParent);
            resource.SetActive(false);
            resourceObjects.Add(resource);
        }

        UpdateResourceBar(0);
    }

    public void UpdateResourceBar(int actualResourceCost) {
        var currentWeapon = WeaponUIController.Instance.CurrentWeapon;
        int currentResourceValue = currentWeapon.Resource;
        int maxResourceForWeapon = currentWeapon.MaxResource;
        int maxPossible = maxResourceForWeapon > maxBarDivisions ? maxBarDivisions : maxResourceForWeapon;

        for (int i = 0; i < resourceObjects.Count; i++) {
            var resource = resourceObjects[i];
            resource.SetActive(i < maxPossible); // Ativar apenas se estiver dentro do limite máximo

            if (resource.activeSelf) {
                // Alterar o sprite dependendo do recurso estar cheio ou vazio
                var image = resource.GetComponentInChildren<Image>();
                image.sprite = i < currentResourceValue ? fullSprite : emptySprite;
            }
        }

        if (shouldShowPreview) {
            ShowResourcePreview(actualResourceCost);
        }
    }

    public void ShowResourcePreview(int resourceCost) {
        var currentWeapon = WeaponUIController.Instance.CurrentWeapon;
        int currentResourceValue = currentWeapon.Resource;
        int previewResource = currentResourceValue - resourceCost;

        if (previewResource < 0) {
            return;
        }

        for (int i = 0; i < resourceObjects.Count; i++) {
            var resource = resourceObjects[i];
            if (!resource.activeSelf)
                continue;

            var image = resource.GetComponentInChildren<Image>();

            // Exibe previewSprite apenas para os recursos que seriam gastos
            if (i >= previewResource && i < currentResourceValue) {
                image.sprite = previewSprite;
            }
            else {
                image.sprite = i < currentResourceValue ? fullSprite : emptySprite;
            }
        }
    }

    public void ChangeShouldShowPreview(bool shouldShow) {
        shouldShowPreview = shouldShow;
    }
}
