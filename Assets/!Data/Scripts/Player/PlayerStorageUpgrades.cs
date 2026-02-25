using System;
using UnityEngine;

public class PlayerStorageUpgrades : MonoBehaviour
{
    public static PlayerStorageUpgrades Instance { get; private set; }

    public event Action OnStorageLevelChanged;

    [Header("Storage")]
    public int storageLevel = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool HasStorage(int level)
    {
        return storageLevel >= level;
    }

    public void UnlockStorage(int level)
    {
        if (storageLevel >= level)
            return;

        storageLevel = level;

        OnStorageLevelChanged?.Invoke();
    }
}