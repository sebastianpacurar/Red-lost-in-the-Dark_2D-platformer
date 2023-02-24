using Enemy;
using UnityEditor;

namespace Editor.Enemy {
    [CustomEditor(typeof(SkeletonController))]
    public class SkeletonControllerEditor : UnityEditor.Editor {
        #region Serialized Properties
        private SerializedProperty _hitCapsuleObj;

        private SerializedProperty _walkSpeed;
        private SerializedProperty _runSpeed;

        private SerializedProperty _rayCastRange;
        private SerializedProperty _minDistanceFromPlayer;

        private SerializedProperty _isPlayerDetected;
        private SerializedProperty _dirX;
        private SerializedProperty _isPlayerFacingSelf;
        private SerializedProperty _isWalking;
        private SerializedProperty _isRunning;
        private SerializedProperty _currentSpeed;

        private SerializedProperty _isAttacking;
        #endregion

        #region Foldout Header Group Booleans
        private bool _isWalkRunGroupOn = true;
        private bool _isDetectionGroupOn = true;
        private bool _isDebuggerOn = true;
        #endregion

        private void OnEnable() {
            _hitCapsuleObj = serializedObject.FindProperty("hitCapsuleObj");
            _walkSpeed = serializedObject.FindProperty("walkSpeed");
            _runSpeed = serializedObject.FindProperty("runSpeed");

            _rayCastRange = serializedObject.FindProperty("rayCastRange");
            _minDistanceFromPlayer = serializedObject.FindProperty("minDistanceFromPlayer");

            _isPlayerDetected = serializedObject.FindProperty("isPlayerDetected");
            _dirX = serializedObject.FindProperty("dirX");
            _isPlayerFacingSelf = serializedObject.FindProperty("isPlayerFacingSelf");
            _isWalking = serializedObject.FindProperty("isWalking");
            _isRunning = serializedObject.FindProperty("isRunning");
            _currentSpeed = serializedObject.FindProperty("currentSpeed");

            _isAttacking = serializedObject.FindProperty("isAttacking");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_hitCapsuleObj);
            EditorGUILayout.Space(5f);

            _isWalkRunGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isWalkRunGroupOn, "Walk and Run");

            if (_isWalkRunGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.Slider(_walkSpeed, 1f, 20f, "Walk Speed");
                EditorGUILayout.Slider(_runSpeed, 1f, 15f, "Run Speed");
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isDetectionGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isDetectionGroupOn, "Player Detection");

            if (_isDetectionGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.Slider(_rayCastRange, 0f, 100f, "Ray Cast Range");
                EditorGUILayout.Slider(_minDistanceFromPlayer, 0f, 15f, "Min Dist from Player");
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();


            _isDebuggerOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isDebuggerOn, "For Debugging Purposes");

            if (_isDebuggerOn) {
                EditorGUILayout.Space(5f);

                EditorGUILayout.LabelField("Player Detection", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isPlayerDetected);
                EditorGUILayout.PropertyField(_dirX);
                EditorGUI.indentLevel--;

                EditorGUILayout.LabelField("Movement Behavior", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isPlayerFacingSelf);
                EditorGUILayout.PropertyField(_isWalking);
                EditorGUILayout.PropertyField(_isRunning);
                EditorGUILayout.PropertyField(_currentSpeed);
                EditorGUI.indentLevel--;

                EditorGUILayout.LabelField("Attack", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isAttacking);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}