using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Vector2 InputVector { get; private set; }

    [Header("UI")]
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    [Header("Values")]
    private float radius;

    private void Awake()
    {
        radius = background.sizeDelta.x * 0.5f;
        handle.anchoredPosition = Vector2.zero;
        InputVector = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        localPoint = Vector2.ClampMagnitude(localPoint, radius);
        handle.anchoredPosition = localPoint;

        InputVector = localPoint / radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        InputVector = Vector2.zero;
    }
}