using TMPro;
using UnityEngine;

public class StorageUI : MonoBehaviour
{
    public GameObject panel;

    [Header("Entries")]
    [SerializeField] private StorageEntryUI woodEntry;
    [SerializeField] private StorageEntryUI stoneEntry;
    [SerializeField] private StorageEntryUI ironEntry;
    [SerializeField] private StorageEntryUI foodEntry;
    [SerializeField] private StorageEntryUI leatherEntry;

    private void Start()
    {
        panel.SetActive(false);
        StorageManager.Instance.OnStorageChanged += Refresh;
        StorageManager.Instance.OnStorageLevelChanged += Refresh;
    }

    private void OnDestroy()
    {
        if (StorageManager.Instance != null)
        {
            StorageManager.Instance.OnStorageChanged -= Refresh;
            StorageManager.Instance.OnStorageLevelChanged -= Refresh;
        }
    }

    public void Show()
    {
        panel.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    public void Refresh()
    {
        if (!panel.activeSelf) return;

        woodEntry.Refresh();
        stoneEntry.Refresh();
        ironEntry.Refresh();
        foodEntry.Refresh();
        leatherEntry.Refresh();
    }
}