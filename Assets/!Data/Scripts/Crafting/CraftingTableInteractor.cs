using UnityEngine;

public class CraftingTableInteractor : MonoBehaviour
{
    [Header("Crafting Canvas")]
    [SerializeField] GameObject craftingCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        craftingCanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        craftingCanvas.GetComponent<CraftingUI>()?.Back();
        craftingCanvas.SetActive(false);
    }
}