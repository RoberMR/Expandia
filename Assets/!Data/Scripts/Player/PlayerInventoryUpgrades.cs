using System;
using UnityEngine;

public class PlayerInventoryUpgrades : MonoBehaviour
{
    public static PlayerInventoryUpgrades Instance { get; private set; }

    public event Action OnBackpackLevelChanged;

    [Header("Backpack")]
    public int backpackLevel = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool HasBackpack(int level)
    {
        return backpackLevel >= level;
    }

    public void UnlockBackpack(int level)
    {
        if (backpackLevel >= level)
            return;

        backpackLevel = level;

        OnBackpackLevelChanged?.Invoke();
    }
}