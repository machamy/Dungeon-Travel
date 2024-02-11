
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class EncaseEditor : EditorWindow
{
    private const string _helpText = "Cannot find 'EncaserList' component on any GameObject in the scene!";
    private string path = "Assets/Resources/Prefebs/Dungeon/";
    
    private static Vector2 _windowsMinSize = Vector2.one * 500f;
    private static Rect _helpRect = new Rect(0f, 0f, 400f, 100f);
    private static Rect _listRect = new Rect(new Vector2(0,30), _windowsMinSize);
 
    private bool _isActive;
 
    SerializedObject _objectSO = null;
    ReorderableList _listRE = null;
 
    EncaserList _pfList;
    
    [MenuItem("Tools/PivotEncase")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(EncaseEditor));
    }
    private void OnEnable()
    {
        
        _pfList = FindObjectOfType<EncaserList>();
 
        if (_pfList)
        {
            _objectSO = new SerializedObject(_pfList);
 
            //init list
            _listRE = new ReorderableList(_objectSO, _objectSO.FindProperty("objects"), true, 
                true, true, true);
 
            //handle drawing
            _listRE.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Game Objects");
            _listRE.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 2f;
                rect.height = EditorGUIUtility.singleLineHeight;
                GUIContent objectLabel = new GUIContent($"GameObject {index}");
                //the index will help numerate the serialized fields
                EditorGUI.PropertyField(rect, _listRE.serializedProperty.GetArrayElementAtIndex(index), objectLabel);
            };
        }
    }
    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private Vector3 axis = Vector3.zero;
    private Vector3 fixAxis = Vector3.zero;
    private bool listFold = false;
    private string directory = "PivotMid";
    private void OnGUI()
    {
        if (_objectSO == null)
        {
            EditorGUI.HelpBox(_helpRect, _helpText, MessageType.Warning);
            return;
        }

        listFold = EditorGUILayout.Foldout(listFold, "ss");
        if (listFold) {
            _objectSO.Update();
            _listRE.DoList(_listRect);
            _objectSO.ApplyModifiedProperties();
        }

        EditorGUILayout.Space(_listRE.GetHeight() + 30f);
        EditorGUILayout.LabelField($"Save를 누르면 {path}{directory}/에 저장됩니다.");
        EditorGUILayout.Space(10f);
        directory = EditorGUILayout.TextField(directory);
        EditorGUILayout.Space(10f);
        
        EditorGUILayout.LabelField("축, 크기에 비례한 값 (0 = 중앙)");
        axis = EditorGUILayout.Vector3Field("", axis);
        EditorGUILayout.LabelField("축, 고정 크기 (0 = 중앙)");
        fixAxis = EditorGUILayout.Vector3Field("", fixAxis);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30f);
 
        if (GUILayout.Button("Save"))
        {
            Save();
        }
 
        GUILayout.Space(30f);
        if (GUILayout.Button("Clear"))
        {
            Clear();
        }
        //GUILayout.Label(_isActive ? "Simulation Activated!" : "Simulation Deactivated!", EditorStyles.boldLabel);
 
        EditorGUILayout.EndHorizontal();
    }

    private void Save()
    {
        foreach (var pf in _pfList.GetList())
        {
            EncasePivot(pf);
        }
    }
    private void Clear(){
        _pfList.Clear();
    }

    private GameObject EncasePivot(GameObject prefab)
    {
        GameObject parent = new GameObject($"{directory}/{prefab.name}_MID.prefab");
        GameObject res = null;
        res = PrefabUtility.InstantiatePrefab(prefab, parent.transform) as GameObject;
        MeshRenderer MR = res.GetComponentInChildren<MeshRenderer>();
        Vector3 center = MR.bounds.center;
        res.transform.position -= center;
        Vector3 size = MR.bounds.size;
        size.Scale(axis);
        res.transform.position += size/2;
        res.transform.position += fixAxis;
        SavePrefab(parent);
        DestroyImmediate(parent);
        return res;
    }



    public void SavePrefab(GameObject prefab)
    {
        string fPath = path + $"/{prefab.name}_MID.prefab";
        fPath = AssetDatabase.GenerateUniqueAssetPath(fPath);
        PrefabUtility.SaveAsPrefabAsset(prefab, fPath);
    }
}