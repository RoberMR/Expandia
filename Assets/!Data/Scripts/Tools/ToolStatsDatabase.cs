using UnityEngine;

[CreateAssetMenu(
    fileName = "ToolStatsDatabase",
    menuName = "Game/Tools/Tool Stats Database"
)]
public class ToolStatsDatabase : ScriptableObject
{
    public ToolStats[] stats;

    public ToolStats GetStats(ToolMaterial material)
    {
        foreach (var s in stats)
        {
            if (s.material == material)
                return s;
        }

        Debug.LogError("No stats found for " + material);
        return null;
    }
}