using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button buildButton;

    [Header("Build point")]
    private BuildPoint currentBuildPoint;

    public void Show(BuildPoint buildPoint)
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