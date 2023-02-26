using Player;
using UnityEditor;

namespace Editor.Player {
    [CustomEditor(typeof(SpawnEnemies))]
    public class SpawnEnemiesEditor : UnityEditor.Editor {
        #region serialized properties
        private MonoScript _script;

        private SerializedProperty _skeletonPrefab;
        private SerializedProperty _skeletonContainer;
        private SerializedProperty _maxAllowedInContainer;

        private SerializedProperty _pointsContainer;
        private SerializedProperty _leftSide;
        private SerializedProperty _rightSide;
        private SerializedProperty _xOffset;
        private SerializedProperty _groundLayer;

        private SerializedProperty _minRandSpawnTime;
        private SerializedProperty _maxRandSpawnTime;

        private SerializedProperty _spawnCdValue;
        private SerializedProperty _spawnTime;

        private SerializedProperty _isLeftAvailable;
        private SerializedProperty _isRightAvailable;
        private SerializedProperty _safeZone;

        private SerializedProperty _skeletonsCount;
        #endregion

        #region Foldout Header Group booleans
        private bool _isEnemyDataGroupOn = true;
        private bool _isSpawnDataGroupOn = true;
        private bool _isCdRandGroupOn = true;
        private bool _isDebuggerOn = true;
        #endregion


        private void OnEnable() {
            _script = MonoScript.FromMonoBehaviour((SpawnEnemies)target);

            _skeletonPrefab = serializedObject.FindProperty("skeletonPrefab");
            _skeletonContainer = serializedObject.FindProperty("skeletonContainer");
            _maxAllowedInContainer = serializedObject.FindProperty("maxAllowedInContainer");

            _pointsContainer = serializedObject.FindProperty("pointsContainer");
            _leftSide = serializedObject.FindProperty("leftSide");
            _rightSide = serializedObject.FindProperty("rightSide");
            _xOffset = serializedObject.FindProperty("xOffset");
            _groundLayer = serializedObject.FindProperty("groundLayer");

            _minRandSpawnTime = serializedObject.FindProperty("minRandSpawnTime");
            _maxRandSpawnTime = serializedObject.FindProperty("maxRandSpawnTime");

            _spawnCdValue = serializedObject.FindProperty("spawnCdValue");
            _spawnTime = serializedObject.FindProperty("spawnTime");

            _isLeftAvailable = serializedObject.FindProperty("isLeftAvailable");
            _isRightAvailable = serializedObject.FindProperty("isRightAvailable");
            _safeZone = serializedObject.FindProperty("safeZone");

            _skeletonsCount = serializedObject.FindProperty("skeletonsCount");
        }


        public override void OnInspectorGUI() {
            serializedObject.Update();

            _script = EditorGUILayout.ObjectField("Script Location", _script, typeof(MonoScript), false) as MonoScript;
            EditorGUILayout.Space(10f);

            _isEnemyDataGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isEnemyDataGroupOn, "Enemy Data");

            if (_isEnemyDataGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_skeletonPrefab);
                EditorGUILayout.PropertyField(_skeletonContainer);
                EditorGUILayout.PropertyField(_maxAllowedInContainer);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isSpawnDataGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isSpawnDataGroupOn, "Spawn Location Data");

            if (_isCdRandGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_pointsContainer);
                EditorGUILayout.PropertyField(_leftSide);
                EditorGUILayout.PropertyField(_rightSide);
                EditorGUILayout.PropertyField(_xOffset);
                EditorGUILayout.PropertyField(_groundLayer);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();


            _isCdRandGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isCdRandGroupOn, "Spawn CD Randomizer");

            if (_isCdRandGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_minRandSpawnTime);
                EditorGUILayout.PropertyField(_maxRandSpawnTime);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();


            _isDebuggerOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isDebuggerOn, "For Debugging Purposes");

            if (_isDebuggerOn) {
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Cooldown Values", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_spawnCdValue);
                EditorGUILayout.PropertyField(_spawnTime);
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("Spawn Points", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isLeftAvailable);
                EditorGUILayout.PropertyField(_isRightAvailable);
                EditorGUILayout.PropertyField(_safeZone);
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("Container Data", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_skeletonsCount);
                EditorGUI.indentLevel--;

                EditorGUI.indentLevel--;
                EditorGUILayout.Space(5f);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}