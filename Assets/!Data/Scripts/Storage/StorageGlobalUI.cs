using UnityEngine;

public class StorageGlobalUI : MonoBehaviour
{
    public void AddAllResources()
    {
        foreach (var type in ResourceManager.Instance.GetAllResourceTypes())
        {
            int amount = ResourceManager.Instance.GetAmount(type);
            TransferToStorage(type, amount);
        }
    }

    public void RemoveAllResources()
    {
        foreach (var type in ResourceManager.Instance.GetAllResourceTypes())
        {
            int amount = StorageManager.Instance.GetStored(type);
            TransferToInventory(type, amount);
        }
    }

    private void TransferToStorage(ResourceType type, int amount)
    {
        int removed = ResourceManager.Instance.Remove(type, amount) ? amount : 0;
        int added = StorageManager.Instance.Add(type, removed);

        if (added < removed)
            ResourceManager.Instance.Add(type, removed - added);
    }

    private void TransferToInventory(ResourceType type, int amount)
    {
        int removed = StorageManager.Instance.Remove(type, amount);
        int added = ResourceManager.Instance.AddClamped(type, removed);

        if (added < removed)
            StorageManager.Instance.Add(type, removed - added);
    }
}