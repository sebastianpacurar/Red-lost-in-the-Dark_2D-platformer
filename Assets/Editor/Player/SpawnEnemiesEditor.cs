using Player;
using UnityEditor;

namespace Editor.Player {
    [CustomEditor(typeof(SpawnEnemies))]
    public class SpawnEnemiesEditor : UnityEditor.Editor {
        #region serialized properties
        private MonoScript _script;

        private SerializedProperty _skeletonPrefab;
        private SerializedProperty _skeletonContainer;

        private SerializedProperty _spawnPoints;
        private SerializedProperty _leftSide;
        private SerializedProperty _rightSide;

        private SerializedProperty _minRandSpawnTime;
        private SerializedProperty _maxRandSpawnTime;

        private SerializedProperty _spawnCdValue;
        private SerializedProperty _spawnTime;
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

            _spawnPoints = serializedObject.FindProperty("spawnPoints");
            _leftSide = serializedObject.FindProperty("leftSide");
            _rightSide = serializedObject.FindProperty("rightSide");

            _minRandSpawnTime = serializedObject.FindProperty("minRandSpawnTime");
            _maxRandSpawnTime = serializedObject.FindProperty("maxRandSpawnTime");

            _spawnCdValue = serializedObject.FindProperty("spawnCdValue");
            _spawnTime = serializedObject.FindProperty("spawnTime");
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
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isSpawnDataGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isSpawnDataGroupOn, "Spawn Location Data");

            if (_isCdRandGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_spawnPoints);
                EditorGUILayout.PropertyField(_leftSide);
                EditorGUILayout.PropertyField(_rightSide);
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


            _isEnemyDataGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isEnemyDataGroupOn, "Enemy Data");

            if (_isEnemyDataGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_skeletonPrefab);
                EditorGUILayout.PropertyField(_skeletonContainer);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();


            _isDebuggerOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isDebuggerOn, "For Debugging Purposes");

            if (_isDebuggerOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_spawnCdValue);
                EditorGUILayout.PropertyField(_spawnTime);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}