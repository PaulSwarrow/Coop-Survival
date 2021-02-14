using System.Collections;
using System.Collections.Generic;
using Game.View;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AimSolver))]
public class AimSolverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save offset"))
        {
            var _target = (AimSolver) target;
            _target.SaveOffset();
        }
    }
}
