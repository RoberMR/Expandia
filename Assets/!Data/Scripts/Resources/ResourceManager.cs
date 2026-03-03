using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event Action<ResourceType, int, int> OnResourceChanged;
    public event Action OnUpdateCraftingUI;

    [Header("Inventory")]
    [SerializeField] private int maxCapacityPerResource = 10;

    [Header("Resources Dictionary")]
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    [SerializeField] private List<ResourceType> allResourceTypesExceptGold;

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
        if (type.isUnlimited)
            return true;

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

        OnUpdateCraftingUI?.Invoke();

        //Debug.Log($"{type.displayName}: {resources[type]}");

        return true;
    }

    public int AddClamped(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        if (type.isUnlimited)
        {
            resources[type] += amount;

            OnResourceChanged?.Invoke(type, resources[type], -1);
            OnUpdateCraftingUI?.Invoke();
            return amount;
        }

        int current = resources[type];
        int maxAddable = maxCapacityPerResource - current;

        if (maxAddable <= 0)
            return 0;

        int finalAmount = Mathf.Min(amount, maxAddable);
        resources[type] += finalAmount;

        OnResourceChanged?.Invoke(type, resources[type], maxCapacityPerResource);
        OnUpdateCraftingUI?.Invoke();

        return finalAmount;
    }

    public bool Remove(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        resources[type] = Mathf.Max(0, resources[type] - amount);

        int max = type.isUnlimited ? -1 : maxCapacityPerResource;

        OnResourceChanged?.Invoke(type, resources[type], max);
        OnUpdateCraftingUI?.Invoke();

        return true;
    }

    public void IncreaseMaxCapacity(int amount)
    {
        maxCapacityPerResource += amount;

        foreach (var kvp in resources)
        {
            OnResourceChanged?.Invoke(
                kvp.Key,
                kvp.Value,
                maxCapacityPerResource
            );
        }

        OnUpdateCraftingUI?.Invoke();
    }

    public int GetTotalAmount(ResourceType type)
    {
        int inInventory = GetAmount(type);
        int inStorage = StorageManager.Instance != null ? StorageManager.Instance.GetStored(type) : 0;
        return inInventory + inStorage;
    }

    public List<ResourceType> GetAllResourceTypes()
    {
        return allResourceTypesExceptGold;
    }
}