#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine;
using UnityEngine.UIElements;

namespace BlockyBlock.Editor 
{
    public class MapGenerator : EditorWindow
    {
        [SerializeField] VisualTreeAsset m_Tree;
        private SliderInt m_WidthSlider;
        private SliderInt m_HeightSlider;
        private Button m_InitButton;
        private Button m_GenerateButton;
        private IMGUIContainer m_ButtonContainer;
        float m_ButtonWidth = 50;
        float m_ButtonHeight = 20;


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
        }
        void GenerateButton()
        {
            m_ButtonContainer.Clear();
            int numberOfButtons = (int)m_WidthSlider.value * (int)m_HeightSlider.value;
            m_ButtonContainer.style.width = m_ButtonWidth * m_WidthSlider.value;
            m_ButtonContainer.style.height = m_ButtonHeight * m_HeightSlider.value;
            for (int i = 0; i < numberOfButtons; i++)
            {
                m_ButtonContainer.Add(CreateButton());
            }
        }
        Button CreateButton()
        {
            Button btn = new Button();
            btn.style.width = 50;
            btn.style.height = 20;
            btn.text = "Ground";
            btn.style.marginLeft = 0;
            btn.style.marginRight = 0;
            btn.style.marginBottom = 0;
            btn.style.marginTop = 0;
            btn.style.paddingLeft = 0;
            btn.style.paddingRight = 0;
            btn.style.paddingBottom = 0;
            btn.style.paddingTop = 0;

            return btn;
        }
    }
}
