using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Camera Config", menuName = "Scriptable Object/Camera Config")]
    public class CameraConfig : ScriptableObject
    {
        public CameraData CameraDatas;
    }
    
    [System.Serializable]
    public class CameraData : SerializableDictionaryBase<int, Vector2> {}
}
