using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Relic))]
public class RelicCustomEditor : Editor
{

    Relic t;
    SerializedObject GetTarget;
    SerializedProperty ThisList;

    private void OnEnable() {
        t = (Relic)target;
        GetTarget = new SerializedObject(t);
        ThisList = GetTarget.FindProperty("modifiers");
    }


    public override void OnInspectorGUI() {
        GetTarget.Update();

        SerializedProperty MySprite = GetTarget.FindProperty("sprite");
        EditorGUILayout.PropertyField(MySprite);

        if(GUILayout.Button("Add New")) {
            if(t.modifiers == null) {
                t.modifiers = new List<Modifier>();
            }
            t.modifiers.Add(new Modifier());
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.MaxWidth(75));
        EditorGUILayout.LabelField("Element", GUILayout.MaxWidth(100));
        EditorGUILayout.LabelField("Buff Type", GUILayout.MaxWidth(100));
        EditorGUILayout.LabelField("Value", GUILayout.MaxWidth(50));
        EditorGUILayout.EndHorizontal();

        for(int i = 0; i < ThisList.arraySize; i++) {
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty MyElement = MyListRef.FindPropertyRelative("element");
            SerializedProperty MyBuff = MyListRef.FindPropertyRelative("buff");
            SerializedProperty MyValue = MyListRef.FindPropertyRelative("value");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Modifier " + i, GUILayout.MaxWidth(75));
            EditorGUILayout.PropertyField(MyElement, new GUIContent(), GUILayout.MaxWidth(100));
            EditorGUILayout.PropertyField(MyBuff, new GUIContent(), GUILayout.MaxWidth(100));
            EditorGUILayout.PropertyField(MyValue, new GUIContent(), GUILayout.MaxWidth(50));
            if(GUILayout.Button("-",GUILayout.Width(50))) {
                t.modifiers.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        GetTarget.ApplyModifiedProperties();
    }
}


