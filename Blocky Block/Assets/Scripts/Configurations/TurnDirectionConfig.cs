using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Turn Direction Config", menuName = "Scriptable Object/Turn Direction Config")]
    public class TurnDirectionConfig : ScriptableObject
    {
        public TurnDirectionData Data;
    }
    [System.Serializable]
    public class TurnDirectionData : SerializableDictionaryBase<TurnDirection, string> {}
}
