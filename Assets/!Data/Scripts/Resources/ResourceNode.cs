using UnityEngine;
using System.Collections;
using TMPro;

public class ResourceNode : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private float intervalSeconds = 1f;

    [Header("Tools")]
    [SerializeField] private ToolStatsDatabase toolStatsDatabase;

    [Header("UI")]
    [SerializeField] private GameObject foodLeatherWarningText;

    private Coroutine harvestCoroutine;

    public void StartHarvesting()
    {
        if (harvestCoroutine == null)
            harvestCoroutine = StartCoroutine(HarvestRoutine());
    }

    public void StopHarvesting()
    {
        ActivateDeactivateFoodLeatherWarningText(false);

        if (harvestCoroutine != null)
        {
            StopCoroutine(harvestCoroutine);
            harvestCoroutine = null;
        }
    }

    private IEnumerator HarvestRoutine()
    {
        while (true)
        {
            float waitTime = intervalSeconds;

            ToolType requiredTool = GetRequiredTool();
            var tool = PlayerToolManager.Instance.GetTool(requiredTool);

            if (requiredTool == ToolType.Sword && tool == null)
            {
                ActivateDeactivateFoodLeatherWarningText(true);
                yield return new WaitForSeconds(1f);
                continue;
            }

            if (tool != null)
            {
                ToolStats stats = toolStatsDatabase.GetStats(tool.material);
                waitTime *= stats.gatherSpeedMultiplier;
            }

            yield return new WaitForSeconds(waitTime);

            if (!ResourceManager.Instance.CanAdd(resourceType, 1))
                continue;

            ResourceManager.Instance.Add(resourceType, 1);

            if (tool != null)
                PlayerToolManager.Instance.ConsumeUse(requiredTool);
        }
    }

    private ToolType GetRequiredTool()
    {
        switch (resourceType.resourceId)
        {
            case "Wood":
                return ToolType.Axe;
            case "Stone":
            case "Iron":
                return ToolType.Pickaxe;
            case "Food":
            case "Leather":
                return ToolType.Sword;
            default:
                return ToolType.Axe;
        }
    }

    private void ActivateDeactivateFoodLeatherWarningText(bool activate)
    {
        if (foodLeatherWarningText != null)
            foodLeatherWarningText.SetActive(activate);
    }
}