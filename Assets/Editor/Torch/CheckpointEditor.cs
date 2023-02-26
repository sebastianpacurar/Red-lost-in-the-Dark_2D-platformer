using Torch;
using UnityEditor;

namespace Editor.Torch {
    [CustomEditor(typeof(Checkpoint))]
    public class CheckpointEditor : UnityEditor.Editor {
        #region Serialized properties
        private MonoScript _script;

        private SerializedProperty _flameObject;
        private SerializedProperty _flameAnimation;

        private SerializedProperty _sineFreq;
        private SerializedProperty _sineAmp;

        private SerializedProperty _minInner;
        private SerializedProperty _maxInner;
        private SerializedProperty _minOuter;
        private SerializedProperty _maxOuter;
        private SerializedProperty _minFallOff;
        private SerializedProperty _maxFallOff;

        private SerializedProperty _sparklesPs;
        private SerializedProperty _hollowPs;

        private SerializedProperty _isMarked;
        #endregion

        # region Foldout Header Group booleans
        private bool _isSineGroupOn = true;
        private bool _isLightGroupOn = true;
        private bool _isParticleSystemGroupOn = true;
        private bool _isDebuggerOn = true;
        #endregion


        private void OnEnable() {
            _script = MonoScript.FromMonoBehaviour((Checkpoint)target);
            _flameObject = serializedObject.FindProperty("flameObject");
            _flameAnimation = serializedObject.FindProperty("flameAnimation");

            _sineFreq = serializedObject.FindProperty("sineFreq");
            _sineAmp = serializedObject.FindProperty("sineAmp");

            _minInner = serializedObject.FindProperty("minInner");
            _maxInner = serializedObject.FindProperty("maxInner");
            _minOuter = serializedObject.FindProperty("minOuter");
            _maxOuter = serializedObject.FindProperty("maxOuter");
            _minFallOff = serializedObject.FindProperty("minFallOff");
            _maxFallOff = serializedObject.FindProperty("maxFallOff");

            _sparklesPs = serializedObject.FindProperty("sparklesPs");
            _hollowPs = serializedObject.FindProperty("hollowPs");

            _isMarked = serializedObject.FindProperty("isMarked");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            _script = EditorGUILayout.ObjectField("Script Location", _script, typeof(MonoScript), false) as MonoScript;
            EditorGUILayout.Space(10f);

            EditorGUILayout.PropertyField(_flameObject);
            EditorGUILayout.PropertyField(_flameAnimation);
            EditorGUILayout.Space(5f);

            _isSineGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isSineGroupOn, "Flame Outer Radius Sine");

            if (_isSineGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.Slider(_sineFreq, 0f, 4.5f);
                EditorGUILayout.Slider(_sineAmp, 0, 3f);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isLightGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isLightGroupOn, "Flame Light Data");

            if (_isLightGroupOn) {
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Inner Radius", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_minInner);
                EditorGUILayout.PropertyField(_maxInner);
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("Outer Radius", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_minOuter);
                EditorGUILayout.PropertyField(_maxOuter);
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("Falloff Strength", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_minFallOff);
                EditorGUILayout.PropertyField(_maxFallOff);
                EditorGUI.indentLevel--;

                EditorGUI.indentLevel--;
                EditorGUILayout.Space(5f);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isParticleSystemGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isParticleSystemGroupOn, "Particle Systems");

            if (_isParticleSystemGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_sparklesPs);
                EditorGUILayout.PropertyField(_hollowPs);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();


            _isDebuggerOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isDebuggerOn, "For Debugging Purposes");

            if (_isDebuggerOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_isMarked);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}