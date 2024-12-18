using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceBarImageController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    ResourceBarManager resourceManager;
    Weapon currentWeapon;
    private void Start() {
        resourceManager = ResourceBarManager.Instance;
    }

    public void OnPointerExit(PointerEventData eventData) {
        resourceManager.ChangeShouldShowPreview(false);
        resourceManager.UpdateResourceBar(WeaponUIController.Instance.CurrentWeapon.SpendResourceValue);

    }

    public void OnPointerEnter(PointerEventData eventData) {
        var resourceManager = ResourceBarManager.Instance;
        resourceManager.ChangeShouldShowPreview(true);
        resourceManager.UpdateResourceBar(WeaponUIController.Instance.CurrentWeapon.SpendResourceValue);
    }
}
