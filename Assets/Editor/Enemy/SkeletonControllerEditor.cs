using Enemy;
using UnityEditor;

namespace Editor.Enemy {
    [CustomEditor(typeof(SkeletonController))]
    public class SkeletonControllerEditor : UnityEditor.Editor {
        #region Serialized Properties
        private SerializedProperty _walkSpeed;
        private SerializedProperty _runSpeed;

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
        private bool _isMoveJumpGroupOn = true;
        private bool _isDetectionGroupOn = true;
        private bool _isDebuggerOn = true;
        #endregion

        private void OnEnable() {
            _walkSpeed = serializedObject.FindProperty("walkSpeed");
            _runSpeed = serializedObject.FindProperty("runSpeed");

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

            _isMoveJumpGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isMoveJumpGroupOn, "Walk and Run");

            if (_isMoveJumpGroupOn) {
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