using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
public class FindMissingScriptsRecursively: EditorWindow
{
    string sourcePath = "";
    DefaultAsset source = null;

    bool pathEnabled = false;
    bool selectedEnabled = true;

    [MenuItem("Tools/Remove Missing Scripts")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:

        FindMissingScriptsRecursively window = (FindMissingScriptsRecursively)EditorWindow.GetWindow(typeof(FindMissingScriptsRecursively));
        window.minSize = new Vector2(400, 175);
        window.Show();
    }

    void OnGUI()
    {
        if (pathEnabled)
        {
            selectedEnabled = EditorGUILayout.BeginToggleGroup("Remove from selected Prefabs/Game Objects", false);
            EditorGUILayout.EndToggleGroup();
        }
        else
        {
            selectedEnabled = EditorGUILayout.BeginToggleGroup("Remove from selected Prefabs/Game Objects", selectedEnabled);
            EditorGUILayout.EndToggleGroup();
        }
        

        GUILayout.Label("");
        if (selectedEnabled)
        {
            pathEnabled = EditorGUILayout.BeginToggleGroup("Remove from path:", false);
        }
        else
        {
            pathEnabled = EditorGUILayout.BeginToggleGroup("Remove from path:", pathEnabled);
        }
        sourcePath = EditorGUILayout.TextField("Remove from", sourcePath);
        EditorGUILayout.BeginHorizontal(new GUIStyle(GUI.skin.box));
        if (GUILayout.Button("Browse"))
        {
            sourcePath = EditorUtility.OpenFolderPanel("Export from", "", "");
            source = null;
        }
        GUILayout.Label("or drag folder here");
        source = (DefaultAsset)EditorGUILayout.ObjectField(source, typeof(DefaultAsset), false);
        if (source != null)
        {
            sourcePath = Application.dataPath.Replace("Assets", string.Empty) + AssetDatabase.GetAssetPath(source);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndToggleGroup();

        GUILayout.Label("");
        if (GUILayout.Button("Execute"))
        {
            FindAndRemoveMissingInSelected();
        }
    }

    private List<Object> SelectFilesFromPath()
    {
        string relativeSourcePath = "";
        string[] files = Directory.GetFiles(sourcePath);
        List<Object> gameObjectList = new List<Object>();
        //GameObject parentObject = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Editor/PanelExport/PanelParent.prefab", typeof(GameObject))) as GameObject;
        foreach (string file in files)
        {
            if (file.EndsWith(".prefab"))
            {

                if (sourcePath.StartsWith(Application.dataPath))
                {
                    relativeSourcePath = "Assets" + sourcePath.Substring(Application.dataPath.Length);
                }
                // Trim the filepath
                string filename = file.Replace(sourcePath + @"\", string.Empty);
                gameObjectList.Add(AssetDatabase.LoadAssetAtPath(relativeSourcePath + "/" + filename, typeof(GameObject)));
                Debug.Log(filename);
            }
        }
        return gameObjectList;
        //return Selection.gameObjects.SelectMany(gameObjectList => gameObjectList.GetComponentsInChildren<Transform>(true)).Select(t => t.gameObject);
    }

    private void FindAndRemoveMissingInSelected()
    {
        // Added function to select files from path
        if (pathEnabled)
        {
            Selection.objects = SelectFilesFromPath().ToArray();
        }

        // EditorUtility.CollectDeepHierarchy does not include inactive children
        var deeperSelection = Selection.gameObjects.SelectMany(go => go.GetComponentsInChildren<Transform>(true))
                   .Select(t => t.gameObject);
        var prefabs = new HashSet<Object>();
        int compCount = 0;
        int goCount = 0;
        foreach (var go in deeperSelection)
        {
            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
            if (count > 0)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(go))
                {
                    RecursivePrefabSource(go, prefabs, ref compCount, ref goCount);
                    count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
                    // if count == 0 the missing scripts has been removed from prefabs
                    if (count == 0)
                        continue;
                    // if not the missing scripts must be prefab overrides on this instance
                }

                Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                compCount += count;
                goCount++;
            }
        }

        Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");
    }

    // Prefabs can both be nested or variants, so best way to clean all is to go through them all
    // rather than jumping straight to the original prefab source.
    private static void RecursivePrefabSource(GameObject instance, HashSet<Object> prefabs, ref int compCount,
        ref int goCount)
    {
        var source = PrefabUtility.GetCorrespondingObjectFromSource(instance);
        // Only visit if source is valid, and hasn't been visited before
        if (source == null || !prefabs.Add(source))
            return;

        // go deep before removing, to differantiate local overrides from missing in source
        RecursivePrefabSource(source, prefabs, ref compCount, ref goCount);

        int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(source);
        if (count > 0)
        {
            Undo.RegisterCompleteObjectUndo(source, "Remove missing scripts");
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(source);
            compCount += count;
            goCount++;
        }
    }
}
#endif