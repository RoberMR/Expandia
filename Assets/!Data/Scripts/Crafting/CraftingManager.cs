using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    ResourceManager inventory;

    void Awake()
    {
        Instance = this;
        inventory = ResourceManager.Instance;
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        if (inventory == null)
            inventory = ResourceManager.Instance;

        foreach (var cost in recipe.costs)
        {
            if (inventory.GetAmount(cost.type) < cost.amount)
                return false;
        }
        return true;
    }

    public void Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe))
            return;

        foreach (var cost in recipe.costs)
            inventory.Remove(cost.type, cost.amount);

        Debug.Log("Crafted: " + recipe.recipeName);
    }
}