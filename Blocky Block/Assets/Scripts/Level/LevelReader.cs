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
            string rawLevelString = _data.LevelRawData;
            string formatString = Regex.Replace(rawLevelString, @"[\r\n\t]+", "");
            // * formatString = "000;000;000;"

            List<string> stringFloor = new List<string>();
            stringFloor = formatString.Split(";").ToList();
            // * stringFloor = <"000","000","000">
            
            //* rawStringEachFloor = ["000","000","000"]
            for (int i = 0; i < stringFloor.Count - 1; i++)
            {
                stringFloor[i] = "4" + stringFloor[i] + "4";
            }
            int stringWidth = stringFloor[0].Length;
            string borderTopBot = new string('4', stringWidth);
            stringFloor.Insert(0, borderTopBot);
            stringFloor[stringFloor.Count - 1] = borderTopBot;
                
            List<LevelGround> levelGrounds = new List<LevelGround>();
            int idxFloor = 0;

            LevelGround levelGround = new LevelGround();
            int squareMapSize = 0;
            int maxRows  = stringFloor.Count;
            int maxColumns = stringFloor[0].Length;

            Core.Grid grid = new Core.Grid(maxColumns, maxRows, Vector3.zero, idxFloor);
            
            for (int i = stringFloor.Count - 1, k = 0; k < stringFloor.Count; i--, k++)
            {
                for (int j = 0; j < stringFloor[i].Length; j++)
                {
                    int levelEachRow = stringFloor[i][j] - '0';
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
            
            GameEvents.SETUP_GROUND?.Invoke();
            // string json = JsonUtility.ToJson(_data);
            // print("[JSON LEVEL] : " + json);
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
