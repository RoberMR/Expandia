using UnityEngine;
using System.Collections;

public class ResourceNode : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private float intervalSeconds = 1f;

    [Header("Harvest Coroutine")]
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
            yield return new WaitForSeconds(intervalSeconds);

            ResourceManager.Instance.Add(resourceType, 1);
        }
    }
}