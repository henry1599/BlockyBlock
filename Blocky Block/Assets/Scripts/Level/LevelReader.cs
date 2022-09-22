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
        public GroundData[,] GroundDatas;
        public LevelGround() {}
    }
    public class GroundData
    {
        public GroundType groundType;
        public int floorIdx;
        public Vector2 idx;
        public GroundData() {}
        public GroundData(GroundType _type, int _floorIdx, Vector2 _idx)
        {
            this.groundType = _type;
            this.floorIdx = _floorIdx;
            this.idx = _idx;
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
                int squareMapSize = 0;
                int maxRows  = levelStringEachRow.Length - 1;
                int maxColumns = levelStringEachRow[0].Length;

                Core.Grid grid = new Core.Grid(maxRows, maxColumns, Vector3.zero);
                
                for (int i = levelStringEachRow.Length - 2, k = 0; k < levelStringEachRow.Length - 1; i--, k++)
                {
                    for (int j = 0; j < levelStringEachRow[i].Length; j++)
                    {
                        int levelEachRow = levelStringEachRow[i][j] - '0';
                        GroundType groundType = (GroundType)levelEachRow;
                        GroundData groundData = new GroundData(groundType, idxFloor, new Vector2(j, k));
                        grid.GroundDatas.Add(groundData);
                    }
                }
                idxFloor++;
                levelGrounds.Add(levelGround);
                squareMapSize = Mathf.Max(maxRows, maxColumns);
                GameEvents.SETUP_CAMERA?.Invoke(squareMapSize, new Vector2(maxRows, maxColumns));

                GridManager.Instance.AddGrid(grid);
            }
            // print(squareMapSize);
            GameEvents.SETUP_GROUND?.Invoke();
        }
    }
}
