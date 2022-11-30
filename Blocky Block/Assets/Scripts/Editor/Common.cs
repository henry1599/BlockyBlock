#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace BlockyBlock.Utils.EditorTools
{
    [ExecuteInEditMode]
    public static partial class Common
    {
        public static void SaveCurrentScenesToPrefs()
        {
            int sceneCount = EditorSceneManager.sceneCount;
            EditorPrefs.SetInt("SceneCount", sceneCount);
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = EditorSceneManager.GetSceneAt(i);
                OpenSceneMode sceneMode;
                if (scene.isLoaded)
                    sceneMode = OpenSceneMode.Additive;
                else
                    sceneMode = OpenSceneMode.AdditiveWithoutLoading;
                if (EditorSceneManager.GetActiveScene() == scene)
                    sceneMode = OpenSceneMode.Single;
                EditorPrefs.SetString($"Scene{i}Path", scene.path);
                EditorPrefs.SetInt($"Scene{i}Mode", (int)sceneMode);
            }
        }
   
        public static void RemovePrefs()
        {
            int sceneCount = EditorPrefs.GetInt("SceneCount");
            for (int i = 0; i < sceneCount; i++)
            {
                EditorPrefs.DeleteKey($"Scene{i}Path");
                EditorPrefs.DeleteKey($"Scene{i}Mode");
            }
        }

        public static void DrawUILine(int thickness = 1, int padding = 10)
        {
            DrawUILine(Color.gray, thickness, padding);
        }

        public static void DrawUILine(Color color, int thickness = 1, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
            r.height = thickness;
            r.y += padding/2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        public static bool IsAsciiLetter(this char c)
        {
            return (c >= 97 && c <= 122) || (c >= 65 && c <= 90);
        }

        public static bool IsAsciiNumber(this char c)
        {
            return c >= 48 && c <= 57;
        }

        public static bool IsAsciiLetterOrNumber(this char c)
        {
            return c.IsAsciiLetter() || c.IsAsciiNumber();
        }

        public static string ToAbsolutePath(this string path)
        {
            // this check works on Windows only
            if (path.Contains(":")) return path;
            return Application.dataPath.Replace("Assets", path);
        }

        public static string ToRelativePath(this string path)
        {
            if (!path.Contains("Assets"))
            {
                return path;
            }
            var paths = path.Split("Assets").Skip(1).ToList();
            if (paths.Count == 1)
            {
                return "Assets" + paths[0];
            }
            else
            {
                return string.Join("Assets", paths);
            }
        }

        public static string GetFileName(this string path, bool withExtension = true)
        {
            if (!withExtension)
            {
                path = path.Split(".")[0];
            }
            if (path.Contains(Path.AltDirectorySeparatorChar))
            {
                return path.Split(Path.AltDirectorySeparatorChar)[^1];
            }
            return path.Split(Path.DirectorySeparatorChar)[^1];
        }

        public static DirectoryInfo CombineInfo(this DirectoryInfo current, string path)
        {
            return new DirectoryInfo(Path.Combine(current.FullName, path));
        }

        public static DirectoryInfo CombineInfo(this DirectoryInfo current, params string[] path)
        {
            return new DirectoryInfo(Path.Combine(current.FullName,  Path.Combine(path)));
        }

        public static FileInfo CombineInfoToFile(this DirectoryInfo current, string path)
        {
            return new FileInfo(Path.Combine(current.FullName, path));
        }

        public static string CombineToPath(this DirectoryInfo current, string path)
        {
            return Path.Combine(current.FullName, path);
        }

        public static void ClearContents(this DirectoryInfo current)
        {
            FileInfo[] fileNames = current.GetFiles();

            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i].Delete();
            }

            Debug.Log($"Cleared {current.FullName}");
        }

        public static string CopyUpdateFile(string sourceFilePath, string destinationFilePath)
        {
            FileInfo srcFileInfo = new FileInfo(sourceFilePath);
            FileInfo destFileInfo = new FileInfo(destinationFilePath);
            bool isDifferentFile = System.DateTime.Compare(srcFileInfo.LastWriteTimeUtc, destFileInfo.LastWriteTimeUtc) != 0;

            if (isDifferentFile)
            {
                File.Copy(sourceFilePath, destinationFilePath, true);
            }

            AssetDatabase.Refresh();

            return destFileInfo.FullName;
        }

        public static List<string> ExtractToyIds(string text)
        {
            Dictionary<string, string> toys = new Dictionary<string, string>();
            StringBuilder toySb = new StringBuilder(); ;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].IsAsciiLetterOrNumber())
                {
                    toySb.Append(text[i]);
                }
                else
                {
                    string toyId = toySb.ToString();
                    if (!toys.ContainsKey(toyId))
                    {
                        toys.Add(toyId, "");
                    }
                    toySb.Clear();
                }
            }
            if (toySb.Length > 0)
            {
                string toyId = toySb.ToString();
                if (!toys.ContainsKey(toyId))
                {
                    toys.Add(toyId, "");
                }
            }
            return toys.Keys.ToList();
        }

        [MenuItem("Assets/Tools/Copy GUID ^&g")]
        static void CopyGUID()
        {
            if (Selection.activeObject != null)
            {
                string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(Selection.activeObject));
                Debug.Log($"GUID {guid} copied");
                GUIUtility.systemCopyBuffer = guid;
            }
        }

        [MenuItem("Assets/Tools/Copy Resources Path ^&r")]
        static void CopyResourcesPath()
        {
            string resourcesPath = AssetDatabase.GetAssetPath(Selection.activeObject).Split("Resources/")[^1].Split(".")[0];
            Debug.Log($"Resources path {resourcesPath} copied");
            GUIUtility.systemCopyBuffer = resourcesPath;
        }

        [MenuItem("Assets/Tools/Copy Resources Path", true)]
        static bool CopyResourcesPath_Validation()
        {
            if (Selection.activeObject != null)
            {
                return AssetDatabase.GetAssetPath(Selection.activeObject).Contains("Resources");
            }
            return false;
        }

        [MenuItem("Assets/Tools/Create Original Prefab from FBX")]
        static void CreatePrefabsFromFBX()
        {
            List<(GameObject, string, string)> prefabs = new List<(GameObject, string, string)>();
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                prefabs.Add(CreatePrefabFromFBX(Selection.gameObjects[i]));
            }
            AssetDatabase.Refresh();
            List<Object> selected = new List<Object>();
            for (int i = 0; i < prefabs.Count; i++)
            {
                selected.Add(AttachModel(prefabs[i].Item1, prefabs[i].Item2, prefabs[i].Item3));
            }
            AssetDatabase.Refresh();
            Selection.objects = selected.ToArray();
        }

        static (GameObject, string, string) CreatePrefabFromFBX(Object selection)
        {
            string modelPath = AssetDatabase.GetAssetPath(selection);
            string prefabPath = modelPath.Replace(".fbx", ".prefab");

            var newObj = new GameObject();
            PrefabUtility.SaveAsPrefabAsset(newObj, prefabPath);

            return (newObj, modelPath, prefabPath);
        }

        static Object AttachModel(GameObject newObj, string modelPath, string prefabPath)
        {
            GameObject toyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
            GameObject rootToyPrefab = PrefabUtility.LoadPrefabContents(prefabPath);
            GameObject nestedToyPrefab = PrefabUtility.InstantiatePrefab(toyPrefab) as GameObject;

            nestedToyPrefab.transform.parent = rootToyPrefab.transform;
            nestedToyPrefab.transform.localScale = Vector3.one;
            newObj.transform.localRotation = nestedToyPrefab.transform.localRotation;
            nestedToyPrefab.transform.localRotation = Quaternion.Euler(0, 0, 0);

            PrefabUtility.SaveAsPrefabAsset(rootToyPrefab, prefabPath);
            GameObject.DestroyImmediate(newObj);
            return rootToyPrefab;
        }

        [MenuItem("Assets/Tools/Create Original Prefab from FBX", true)]
        static bool CreatePrefabFromFbx_Validation()
        {
            if (Selection.activeObject != null)
            {
                bool show = true;
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    show &= AssetDatabase.GetAssetPath(Selection.gameObjects[i]).Contains(".fbx");
                }
                return show;
            }
            return false;
        }

        public static DirectoryInfo GetToyModelDirectoryInfo(string toyFolderPath, string toyId, bool createIfNotExist = false)
        {
            DirectoryInfo toyFolderInfo = new DirectoryInfo(toyFolderPath).CombineInfo(toyId);
            if (toyFolderInfo.Exists)
            {
                return toyFolderInfo;
            }
            foreach (var toyFolder in new DirectoryInfo(toyFolderPath).EnumerateDirectories())
            {
                if (toyFolder.Name.Contains(toyId + "_")) return toyFolder;
            }
            if (!createIfNotExist)
            {
                Debug.Log($"Toy Model folder not found: {toyId}");
                return null;
            }
            else
            {
                Debug.Log($"Toy Model folder not found: {toyId}. Will create new one");
                toyFolderInfo.Create();
                return toyFolderInfo;
            }
        }

        public static FileInfo[] GetAssetFilesContains(this DirectoryInfo directoryInfo, string searchStr, bool caseSensitive = false)
        {
            List<FileInfo> result = new List<FileInfo>();
            if (!caseSensitive)
            {
                searchStr = searchStr.ToLower();
            }
            foreach (var fileInfo in directoryInfo.EnumerateFiles())
            {
                if (fileInfo.Name.EndsWith(".meta")) continue;
                if (caseSensitive)
                {
                    if (fileInfo.Name.Contains(searchStr))
                    {
                        result.Add(fileInfo);
                    }
                }
                else
                {
                    if (fileInfo.Name.ToLower().Contains(searchStr))
                    {
                        result.Add(fileInfo);
                    }
                }
            }
            return result.ToArray();
        }

        public static FileInfo GetAssetFileContains(this DirectoryInfo directoryInfo, string searchStr, bool caseSensitive = false)
        {
            FileInfo[] result = GetAssetFilesContains(directoryInfo, searchStr, caseSensitive: caseSensitive);
            if (result.Length > 0)
            {
                return result[0];
            }
            Debug.LogWarning($"Search for {searchStr}: Found 0 result in {directoryInfo}");
            return null;
        }

        public static void Log(string logText, ref StringBuilder sb, bool appendNewLine = true)
        {
            if (appendNewLine)
            {
                if (!logText.EndsWith("\n"))
                {
                    logText += "\n";
                }
            }
            Debug.Log(logText);
            sb.Append(logText);
        }

        public static void LogWarning(string logText, ref StringBuilder sb, bool appendNewLine = true, bool richText = true)
        {
            if (appendNewLine)
            {
                if (!logText.EndsWith("\n"))
                {
                    logText += "\n";
                }
            }
            Debug.LogWarning(logText);
            if (richText)
            {
                sb.Append($"<color=yellow>{logText}</color>");
            }
            else
            {
                sb.Append(logText);
            }
        }

        public static void LogError(string logText, ref StringBuilder sb, bool appendNewLine = true, bool richText = true)
        {
            if (appendNewLine)
            {
                if (!logText.EndsWith("\n"))
                {
                    logText += "\n";
                }
            }
            Debug.LogError(logText);
            if (richText)
            {
                sb.Append($"<color=red>{logText}</color>");
            }
            else
            {
                sb.Append(logText);
            }
        }

        public static string SetImportTextureType(string assetPath, TextureImporterType type = TextureImporterType.Sprite)
        {
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            importer.textureType = type;
            AssetDatabase.WriteImportSettingsIfDirty(assetPath);
            AssetDatabase.Refresh();
            return assetPath;
        }

        public static string[] ExtractFbxMaterials(string assetPath, string destFolder, string prefix)
        {
            List<string> extractedMaterialPaths = new List<string>();
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            HashSet<string> hashSet = new HashSet<string>();
            foreach (var obj in assets)
            {
                if (obj.GetType() == typeof(Material))
                {
                    if (obj.name.Equals("No Name")) continue;
                    string destName = obj.name.Split('_').Last();
                    string path = Path.Combine(destFolder, $"{prefix}_{destName}.mat");

                    string newPath = AssetDatabase.GenerateUniqueAssetPath(path);
                    if (newPath != path)
                    {
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.Refresh();
                    }
                    string value = AssetDatabase.ExtractAsset(obj, path);
                    if (string.IsNullOrEmpty(value))
                    {
                        hashSet.Add(assetPath);
                        extractedMaterialPaths.Add(path);
                    }
                }
            }
            foreach (string item2 in hashSet)
            {
                AssetDatabase.WriteImportSettingsIfDirty(item2);
                AssetDatabase.ImportAsset(item2, ImportAssetOptions.ForceUpdate);
            }
            AssetDatabase.Refresh();

            return extractedMaterialPaths.ToArray();
        }

        public static T CreateScriptableObjectIfNotExist<T>(string assetPath) where T : ScriptableObject
        {
            assetPath = assetPath.ToRelativePath();
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, assetPath);
                AssetDatabase.Refresh();
            }
            return asset;
        }
    }
}
#endif