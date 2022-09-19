using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Blocks Config", menuName = "Scriptable Object/Blocks Config")]
    public class BlockConfig : ScriptableObject
    {
        public Block Blocks;
        public float GetDelayTime(BlockType _type)
        {
            return Blocks[_type].ExecutionTime;
        }
    }
    [System.Serializable]
    public class Block : SerializableDictionaryBase<BlockType, UIBlockData> {}
    [System.Serializable]
    public class UIBlockData
    {
        public string Text;
        public float ExecutionTime;
        public Color TopColor;
        public Color ShadowColor;
    }
}
