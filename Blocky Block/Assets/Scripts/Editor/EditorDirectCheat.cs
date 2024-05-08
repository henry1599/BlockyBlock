using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BlockyBlock.Utils.EditorTools
{
    public class EditorDirectCheat : EditorWindow
    {
        [SerializeField] VisualTreeAsset m_Tree;
        private static EditorDirectCheat thisWindow;
        private Toggle toggleEnableCheat;
        private Toggle toggleEnableLogin;
        private Button btnResetSave;
        private readonly string ENABLE_CHEAT_SDS = "ENABLE_CHEAT";
        private readonly string DISABLE_LOGIN_SDS = "DISABLE_LOGIN";
        
        [MenuItem("Tools/Editor Direct Cheat")]
        public static void ShowEditor()
        {
            var window = GetWindow<EditorDirectCheat>();
            window.titleContent = new GUIContent("Editor Direct Cheat");

            thisWindow = window;
        }
        void CreateGUI()
        {
            m_Tree.CloneTree(rootVisualElement);

            var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';').ToList();
            bool isEnableCheatOn = defineSymbols.Contains(ENABLE_CHEAT_SDS);
            bool isEnableLoginOn = !defineSymbols.Contains(DISABLE_LOGIN_SDS);

            this.toggleEnableCheat = rootVisualElement.Q<Toggle>("_toggleEnableCheat");
            this.toggleEnableLogin = rootVisualElement.Q<Toggle>("_toggleEnableLogin");
            this.btnResetSave = rootVisualElement.Q<Button>("_btnResetSave");
            this.btnResetSave.clicked += HandleResetSaveClicked;

            this.toggleEnableCheat.value = isEnableCheatOn;
            this.toggleEnableLogin.value = isEnableLoginOn;

            this.toggleEnableCheat.RegisterValueChangedCallback(OnToggleEnableCheat);
            this.toggleEnableLogin.RegisterValueChangedCallback(OnToggleEnableLogin);
        }

        private void HandleResetSaveClicked()
        {
            PlayerPrefs.DeleteAll();
        }

        void OnDestroy() 
        {
            this.btnResetSave.clicked -= HandleResetSaveClicked;
        }

        void OnToggleEnableCheat(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
                Common.AddScriptingDefine(BuildTargetGroup.Standalone, ENABLE_CHEAT_SDS);
            else
                Common.RemoveScriptingDefine(BuildTargetGroup.Standalone, ENABLE_CHEAT_SDS);
        }
        void OnToggleEnableLogin(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
                Common.RemoveScriptingDefine(BuildTargetGroup.Standalone, DISABLE_LOGIN_SDS);
            else
                Common.AddScriptingDefine(BuildTargetGroup.Standalone, DISABLE_LOGIN_SDS);
        }
        
    }
}
