using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] private ToolType slotToolType;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private ToolIconDatabase iconDatabase;

    [Header("Tool Info Panel")]
    public GameObject toolInfoPanel;
    [SerializeField] private Image toolInfoPanelIcon;
    [SerializeField] private TextMeshProUGUI toolInfoPanelNameText;
    [SerializeField] private TextMeshProUGUI toolInfoPanelDescriptionText;
    [SerializeField] private TextMeshProUGUI toolInfoPanelUsesLeftText;

    [Header("ToolsList")]
    [SerializeField] private ItemsList itemsList;

    private void Start()
    {
        SetActive(false);
        toolInfoPanel.SetActive(false);
        PlayerToolManager.Instance.OnToolChanged += OnToolChanged;
        PlayerToolManager.Instance.OnToolUpdated += OnToolUpdated;
    }

    private void OnDestroy()
    {
        if (PlayerToolManager.Instance != null)
        {
            PlayerToolManager.Instance.OnToolChanged -= OnToolChanged;
            PlayerToolManager.Instance.OnToolUpdated -= OnToolUpdated;
        }
    }

    private void OnToolChanged(ToolType type, EquippedTool tool)
    {
        if (type != slotToolType) return;

        if (tool != null)
            icon.sprite = iconDatabase.GetIcon(type, tool.material);
        else
        {
            icon.sprite = inactiveSprite;
            if (toolInfoPanel.activeSelf)
                toolInfoPanel.SetActive(false);
        }
    }

    public void SetActive(bool active)
    {
        if (!active)
            icon.sprite = inactiveSprite;
    }

    public void OnClick()
    {
        var tool = PlayerToolManager.Instance.GetTool(slotToolType);
        if (tool == null) return;

        toolInfoPanel.SetActive(!toolInfoPanel.activeInHierarchy);
        DeactivateAllInfoPanels();

        toolInfoPanelIcon.sprite = icon.sprite;
        string en = "";
        if (tool.material == ToolMaterial.Wood) en = "en";
        else en = "";
        toolInfoPanelNameText.text = tool.material.ToString() + en + " " + tool.type.ToString(); ;
        toolInfoPanelDescriptionText.text = GetDescription(tool.type, tool.material);
        toolInfoPanelUsesLeftText.text = "Uses left: " + tool.remainingUses + "/" + tool.maxUses;
    }

    private void DeactivateAllInfoPanels()
    {
        for (int i = 0; i < itemsList.itemsList.Count; i++)
        {
            if (itemsList.itemsList[i] != this.gameObject && itemsList.itemsList[i].GetComponent<EquipmentSlotUI>() != null)
                itemsList.itemsList[i].GetComponent<EquipmentSlotUI>().toolInfoPanel.SetActive(false);
            else if (itemsList.itemsList[i].GetComponent<BackpackSlotUI>() != null)
                itemsList.itemsList[i].GetComponent<BackpackSlotUI>().toolInfoPanel.SetActive(false);
        }
    }

    private string GetDescription(ToolType type, ToolMaterial material)
    {
        string descriptionText = "";

        if (type == ToolType.Sword)
            descriptionText += "Collects Food and Leather ";
        else if (type == ToolType.Axe)
            descriptionText += "Collects Wood ";
        else if (type == ToolType.Pickaxe)
            descriptionText = "Collects Stone and Iron ";

        if (material == ToolMaterial.Wood)
            descriptionText += "20% faster.";
        else if (material == ToolMaterial.Stone)
            descriptionText += "40% faster.";
        else if (material == ToolMaterial.Iron)
            descriptionText += "60% faster.";

        return descriptionText;
    }

    private void OnToolUpdated(ToolType type, EquippedTool tool)
    {
        if (type != slotToolType)
            return;

        if (toolInfoPanel.activeInHierarchy)
        {
            toolInfoPanelUsesLeftText.text =
                "Uses left: " + tool.remainingUses + "/" + tool.maxUses;
        }
    }
}