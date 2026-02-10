using UnityEngine;

[CreateAssetMenu(menuName = "Expandia/Tool Icon Database")]
public class ToolIconDatabase : ScriptableObject
{
    [System.Serializable]
    public class ToolIconEntry
    {
        public ToolType type;
        public ToolMaterial material;
        public Sprite icon;
    }

    public ToolIconEntry[] icons;

    public Sprite GetIcon(ToolType type, ToolMaterial material)
    {
        foreach (var entry in icons)
        {
            if (entry.type == type && entry.material == material)
                return entry.icon;
        }

        return null;
    }
}