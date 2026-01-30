using TMPro;
using UnityEngine;

public class BuildPoint : MonoBehaviour
{
    [Header("Build Cost")]
    [SerializeField] private ResourceType wood;
    [SerializeField] private ResourceType stone;
    [SerializeField] private ResourceType iron;

    [SerializeField] private int woodCost = 10;
    [SerializeField] private int stoneCost = 10;
    [SerializeField] private int ironCost = 10;

    [Header("References")]
    [SerializeField] private GameObject groundCircle;
    [SerializeField] private GameObject craftingTableObject;
    [SerializeField] private BuildUI buildUI;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI woodNeededText;
    [SerializeField] private TextMeshProUGUI stoneNeededText;
    [SerializeField] private TextMeshProUGUI ironNeededText;

    private bool isBuilt = false;

    private void Awake()
    {
        craftingTableObject.SetActive(false);
        buildUI.Hide();
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
        return ResourceManager.Instance.GetAmount(wood) >= woodCost &&
               ResourceManager.Instance.GetAmount(stone) >= stoneCost &&
               ResourceManager.Instance.GetAmount(iron) >= ironCost;
    }

    public void Build()
    {
        if (isBuilt)
            return;

        if (!CanBuild())
            return;

        ResourceManager.Instance.Remove(wood, woodCost);
        ResourceManager.Instance.Remove(stone, stoneCost);
        ResourceManager.Instance.Remove(iron, ironCost);

        isBuilt = true;

        groundCircle.SetActive(false);
        buildUI.Hide();
        craftingTableObject.SetActive(true);
    }

    public void ShowUI()
    {
        if (isBuilt)
            return;

        UpdateUI();

        buildUI.Show(this);
    }

    public void HideUI()
    {
        buildUI.Hide();
    }

    private void UpdateUI()
    {
        woodNeededText.text = ResourceManager.Instance.GetAmount(wood) + "/" + woodCost;
        stoneNeededText.text = ResourceManager.Instance.GetAmount(stone) + "/" + stoneCost;
        ironNeededText.text = ResourceManager.Instance.GetAmount(iron) + "/" + ironCost;
    }
}