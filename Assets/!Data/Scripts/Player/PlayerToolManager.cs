using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToolManager : MonoBehaviour
{
    public static PlayerToolManager Instance;

    private Dictionary<ToolType, EquippedTool> equippedTools =
        new Dictionary<ToolType, EquippedTool>();

    public event Action<ToolType, EquippedTool> OnToolChanged;

    private void Awake()
    {
        Instance = this;
    }

    public bool HasTool(ToolType type)
    {
        return equippedTools.ContainsKey(type);
    }

    public EquippedTool GetTool(ToolType type)
    {
        equippedTools.TryGetValue(type, out var tool);
        return tool;
    }

    public void EquipTool(ToolType type, ToolMaterial material, int uses)
    {
        equippedTools[type] = new EquippedTool
        {
            type = type,
            material = material,
            remainingUses = uses
        };

        OnToolChanged?.Invoke(type, equippedTools[type]);
    }

    public void BreakTool(ToolType type)
    {
        equippedTools.Remove(type);
        OnToolChanged?.Invoke(type, null);
    }
}

// -------------------------------------------------- //

[System.Serializable]
public class EquippedTool
{
    public ToolType type;
    public ToolMaterial material;
    public int remainingUses;
}

// -------------------------------------------------- //