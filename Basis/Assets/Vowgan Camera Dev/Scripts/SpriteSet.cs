using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BasisUI/Sprite Set")]
public class SpriteSet : ScriptableObject
{
    [SerializeField] private Dictionary<string, Sprite> _sprites = new();

    public bool TryGetSprite(string spriteName, out Sprite sprite)
    {
        if (string.IsNullOrEmpty(spriteName))
        {
            sprite = null;
            return false;
        }

        if (_sprites.TryGetValue(spriteName, out sprite)) return true;

        _sprites[spriteName] = null;
        return false;
    }

    public void SetSprite(string spriteName, Sprite sprite)
    {
        _sprites[spriteName] = sprite;
    }
}
