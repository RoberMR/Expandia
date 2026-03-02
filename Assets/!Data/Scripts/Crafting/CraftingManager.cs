using System;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    public event Action OnBackpackCrafted;
    public event Action OnStorageCrafted;

    [Header("ToolStatsDatabase")]
    [SerializeField] private ToolStatsDatabase toolStatsDatabase;

    [Header("Inventory reference")]
    private ResourceManager inventory;

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
        if (StorageCanCraftCheck(recipe))
            return false;

        if (recipe.IsTool())
        {
            ToolType toolType = recipe.GetToolType();
            if (PlayerToolManager.Instance.HasTool(toolType))
                return false;
        }

        foreach (var cost in recipe.costs)
        {
            if (ResourceManager.Instance.GetTotalAmount(cost.type) < cost.amount)
                return false;
        }

        return true;
    }

    public void Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe))
            return;

        BackpackCraftChecks(recipe);
        StorageCraftChecks(recipe);

        foreach (var cost in recipe.costs)
        {
            int remainingCost = cost.amount;

            int inventoryAmount = ResourceManager.Instance.GetAmount(cost.type);
            int removeFromInventory = Mathf.Min(inventoryAmount, remainingCost);

            if (removeFromInventory > 0)
            {
                ResourceManager.Instance.Remove(cost.type, removeFromInventory);
                remainingCost -= removeFromInventory;
            }

            if (remainingCost > 0 && StorageManager.Instance != null)
                StorageManager.Instance.Remove(cost.type, remainingCost);
        }

        if (recipe.IsTool())
        {
            ToolStats stats = toolStatsDatabase.GetStats(recipe.toolMaterial);

            PlayerToolManager.Instance.EquipTool(
                recipe.GetToolType(),
                recipe.toolMaterial,
                stats.maxUses
            );
        }

        //Debug.Log("Crafted: " + recipe.recipeName);
    }

    private bool BackpackCanCraftCheck(CraftingRecipe recipe)
    {
        return (recipe.recipeName == "Backpack Level 1" && PlayerInventoryUpgrades.Instance.HasBackpack(1)) ||
               (recipe.recipeName == "Backpack Level 2" && PlayerInventoryUpgrades.Instance.HasBackpack(2)) ||
               (recipe.recipeName == "Backpack Level 3" && PlayerInventoryUpgrades.Instance.HasBackpack(3));
    }

    private bool StorageCanCraftCheck(CraftingRecipe recipe)
    {
        return (recipe.recipeName == "Storage Level 1" && PlayerStorageUpgrades.Instance.HasStorage(1)) ||
               (recipe.recipeName == "Storage Level 2" && PlayerStorageUpgrades.Instance.HasStorage(2)) ||
               (recipe.recipeName == "Storage Level 3" && PlayerStorageUpgrades.Instance.HasStorage(3));
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

    private void StorageCraftChecks(CraftingRecipe recipe)
    {
        int newLevel = 0;

        if (recipe.recipeName == "Storage Level 1") newLevel = 1;
        else if (recipe.recipeName == "Storage Level 2") newLevel = 2;
        else if (recipe.recipeName == "Storage Level 3") newLevel = 3;

        if (newLevel > 0)
        {
            StorageManager.Instance.SetLevel(newLevel);
            OnStorageCrafted?.Invoke();
        }
    }
}