using Data;
using PlayerStates.SubStates;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerFiniteStateMachine {
    public class PlayerScript : MonoBehaviour {
        #region State Variables
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerState MoveState { get; private set; }
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
        public float WallClimbCurrDuration { get; private set; }
        public RaycastHit2D HitLeft;
        public RaycastHit2D HitRight;
        private Vector2 _workspace;
        #endregion

        #region Unity Callback Functions
        private void Awake() {
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            TurnaroundState = new PlayerTurnaroundState(this, StateMachine, playerData, "turnaround");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        }

        private void Start() {
            Anim = GetComponent<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            Rb2D = GetComponent<Rigidbody2D>();
            Sr = GetComponent<SpriteRenderer>();

            // start facing towards right
            FacingDirection = 1;

            StateMachine.Initialize(IdleState);
        }

        private void Update() {
            CurrentVelocity = Rb2D.velocity;

            var pos = transform.position;
            HitLeft = Physics2D.Raycast(pos, Vector2.left, 5.1f, playerData.groundMask);
            HitRight = Physics2D.Raycast(pos, Vector2.right, 5.1f, playerData.groundMask);
            SetWallClimbData();

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

        public void SetWallClimbData() {
            // in case both walls are detected, make value dynamic, else set to default
            if (HitLeft && HitRight) {
                var dist = HitLeft.distance + HitRight.distance;

                WallClimbCurrDuration = dist switch {
                    < 2 => 0.275f, // 2 tiles distance between walls
                    > 2f and < 3f => 0.375f, // 3 tiles distance between walls
                    > 3f and < 5f => 0.525f, // 4 tiles distance between walls
                    _ => playerData.wallClimbDefaultDuration
                };
            } else {
                WallClimbCurrDuration = playerData.wallClimbDefaultDuration;
            }
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
            if (xInput != 0 && xInput != FacingDirection) {
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

        public bool CheckAutoClimbOn() {
            return InputHandler.JumpInput && HitLeft && HitRight;
        }
        #endregion


        #region Misc Functions
        private void Flip() {
            FacingDirection *= -1;
            transform.localScale = new Vector3(FacingDirection, 1f, 1f);
        }

        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion
    }
}