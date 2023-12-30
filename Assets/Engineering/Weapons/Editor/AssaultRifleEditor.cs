using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gun))]
public class AssaultRifleEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Gun shootingTarget = (Gun)target;

        if (GUILayout.Button("Shoot")) {
            shootingTarget.Fire();
        }
        if (GUILayout.Button("Reload")) {
            shootingTarget.ReloadEnd();
        }
    }
}
