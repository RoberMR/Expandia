using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public Sprite icon;

    public List<ResourceCost> costs;

    // Para más adelante:
    public RecipeType recipeType;
    public int level;
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