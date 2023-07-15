/*
    >>
    >>
    >>
*/

using UnityEngine;
using UnityEditor;

public class PrefabWindow : EditorWindow
{
    private static string prefabFolderPath = "Assets/Prefabs";
    private static string[] prefabGuids;
    private static Object[] prefabs;

    private Vector2 scrollPosition;
private float iconSize = 80f;
GameObject _parent_Object;

    [MenuItem("Window/Prefabs Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<PrefabWindow>();
        window.titleContent = new GUIContent("Prefabs Window");
        LoadPrefabs();
    }

    private static void LoadPrefabs()
    {
        prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolderPath });
        if(!(prefabGuids.Length>=0))
        {
            Debug.Log("No prefabs found at location Assets/Prefabs");
        }
        prefabs = new Object[prefabGuids.Length];
        for (int i = 0; i < prefabGuids.Length; i++)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuids[i]);
            prefabs[i] = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(Object));
        }
    }

private void OnGUI()
{
    EditorGUILayout.BeginVertical();    
    GUILayout.Label("Parent Object in Hierarchy", EditorStyles.boldLabel);
    _parent_Object=EditorGUILayout.ObjectField("Parent GameObject",_parent_Object, typeof(GameObject), true) as GameObject;
    EditorGUILayout.EndVertical();


    // Icon Size Slider
    GUILayout.BeginHorizontal();
    GUILayout.Label("Icon Size", GUILayout.Width(140));
    EditorGUIUtility.labelWidth = 30;
    iconSize = GUILayout.HorizontalSlider(iconSize, 80f, 120f);
    EditorGUIUtility.labelWidth = 0;
    GUILayout.EndHorizontal();



    EditorGUILayout.Space();

    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

    if (prefabs == null || prefabs.Length == 0)
    {
        EditorGUILayout.HelpBox("No prefabs found.", MessageType.Info);
    }
    else
    {
        int maxIconsPerRow = Mathf.FloorToInt(position.width / iconSize);

        int rowCount = Mathf.CeilToInt((float)prefabs.Length / maxIconsPerRow);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int row = 0; row < rowCount; row++)
        {
            EditorGUILayout.BeginHorizontal();

            int startIndex = row * maxIconsPerRow;
            int endIndex = Mathf.Min(startIndex + maxIconsPerRow, prefabs.Length);

            for (int i = startIndex; i < endIndex; i++)
{
    GUILayout.BeginVertical(GUILayout.Width(iconSize));
    GUILayout.Space(iconSize - 60); // Adjust vertical spacing for the icon

    GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
    buttonStyle.imagePosition = ImagePosition.ImageAbove;

    GUIContent prefabContent = new GUIContent(prefabs[i].name, EditorGUIUtility.IconContent("Prefab Icon").image);
                
    Texture2D prefabIcon = AssetPreview.GetAssetPreview(prefabs[i]);
    if (prefabIcon != null)
    {
        float scaleFactor = Mathf.Min(iconSize / prefabIcon.width, iconSize / prefabIcon.height, 1f);
        if (GUILayout.Button(prefabIcon, buttonStyle, GUILayout.Width(prefabIcon.width * scaleFactor), GUILayout.Height(prefabIcon.height * scaleFactor)))
        {
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabs[i]);
            if (newObject != null)
            {
                if(_parent_Object!=null)
                {
                    newObject.transform.parent=_parent_Object.transform;
                }
                newObject.transform.localPosition  = Vector3.zero;
                newObject.transform.localRotation  = Quaternion.identity;
                Selection.activeObject = newObject;
            }
        }
    }
    else
    {
        if (GUILayout.Button(prefabContent, buttonStyle, GUILayout.Width(iconSize), GUILayout.Height(iconSize)))
        {
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabs[i]);
            if (newObject != null)
            {
                if(_parent_Object!=null)
                {
                    newObject.transform.parent=_parent_Object.transform;
                }
                newObject.transform.localPosition  = Vector3.zero;
                newObject.transform.localRotation  = Quaternion.identity;
                Selection.activeObject = newObject;
            }
        }
    }

    GUILayout.Label(prefabs[i].name, GUI.skin.label, GUILayout.Width(iconSize), GUILayout.Height(20)); // Display the prefab name below the icon

    GUILayout.EndVertical();
}


            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    EditorGUILayout.EndVertical();

    EditorGUILayout.Space();

    EditorGUILayout.BeginVertical();    
    EditorGUILayout.BeginHorizontal();
    GUILayout.FlexibleSpace();
    if(GUILayout.Button("Reload Window", GUILayout.Width(150)))
    {
        // Close the current window
    Close();

    // Create a new instance of the window
    var window = EditorWindow.GetWindow<PrefabWindow>();
    window.Show();
    }
    GUILayout.FlexibleSpace();
    EditorGUILayout.EndHorizontal();
    EditorGUILayout.EndVertical();
    
    EditorGUILayout.Space();
    EditorGUILayout.Space();
}

private void OnEnable()
    {
        this.minSize = new Vector2(320f, 350f);
        this.maxSize = new Vector2(1000f, 1000f);
    }


}