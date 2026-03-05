using UnityEngine;

public class StorageBuilding : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject storageLvl1;
    [SerializeField] private GameObject storageLvl2;
    [SerializeField] private GameObject storageLvl3;
    [SerializeField] private GameObject storageLvl4;
    [SerializeField] private GameObject storageLvl5;
    [SerializeField] private GameObject storageLvl6;
    [SerializeField] private GameObject storageLvl7;
    [SerializeField] private GameObject storageLvl8;
    [SerializeField] private GameObject storageLvl9;
    [SerializeField] private GameObject storageLvl10;

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
        storageLvl4.SetActive(level == 4);
        storageLvl5.SetActive(level == 5);
        storageLvl6.SetActive(level == 6);
        storageLvl7.SetActive(level == 7);
        storageLvl8.SetActive(level == 8);
        storageLvl9.SetActive(level == 9);
        storageLvl10.SetActive(level == 10);
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