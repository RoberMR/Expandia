using TMPro;
using UnityEngine;

public class ConstructionZoneBuildPoint : MonoBehaviour
{
    [Header("Build Cost")]
    [SerializeField] private ResourceType wood;
    [SerializeField] private ResourceType stone;
    [SerializeField] private ResourceType iron;
    [SerializeField] private ResourceType leather;

    [SerializeField] private int woodCost = 500;
    [SerializeField] private int stoneCost = 400;
    [SerializeField] private int ironCost = 300;
    [SerializeField] private int leatherCost = 250;

    [Header("References")]
    [SerializeField] private GameObject groundCircle;
    [SerializeField] private GameObject constructionZoneObject;
    [SerializeField] private ConstructionZoneBuildUI constructionZoneBuildUI;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI woodNeededText;
    [SerializeField] private TextMeshProUGUI stoneNeededText;
    [SerializeField] private TextMeshProUGUI ironNeededText;
    [SerializeField] private TextMeshProUGUI leatherNeededText;

    private bool isBuilt = false;

    private void Awake()
    {
        constructionZoneObject.SetActive(false);
        constructionZoneBuildUI.Hide();
    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceChanged += OnResourceChanged;
    }

    private void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= OnResourceChanged;
    }

    private void OnResourceChanged(ResourceType type, int current, int max)
    {
        UpdateUI();
    }

    public bool CanBuild()
    {
        return ResourceManager.Instance.GetTotalAmount(wood) >= woodCost &&
               ResourceManager.Instance.GetTotalAmount(stone) >= stoneCost &&
               ResourceManager.Instance.GetTotalAmount(iron) >= ironCost &&
               ResourceManager.Instance.GetTotalAmount(leather) >= leatherCost;
    }

    public void Build()
    {
        if (isBuilt)
            return;

        if (!CanBuild())
            return;

        RemoveTotalResource(wood, woodCost);
        RemoveTotalResource(stone, stoneCost);
        RemoveTotalResource(iron, ironCost);
        RemoveTotalResource(leather, leatherCost);

        isBuilt = true;

        groundCircle.SetActive(false);
        constructionZoneBuildUI.Hide();
        constructionZoneObject.SetActive(true);
    }

    public void ShowUI()
    {
        if (isBuilt)
            return;

        UpdateUI();

        constructionZoneBuildUI.Show(this);
    }

    public void HideUI()
    {
        constructionZoneBuildUI.Hide();
    }

    private void UpdateUI()
    {
        woodNeededText.text = ResourceManager.Instance.GetTotalAmount(wood) + "/" + woodCost;
        stoneNeededText.text = ResourceManager.Instance.GetTotalAmount(stone) + "/" + stoneCost;
        ironNeededText.text = ResourceManager.Instance.GetTotalAmount(iron) + "/" + ironCost;
        leatherNeededText.text = ResourceManager.Instance.GetTotalAmount(leather) + "/" + leatherCost;
    }

    private void RemoveTotalResource(ResourceType type, int amount)
    {
        int inventoryAmount = ResourceManager.Instance.GetAmount(type);

        int removeFromInventory = Mathf.Min(inventoryAmount, amount);
        int remaining = amount - removeFromInventory;

        if (removeFromInventory > 0)
            ResourceManager.Instance.Remove(type, removeFromInventory);

        if (remaining > 0)
            StorageManager.Instance.Remove(type, remaining);
    }
}