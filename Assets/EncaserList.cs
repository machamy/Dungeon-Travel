using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;


public class EncaserList : MonoBehaviour
{
   
    public GameObject[] objects = new GameObject[] {};

    //public getter method
    public GameObject[] GetList()
    {
        return objects;
    }
}

public class EncaseEditor : EditorWindow
{
    private const string _helpText = "Cannot find 'EncaserList' component on any GameObject in the scene!";
    private string path = "Assets/Resources/Prefebs/Dungeon/PivotMid";
    private static Vector2 _windowsMinSize = Vector2.one * 500f;
    private static Rect _helpRect = new Rect(0f, 0f, 400f, 100f);
    private static Rect _listRect = new Rect(Vector2.zero, _windowsMinSize);
 
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
    private void OnGUI()
    {
        if (_objectSO == null)
        {
            EditorGUI.HelpBox(_helpRect, _helpText, MessageType.Warning);
            return;
        }
  
        _objectSO.Update();
        _listRE.DoList(_listRect);
        _objectSO.ApplyModifiedProperties();
        
 
        GUILayout.Space(_listRE.GetHeight() + 30f);
        GUILayout.Label("Please select Game Objects to simulate");
        GUILayout.Space(10f);
 
        EditorGUILayout.BeginHorizontal();
        
 
        GUILayout.Space(30f);
 
        if (GUILayout.Button("Save"))
        {
            Save();
        }
 
        GUILayout.Space(30f);

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

    private GameObject EncasePivot(GameObject prefab)
    {
        GameObject parent = new GameObject($"{prefab.name}_MID.prefab");
        GameObject res = null;
        res = PrefabUtility.InstantiatePrefab(prefab, parent.transform) as GameObject;
        MeshRenderer MR = res.GetComponent<MeshRenderer>();
        Vector3 center = MR.bounds.center;
        res.transform.position -= center;
        
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