using Data;
using PlayerStates.SubStates;
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
        [SerializeField] private PlayerData playerData;
        #endregion

        #region Check Transforms
        [SerializeField] private Transform groundChecker;
        #endregion

        #region Components
        public Animator Anim { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public Rigidbody2D Rb2D { get; private set; }
        #endregion

        public Vector2 CurrentVelocity { get; private set; }
        public int FacingDirection { get; private set; }
        private Vector2 _workspace;


        #region Unity Callback Functions
        private void Awake() {
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        }

        private void Start() {
            Anim = GetComponent<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            Rb2D = GetComponent<Rigidbody2D>();

            FacingDirection = 1;

            StateMachine.Initialize(IdleState);
        }

        private void Update() {
            CurrentVelocity = Rb2D.velocity;
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

        public void SetVelocityYToZero() {
            _workspace.Set(CurrentVelocity.x, 0f);
            Rb2D.velocity = _workspace;
            CurrentVelocity = _workspace;
        }

        public void AddJumpForce(float force) {
            Rb2D.AddForce(new Vector2(0f, force), ForceMode2D.Impulse);
            CurrentVelocity = Rb2D.velocity;
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
        #endregion


        #region Misc Functions
        private void Flip() {
            FacingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion
    }
}