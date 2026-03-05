using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ConstructionUI : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private Transform recipesGrid;
    [SerializeField] private RecipeButtonUI recipeButtonPrefab;
    [SerializeField] private List<CraftingRecipe> recipes;

    [Header("Detail Panel")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private Image detailIcon;
    [SerializeField] private TMP_Text detailName;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text resourcesNeededText;
    [SerializeField] private Transform costsParent;
    [SerializeField] private CostEntryUI costEntryPrefab;
    [SerializeField] private Button buildButton;

    [Header("Recipe")]
    private CraftingRecipe currentRecipe;

    private void Start()
    {
        detailPanel.SetActive(false);

        foreach (var btn in GetComponentsInChildren<ConstructionRecipeButtonUI>())
            btn.Setup(this);
    }

    public void ShowRecipeDetail(CraftingRecipe recipe)
    {
        currentRecipe = recipe;

        detailPanel.SetActive(true);
        detailIcon.sprite = recipe.icon;
        detailName.text = recipe.recipeName;
        ReturnInfoText(recipe);
        resourcesNeededText.text = "Resources needed:";

        foreach (Transform c in costsParent)
            Destroy(c.gameObject);

        if (CheckForCraftedBackpacks(recipe))
            return;

        if (CheckForCraftedStorage(recipe))
            return;

        if (CheckForEquippedTool(recipe))
            return;

        foreach (var cost in recipe.costs)
        {
            var entry = Instantiate(costEntryPrefab, costsParent);
            entry.Setup(cost);
        }

        buildButton.interactable = CraftingManager.Instance.CanCraft(recipe);
    }

    public void Build()
    {
        CraftingManager.Instance.Craft(currentRecipe);
        buildButton.interactable = false;
        Back();
    }

    public void Back()
    {
        DeselectAllRecipeButtons();
        detailPanel.SetActive(false);
    }

    private void DeselectAllRecipeButtons()
    {
        foreach (var btn in GetComponentsInChildren<RecipeButtonUI>())
            btn.buttonPressed = false;
    }

    private bool CheckForCraftedBackpacks(CraftingRecipe recipe)
    {
        if ((recipe.recipeName == "Backpack Level 1" && PlayerInventoryUpgrades.Instance.HasBackpack(1)) ||
            (recipe.recipeName == "Backpack Level 2" && PlayerInventoryUpgrades.Instance.HasBackpack(2)) ||
            (recipe.recipeName == "Backpack Level 3" && PlayerInventoryUpgrades.Instance.HasBackpack(3)))
        {
            buildButton.interactable = false;
            resourcesNeededText.text = "You already possess this item";
            return true;
        }
        return false;
    }

    private bool CheckForCraftedStorage(CraftingRecipe recipe)
    {
        if ((recipe.recipeName == "Storage Level 1" && PlayerStorageUpgrades.Instance.HasStorage(1)) ||
            (recipe.recipeName == "Storage Level 2" && PlayerStorageUpgrades.Instance.HasStorage(2)) ||
            (recipe.recipeName == "Storage Level 3" && PlayerStorageUpgrades.Instance.HasStorage(3)))
        {
            buildButton.interactable = false;
            resourcesNeededText.text = "You already possess this item";

            foreach (Transform c in costsParent)
                Destroy(c.gameObject);

            return true;
        }

        return false;
    }

    private bool CheckForEquippedTool(CraftingRecipe recipe)
    {
        bool isTool = recipe.recipeType == RecipeType.Sword ||
                      recipe.recipeType == RecipeType.Axe ||
                      recipe.recipeType == RecipeType.Pickaxe;

        if (!isTool)
            return false;

        ToolType type = GetToolTypeFromRecipe(recipe);

        if (PlayerToolManager.Instance.HasTool(type))
        {
            buildButton.interactable = false;
            resourcesNeededText.text = $"You already possess a {type.ToString().ToLower()}";

            foreach (Transform c in costsParent)
                Destroy(c.gameObject);

            return true;
        }

        return false;
    }

    private ToolType GetToolTypeFromRecipe(CraftingRecipe recipe)
    {
        return recipe.recipeType switch
        {
            RecipeType.Sword => ToolType.Sword,
            RecipeType.Axe => ToolType.Axe,
            RecipeType.Pickaxe => ToolType.Pickaxe,
            _ => throw new System.Exception("Recipe is not a tool")
        };
    }

    private void ReturnInfoText(CraftingRecipe recipe)
    {
        infoText.text = "";

        if (recipe.recipeType == RecipeType.Storage)
        {
            if (recipe.level == 4)
                infoText.text += "Allows you to store 1000 of each resource in the Storage.";
            else if (recipe.level == 5)
                infoText.text += "Allows you to store 2500 of each resource in the Storage.";
            else if (recipe.level == 6)
                infoText.text += "Allows you to store 5000 of each resource in the Storage.";
            else if (recipe.level == 7)
                infoText.text += "Allows you to store 10000 of each resource in the Storage.";
            else if (recipe.level == 8)
                infoText.text += "Allows you to store 25000 of each resource in the Storage.";
            else if (recipe.level == 9)
                infoText.text += "Allows you to store 50000 of each resource in the Storage.";
            else if (recipe.level == 10)
                infoText.text += "Allows you to store 100000 of each resource in the Storage.";

            return;
        }

        else if (recipe.recipeType == RecipeType.MyHouse)
        {
            if (recipe.level == 1)
                infoText.text += "Builds your house, which allows you to collect gold from your villagers, when you have them.";
            else if (recipe.level == 2)
                infoText.text += "You double all the gold collected from your villagers.";

            return;
        }

        else if (recipe.recipeType == RecipeType.VillagersHouse)
        {
            if (recipe.level == 1)
                infoText.text += "Builds the villagers' house. It has 10 villagers, who will produce 100 gold per hour, if you have your own house.";
            else if (recipe.level == 2)
                infoText.text += "Increases the total number of villagers to 20, who will produce 200 gold per hour.";
            else if (recipe.level == 3)
                infoText.text += "Increases the total number of villagers to 30, who will produce 300 gold per hour.";
            else if (recipe.level == 4)
                infoText.text += "Increases the total number of villagers to 40, who will produce 400 gold per hour.";
            else if (recipe.level == 5)
                infoText.text += "Increases the total number of villagers to 50, who will produce 500 gold per hour.";

            return;
        }

        else if (recipe.recipeType == RecipeType.Sawmill)
        {
            infoText.text += "Builds a sawmill that will automatically produce Wood. " +
                "The collected wood is deposited directly into your Storage.";

            return;
        }

        else if (recipe.recipeType == RecipeType.Quarry)
        {
            infoText.text += "Builds a quarry that will automatically produce Stone. " +
                "The collected stone is deposited directly into your Storage.";

            return;
        }

        else if (recipe.recipeType == RecipeType.Mine)
        {
            infoText.text += "Builds a mine that will automatically produce Iron. " +
                "The collected iron is deposited directly into your Storage.";

            return;
        }

        else if (recipe.recipeType == RecipeType.Farm)
        {
            infoText.text += "Builds a farm that will automatically produce Food and Leather. " +
                "The collected Food and Leather are deposited directly into your Storage.";

            return;
        }

        else if (recipe.recipeType == RecipeType.Market)
        {
            if (recipe.level == 1)
                infoText.text += "Builds a market that allows you to sell resources in exchange for gold.";
            else if (recipe.level == 2)
                infoText.text += "It allows you to buy things at the market.";
            else if (recipe.level == 3)
                infoText.text += "Improves selling prices.";
            else if (recipe.level == 4)
                infoText.text += "Improves purchase prices.";

            return;
        }
    }
}