using Player;
using UnityEditor;

namespace Editor.Player {
    [CustomEditor(typeof(HandleHpSanity))]
    public class HandleHpSanityEditor : UnityEditor.Editor {
        #region Serialized properties
        private MonoScript _script;

        private SerializedProperty _playerLight;
        private SerializedProperty _timeMultiplier;

        private SerializedProperty _maxOuterRadius;
        private SerializedProperty _minOuterRadius;
        private SerializedProperty _outerRadiusIncreaseUnit;
        private SerializedProperty _outerRadiusDecreaseUnit;

        private SerializedProperty _hpIncreaseUnit;
        private SerializedProperty _maxHp;

        private SerializedProperty _currentHp;
        private SerializedProperty _currentSanity;
        private SerializedProperty _maxSanity;
        private SerializedProperty _minSanity;

        private SerializedProperty _isIntensityIncreasing;
        private SerializedProperty _torchTag;
        #endregion

        # region Foldout Header Group booleans
        private bool _isOuterRadiusOn = true;
        private bool _isHpGroupOn = true;
        private bool _isDebuggerOn = true;
        #endregion

        private void OnEnable() {
            _script = MonoScript.FromMonoBehaviour((HandleHpSanity)target);
            _playerLight = serializedObject.FindProperty("playerLight");
            _timeMultiplier = serializedObject.FindProperty("timeMultiplier");

            _maxOuterRadius = serializedObject.FindProperty("maxOuterRadius");
            _minOuterRadius = serializedObject.FindProperty("minOuterRadius");
            _outerRadiusIncreaseUnit = serializedObject.FindProperty("outerRadiusIncreaseUnit");
            _outerRadiusDecreaseUnit = serializedObject.FindProperty("outerRadiusDecreaseUnit");

            _hpIncreaseUnit = serializedObject.FindProperty("hpIncreaseUnit");
            _maxHp = serializedObject.FindProperty("maxHp");

            _currentHp = serializedObject.FindProperty("currentHp");
            _currentSanity = serializedObject.FindProperty("currentSanity");
            _maxSanity = serializedObject.FindProperty("maxSanity");
            _minSanity = serializedObject.FindProperty("minSanity");

            _isIntensityIncreasing = serializedObject.FindProperty("isIntensityIncreasing");
            _torchTag = serializedObject.FindProperty("torchTag");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            _script = EditorGUILayout.ObjectField("Script Location", _script, typeof(MonoScript), false) as MonoScript;
            EditorGUILayout.Space(10f);

            EditorGUILayout.PropertyField(_playerLight);
            EditorGUILayout.Space(5f);
            EditorGUILayout.PropertyField(_timeMultiplier);
            EditorGUILayout.Space(5f);

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isOuterRadiusOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isOuterRadiusOn, "Outer Radius Light");

            if (_isOuterRadiusOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_minOuterRadius);
                EditorGUILayout.PropertyField(_maxOuterRadius);
                EditorGUILayout.PropertyField(_outerRadiusIncreaseUnit);
                EditorGUILayout.PropertyField(_outerRadiusDecreaseUnit);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isHpGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isHpGroupOn, "Outer Radius Light");

            if (_isOuterRadiusOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_hpIncreaseUnit);
                EditorGUILayout.PropertyField(_maxHp);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isDebuggerOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isDebuggerOn, "For Debugging Purposes");

            if (_isDebuggerOn) {
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Player Stats", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_currentHp);
                EditorGUILayout.PropertyField(_currentSanity);
                EditorGUILayout.PropertyField(_maxSanity);
                EditorGUILayout.PropertyField(_minSanity);
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("Torch Interaction", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_torchTag);
                EditorGUILayout.PropertyField(_isIntensityIncreasing);
                EditorGUI.indentLevel--;

                EditorGUI.indentLevel--;
                EditorGUILayout.Space(5f);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}