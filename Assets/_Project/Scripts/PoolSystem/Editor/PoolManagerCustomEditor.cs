using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PoolManager))]
public class PoolManagerCustomEditor : Editor
{
    int ListSize;
    PoolManager _target;
    //Dictionary<PoolStruct, bool> DetailShowed = new Dictionary<PoolStruct, bool>();

    public override void OnInspectorGUI()
    {
        _target = (PoolManager)target;

        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((PoolManager)target), typeof(PoolManager), false);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        /// Creation of the list Details
        for (int i = 0; i < _target.ObjectsToPool.Count; i++)
        {
            PoolStruct _currentItem = _target.ObjectsToPool[i];

            //////////////////////////////////////////////
            // Start of the object, with ID and remove button

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();


            //DetailShowed.TryGetValue(_currentItem, out itemShowDetails);
            GUILayout.Label("", GUILayout.Width(20f));
            _currentItem.inspectorExplandeToggle = EditorGUILayout.Foldout(_currentItem.inspectorExplandeToggle, _currentItem.ID, _currentItem.inspectorExplandeToggle);

            //DetailShowed[_currentItem] = itemShowDetails;

            if (GUILayout.Button("Remove", GUILayout.Width(100f)))
            {
                _target.ObjectsToPool.Remove(_currentItem);
                break;
            }
            GUILayout.Label("", GUILayout.Width(20f));

            EditorGUILayout.EndHorizontal();
            

            if (_currentItem.inspectorExplandeToggle)
            {
                EditorGUILayout.Space();
                /////////////////////////////////////////////////
                // Draw the details for each item of the list 
                DrawIDField(_currentItem);
                DrawPrefabField(_currentItem);
                DrawQuantityField(_currentItem);
                DrawObjectRetrieveStateField(_currentItem);

                EditorGUILayout.Space();
                
            }
            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
            // Space to the next item of the list
            EditorGUILayout.Space();
        }

        ///////////////////////////////////

        if(GUILayout.Button("Add"))
        {
            PoolStruct newPoolStruct = new PoolStruct();
            newPoolStruct.inspectorExplandeToggle = true;
            _target.ObjectsToPool.Add(newPoolStruct);
        }

        EditorUtility.SetDirty(_target);
    }

    private void DrawIDField(PoolStruct _currentItem)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID");

        _currentItem.ID = EditorGUILayout.TextField(_currentItem.ID);

        EditorGUILayout.EndHorizontal();
    }

    private void DrawPrefabField(PoolStruct _currentItem)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Prefab");

        _currentItem.Prefab = (PoolObjectBase)EditorGUILayout.ObjectField(_currentItem.Prefab, typeof(PoolObjectBase), false);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawQuantityField(PoolStruct _currentItem)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Quantity");

        _currentItem.Quantity = EditorGUILayout.IntField(_currentItem.Quantity);

        EditorGUILayout.EndHorizontal();
    }
    private void DrawObjectRetrieveStateField(PoolStruct _currentItem)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Status on retrieve");

        _currentItem.ObjectStateOnRetrieve = EditorGUILayout.Toggle(_currentItem.ObjectStateOnRetrieve);

        EditorGUILayout.EndHorizontal();
    }

}
