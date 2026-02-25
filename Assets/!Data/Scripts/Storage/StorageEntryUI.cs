using TMPro;
using UnityEngine;

public class StorageEntryUI : MonoBehaviour
{
    [Header("Storage Entry Elements")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Refresh()
    {
        int stored = StorageManager.Instance.GetStored(resourceType);
        int capacity = StorageManager.Instance.GetCapacity();

        amountText.text = stored + "/" + capacity;
    }

    public void Add10()
    {
        TransferToStorage(10);
    }

    public void AddAll()
    {
        int amount = ResourceManager.Instance.GetAmount(resourceType);
        TransferToStorage(amount);
    }

    public void Remove10()
    {
        TransferToInventory(10);
    }

    public void RemoveAll()
    {
        int amount = StorageManager.Instance.GetStored(resourceType);
        TransferToInventory(amount);
    }

    private void TransferToStorage(int amount)
    {
        int removed = ResourceManager.Instance.Remove(resourceType, amount) ? amount : 0;
        int added = StorageManager.Instance.Add(resourceType, removed);

        if (added < removed)
            ResourceManager.Instance.Add(resourceType, removed - added);

        Refresh();
    }

    private void TransferToInventory(int amount)
    {
        int removed = StorageManager.Instance.Remove(resourceType, amount);
        int added = ResourceManager.Instance.AddClamped(resourceType, removed);

        if (added < removed)
            StorageManager.Instance.Add(resourceType, removed - added);

        Refresh();
    }
}