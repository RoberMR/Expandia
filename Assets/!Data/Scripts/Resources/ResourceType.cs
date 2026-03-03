using UnityEngine;

[CreateAssetMenu(menuName = "Expandia/Resource Type")]
public class ResourceType : ScriptableObject
{
    public string resourceId;
    public string displayName;
    public Sprite icon;

    [Header("Behaviour")]
    public bool isUnlimited;
}