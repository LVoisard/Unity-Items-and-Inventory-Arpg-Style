using UnityEngine;

[System.Serializable]
public abstract class BaseItem
{
    internal Vector2Int itemSize;
    internal string imagePath;
    internal EquipmentSlotType ItemType;
}