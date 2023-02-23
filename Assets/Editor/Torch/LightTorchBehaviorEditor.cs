using Torch;
using UnityEditor;

namespace Editor.Torch {
    [CustomEditor(typeof(LightTorchBehavior))]
    public class LightTorchBehaviorEditor : UnityEditor.Editor {
        #region Serialized properties
        private SerializedProperty _flameObject;
        private SerializedProperty _flameAnimation;

        private SerializedProperty _sineFreq;
        private SerializedProperty _sineAmp;

        private SerializedProperty _timeMultiplier;
        private SerializedProperty _increaseUnit;
        private SerializedProperty _decreaseUnit;
        private SerializedProperty _minIntensity;
        private SerializedProperty _maxIntensity;

        private SerializedProperty _isIntensityIncreasing;
        private SerializedProperty _lightIntensityValue;
        private SerializedProperty _outerRadiusValue;
        #endregion

        # region Foldout Header Group booleans
        private bool _isSineOn = true;
        private bool _isIntensityOn = true;
        private bool _isDebuggerOn = true;
        #endregion

        private void OnEnable() {
            _flameObject = serializedObject.FindProperty("flameObject");
            _flameAnimation = serializedObject.FindProperty("flameAnimation");

            _sineFreq = serializedObject.FindProperty("sineFreq");
            _sineAmp = serializedObject.FindProperty("sineAmp");

            _timeMultiplier = serializedObject.FindProperty("timeMultiplier");
            _increaseUnit = serializedObject.FindProperty("increaseUnit");
            _decreaseUnit = serializedObject.FindProperty("decreaseUnit");
            _minIntensity = serializedObject.FindProperty("minIntensity");
            _maxIntensity = serializedObject.FindProperty("maxIntensity");

            _isIntensityIncreasing = serializedObject.FindProperty("isIntensityIncreasing");
            _lightIntensityValue = serializedObject.FindProperty("lightIntensityValue");
            _outerRadiusValue = serializedObject.FindProperty("outerRadiusValue");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_flameObject);
            EditorGUILayout.PropertyField(_flameAnimation);
            EditorGUILayout.Space(5f);

            _isSineOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isSineOn, "Flame Outer Radius Sine");

            if (_isSineOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.Slider(_sineFreq, 0f, 4.5f);
                EditorGUILayout.Slider(_sineAmp, 0, 3f);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isIntensityOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isIntensityOn, "Flame Light Intensity");

            if (_isIntensityOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_timeMultiplier);
                EditorGUILayout.PropertyField(_minIntensity);
                EditorGUILayout.PropertyField(_maxIntensity);
                EditorGUILayout.PropertyField(_increaseUnit);
                EditorGUILayout.PropertyField(_decreaseUnit);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isDebuggerOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isDebuggerOn, "For Debugging Purposes");

            if (_isDebuggerOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_isIntensityIncreasing);
                EditorGUILayout.PropertyField(_lightIntensityValue);
                EditorGUILayout.PropertyField(_outerRadiusValue);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}