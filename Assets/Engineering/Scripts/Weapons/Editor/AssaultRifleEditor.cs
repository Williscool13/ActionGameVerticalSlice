using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AssaultRifle))]
public class AssaultRifleEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        AssaultRifle shootingTarget = (AssaultRifle)target;

        if (GUILayout.Button("Shoot")) {
            shootingTarget.Fire();
        }
        if (GUILayout.Button("Reload")) {
            shootingTarget.ReloadEnd();
        }
    }
}
