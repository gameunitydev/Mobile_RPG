﻿// DFVolume - Distance field volume generator for Unity
// https://github.com/keijiro/DFVolume

using UnityEditor;

namespace DFVolume
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VolumeVisualizer))]
    internal class VolumeVisualizerEditor : Editor
    {
        private SerializedProperty _data;
        private SerializedProperty _depth;
        private SerializedProperty _mode;

        private void OnEnable()
        {
            _data = serializedObject.FindProperty("_data");
            _mode = serializedObject.FindProperty("_mode");
            _depth = serializedObject.FindProperty("_depth");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_data);
            EditorGUILayout.PropertyField(_mode);
            EditorGUILayout.PropertyField(_depth);

            serializedObject.ApplyModifiedProperties();
        }
    }
}