using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "API Config", menuName = "Scriptable Object/APIs Config")]
    public class APIConfig : ScriptableObject
    {
        public WebData WebData;
        public APIData APIData;
    }
    [System.Serializable]
    public class APIData : SerializableDictionaryBase<APIType, string> {}
    [System.Serializable]
    public class WebData : SerializableDictionaryBase<WebType, string> {}
}
