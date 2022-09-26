using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using BlockyBlock.Enums;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Error Config", menuName = "Scriptable Object/Error Config")]
    public class ErrorConfig : ScriptableObject
    {
        public ErrorData ErrorData;
    }
    [System.Serializable]
    public class ErrorData : SerializableDictionaryBase<ErrorType, string> {}
}
