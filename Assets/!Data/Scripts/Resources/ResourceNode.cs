using UnityEngine;
using System.Collections;

public class ResourceNode : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private float intervalSeconds = 1f;

    [Header("Tools")]
    [SerializeField] private ToolStatsDatabase toolStatsDatabase;

    private Coroutine harvestCoroutine;

    public void StartHarvesting()
    {
        if (harvestCoroutine == null)
            harvestCoroutine = StartCoroutine(HarvestRoutine());
    }

    public void StopHarvesting()
    {
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
            {
                tool.remainingUses--;

                if (tool.remainingUses <= 0)
                {
                    PlayerToolManager.Instance.BreakTool(requiredTool);
                }
            }
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
}