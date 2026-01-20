using System.Collections.Generic;
using UnityEngine;

public class ResourceUIController : MonoBehaviour
{
    [Header("UI Entries")]
    [SerializeField] private List<ResourceUIEntry> entries;

    private void Start()
    {
        foreach (var entry in entries)
        {
            ResourceType type = entry.GetResourceType();
            int current = ResourceManager.Instance.GetAmount(type);
            int max = GetMaxCapacity();

            entry.UpdateAmount(current, max);
        }

        ResourceManager.Instance.OnResourceChanged += OnResourceChanged;
    }

    private void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= OnResourceChanged;
    }

    private void OnResourceChanged(ResourceType type, int current, int max)
    {
        foreach (var entry in entries)
        {
            if (entry.GetResourceType() == type)
            {
                entry.UpdateAmount(current, max);
                break;
            }
        }
    }

    private int GetMaxCapacity()
    {
        // Por ahora es global, más adelante vendrá de mochila/equipamiento
        return 10;
    }
}