using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; private set; }

    public event Action OnStorageChanged;
    public event Action OnStorageLevelChanged;

    private Dictionary<ResourceType, int> storedResources = new();

    [Header("Capacity")]
    private int capacityPerResource = 0;

    public int StorageLevel => PlayerStorageUpgrades.Instance.storageLevel;

    [Header("Storage Building")]
    [SerializeField] private GameObject storageLvl1;
    [SerializeField] private GameObject storageLvl2;
    [SerializeField] private GameObject storageLvl3;
    [SerializeField] private GameObject storageLvl4;
    [SerializeField] private GameObject storageLvl5;
    [SerializeField] private GameObject storageLvl6;
    [SerializeField] private GameObject storageLvl7;
    [SerializeField] private GameObject storageLvl8;
    [SerializeField] private GameObject storageLvl9;
    [SerializeField] private GameObject storageLvl10;

    [SerializeField] private StorageUI storageUI;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PlayerStorageUpgrades.Instance.OnStorageLevelChanged += UpdateCapacityFromLevel;
    }

    private void OnDestroy()
    {
        if (PlayerStorageUpgrades.Instance != null)
            PlayerStorageUpgrades.Instance.OnStorageLevelChanged -= UpdateCapacityFromLevel;
    }

    private void UpdateCapacityFromLevel()
    {
        int level = StorageLevel;

        if (level == 1) capacityPerResource = 150;
        else if (level == 2) capacityPerResource = 300;
        else if (level == 3) capacityPerResource = 500;

        OnStorageLevelChanged?.Invoke();
        OnStorageChanged?.Invoke();
    }

    public int GetStored(ResourceType type)
    {
        if (storedResources.TryGetValue(type, out int amount))
            return amount;

        return 0;
    }

    public int GetCapacity()
    {
        return capacityPerResource;
    }

    public int Add(ResourceType type, int amount)
    {
        if (!storedResources.ContainsKey(type))
            storedResources[type] = 0;

        int current = storedResources[type];
        int spaceLeft = capacityPerResource - current;

        if (spaceLeft <= 0)
            return 0;

        int finalAmount = Mathf.Min(spaceLeft, amount);
        storedResources[type] += finalAmount;

        OnStorageChanged?.Invoke();
        return finalAmount;
    }

    public int Remove(ResourceType type, int amount)
    {
        if (!storedResources.ContainsKey(type))
            storedResources[type] = 0;

        int current = storedResources[type];
        int finalAmount = Mathf.Min(current, amount);

        storedResources[type] -= finalAmount;

        OnStorageChanged?.Invoke();
        return finalAmount;
    }

    public void SetLevel(int level)
    {
        PlayerStorageUpgrades.Instance.UnlockStorage(level);

        if (level == 1) capacityPerResource = 150;
        else if (level == 2) capacityPerResource = 300;
        else if (level == 3) capacityPerResource = 500;
        else if (level == 4) capacityPerResource = 1000;
        else if (level == 5) capacityPerResource = 2500;
        else if (level == 6) capacityPerResource = 5000;
        else if (level == 7) capacityPerResource = 10000;
        else if (level == 8) capacityPerResource = 25000;
        else if (level == 9) capacityPerResource = 50000;
        else if (level == 10) capacityPerResource = 100000;

        OnStorageLevelChanged?.Invoke();
        OnStorageChanged?.Invoke();

        storageLvl1.SetActive(level == 1);
        storageLvl2.SetActive(level == 2);
        storageLvl3.SetActive(level == 3);
        storageLvl4.SetActive(level == 4);
        storageLvl5.SetActive(level == 5);
        storageLvl6.SetActive(level == 6);
        storageLvl7.SetActive(level == 7);
        storageLvl8.SetActive(level == 8);
        storageLvl9.SetActive(level == 9);
        storageLvl10.SetActive(level == 10);

        if (storageUI != null && storageUI.panel.activeSelf)
            storageUI.Refresh();
    }
}