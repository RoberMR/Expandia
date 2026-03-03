using UnityEngine;

public class ConstructionRecipeButtonUI : MonoBehaviour
{
    [Header("Crafting")]
    [SerializeField] private CraftingRecipe recipe;
    private ConstructionUI constructionUI;

    [Header("Button state")]
    public bool buttonPressed;

    private void Start()
    {
        buttonPressed = false;
        ResourceManager.Instance.OnUpdateCraftingUI += OnUpdateConstructionUI;
    }

    private void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnUpdateCraftingUI -= OnUpdateConstructionUI;
    }

    private void OnUpdateConstructionUI()
    {
        if (!buttonPressed) return;
        OnClick();
    }

    public void Setup(ConstructionUI ui)
    {
        constructionUI = ui;
    }

    public void OnClick()
    {
        buttonPressed = true;
        constructionUI.ShowRecipeDetail(recipe);
    }
}