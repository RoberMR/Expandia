using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ResourceNode[] nodes = other.GetComponents<ResourceNode>();
        if (nodes.Length == 0) return;

        foreach (ResourceNode node in nodes)
            node.StartHarvesting();
    }

    private void OnTriggerExit(Collider other)
    {
        ResourceNode[] nodes = other.GetComponents<ResourceNode>();
        if (nodes.Length == 0) return;

        foreach (ResourceNode node in nodes)
            node.StopHarvesting();
    }
}