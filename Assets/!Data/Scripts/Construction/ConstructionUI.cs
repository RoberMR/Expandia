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
}