using Data;
using Player;
using PlayerStates.SubStates;
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
        public PlayerFallDamageState FallDamageState { get; private set; }
        public PlayerDeathState DeathState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerData playerData;
        #endregion

        #region Check Transforms
        [SerializeField] private Transform groundChecker;
        [SerializeField] private Transform wallChecker;
        #endregion

        #region Components
        public Animator Anim { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        private Rigidbody2D _rb2D;
        private SpriteRenderer _sr;
        private CapsuleCollider2D _capsule;
        #endregion

        #region Misc Vars
        public Vector2 CurrentVelocity { get; private set; }
        public int FacingDirection { get; private set; }
        public float WallJumpDuration { get; private set; }
        public float WallSlideHangDuration { get; private set; }
        public Vector2 WallJumpForce { get; private set; }
        public bool CanDash { get; private set; }
        public RaycastHit2D HitUp;
        private float _dashCurrCd;
        private RaycastHit2D _hitLeft;
        private RaycastHit2D _hitRight;
        private Vector2 _workspace;
        private Vector3 _checkpointPos;
        private Vector2 _lastAfterImgPos;
        #endregion

        #region Player Stats Vars
        public float HealthPoints { get; set; }
        public float SanityPoints { get; set; }
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
            FallDamageState = new PlayerFallDamageState(this, StateMachine, playerData, "fallDamage");
            DeathState = new PlayerDeathState(this, StateMachine, playerData, "death");
            DashState = new PlayerDashState(this, StateMachine, playerData, "dash");

            HealthPoints = playerData.maxHp;
            SanityPoints = playerData.maxSanity;
        }

        private void Start() {
            Anim = GetComponent<Animator>();
            _rb2D = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _capsule = transform.Find("CapsuleCollider").GetComponent<CapsuleCollider2D>();
            _checkpointPos = GameObject.FindGameObjectsWithTag("CheckpointTorch")[0].transform.position;

            // start facing towards right
            FacingDirection = 1;
            StateMachine.Initialize(IdleState);
        }

        private void Update() {
            CurrentVelocity = _rb2D.velocity;

            SetWallJumpData();
            SetCanDash();

            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate() {
            StateMachine.CurrentState.PhysicsUpdate();
        }
        #endregion


        #region Set Functions
        public void SetVelocityX(float velocity) {
            _workspace.Set(velocity, CurrentVelocity.y);
            _rb2D.velocity = _workspace;
            CurrentVelocity = _workspace;
        }

        public void SetVelocityY(float velocity) {
            _workspace.Set(CurrentVelocity.x, velocity);
            _rb2D.velocity = _workspace;
            CurrentVelocity = _workspace;
        }

        public void AddJumpForce(float force) {
            _rb2D.AddForce(new Vector2(0f, force), ForceMode2D.Impulse);
            CurrentVelocity = _rb2D.velocity;
        }

        public void AddWallJumpForce(Vector2 angle) {
            _rb2D.AddForce(angle, ForceMode2D.Impulse);
            CurrentVelocity = _rb2D.velocity;
        }

        public void AddDashForce(float force) {
            _rb2D.AddForce(new Vector2(force, 0f), ForceMode2D.Impulse);
            CurrentVelocity = _rb2D.velocity;
        }

        public void FlipScale() => Flip();
        public void FlipSpriteX() => _sr.flipX = !_sr.flipX;
        public void FreezePlayer() => _rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        public void UnFreezePlayer() => _rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        public void SetCanDashFalse() => CanDash = false;
        public int SetDashDirection() => InputHandler.MovementInput != 0 ? InputHandler.MovementInput : FacingDirection;

        public void RestartFromCheckpoint() {
            transform.position = _checkpointPos;
            HealthPoints = playerData.maxHp;
        }

        public void SetCapsuleToGroundSlide() {
            _capsule.transform.localPosition = new Vector3(0f, -0.6f, 0f);
            _capsule.offset = playerData.capsuleProps["groundSlide"][0];
            _capsule.size = playerData.capsuleProps["groundSlide"][1];
            _capsule.direction = CapsuleDirection2D.Horizontal;
        }

        public void SetCapsuleToNormal() {
            _capsule.transform.localPosition = Vector3.zero;
            _capsule.offset = playerData.capsuleProps["normal"][0];
            _capsule.size = playerData.capsuleProps["normal"][1];
            _capsule.direction = CapsuleDirection2D.Vertical;
        }

        #endregion


        #region Check Functions
        public void CheckIfShouldFlip(int xInput) {
            if (xInput != 0 && !CheckIfFacingInputDirection(xInput)) {
                Flip();
            }
        }

        public void CheckIfShouldPlaceAfterImg() {
            if (Vector2.Distance(transform.position, _lastAfterImgPos) >= playerData.distBetweenAfterImgs) {
                PlaceAfterImage();
            }
        }

        public bool CheckIfGrounded() => Physics2D.OverlapCapsule(groundChecker.position, playerData.capsuleSize, CapsuleDirection2D.Horizontal, 0, playerData.groundMask);
        public bool CheckIfTouchingWall() => Physics2D.OverlapCapsule(wallChecker.position, new Vector2(0.05f, 0.425f), CapsuleDirection2D.Vertical, 0, playerData.groundMask);
        public bool CheckIfFacingInputDirection(int xInput) => FacingDirection == xInput;
        public bool CheckIfAutoClimbOn() => InputHandler.JumpInput && _hitLeft && _hitRight;
        public bool CheckIfAutoWallJumpOn() => InputHandler.JumpInput && (_hitLeft && !_hitRight && InputHandler.MovementInput == -1 || _hitRight && !_hitLeft && InputHandler.MovementInput == 1);
        public bool CheckIfDead() => HealthPoints <= 0f;
        public bool CheckIfShouldDieFromNoRoom() => Physics2D.Raycast(transform.position, Vector2.up, 1f, playerData.groundMask);
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

            _rb2D.gravityScale = CheckIfAutoClimbOn() ? playerData.gravityForce[distInInt] : playerData.gravityForce[0];
        }

        private void SetCanDash() {
            if (!CanDash) {
                _dashCurrCd += Time.deltaTime;

                if (_dashCurrCd > playerData.dashCoolDown) {
                    CanDash = true;
                    _dashCurrCd = 0f;
                }
            }
        }

        private void PlaceAfterImage() {
            var img = PlayerAfterImagePool.Instance.GetFromPool();
            img.transform.localScale = transform.localScale;
        }

        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        #endregion
    }
}