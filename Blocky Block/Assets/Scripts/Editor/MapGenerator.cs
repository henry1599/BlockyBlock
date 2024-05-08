#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
#endif
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;

namespace BlockyBlock.Utils.EditorTools
{
    public enum LevelIDEditor
    {
        HOME = 0,
        LEVEL_MANNUAL_00 = 1000,
        LEVEL_MANNUAL_01 = 1001,
        LEVEL_MANNUAL_02 = 1002,
        LEVEL_MANNUAL_03 = 1003,
        LEVEL_MANNUAL_04 = 1004,
        LEVEL_MANNUAL_05 = 1005,
        LEVEL_MANNUAL_06 = 1006,
        LEVEL_MANNUAL_07 = 1007,
        LEVEL_MANNUAL_08 = 1008,
        LEVEL_MANNUAL_09 = 1009,
        LEVEL_MANNUAL_10 = 1010,
        LEVEL_MANNUAL_11 = 1011,
        LEVEL_MANNUAL_12 = 1012,
    }
    public enum ChapterTypeEditor
    {
        CHAPTER_01 = 0,
        CHAPTER_02 = 1,
        CHAPTER_03 = 2,
    }
    public enum LevelTypeEditor
    {
        HOME = 0,
        MANUAL = 1,
        CUSTOM = 2,
        EVENT = 3
    }
    public enum UnitDirectionEditor
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3
    }
    public enum WinTypeEditor
    {
        COLLECT_ALL_STUFF = 1000,
        COLLECT_THE_CHEST = 1001,
        REACH_TO_POSITION = 1002
    }
    public enum BlockTypeEditor
    {
        MOVE_FORWARD = 0,
        PICK_UP = 1,
        PUT_DOWN = 2,
        DO_UNTIL = 3,
        TURN = 4,
        JUMP = 5,
        PUSH = 7,
        JUMP_GRAB_STH = 8,
        JUMP_IF_STH_FRONT = 12
    }
    public enum GroundTypeEditor {
        Ground = 0,
        Water = 1,
        Tree = 2,
        Box = 3,
        None = 4,
        Trap = 5,
        Star = 6
    }
    public class MapGenerator : EditorWindow
    {
        [SerializeField] VisualTreeAsset m_Tree;
        private EnumField m_ChapterTypeEnumField;
        private EnumField m_LevelTypeEnumField;
        private EnumField m_LevelIDEnumField;
        private EnumField m_WinConditionEnumField;
        private SliderInt m_StuffToCollectField;
        private IntegerField m_MinBlocksUsedField;
        private IntegerField m_MinStepsPassedField;
        private SliderInt m_BlockNumberField;
        private Vector2IntField m_UnitPositionField;
        private EnumField m_UnitStartDirectionEnumField;
        private IMGUIContainer m_BlockEnumContainer;
        List<EnumField> m_BlockEnums = new List<EnumField>();
        private Button m_BlocksButton;
        float m_BlockButtonWidth = 200;
        float m_BlockButtonHeight = 20;
        private SliderInt m_WidthSlider;
        private SliderInt m_HeightSlider;
        private Button m_InitButton;
        private Button m_GenerateButton;
        private IMGUIContainer m_ButtonContainer;
        float m_ButtonWidth = 65;
        float m_ButtonHeight = 20;
        List<EnumField> m_Enums = new List<EnumField>();
        LevelData m_LevelData;
        public const string LevelFolder = "Resources/Data/Levels";
        string TextFileName = "Level0";
        static MapGenerator thisWindow;
        public string GetPath()
        {
#if UNITY_EDITOR
            string path = Application.dataPath + $"/{LevelFolder}/" ;
            return path;
#elif UNITY_ANDROID
            string path = Application.persistentDataPath + $"/{LevelFolder}/";
            return path;
#elif UNITY_IPHONE
            string path = Application.persistentDataPath + $"/{LevelFolder}/";
            return path;
#else
            string path = Application.dataPath + $"/{LevelFolder}/";
            return path;
#endif
        }

        [MenuItem("Tools/Map Generator")]
        public static void ShowEditor()
        {
            var window = GetWindow<MapGenerator>();
            window.titleContent = new GUIContent("Map Generator");

            thisWindow = window;
        }
        void CreateGUI()
        {
            m_Tree.CloneTree(rootVisualElement);

            m_ChapterTypeEnumField = rootVisualElement.Q<EnumField>("_chapterType");
            m_LevelTypeEnumField = rootVisualElement.Q<EnumField>("_levelType");
            m_LevelIDEnumField = rootVisualElement.Q<EnumField>("_levelID");
            m_WinConditionEnumField = rootVisualElement.Q<EnumField>("_winCondition");
            m_StuffToCollectField = rootVisualElement.Q<SliderInt>("_stuffToCollect");
            m_MinBlocksUsedField = rootVisualElement.Q<IntegerField>("_minBlockUsed");
            m_MinStepsPassedField = rootVisualElement.Q<IntegerField>("_minStepPassed");
            m_BlockNumberField = rootVisualElement.Q<SliderInt>("_blockNumber");
            m_UnitPositionField = rootVisualElement.Q<Vector2IntField>("_unitPosition");
            m_UnitStartDirectionEnumField = rootVisualElement.Q<EnumField>("_unitStartDirection");
            m_BlockEnumContainer = rootVisualElement.Q<IMGUIContainer>("_blocksContainer");
            m_BlocksButton = rootVisualElement.Q<Button>("_blocksButton");

            m_WidthSlider = rootVisualElement.Q<SliderInt>("_width");
            m_HeightSlider = rootVisualElement.Q<SliderInt>("_height");
            m_ButtonContainer = rootVisualElement.Q<IMGUIContainer>("_mapElementsField");
            m_InitButton = rootVisualElement.Q<Button>("_initBtn");
            m_GenerateButton = rootVisualElement.Q<Button>("_generateBtn");

            m_InitButton.clicked += GenerateButton;
            m_GenerateButton.clicked += HandleGenerateBtnClicked;
            m_BlocksButton.clicked += AddBlockButtons;

            m_ChapterTypeEnumField.Init(ChapterTypeEditor.CHAPTER_01);
            m_LevelTypeEnumField.Init(LevelTypeEditor.MANUAL);
            m_LevelIDEnumField.Init(LevelIDEditor.LEVEL_MANNUAL_00);
            m_WinConditionEnumField.Init(WinTypeEditor.COLLECT_ALL_STUFF);
            m_UnitStartDirectionEnumField.Init(UnitDirectionEditor.UP);
        }
        void OnDestroy() 
        {
            m_InitButton.clicked -= GenerateButton;  
            m_GenerateButton.clicked -= HandleGenerateBtnClicked;  
            m_BlocksButton.clicked -= AddBlockButtons;
        }
        void AddBlockButtons()
        {
            foreach (EnumField e in m_BlockEnums)
            {
                e.UnregisterCallback<ChangeEvent<System.Enum>>(evt =>
                {
                    e.name = GetCharByGroundTypeEditor((GroundTypeEditor)evt.newValue);
                });
            }
            m_BlockEnums.Clear();
            m_BlockEnumContainer?.Clear();

            int numberOfButtons = m_BlockNumberField.value;
            Debug.Log(numberOfButtons);
            m_BlockEnumContainer.style.width = m_BlockButtonWidth;
            m_BlockEnumContainer.style.height = m_BlockButtonHeight * numberOfButtons;
            for (int i = 0; i < numberOfButtons; i++)
            {
                m_BlockEnumContainer?.Add(CreateBlockEnum());
            }
        }
        void GenerateButton()
        {
            foreach (EnumField e in m_Enums)
            {
                e.UnregisterCallback<ChangeEvent<System.Enum>>(evt =>
                {
                    e.name = GetCharByGroundTypeEditor((GroundTypeEditor)evt.newValue);
                });
            }
            m_Enums.Clear();
            m_ButtonContainer.Clear();
            int numberOfButtons = (int)m_WidthSlider.value * (int)m_HeightSlider.value;
            m_ButtonContainer.style.width = m_ButtonWidth * m_WidthSlider.value;
            m_ButtonContainer.style.height = m_ButtonHeight * m_HeightSlider.value;
            for (int i = 0; i < numberOfButtons; i++)
            {
                m_ButtonContainer.Add(CreateEnum());
            }
        }
        EnumField CreateBlockEnum()
        {
            EnumField e = new EnumField(BlockTypeEditor.MOVE_FORWARD);
            e.style.width = m_BlockButtonWidth;
            e.style.height = m_BlockButtonHeight;
            e.label = "";
            e.style.marginLeft = 0;
            e.style.marginRight = 0;
            e.style.marginBottom = 0;
            e.style.marginTop = 0;
            e.style.paddingLeft = 0;
            e.style.paddingRight = 0;
            e.style.paddingBottom = 0;
            e.style.paddingTop = 0;
            e.name = "0";
            
            e.RegisterCallback<ChangeEvent<System.Enum>>(evt =>
            {
                e.name = GetCharByBlockTypeEditor((BlockTypeEditor)evt.newValue);
            });

            m_BlockEnums.Add(e);

            return e;
        }
        EnumField CreateEnum()
        {
            EnumField e = new EnumField(GroundTypeEditor.Ground);
            e.style.width = m_ButtonWidth;
            e.style.height = m_ButtonHeight;
            e.label = "";
            e.style.marginLeft = 0;
            e.style.marginRight = 0;
            e.style.marginBottom = 0;
            e.style.marginTop = 0;
            e.style.paddingLeft = 0;
            e.style.paddingRight = 0;
            e.style.paddingBottom = 0;
            e.style.paddingTop = 0;
            e.name = "0";
            
            e.RegisterCallback<ChangeEvent<System.Enum>>(evt =>
            {
                e.name = GetCharByGroundTypeEditor((GroundTypeEditor)evt.newValue);
            });

            m_Enums.Add(e);

            return e;
        }
        void HandleGenerateBtnClicked()
        {
            int width = m_WidthSlider.value;
            int height = m_HeightSlider.value;

            string resultString = "";
            string folderPath = GetPath() + GetChapterPathByChapterTypeEditor((ChapterTypeEditor)m_ChapterTypeEnumField.value);
            // var dir = new System.IO.DirectoryInfo(folderPath);
            // int fileNum = dir.GetFiles().Length / 2;
            int levelId = (int)(LevelIDEditor)this.m_LevelIDEnumField.value - 999;
            string levelName = "Level " + GetLevelNameByEnum((LevelTypeEditor)m_LevelTypeEnumField.value) + " " + levelId.ToString("00");
            string textName = TextFileName + levelId.ToString();
            string finalPath = folderPath + textName + ".txt";

            for (int i = 0; i < height; i++)
            {
                string stringEachRow = "";
                for (int j = 0; j < width; j++)
                {
                    string split = m_Enums[i * width + j].name;
                    stringEachRow += split;
                }
                stringEachRow += ";";
                resultString += stringEachRow;
            }
            List<UnitData> unitDatas = new List<UnitData>();
            unitDatas.Add(new UnitData(
                m_UnitPositionField.value.x + 1,
                m_UnitPositionField.value.y + 1,
                (UnitDirection)(m_UnitStartDirectionEnumField.value)
            ));
            List<BlockType> blockTypes = new List<BlockType>();
            foreach (EnumField e in m_BlockEnums)
            {
                blockTypes.Add((BlockType)e.value);
            }
            m_LevelData = new LevelData(
                (ChapterID)m_ChapterTypeEnumField.value,
                (LevelType)m_LevelTypeEnumField.value,
                (LevelID)m_LevelIDEnumField.value,
                (WinType)m_WinConditionEnumField.value,
                m_StuffToCollectField.value,
                levelName,
                blockTypes,
                unitDatas,
                resultString,
                m_MinBlocksUsedField.value,
                m_MinStepsPassedField.value
            );
            string json = JsonUtility.ToJson(m_LevelData);

            File.WriteAllText(finalPath, json);
            AssetDatabase.Refresh();

            TextAsset textAsset = new TextAsset(json);
            AssetDatabase.SaveAssets();

            Debug.Log(string.Format("Successfully created data txt named: {0} at path: {1}", textName, finalPath));
        }
        string GetLevelNameByEnum(LevelTypeEditor _type)
        {
            switch (_type)
            {
                case LevelTypeEditor.HOME:
                    return "Home";
                case LevelTypeEditor.MANUAL:
                    return "Mannual";
                case LevelTypeEditor.CUSTOM:
                    return "Custom";
                case LevelTypeEditor.EVENT:
                    return "Event";
                default:
                    return "Mannual";
            }
        }
        string GetChapterPathByChapterTypeEditor(ChapterTypeEditor _type)
        {
            switch (_type)
            {
                case ChapterTypeEditor.CHAPTER_01:
                    return "Chapter 1/";
                case ChapterTypeEditor.CHAPTER_02:
                    return "Chapter 2/";
                case ChapterTypeEditor.CHAPTER_03:
                    return "Chapter 3/";
                default:
                    return "Chapter 1/";
            }
        }
        string GetCharByLevelTypeEditor(LevelTypeEditor _type)
        {
            switch (_type)
            {
                case LevelTypeEditor.HOME:
                    return "0";
                case LevelTypeEditor.MANUAL:
                    return "1";
                case LevelTypeEditor.CUSTOM:
                    return "2";
                case LevelTypeEditor.EVENT:
                    return "3";
                default:
                    return "0";
            }
        }
        string GetCharByWinTypeEditor(WinTypeEditor _type)
        {
            switch (_type)
            {
                case WinTypeEditor.COLLECT_ALL_STUFF:
                    return "1000";
                case WinTypeEditor.COLLECT_THE_CHEST:
                    return "1001";
                case WinTypeEditor.REACH_TO_POSITION:
                    return "1002";
                default:
                    return "1000";
            }
        }
        string GetCharByUnitDirectionEditor(UnitDirectionEditor _type)
        {
            switch (_type)
            {
                case UnitDirectionEditor.UP:
                    return "0";
                case UnitDirectionEditor.DOWN:
                    return "1";
                case UnitDirectionEditor.LEFT:
                    return "2";
                case UnitDirectionEditor.RIGHT:
                    return "3";
                default:
                    return "0";
            }
        }
        string GetCharByBlockTypeEditor(BlockTypeEditor _type)
        {
            switch (_type)
            {
                case BlockTypeEditor.MOVE_FORWARD:
                    return "0";
                case BlockTypeEditor.PICK_UP:
                    return "1";
                case BlockTypeEditor.PUT_DOWN:
                    return "2";
                case BlockTypeEditor.DO_UNTIL:
                    return "3";
                case BlockTypeEditor.TURN:
                    return "4";
                case BlockTypeEditor.JUMP:
                    return "5";
                case BlockTypeEditor.PUSH:
                    return "7";
                case BlockTypeEditor.JUMP_GRAB_STH:
                    return "8";
                case BlockTypeEditor.JUMP_IF_STH_FRONT:
                    return "12";
                default:
                    return "0";
            }
        }
        string GetCharByGroundTypeEditor(GroundTypeEditor _type)
        {
            switch (_type)
            {
                case GroundTypeEditor.Ground:
                    return "0";
                case GroundTypeEditor.Water:
                    return "1";
                case GroundTypeEditor.Tree:
                    return "2";
                case GroundTypeEditor.Box:
                    return "3";
                case GroundTypeEditor.None:
                    return "4";
                case GroundTypeEditor.Trap:
                    return "5";
                case GroundTypeEditor.Star:
                    return "6";
                default:
                    return "4";
            }
        }
    }
}
