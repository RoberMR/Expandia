using UnityEngine;
using UnityEngine.UI;

public class BackpackSlotUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private Image icon;

    [Header("Sprites")]
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    private void Start()
    {
        if (icon == null)
            icon = GetComponent<Image>();

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
        if (icon == null)
            return;

        bool hasBackpack = PlayerInventoryUpgrades.Instance.backpackLevel > 0;
        icon.sprite = hasBackpack ? activeSprite : inactiveSprite;
    }
}