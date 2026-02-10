using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public Sprite icon;

    public List<ResourceCost> costs;

    public RecipeType recipeType;
    public int level;

    [Header("Tool Data (only if tool)")]
    public ToolMaterial toolMaterial;

    public bool IsTool()
    {
        return recipeType == RecipeType.Sword
            || recipeType == RecipeType.Axe
            || recipeType == RecipeType.Pickaxe;
    }

    public ToolType GetToolType()
    {
        return recipeType switch
        {
            RecipeType.Sword => ToolType.Sword,
            RecipeType.Axe => ToolType.Axe,
            RecipeType.Pickaxe => ToolType.Pickaxe,
            _ => throw new Exception("Recipe is not a tool")
        };
    }
}

[System.Serializable]
public class ResourceCost
{
    public ResourceType type;
    public int amount;
}

public enum RecipeType
{
    Sword,
    Axe,
    Pickaxe,
    Backpack,
    Storage
}