using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Level Config", menuName = "Scriptable Object/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public float SpaceEachFloor;
        public Level LevelDatas;
        public LevelData GetLevelDataByID(LevelID _id)
        {
            return LevelDatas[_id];
        }
        public string GetSceneNameByID(LevelID _id)
        {
            return LevelDatas[_id].LevelName;
        }
    }
    [System.Serializable]
    public class Level : SerializableDictionaryBase<LevelID, LevelData> {}
    [System.Serializable]
    public class UnitData 
    {
        [Range(0, 2)]
        public int Floor;
        public Vector3 StartPosition;
        public UnitDirection StartDirection;
    }
    [System.Serializable]
    public class LevelData
    {
        public LevelType LevelType;
        public string LevelName; 
        public List<BlockType> BlockTypes;
        public List<UnitData> UnitDatas;
        // * 00000
        // * 00000
        // * 00000
        // * 00000
        // * 00000
        // * 0 : ground
        // * 1 : water
        public TextAsset LevelRawData;
    }
}
