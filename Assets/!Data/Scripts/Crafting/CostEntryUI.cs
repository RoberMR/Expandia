using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostEntryUI : MonoBehaviour
{
    [Header("Cost Entry UI Elements")]
    [SerializeField] Image icon;
    [SerializeField] TMP_Text amount;

    public void Setup(ResourceCost cost)
    {
        icon.sprite = cost.type.icon;

        int total = ResourceManager.Instance.GetTotalAmount(cost.type);
        amount.text = $"{total}/{cost.amount}";
    }
}