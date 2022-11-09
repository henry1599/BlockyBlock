using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Ground Type Data Config", menuName = "Scriptable Object/Ground Type Data Config")]
    public class GroundFrontDataConfig : ScriptableObject
    {
        public GroundTypeData Data;
    }
    [System.Serializable]
    public class GroundTypeData : SerializableDictionaryBase<GroundType, string> {}
}
