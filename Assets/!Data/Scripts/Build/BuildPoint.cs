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

    private bool isBuilt = false;

    private void Awake()
    {
        craftingTableObject.SetActive(false);
        buildUI.Hide();
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

        ResourceManager.Instance.Add(wood, -woodCost);
        ResourceManager.Instance.Add(stone, -stoneCost);
        ResourceManager.Instance.Add(iron, -ironCost);

        isBuilt = true;

        groundCircle.SetActive(false);
        buildUI.Hide();
        craftingTableObject.SetActive(true);
    }

    public void ShowUI()
    {
        if (isBuilt)
            return;

        buildUI.Show(this);
    }

    public void HideUI()
    {
        buildUI.Hide();
    }
}