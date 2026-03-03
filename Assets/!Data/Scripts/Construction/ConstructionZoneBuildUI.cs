using UnityEngine;
using UnityEngine.UI;

public class ConstructionZoneBuildUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button buildButton;

    [Header("Build point")]
    private ConstructionZoneBuildPoint currentBuildPoint;

    public void Show(ConstructionZoneBuildPoint buildPoint)
    {
        currentBuildPoint = buildPoint;
        gameObject.SetActive(true);
        UpdateUI();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        currentBuildPoint = null;
    }

    private void Update()
    {
        if (currentBuildPoint != null)
            UpdateUI();
    }

    public void UpdateUI()
    {
        bool canBuild = currentBuildPoint.CanBuild();
        buildButton.interactable = canBuild;
    }

    public void OnBuildButtonPressed()
    {
        if (currentBuildPoint == null)
            return;

        currentBuildPoint.Build();
    }
}