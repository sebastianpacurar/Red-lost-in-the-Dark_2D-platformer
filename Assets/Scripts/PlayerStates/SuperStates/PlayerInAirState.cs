using Data;
using PlayerFiniteStateMachine;

namespace PlayerStates.SuperStates {
    public class PlayerInAirState : PlayerState {
        private int _xInput;
        private bool _isGrounded;
        private bool _isTouchingWall;
        private bool _isAutoClimbOn;

        public PlayerInAirState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();
            _xInput = Player.InputHandler.MovementInput;

            // enter Land State
            if (_isGrounded && Player.CurrentVelocity.y < 0.01f) {
                StateMachine.ChangeState(Player.LandState);
            }
            // enter wall slide state
            else if (_isTouchingWall) {
                StateMachine.ChangeState(Player.WallSlideState);
            }
            // enter auto climb process
            else if (_isTouchingWall && _isAutoClimbOn) {
                Player.SetVelocityX(PlayerData.movementVelocity * Player.FacingDirection);
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
            _isAutoClimbOn = Player.CheckIfAutoClimbOn();
        }
    }
}