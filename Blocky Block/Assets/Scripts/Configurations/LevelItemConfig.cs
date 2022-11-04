using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using BlockyBlock.Enums;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Level Menu Config", menuName = "Scriptable Object/Level Menu Config")]
    public class LevelItemConfig : ScriptableObject
    {
        public LevelItem LevelItemTemplate;
        public LevelItems LevelItems;
        public int GetLevelCount()
        {
            return LevelItems.Count;
        }
    }
    [System.Serializable]
    public class LevelItems : SerializableDictionaryBase<LevelID, LevelItemData> {}
    [System.Serializable]
    public class LevelItemData
    {
        public string LevelNameDisplay;
    }
}
