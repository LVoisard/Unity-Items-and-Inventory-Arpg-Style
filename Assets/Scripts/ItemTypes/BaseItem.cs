using UnityEngine;

[System.Serializable]
public abstract class BaseItem : ScriptableObject
{
    public string itemName;
    public Vector2Int itemSize;
    public Sprite image;
    public AudioClip dropSound;
}