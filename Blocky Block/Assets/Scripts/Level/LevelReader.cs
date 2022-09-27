using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using RotaryHeart.Lib.SerializableDictionary;
using System.Text.RegularExpressions;
using System.Linq;

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
            // * levelFloors = ["000;000;000;","111;111;111"]

            List<List<string>> stringEachFloor = new List<List<string>>();
            foreach (string levelFloor in levelFloors)
            {
                stringEachFloor.Add(levelFloor.Split(";").ToList());
            }
            // * stringEachFloor = <<"000","000","000">,<"111","111","111">>
            
            for (int j = 0; j < stringEachFloor.Count; j++)
            {
                //* rawStringEachFloor = ["000","000","000"]
                for (int i = 0; i < stringEachFloor[j].Count - 1; i++)
                {
                    stringEachFloor[j][i] = "4" + stringEachFloor[j][i] + "4";
                }
                int stringWidth = stringEachFloor[j][0].Length;
                string borderTopBot = new string('4', stringWidth);
                stringEachFloor[j].Insert(0, borderTopBot);
                stringEachFloor[j][stringEachFloor[j].Count - 1] = borderTopBot;
            }
            List<LevelGround> levelGrounds = new List<LevelGround>();
            int idxFloor = 0;
            foreach (List<string> levelStringEachRow in stringEachFloor)
            {
                LevelGround levelGround = new LevelGround();
                int squareMapSize = 0;
                int maxRows  = levelStringEachRow.Count;
                int maxColumns = levelStringEachRow[0].Length;

                Core.Grid grid = new Core.Grid(maxColumns, maxRows, Vector3.zero, idxFloor);
                
                for (int i = levelStringEachRow.Count - 1, k = 0; k < levelStringEachRow.Count; i--, k++)
                {
                    for (int j = 0; j < levelStringEachRow[i].Length; j++)
                    {
                        int levelEachRow = levelStringEachRow[i][j] - '0';
                        GroundType groundType = GetGroundTypeByInt(levelEachRow);
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
        GroundType GetGroundTypeByInt(int i)
        {
            switch (i)
            {
                case 0:
                    return GroundType.GROUND;
                case 1:
                    return GroundType.WATER;
                case 2:
                    return GroundType.TREE;
                case 3:
                    return GroundType.BOX_ON_GROUND;
                case 4:
                    return GroundType.SPACE;
                case 5:
                    return GroundType.TRAP;
                case 6:
                    return GroundType.COLLECTIBLE;
                case 7:
                    return GroundType.BOX_IN_WATER;
                default:
                    return GroundType.GROUND;
            }
        }
    }
}
