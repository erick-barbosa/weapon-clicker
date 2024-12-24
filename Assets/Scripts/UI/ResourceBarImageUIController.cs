using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceBarImageUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    ResourceBarUIManager resourceManager;
    Weapon currentWeapon;
    private void Start() {
        resourceManager = ResourceBarUIManager.Instance;
    }

    public void OnPointerExit(PointerEventData eventData) {
        resourceManager.ChangeShouldShowPreview(false);
        resourceManager.UpdateResourceBar(WeaponUIController.Instance.CurrentWeapon.SpendResourceValue);

    }

    public void OnPointerEnter(PointerEventData eventData) {
        var resourceManager = ResourceBarUIManager.Instance;
        resourceManager.ChangeShouldShowPreview(true);
        resourceManager.UpdateResourceBar(WeaponUIController.Instance.CurrentWeapon.SpendResourceValue);
    }
}
