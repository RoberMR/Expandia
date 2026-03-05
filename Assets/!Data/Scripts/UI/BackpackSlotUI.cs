using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackpackSlotUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private Image icon;

    [Header("Sprites")]
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    [Header("Info Panel")]
    public GameObject toolInfoPanel;
    [SerializeField] private Image toolInfoPanelIcon;
    [SerializeField] private TextMeshProUGUI toolInfoPanelNameText;
    [SerializeField] private TextMeshProUGUI toolInfoPanelDescriptionText;

    [Header("Items List")]
    [SerializeField] private ItemsList itemsList;

    private void Start()
    {
        if (icon == null)
            icon = GetComponent<Image>();

        toolInfoPanel.SetActive(false);

        ResourceManager.Instance.OnResourceChanged += OnResourceChanged;
        PlayerInventoryUpgrades.Instance.OnBackpackLevelChanged += OnBackpackLevelChanged;

        Refresh();
    }

    private void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= OnResourceChanged;

        if (PlayerInventoryUpgrades.Instance != null)
            PlayerInventoryUpgrades.Instance.OnBackpackLevelChanged -= OnBackpackLevelChanged;
    }

    private void OnResourceChanged(ResourceType type, int current, int max)
    {
        Refresh();
    }

    private void OnBackpackLevelChanged()
    {
        Refresh();

        if (toolInfoPanel.activeSelf)
            UpdateInfoPanel();
    }

    public void Refresh()
    {
        if (icon == null)
            return;

        bool hasBackpack = PlayerInventoryUpgrades.Instance.backpackLevel > 0;
        icon.sprite = hasBackpack ? activeSprite : inactiveSprite;
    }

    public void OnClick()
    {
        if (PlayerInventoryUpgrades.Instance.backpackLevel <= 0)
            return;

        toolInfoPanel.SetActive(!toolInfoPanel.activeSelf);

        DeactivateAllInfoPanels();

        if (toolInfoPanel.activeSelf)
            UpdateInfoPanel();
    }

    private void UpdateInfoPanel()
    {
        int level = PlayerInventoryUpgrades.Instance.backpackLevel;

        toolInfoPanelIcon.sprite = icon.sprite;
        toolInfoPanelNameText.text = "Backpack Level " + level;

        if (level == 1)
            toolInfoPanelDescriptionText.text = "Allows you to carry 25 of each resource in your inventory.";
        else if (level == 2)
            toolInfoPanelDescriptionText.text = "Allows you to carry 50 of each resource in your inventory.";
        else if (level == 3)
            toolInfoPanelDescriptionText.text = "Allows you to carry 100 of each resource in your inventory.";
    }

    private void DeactivateAllInfoPanels()
    {
        for (int i = 0; i < itemsList.itemsList.Count; i++)
        {
            if (itemsList.itemsList[i] != this.gameObject)
            {
                EquipmentSlotUI toolSlot =
                    itemsList.itemsList[i].GetComponent<EquipmentSlotUI>();

                BackpackSlotUI backpackSlot =
                    itemsList.itemsList[i].GetComponent<BackpackSlotUI>();

                if (toolSlot != null)
                    toolSlot.toolInfoPanel.SetActive(false);

                if (backpackSlot != null && backpackSlot != this)
                    backpackSlot.toolInfoPanel.SetActive(false);
            }
        }
    }
}