using Player;
using UnityEditor;

namespace Editor.Player {
    [CustomEditor(typeof(HandleSanity))]
    public class HandleSanityEditor : UnityEditor.Editor {
        #region Serialized properties
        private SerializedProperty _playerLight;
        private SerializedProperty _timeMultiplier;

        private SerializedProperty _maxInnerRadius;
        private SerializedProperty _minInnerRadius;
        private SerializedProperty _innerRadiusIncreaseVal;
        private SerializedProperty _innerRadiusDecreaseVal;

        private SerializedProperty _maxOuterRadius;
        private SerializedProperty _minOuterRadius;
        private SerializedProperty _outerRadiusIncreaseVal;
        private SerializedProperty _outerRadiusDecreaseVal;

        private SerializedProperty _isIntensityIncreasing;
        #endregion

        # region Foldout Header Group booleans
        private bool _isInnerRadiusOn = true;
        private bool _isOuterRadiusOn = true;
        private bool _isDebuggingOn;
        #endregion

        private void OnEnable() {
            _playerLight = serializedObject.FindProperty("playerLight");
            _timeMultiplier = serializedObject.FindProperty("timeMultiplier");

            _maxInnerRadius = serializedObject.FindProperty("maxInnerRadius");
            _minInnerRadius = serializedObject.FindProperty("minInnerRadius");
            _innerRadiusIncreaseVal = serializedObject.FindProperty("innerRadiusIncreaseVal");
            _innerRadiusDecreaseVal = serializedObject.FindProperty("innerRadiusDecreaseVal");

            _maxOuterRadius = serializedObject.FindProperty("maxOuterRadius");
            _minOuterRadius = serializedObject.FindProperty("minOuterRadius");
            _outerRadiusIncreaseVal = serializedObject.FindProperty("outerRadiusIncreaseVal");
            _outerRadiusDecreaseVal = serializedObject.FindProperty("outerRadiusDecreaseVal");

            _isIntensityIncreasing = serializedObject.FindProperty("isIntensityIncreasing");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_playerLight);
            EditorGUILayout.Space(5f);
            EditorGUILayout.PropertyField(_timeMultiplier);
            EditorGUILayout.Space(5f);

            _isInnerRadiusOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isInnerRadiusOn, "Inner Radius Light");

            if (_isInnerRadiusOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_minInnerRadius);
                EditorGUILayout.PropertyField(_maxInnerRadius);
                EditorGUILayout.PropertyField(_innerRadiusIncreaseVal);
                EditorGUILayout.PropertyField(_innerRadiusDecreaseVal);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isOuterRadiusOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isOuterRadiusOn, "Outer Radius Light");

            if (_isOuterRadiusOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_minOuterRadius);
                EditorGUILayout.PropertyField(_maxOuterRadius);
                EditorGUILayout.PropertyField(_outerRadiusIncreaseVal);
                EditorGUILayout.PropertyField(_outerRadiusDecreaseVal);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isDebuggingOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isDebuggingOn, "For Debugging Purposes");

            if (_isDebuggingOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isIntensityIncreasing);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}