using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using RotaryHeart.Lib.SerializableDictionary;
using System.Linq;
using NaughtyAttributes;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Level Config", menuName = "Scriptable Object/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelData> LevelDatas;
        public string[] LevelTextPaths;
        public LevelData GetLevelDataByID(LevelID _id)
        {
            foreach (LevelData ld in LevelDatas)
            {
                if (ld.LevelID == _id)
                {
                    return ld;
                }
            }
            return new LevelData();
        }
        [Button]
        public void LoadLevelDatas()
        {
            LevelDatas.Clear();
            foreach (string levelPath in LevelTextPaths)
            {
                var gos = Resources.LoadAll(levelPath);
                if (gos == null || gos.Length == 0) return;
                foreach (var go in gos)
                {
                    TextAsset data = (TextAsset)go;
                    string jsonText = data.text;

                    LevelData leveData = JsonUtility.FromJson<LevelData>(jsonText);
                    LevelDatas.Add(leveData);
                }
            }
        }
    }
    [System.Serializable]
    public class UnitData 
    {
        [Range(0, 2)]
        public int Floor;
        public int X;
        public int Y;
        public UnitDirection StartDirection;
        public UnitData(int _x, int _y, UnitDirection _direction)
        {
            X = _x;
            Y = _y;
            StartDirection = _direction;
        }
    }
    [System.Serializable]
    public class LevelData
    {
        public ChapterID ChapterID;
        public LevelType LevelType;
        public LevelID LevelID;
        public WinType WinCondition;
        public int StuffToCollect;
        public string LevelName; 
        public List<BlockType> BlockTypes;
        public List<UnitData> UnitDatas;
        public string LevelRawData;
        public int MinimumExecutionBlock;
        public int MinimumExecutionStep;
        public LevelData(ChapterID _chapterId, LevelType _levelType, LevelID _levelID, WinType _winType, int _stuffToCollect, string _levelName, List<BlockType> _blockTypes, List<UnitData> _unitData, string _levelRawData, int _minimumExecutionBlocks, int _minimumExecutionSteps)
        {
            ChapterID = _chapterId;
            LevelType = _levelType;
            LevelID = _levelID;
            WinCondition = _winType;
            StuffToCollect = _stuffToCollect;
            LevelName = _levelName;
            BlockTypes = _blockTypes;
            UnitDatas = _unitData;
            LevelRawData = _levelRawData;
            MinimumExecutionBlock = _minimumExecutionBlocks;
            MinimumExecutionStep = _minimumExecutionSteps;
        }
        public LevelData()
        {
            ChapterID = ChapterID.CHAPTER_01;
            LevelType = LevelType.HOME;
            WinCondition = WinType.COLLECT_ALL_STUFF;
            StuffToCollect = 0;
            LevelName = "Home";
            BlockTypes = new List<BlockType>();
            UnitDatas = new List<UnitData>();
            LevelRawData = string.Empty;
            MinimumExecutionBlock = 1;
            MinimumExecutionStep = 1;
        }
    }
}
