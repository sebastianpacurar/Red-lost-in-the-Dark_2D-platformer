using UnityEditor;
using UnityEngine;

namespace Utils {
    [CustomPropertyDrawer(typeof(ReadOnlyPropAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }
}