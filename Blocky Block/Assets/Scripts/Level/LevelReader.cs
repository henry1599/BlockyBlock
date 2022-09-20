using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using RotaryHeart.Lib.SerializableDictionary;
using System.Text.RegularExpressions;

namespace BlockyBlock.Managers
{
    [System.Serializable]
    public class LevelGround
    {
        public float Height;
        public List<GroundData> groundDatas;
        public LevelGround() => groundDatas = new List<GroundData>();
    }
    public class GroundData
    {
        public GroundType groundType;
        public Vector3 position;
        public GroundData(GroundType _type, Vector3 _position)
        {
            this.groundType = _type;
            this.position = _position;
        }
    }
    public class LevelReader : MonoBehaviour
    {
        public static LevelReader Instance {get; private set;}
        void Awake()
        {
            Instance = this;
        }
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
            string formatString = Regex.Replace(rawLevelString, @"[\r\n\t ]+", "");
            string[] levelFloors = formatString.Split("-");
            List<string[]> stringEachFloor = new List<string[]>();
            foreach (string levelFloor in levelFloors)
            {
                stringEachFloor.Add(levelFloor.Split(";"));
            }
            List<LevelGround> levelGrounds = new List<LevelGround>();
            int idxFloor = 0;
            foreach (string[] levelStringEachRow in stringEachFloor)
            {
                LevelGround levelGround = new LevelGround();
                int maxRows = 0;
                int maxColumns = 0;
                int squareMapSize = 0;
                maxRows  = Mathf.Max(maxRows, levelStringEachRow.Length - 1);
                for (int i = levelStringEachRow.Length - 2, k = 0; k < levelStringEachRow.Length - 1; i--, k++)
                {
                    maxColumns = Mathf.Max(maxColumns, levelStringEachRow[i].Length);
                    for (int j = 0; j < levelStringEachRow[i].Length; j++)
                    {
                        int levelEachRow = levelStringEachRow[i][j] - '0';
                        GroundType groundType = (GroundType)levelEachRow;
                        Vector3 position = new Vector3(j, 0, k);
                        levelGround.groundDatas.Add(new GroundData(groundType, position));
                        levelGround.Height = idxFloor * ConfigManager.Instance.LevelConfig.SpaceEachFloor;
                    }
                }
                idxFloor++;
                levelGrounds.Add(levelGround);
                squareMapSize = Mathf.Max(maxRows, maxColumns);
                GameEvents.SETUP_CAMERA?.Invoke(squareMapSize, new Vector2(maxRows, maxColumns));
            }
            // print(squareMapSize);
            GameEvents.SETUP_GROUND?.Invoke(levelGrounds);
        }
    }
}
