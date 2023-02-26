using Cinemachine.Editor;
using Enemy;
using UnityEditor;
using UnityEditor.Rendering;

namespace Editor.Enemy {
    [CustomEditor(typeof(SkeletonController))]
    public class SkeletonControllerEditor : UnityEditor.Editor {
        #region Serialized Properties
        private MonoScript _script;
        private SerializedProperty _hitCapsuleObj;

        private SerializedProperty _groundChecker;
        private SerializedProperty _groundLayer;

        private SerializedProperty _minRandomWalk;
        private SerializedProperty _maxRandomWalk;
        private SerializedProperty _minRandomRun;
        private SerializedProperty _maxRandomRun;

        private SerializedProperty _walkSpeed;
        private SerializedProperty _runSpeed;

        private SerializedProperty _rayCastRange;
        private SerializedProperty _minAllowedDistance;
        private SerializedProperty _maxAllowedDistance;

        private SerializedProperty _isPlayerDetected;
        private SerializedProperty _dirX;
        private SerializedProperty _isPlayerFacingSelf;
        private SerializedProperty _isWalking;
        private SerializedProperty _isRunning;
        private SerializedProperty _currentSpeed;

        private SerializedProperty _isAttacking;
        private SerializedProperty _isGrounded;
        private SerializedProperty _isDeathTriggered;
        #endregion

        #region Foldout Header Group Booleans
        private bool _isGroundedGroupOn = true;
        private bool _isWalkRunGeneratorGroupOn = true;
        private bool _isWalkRunGroupOn = true;
        private bool _isDetectionGroupOn = true;
        private bool _isDebuggerOn = true;
        #endregion

        private void OnEnable() {
            _script = MonoScript.FromMonoBehaviour((SkeletonController)target);
            _hitCapsuleObj = serializedObject.FindProperty("hitCapsuleObj");

            _groundChecker = serializedObject.FindProperty("groundChecker");
            _groundLayer = serializedObject.FindProperty("groundLayer");

            _minRandomWalk = serializedObject.FindProperty("minRandomWalk");
            _maxRandomWalk = serializedObject.FindProperty("maxRandomWalk");
            _minRandomRun = serializedObject.FindProperty("minRandomRun");
            _maxRandomRun = serializedObject.FindProperty("maxRandomRun");

            _walkSpeed = serializedObject.FindProperty("walkSpeed");
            _runSpeed = serializedObject.FindProperty("runSpeed");

            _rayCastRange = serializedObject.FindProperty("rayCastRange");
            _minAllowedDistance = serializedObject.FindProperty("minAllowedDistance");
            _maxAllowedDistance = serializedObject.FindProperty("maxAllowedDistance");

            _isPlayerDetected = serializedObject.FindProperty("isPlayerDetected");
            _dirX = serializedObject.FindProperty("dirX");
            _isPlayerFacingSelf = serializedObject.FindProperty("isPlayerFacingSelf");
            _isWalking = serializedObject.FindProperty("isWalking");
            _isRunning = serializedObject.FindProperty("isRunning");
            _currentSpeed = serializedObject.FindProperty("currentSpeed");

            _isAttacking = serializedObject.FindProperty("isAttacking");
            _isGrounded = serializedObject.FindProperty("isGrounded");
            _isDeathTriggered = serializedObject.FindProperty("isDeathTriggered");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            _script = EditorGUILayout.ObjectField("Script Location", _script, typeof(MonoScript), false) as MonoScript;
            EditorGUILayout.Space(10f);

            EditorGUILayout.PropertyField(_hitCapsuleObj);
            EditorGUILayout.Space(5f);

            _isGroundedGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isGroundedGroupOn, "Grounded Data");

            if (_isWalkRunGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_groundChecker);
                EditorGUILayout.PropertyField(_groundLayer);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();


            _isWalkRunGeneratorGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isWalkRunGeneratorGroupOn, "Speed Randomizer");

            if (_isWalkRunGeneratorGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.Slider(_minRandomWalk, 1f, 3.5f, "Min Random Walk");
                EditorGUILayout.Slider(_maxRandomWalk, 3.5f, 6f, "Max Random Walk");
                EditorGUILayout.Separator();
                EditorGUILayout.Slider(_minRandomRun, 6f, 9.5f, "Min Random Run");
                EditorGUILayout.Slider(_maxRandomRun, 9.5f, 12f, "Max Random Run");
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

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
                EditorGUILayout.Slider(_minAllowedDistance, 0f, 15f, "Min Distance from Player");
                EditorGUILayout.Slider(_maxAllowedDistance, 15f, 50f, "Max Distance from Player");
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();


            _isDebuggerOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isDebuggerOn, "For Debugging Purposes");

            if (_isDebuggerOn) {
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Player Detection", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isPlayerDetected);
                EditorGUILayout.PropertyField(_dirX);
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("Movement Behavior", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isPlayerFacingSelf);
                EditorGUILayout.PropertyField(_isWalking);
                EditorGUILayout.PropertyField(_isRunning);
                EditorGUILayout.PropertyField(_currentSpeed);
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("Triggers", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isAttacking);
                EditorGUILayout.PropertyField(_isGrounded);
                EditorGUILayout.PropertyField(_isDeathTriggered);
                EditorGUILayout.Separator();

                EditorGUI.indentLevel--;
                EditorGUILayout.Space(5f);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}