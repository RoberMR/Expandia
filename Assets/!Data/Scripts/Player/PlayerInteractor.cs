using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ResourceNode node = other.GetComponent<ResourceNode>();
        if (node != null)
        {
            Debug.Log("Player enters " + other.gameObject.name + " resource point.");
            node.StartHarvesting();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ResourceNode node = other.GetComponent<ResourceNode>();
        if (node != null)
        {
            Debug.Log("Player exits " + other.gameObject.name + " resource point.");
            node.StopHarvesting();
        }
    }
}