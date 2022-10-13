#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
#endif
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using BlockyBlock.Enums;

namespace BlockyBlock.Editor 
{
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
        private SliderInt m_WidthSlider;
        private SliderInt m_HeightSlider;
        private Button m_InitButton;
        private Button m_GenerateButton;
        private IMGUIContainer m_ButtonContainer;
        float m_ButtonWidth = 65;
        float m_ButtonHeight = 20;
        List<EnumField> m_Enums = new List<EnumField>();
        public const string LevelFolder = "Resources/Data/Levels";
        string TextFileName = "Level0";
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
        }
        void CreateGUI()
        {
            m_Tree.CloneTree(rootVisualElement);
            m_WidthSlider = rootVisualElement.Q<SliderInt>("_width");
            m_HeightSlider = rootVisualElement.Q<SliderInt>("_height");
            m_ButtonContainer = rootVisualElement.Q<IMGUIContainer>("_mapElementsField");
            m_InitButton = rootVisualElement.Q<Button>("_initBtn");
            m_GenerateButton = rootVisualElement.Q<Button>("_generateBtn");

            m_InitButton.clicked += GenerateButton;
            m_GenerateButton.clicked += HandleGenerateBtnClicked;
        }
        void OnDestroy() 
        {
            m_InitButton.clicked -= GenerateButton;  
            m_GenerateButton.clicked -= HandleGenerateBtnClicked;  
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
            var dir = new System.IO.DirectoryInfo(GetPath());
            int fileNum = dir.GetFiles().Length / 2;
            string textName = TextFileName + (fileNum + 1).ToString();
            string finalPath = GetPath() + textName + ".txt";

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
            File.WriteAllText(finalPath, resultString);
            AssetDatabase.Refresh();

            TextAsset textAsset = new TextAsset(resultString);
            AssetDatabase.SaveAssets();
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
