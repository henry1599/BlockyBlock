using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Security.Cryptography;

namespace BlockyBlock.Tools
{
    public class AutoBuild : MonoBehaviour
    {
        [MenuItem("Auto Build/Windows 64 Cheat")]
        public static void AutoBuildWin64_Cheat()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            List<string> sceneArrays = new List<string>();
            int count = EditorBuildSettings.scenes.Length;
            for (int i = 0; i < count; i++)
            {
                string path = EditorBuildSettings.scenes[i].path;
                if (EditorBuildSettings.scenes[i].enabled)
                {
                    sceneArrays.Add(path);
                }
            }

            string outFolder = "./_build/win64/cheat";
            string productName = PlayerSettings.productName;

            CreateFolder(outFolder);

            buildPlayerOptions.scenes = sceneArrays.ToArray();
            buildPlayerOptions.locationPathName = outFolder + "/" + productName + ".exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;

            buildPlayerOptions.options = GetBuildOptions(BuildTargetGroup.Standalone, true);

            // var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            // int resultCode = (report.summary.result == BuildResult.Succeeded) ? 0 : 1;
            // EditorApplication.Exit(resultCode);
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
        [MenuItem("Auto Build/Windows 64 Release")]
        public static void AutoBuildWin64_Release()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            List<string> sceneArrays = new List<string>();
            int count = EditorBuildSettings.scenes.Length;
            for (int i = 0; i < count; i++)
            {
                string path = EditorBuildSettings.scenes[i].path;
                if (EditorBuildSettings.scenes[i].enabled)
                {
                    sceneArrays.Add(path);
                }
            }

            string outFolder = "./_build/win64/release";
            string productName = PlayerSettings.productName;

            CreateFolder(outFolder);

            buildPlayerOptions.scenes = sceneArrays.ToArray();
            buildPlayerOptions.locationPathName = outFolder + "/" + productName + ".exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;

            buildPlayerOptions.options = GetBuildOptions(BuildTargetGroup.Standalone, false);

            // var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            // int resultCode = (report.summary.result == BuildResult.Succeeded) ? 0 : 1;
            // EditorApplication.Exit(resultCode);
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
        static void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }
        static void DeleteFileOrFolder(string path)
        {
            string tmp = Application.dataPath + "/../" + path;
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(tmp);
                if (dirInfo.Exists)
                    dirInfo.Delete(true);

                FileInfo fileInfo = new FileInfo(tmp);
                if (fileInfo.Exists)
                    fileInfo.Delete();
            }
            catch (Exception)
            {
            }
        }
        static BuildOptions GetBuildOptions(BuildTargetGroup target, bool isCheat, List<string> customScriptingDefine = null)
        {
            if (isCheat)
                AddScriptingDefine(target, "ENABLE_CHEAT");
            else
                RemoveScriptingDefine(target, "ENABLE_CHEAT");
            if (customScriptingDefine != null)
            {
                foreach (var key in customScriptingDefine)
                {
                    AddScriptingDefine(target, key);
                }
            }

            BuildOptions options = BuildOptions.None;
            options |= BuildOptions.ShowBuiltPlayer;
            return options;
        }
        static void AddScriptingDefine(BuildTargetGroup buildTargetGroup, string define)
        {
            string current = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (current.Contains(define))
                return;
            string result = current + ";" + define;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, result);
        }
        static void RemoveScriptingDefine(BuildTargetGroup buildTargetGroup, string define)
        {
            string current = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (!current.Contains(define))
                return;
            string result = current.Replace(define, String.Empty);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, result);
        }
    }
}
