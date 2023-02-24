using Player;
using UnityEditor;

namespace Editor.Player {
    [CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : UnityEditor.Editor {
        #region Serialized Properties
        private SerializedProperty _moveSpeed;
        private SerializedProperty _jumpForce;

        private SerializedProperty _groundChecker;
        private SerializedProperty _wallChecker;
        private SerializedProperty _groundLayer;

        private SerializedProperty _wallJumpForce;
        private SerializedProperty _wallJumpDuration;
        private SerializedProperty _wallSlidingSpeed;

        private SerializedProperty _cachedXInputVal;
        private SerializedProperty _xInputVal;

        private SerializedProperty _isGrounded;
        private SerializedProperty _isFalling;

        private SerializedProperty _isWallActive;
        private SerializedProperty _isWallClimbing;
        private SerializedProperty _isSliding;
        private SerializedProperty _wallSlideMaxSpeed;

        private SerializedProperty _isWallJumpInitiated;
        private SerializedProperty _isWallJumpInProgress;
        private SerializedProperty _wallJumpDirection;

        private SerializedProperty _isAttacking;
        private SerializedProperty _firstAttack;
        #endregion

        #region Foldout Header Group booleans
        private bool _isMoveJumpGroupOn = true;
        private bool _isGroundAndWallGroupOn = true;
        private bool _isWallJumpGroupOn = true;
        private bool _isDebuggerOn = true;
        #endregion

        private void OnEnable() {
            _moveSpeed = serializedObject.FindProperty("moveSpeed");
            _jumpForce = serializedObject.FindProperty("jumpForce");

            _groundChecker = serializedObject.FindProperty("groundChecker");
            _wallChecker = serializedObject.FindProperty("wallChecker");
            _groundLayer = serializedObject.FindProperty("groundLayer");

            _wallJumpForce = serializedObject.FindProperty("wallJumpForce");
            _wallJumpDuration = serializedObject.FindProperty("wallJumpDuration");
            _wallSlidingSpeed = serializedObject.FindProperty("wallSlidingSpeed");

            _xInputVal = serializedObject.FindProperty("xInputVal");
            _cachedXInputVal = serializedObject.FindProperty("cachedXInputVal");

            _isGrounded = serializedObject.FindProperty("isGrounded");
            _isFalling = serializedObject.FindProperty("isFalling");

            _isWallActive = serializedObject.FindProperty("isWallActive");
            _isWallClimbing = serializedObject.FindProperty("isWallClimbing");
            _isSliding = serializedObject.FindProperty("isSliding");
            _wallSlideMaxSpeed = serializedObject.FindProperty("wallSlideMaxSpeed");

            _isWallJumpInitiated = serializedObject.FindProperty("isWallJumpInitiated");
            _isWallJumpInProgress = serializedObject.FindProperty("isWallJumpInProgress");
            _wallJumpDirection = serializedObject.FindProperty("wallJumpDirection");

            _isAttacking = serializedObject.FindProperty("isAttacking");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            _isGroundAndWallGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isGroundAndWallGroupOn, "Ground and Wall Checkers");

            if (_isGroundAndWallGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_groundChecker);
                EditorGUILayout.PropertyField(_wallChecker);
                EditorGUILayout.PropertyField(_groundLayer);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isMoveJumpGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isMoveJumpGroupOn, "Move and Jump");

            if (_isMoveJumpGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.Slider(_moveSpeed, 1f, 20f, "Move Speed");
                EditorGUILayout.Slider(_jumpForce, 1f, 15f, "Jump Force");
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isWallJumpGroupOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isWallJumpGroupOn, "Wall Jump");

            if (_isWallJumpGroupOn) {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(_wallJumpForce);
                EditorGUILayout.PropertyField(_wallJumpDuration);
                EditorGUILayout.PropertyField(_wallSlidingSpeed);
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _isDebuggerOn = EditorGUILayout.BeginFoldoutHeaderGroup(_isDebuggerOn, "For Debugging Purposes");

            if (_isDebuggerOn) {
                EditorGUILayout.Space(5f);
                EditorGUI.indentLevel++;
                
                EditorGUILayout.LabelField("Move Input", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_xInputVal);
                EditorGUILayout.PropertyField(_cachedXInputVal);
                EditorGUI.indentLevel--;

                EditorGUILayout.LabelField("Grounded", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isGrounded);
                EditorGUILayout.PropertyField(_isFalling);
                EditorGUI.indentLevel--;

                EditorGUILayout.LabelField("Wall Slide", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isWallActive);
                EditorGUILayout.PropertyField(_isWallClimbing);
                EditorGUILayout.PropertyField(_isSliding);
                EditorGUILayout.PropertyField(_wallSlideMaxSpeed);
                EditorGUI.indentLevel--;

                EditorGUILayout.LabelField("Wall Jump", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_isWallJumpInitiated);
                EditorGUILayout.PropertyField(_isWallJumpInProgress);
                EditorGUILayout.PropertyField(_wallJumpDirection);
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