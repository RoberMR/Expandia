using UnityEngine;

public class StorageBuilding : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject storageLvl1;
    [SerializeField] private GameObject storageLvl2;
    [SerializeField] private GameObject storageLvl3;

    [Header("UI")]
    [SerializeField] private StorageUI storageUI;

    private void Start()
    {
        CraftingManager.Instance.OnStorageCrafted += UpdateVisual;
        PlayerStorageUpgrades.Instance.OnStorageLevelChanged += UpdateVisual;

        UpdateVisual();
    }

    private void OnDestroy()
    {
        if (CraftingManager.Instance != null)
            CraftingManager.Instance.OnStorageCrafted -= UpdateVisual;

        if (PlayerStorageUpgrades.Instance != null)
            PlayerStorageUpgrades.Instance.OnStorageLevelChanged -= UpdateVisual;
    }

    private void UpdateVisual()
    {
        int level = PlayerStorageUpgrades.Instance.storageLevel;

        storageLvl1.SetActive(level == 1);
        storageLvl2.SetActive(level == 2);
        storageLvl3.SetActive(level == 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerStorageUpgrades.Instance.storageLevel > 0)
            storageUI.Show();
    }

    private void OnTriggerExit(Collider other)
    {
        storageUI.Hide();
    }
}