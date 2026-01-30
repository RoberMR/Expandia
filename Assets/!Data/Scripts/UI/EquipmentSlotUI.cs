using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    public void SetActive(bool active)
    {
        icon.sprite = active ? activeSprite : inactiveSprite;
        icon.color = active ? Color.white : Color.black;
    }
}