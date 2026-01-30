using System;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    public event Action OnBackpackCrafted;

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

        if (BackpackCanCraftCheck(recipe))
            return false;

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

        BackpackCraftChecks(recipe);

        foreach (var cost in recipe.costs)
            inventory.Remove(cost.type, cost.amount);

        Debug.Log("Crafted: " + recipe.recipeName);
    }

    private bool BackpackCanCraftCheck(CraftingRecipe recipe)
    {
        return (recipe.recipeName == "Backpack Level 1" && PlayerInventoryUpgrades.Instance.HasBackpack(1)) ||
            (recipe.recipeName == "Backpack Level 2" && PlayerInventoryUpgrades.Instance.HasBackpack(2)) ||
            (recipe.recipeName == "Backpack Level 3" && PlayerInventoryUpgrades.Instance.HasBackpack(3));
    }

    private void BackpackCraftChecks(CraftingRecipe recipe)
    {
        if (recipe.recipeName == "Backpack Level 1")
        {
            PlayerInventoryUpgrades.Instance.UnlockBackpack(1);
            ResourceManager.Instance.IncreaseMaxCapacity(15);
        }
        else if (recipe.recipeName == "Backpack Level 2")
        {
            PlayerInventoryUpgrades.Instance.UnlockBackpack(2);
            ResourceManager.Instance.IncreaseMaxCapacity(25);
        }
        else if (recipe.recipeName == "Backpack Level 3")
        {
            PlayerInventoryUpgrades.Instance.UnlockBackpack(3);
            ResourceManager.Instance.IncreaseMaxCapacity(50);
        }

        OnBackpackCrafted?.Invoke();
    }
}