using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using BlockyBlock.Enums;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Behaviour Config", menuName = "Scriptable Object/Behaviour Config")]
    public class BehaviourConfig : ScriptableObject
    {
        public BehaviourData BehaviourData;
    }
    [System.Serializable]
    public class GroudTypes
    {
        public List<GroundType> GroundTypes;
    }
    [System.Serializable]
    public class BehaviourData : SerializableDictionaryBase<ErrorType, GroudTypes> {}
}
