using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CustomEditor(typeof(DamageDataEvent))]
    public class DamageEventDataEventEditor : Editor
    {
        DamageData variable = new();
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            DamageDataEvent e = target as DamageDataEvent;
            variable.DamageName = EditorGUILayout.TextField("Damage Name", variable.DamageName);
            variable.RawDamage = EditorGUILayout.FloatField("Raw Damage", variable.RawDamage);
            variable.DamageMultiplier = EditorGUILayout.IntField("Damage Multiplier", variable.DamageMultiplier);
            variable.DamageSender = EditorGUILayout.ObjectField("Damage Sender", variable.DamageSender, typeof(GameObject), true) as GameObject;
            if (GUILayout.Button("Raise")) {
                if (variable.DamageSender == null || EditorUtility.IsPersistent(variable.DamageSender)) {
                    Debug.LogError("Object not valid (null or not in scene).");
                    return;
                }
                e.Raise(variable);

            }
        }
    }
}