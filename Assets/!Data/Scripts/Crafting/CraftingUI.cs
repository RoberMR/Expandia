using System.Collections.Generic;
using TMPro;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
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
    [SerializeField] private Button craftButton;

    [Header("Recipe")]
    private CraftingRecipe currentRecipe;

    private void Start()
    {
        detailPanel.SetActive(false);

        foreach (var btn in GetComponentsInChildren<RecipeButtonUI>())
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

        craftButton.interactable = CraftingManager.Instance.CanCraft(recipe);
    }

    public void Craft()
    {
        CraftingManager.Instance.Craft(currentRecipe);
        craftButton.interactable = false;
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
            craftButton.interactable = false;
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
            craftButton.interactable = false;
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
            craftButton.interactable = false;
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

        if (recipe.recipeType == RecipeType.Backpack)
        {
            infoText.text = "Allows you to carry ";

            if (recipe.level == 1)
                infoText.text += "25 of each resource in your inventory.";
            else if (recipe.level == 2)
                infoText.text += "50 of each resource in your inventory.";
            else if (recipe.level == 3)
                infoText.text += "100 of each resource in your inventory.";

            return;
        }

        else if (recipe.recipeType == RecipeType.Storage)
        {
            if (recipe.level == 1)
                infoText.text += "Creates a Storage where you can store 150 of each resource.";
            else if (recipe.level == 2)
                infoText.text += "Allows you to store 300 of each resource in the Storage.";
            else if (recipe.level == 3)
                infoText.text += "Allows you to store 500 of each resource in the Storage.";

            return;
        }

        if (recipe.recipeType == RecipeType.Sword)
            infoText.text = "Collects Food and Leather ";
        else if (recipe.recipeType == RecipeType.Axe)
            infoText.text = "Collects Wood ";
        else if (recipe.recipeType == RecipeType.Pickaxe)
            infoText.text = "Collects Stone and Iron ";

        if (recipe.toolMaterial == ToolMaterial.Wood)
            infoText.text += "20% faster.";
        else if (recipe.toolMaterial == ToolMaterial.Stone)
            infoText.text += "50% faster.";
        else if (recipe.toolMaterial == ToolMaterial.Iron)
            infoText.text += "80% faster.";
    }
}