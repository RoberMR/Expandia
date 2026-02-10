using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] private ToolType slotToolType;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private ToolIconDatabase iconDatabase;

    private void Start()
    {
        SetActive(false);
        PlayerToolManager.Instance.OnToolChanged += OnToolChanged;
    }

    private void OnToolChanged(ToolType type, EquippedTool tool)
    {
        if (type != slotToolType) return;

        if (tool != null)
            icon.sprite = iconDatabase.GetIcon(type, tool.material);
        else
            icon.sprite = inactiveSprite;
    }

    public void SetActive(bool active)
    {
        if (!active) icon.sprite = inactiveSprite;
    }
}