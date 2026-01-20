using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIEntry : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private ResourceType resourceType;

    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    private void Awake()
    {
        if (resourceType != null)
            iconImage.sprite = resourceType.icon;
    }

    public void UpdateAmount(int current, int max)
    {
        amountText.text = $"{current} / {max}";
    }

    public ResourceType GetResourceType()
    {
        return resourceType;
    }
}