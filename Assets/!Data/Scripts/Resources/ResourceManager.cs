using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event Action<ResourceType, int, int> OnResourceChanged;

    [Header("Inventory")]
    [SerializeField] private int maxCapacityPerResource = 10;

    [Header("Resources Dictionary")]
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public int GetAmount(ResourceType type)
    {
        if (resources.TryGetValue(type, out int amount))
            return amount;

        return 0;
    }

    public bool CanAdd(ResourceType type, int amount)
    {
        int current = GetAmount(type);
        return current + amount <= maxCapacityPerResource;
    }

    public bool Add(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        if (!CanAdd(type, amount))
            return false;

        resources[type] += amount;

        OnResourceChanged?.Invoke(
            type,
            resources[type],
            maxCapacityPerResource
        );

        return true;
    }
}