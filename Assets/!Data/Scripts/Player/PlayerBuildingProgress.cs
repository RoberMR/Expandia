using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingProgress : MonoBehaviour
{
    public static PlayerBuildingProgress Instance { get; private set; }

    public event Action OnBuildingLevelChanged;

    private Dictionary<RecipeType, int> buildingLevels = new Dictionary<RecipeType, int>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public int GetLevel(RecipeType type)
    {
        if (!buildingLevels.ContainsKey(type))
            return 0;

        return buildingLevels[type];
    }

    public bool HasBuilding(RecipeType type, int level)
    {
        return GetLevel(type) >= level;
    }

    public void UnlockBuilding(RecipeType type, int level)
    {
        if (HasBuilding(type, level))
            return;

        buildingLevels[type] = level;

        OnBuildingLevelChanged?.Invoke();
    }
}