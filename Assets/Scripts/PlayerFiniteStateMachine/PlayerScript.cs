using Data;
using PlayerStates.SubStates;
using PlayerStates.SuperStates;
using UnityEngine;

namespace PlayerFiniteStateMachine {
    public class PlayerScript : MonoBehaviour {
        #region State Variables
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerGroundSlideState GroundSlideState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerLandState LandState { get; private set; }
        public PlayerTurnaroundState TurnaroundState { get; private set; }
        public PlayerWallSlideState WallSlideState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        [SerializeField] private PlayerData playerData;
        #endregion

        #region Check Transforms
        [SerializeField] private Transform groundChecker;
        [SerializeField] private Transform wallChecker;
        #endregion

        #region Components
        public Animator Anim { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        private Rigidbody2D Rb2D { get; set; }
        private SpriteRenderer Sr { get; set; }
        #endregion

        #region Misc Vars
        public Vector2 CurrentVelocity { get; private set; }
        public int FacingDirection { get; private set; }
        public float WallJumpDuration { get; private set; }
        public float WallSlideHangDuration { get; private set; }
        public Vector2 WallJumpForce { get; private set; }
        private RaycastHit2D _hitLeft;
        private RaycastHit2D _hitRight;
        private Vector2 _workspace;
        #endregion

        #region Unity Callback Functions
        private void Awake() {
            InputHandler = PlayerInputHandler.Instance;
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            GroundSlideState = new PlayerGroundSlideState(this, StateMachine, playerData, "groundSlide");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            TurnaroundState = new PlayerTurnaroundState(this, StateMachine, playerData, "turnaround");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        }

        private void Start() {
            Anim = GetComponent<Animator>();
            Rb2D = GetComponent<Rigidbody2D>();
            Sr = GetComponent<SpriteRenderer>();

            // start facing towards right
            FacingDirection = 1;

            StateMachine.Initialize(IdleState);
        }

        private void Update() {
            CurrentVelocity = Rb2D.velocity;

            SetWallJumpData();

            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate() {
            StateMachine.CurrentState.PhysicsUpdate();
        }
        #endregion


        #region Set Functions
        public void SetVelocityX(float velocity) {
            _workspace.Set(velocity, CurrentVelocity.y);
            Rb2D.velocity = _workspace;
            CurrentVelocity = _workspace;
        }

        public void SetVelocityY(float velocity) {
            _workspace.Set(CurrentVelocity.x, velocity);
            Rb2D.velocity = _workspace;
            CurrentVelocity = _workspace;
        }

        public void AddJumpForce(float force) {
            Rb2D.AddForce(new Vector2(0f, force), ForceMode2D.Impulse);
            CurrentVelocity = Rb2D.velocity;
        }

        public void AddWallJumpForce(Vector2 angle) {
            Rb2D.AddForce(angle, ForceMode2D.Impulse);
            CurrentVelocity = Rb2D.velocity;
        }

        public void FlipScale() {
            Flip();
        }

        public void FlipSpriteX() {
            Sr.flipX = !Sr.flipX;
        }
        #endregion


        #region Check Functions
        public void CheckIfShouldFlip(int xInput) {
            if (xInput != 0 && !CheckIfFacingInputDirection(xInput)) {
                Flip();
            }
        }

        public bool CheckIfGrounded() {
            return Physics2D.OverlapCapsule(groundChecker.position, playerData.capsuleSize, CapsuleDirection2D.Horizontal, 0, playerData.groundMask);
        }

        public bool CheckIfTouchingWall() {
            return Physics2D.OverlapCapsule(wallChecker.position, new Vector2(0.05f, 0.425f), CapsuleDirection2D.Vertical, 0, playerData.groundMask);
        }

        public bool CheckIfFacingInputDirection(int xInput) {
            return FacingDirection == xInput;
        }

        public bool CheckIfAutoClimbOn() {
            return InputHandler.JumpInput && _hitLeft && _hitRight;
        }

        public bool CheckIfAutoWallJumpOn() {
            return InputHandler.JumpInput && (_hitLeft && !_hitRight && InputHandler.MovementInput == -1 || _hitRight && !_hitLeft && InputHandler.MovementInput == 1);
        }
        #endregion


        #region Misc Functions
        private void Flip() {
            FacingDirection *= -1;
            transform.localScale = new Vector3(FacingDirection, 1f, 1f);
        }

        private void SetWallJumpData() {
            var pos = transform.position;
            _hitLeft = Physics2D.Raycast(pos, Vector2.left, 5.1f, playerData.groundMask);
            _hitRight = Physics2D.Raycast(pos, Vector2.right, 5.1f, playerData.groundMask);

            var distInInt = _hitLeft && _hitRight ? Mathf.RoundToInt(_hitLeft.distance + _hitRight.distance) : 0;
            WallJumpDuration = playerData.wallJumpDur[distInInt];
            WallJumpForce = playerData.wallJumpForce[distInInt];
            WallSlideHangDuration = playerData.wallSlideHangDuration[distInInt];

            Rb2D.gravityScale = CheckIfAutoClimbOn() ? playerData.gravityForce[distInInt] : playerData.gravityForce[0];
        }

        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion
    }
}