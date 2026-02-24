using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToolManager : MonoBehaviour
{
    public static PlayerToolManager Instance;

    private Dictionary<ToolType, EquippedTool> equippedTools =
        new Dictionary<ToolType, EquippedTool>();

    public event Action<ToolType, EquippedTool> OnToolChanged;
    public event Action<ToolType, EquippedTool> OnToolUpdated;

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
            remainingUses = uses,
            maxUses = uses
        };

        OnToolChanged?.Invoke(type, equippedTools[type]);
    }

    public void BreakTool(ToolType type)
    {
        equippedTools.Remove(type);
        OnToolChanged?.Invoke(type, null);
    }

    public void ConsumeUse(ToolType type)
    {
        if (!equippedTools.ContainsKey(type))
            return;

        var tool = equippedTools[type];
        tool.remainingUses--;

        OnToolUpdated?.Invoke(type, tool);

        if (tool.remainingUses <= 0)
        {
            BreakTool(type);
        }
    }
}

// -------------------------------------------------- //

[System.Serializable]
public class EquippedTool
{
    public ToolType type;
    public ToolMaterial material;
    public int remainingUses;
    public int maxUses;
}

// -------------------------------------------------- //