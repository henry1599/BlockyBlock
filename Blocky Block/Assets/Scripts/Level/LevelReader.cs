using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Managers
{
    // [System.Serializable]
    // public class LevelBlock
    // {
    //     public List<BlockData1> blockDatas;
    // }
    // class BlockData1
    // {
    //     public GroundType groundType;
    //     public Vector3 position;
    // }
    public class LevelReader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameEvents.SETUP_LEVEL += HandleSetupLevel;
        }
        void OnDestroy()
        {
            GameEvents.SETUP_LEVEL -= HandleSetupLevel;
        }
        void HandleSetupLevel(LevelData _data)
        {
            string rawLevelString = _data.LevelRawData.text;
            string[] levelStringEachRow = rawLevelString.Split(";");
            // LevelBlock levelBlock;
            for (int i = levelStringEachRow.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < levelStringEachRow[i].Length; j++)
                {
                    // GroundType groundType = GroundType.GROUND;
                    // switch (levelStringEachRow[i][j])
                    // {
                    //     case 0:
                    //         groundType = GroundType.GROUND;
                    //         break;
                    //     case 1:
                    //         groundType = GroundType.WATER;
                    //         break;
                    // }
                    // Vector3 position = new Vector3(i, 0, j);
                }
            }
        }
    }
}
