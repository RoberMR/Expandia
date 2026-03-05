using UnityEngine;

public class ConstructionZoneInteractor : MonoBehaviour
{
    [Header("Construction Canvas")]
    [SerializeField] private GameObject constructionCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        constructionCanvas.SetActive(true);
        constructionCanvas.GetComponent<ConstructionUI>()?.Back();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        constructionCanvas.GetComponent<ConstructionUI>()?.Back();
        constructionCanvas.SetActive(false);
    }
}