using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using BlockyBlock.Enums;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Level Item Config", menuName = "Scriptable Object/Level Item Config")]
    public class LevelItemConfig : ScriptableObject
    {
        public LevelItems LevelItems;
    }
    [System.Serializable]
    public class LevelItems : SerializableDictionaryBase<LevelStatus, LevelItem> {}
}
