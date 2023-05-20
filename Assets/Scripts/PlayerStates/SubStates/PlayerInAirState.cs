using Data;
using PlayerFiniteStateMachine;

namespace PlayerStates.SubStates {
    public class PlayerInAirState : PlayerState {
        private int _xInput;
        private bool _dashInput;
        private bool _isGrounded;
        private bool _isTouchingWall;
        private bool _isAutoClimbOn;
        private bool _isAutoWallJumpOn;
        private float _currFallingSpeed;

        public PlayerInAirState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            _currFallingSpeed = 0f;
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();
            UpdateCurrFallingSpeed();

            _xInput = Player.InputHandler.MovementInput;
            _dashInput = Player.InputHandler.DashInput;

            _isAutoClimbOn = Player.CheckIfAutoClimbOn();
            _isAutoWallJumpOn = Player.CheckIfAutoWallJumpOn();

            if (_currFallingSpeed < PlayerData.maxSafeFallSpeed) {
                Player.CheckIfShouldPlaceAfterImg();
            }

            // enter FallDamage state 
            if (_isGrounded && _currFallingSpeed < PlayerData.maxSafeFallSpeed) {
                StateMachine.ChangeState(Player.FallDamageState);
            }
            // enter Land State
            else if (_isGrounded && Player.CurrentVelocity.y < 0.01f) {
                StateMachine.ChangeState(Player.LandState);
            }
            // enter WallSlide state
            else if (_isTouchingWall) {
                StateMachine.ChangeState(Player.WallSlideState);
            }
            // enter auto climb process
            else if (_isTouchingWall && _isAutoClimbOn) {
                Player.SetVelocityX(PlayerData.movementVelocity * Player.FacingDirection);
            } else if (_dashInput && Player.CanDash && !_isAutoClimbOn && !_isAutoWallJumpOn) {
                StateMachine.ChangeState(Player.DashState);
            }
            // perform regular movement in air
            else {
                Player.CheckIfShouldFlip(_xInput);
                Player.SetVelocityX(PlayerData.movementVelocity * _xInput);
            }

            Player.Anim.SetFloat("yVelocity", Player.CurrentVelocity.y);
        }


        protected override void DoChecks() {
            base.DoChecks();

            _isGrounded = Player.CheckIfGrounded();
            _isTouchingWall = Player.CheckIfTouchingWall();
        }

        private void UpdateCurrFallingSpeed() {
            if (_currFallingSpeed > Player.CurrentVelocity.y) {
                _currFallingSpeed = Player.CurrentVelocity.y;
            }
        }
    }
}