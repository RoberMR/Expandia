using UnityEngine;

public class DebugResourceCheats : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private ResourceType wood;
    [SerializeField] private ResourceType stone;
    [SerializeField] private ResourceType iron;
    [SerializeField] private ResourceType food;
    [SerializeField] private ResourceType leather;
    [SerializeField] private ResourceType gold;

    [Header("Debug Values")]
    [SerializeField] private int debugAddAmount = 100;
    [SerializeField] private int debugRemoveAmount = 100;

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

        if (Input.GetKeyDown(KeyCode.G))
            AddDebug(gold);


        if (Input.GetKeyDown(KeyCode.Alpha6))
            RemoveDebug(wood);

        if (Input.GetKeyDown(KeyCode.Alpha7))
            RemoveDebug(stone);

        if (Input.GetKeyDown(KeyCode.Alpha8))
            RemoveDebug(iron);

        if (Input.GetKeyDown(KeyCode.Alpha9))
            RemoveDebug(food);

        if (Input.GetKeyDown(KeyCode.Alpha0))
            RemoveDebug(leather);

        if (Input.GetKeyDown(KeyCode.H))
            RemoveDebug(gold);
    }

    private void AddDebug(ResourceType type)
    {
        if (type == null)
            return;

        ResourceManager.Instance.AddClamped(type, debugAddAmount);
    }

    private void RemoveDebug(ResourceType type)
    {
        if (type == null)
            return;

        ResourceManager.Instance.Remove(type, debugRemoveAmount);
    }
}