using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
#if false
using UnityEngine.AddressableAssets;


public class SpriteSheetManager
{
    private static Dictionary<string, Sprite> spriteSheets = new Dictionary<string, Sprite>();

    public static void Load()
    {
        var loadCall = Addressables.LoadAssetAsync<SpriteAtlas>("Assets/Images/Atlas/Image_Atlas.spriteatlas");
        var atals = loadCall.WaitForCompletion();

        Sprite[] sprites =new Sprite[atals.spriteCount];
        atals.GetSprites(sprites);
        
        foreach (Sprite sprite in sprites)
        {
            var newName= sprite.name.Replace("(Clone)", "");
            if (!spriteSheets.ContainsKey(sprite.name))
                spriteSheets.Add(newName, sprite);
        }
    }

    public static Sprite GetSpriteByName(string name)
    {
        if (spriteSheets.ContainsKey(name))
            return spriteSheets[name];
        
        return null;
    }
}
#endif
