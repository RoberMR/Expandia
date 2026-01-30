using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostEntryUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text amount;

    public void Setup(ResourceCost cost)
    {
        icon.sprite = cost.type.icon;
        amount.text = ResourceManager.Instance.GetAmount(cost.type) + "/" + cost.amount.ToString();
    }
}