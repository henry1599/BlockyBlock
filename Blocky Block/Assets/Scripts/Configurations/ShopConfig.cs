using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using BlockyBlock.Enums;
using NaughtyAttributes;

namespace BlockyBlock.Configurations
{
    [System.Serializable]
    public class ShopDict : SerializableDictionaryBase<CustomizationType, ShopItemList>{}
    [System.Serializable]
    public class ShopDictPath : SerializableDictionaryBase<CustomizationType, string>{}
    [CreateAssetMenu(fileName = "Shop Config", menuName = "Scriptable Object/Shop Config")]
    public class ShopConfig : ScriptableObject
    {
        public Dictionary<string, string> abc;
        public ShopDictPath ShopPaths;
        public ShopDict ShopDict;
        public List<ShopItemData> GetItemListByCustomizationType(CustomizationType type) => ShopDict[type].itemList;
        [Button("Clear Sprites")]
        public void ClearSprites()
        {
            ShopDict.Clear();
        }
        [Button("Load Sprites")]
        public void LoadSprites()
        {
            ShopDict.Clear();
            foreach (var (type, path) in ShopPaths)
            {
                var gos = Resources.LoadAll(path);
                Debug.Log(gos.Length);
                if (gos == null || gos.Length == 0)
                    return;
                ShopItemList shopItemList = new ShopItemList();
                int idx = 0;
                foreach (var go in gos)
                {
                    Texture2D sprite = go as Texture2D;
                    if (sprite == null)
                    {
                        continue;
                    }
                    shopItemList.itemList.Add(
                        new ShopItemData(
                            idx,
                            sprite
                        )
                    );

                    idx++;
                }
                ShopDict.Add(type, shopItemList);
            }
        }
    }
    [System.Serializable]
    public class ShopItemList
    {
        public List<ShopItemData> itemList;
        public ShopItemList() => this.itemList = new List<ShopItemData>();
#if UNITY_EDITOR
        void OnValidate() 
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                this.itemList[i].id = i;
            }
        }
#endif
    }
    [System.Serializable]
    public class ShopItemData
    {
        public int id;
        public Texture2D sprite;
        public ShopItemData(int id, Texture2D sprite)
        {
            this.id = id;
            this.sprite = sprite;
        }
    }
}
