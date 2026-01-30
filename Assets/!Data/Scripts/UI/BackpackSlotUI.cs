using UnityEngine;

public class BackpackSlotUI : MonoBehaviour
{
    [SerializeField] EquipmentSlotUI slot;

    private void Start()
    {
        ResourceManager.Instance.OnResourceChanged += OnResourceChanged;

        Refresh();
    }

    private void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= OnResourceChanged;
    }

    private void OnResourceChanged(ResourceType type, int current, int max)
    {
        Refresh();
    }

    public void Refresh()
    {
        slot.SetActive(
            PlayerInventoryUpgrades.Instance.backpackLevel > 0
        );
    }
}