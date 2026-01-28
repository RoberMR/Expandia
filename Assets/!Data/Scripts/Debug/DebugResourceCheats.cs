using UnityEngine;

public class DebugResourceCheats : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private ResourceType wood;
    [SerializeField] private ResourceType stone;
    [SerializeField] private ResourceType iron;
    [SerializeField] private ResourceType food;
    [SerializeField] private ResourceType leather;

    [Header("Debug Values")]
    [SerializeField] private int debugAddAmount = 100;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddDebug(wood);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            AddDebug(stone);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            AddDebug(iron);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            AddDebug(food);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            AddDebug(leather);
    }

    private void AddDebug(ResourceType type)
    {
        if (type == null)
            return;

        ResourceManager.Instance.AddClamped(type, debugAddAmount);
    }
}