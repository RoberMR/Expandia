using UnityEngine;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Transform costsParent;
    [SerializeField] private CostEntryUI costEntryPrefab;
    [SerializeField] private Button craftButton;

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

        foreach (Transform c in costsParent)
            Destroy(c.gameObject);

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
}