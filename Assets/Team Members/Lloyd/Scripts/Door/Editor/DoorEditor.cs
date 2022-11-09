using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lloyd
{
    [CustomEditor(typeof(Lloyd.DoorDoubleModel))]
    public class DoorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Interact: Open / Close"))
            {
                (target as Lloyd.DoorDoubleModel)?.Interact();
            }

            if (GUILayout.Button("Set on Fire") && Application.isPlaying)
            {
                (target as Lloyd.DoorDoubleModel)?.SetOnFire();
            }


        }
    }
}