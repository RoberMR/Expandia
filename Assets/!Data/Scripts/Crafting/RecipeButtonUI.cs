using UnityEngine;

public class RecipeButtonUI : MonoBehaviour
{
    [Header("Crafting")]
    [SerializeField] private CraftingRecipe recipe;
    private CraftingUI craftingUI;

    [Header("Button state")]
    public bool buttonPressed;

    private void Start()
    {
        buttonPressed = false;
        ResourceManager.Instance.OnUpdateCraftingUI += OnUpdateCraftingUI;
    }

    private void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnUpdateCraftingUI -= OnUpdateCraftingUI;
    }

    private void OnUpdateCraftingUI()
    {
        if (!buttonPressed) return;
        OnClick();
    }

    public void Setup(CraftingUI ui)
    {
        craftingUI = ui;
    }

    public void OnClick()
    {
        buttonPressed = true;
        craftingUI.ShowRecipeDetail(recipe);
    }
}