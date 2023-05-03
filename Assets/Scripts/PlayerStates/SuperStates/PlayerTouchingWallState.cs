using Data;
using PlayerFiniteStateMachine;

namespace PlayerStates.SuperStates {
    public class PlayerTouchingWallState : PlayerState {
        protected int XInput;
        protected bool JumpInput;
        private bool _isGrounded;
        private bool _isTouchingWall;

        protected PlayerTouchingWallState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            XInput = Player.InputHandler.MovementInput;
            JumpInput = Player.InputHandler.JumpInput;

            if (_isGrounded) {
                StateMachine.ChangeState(Player.IdleState);
            } else if (!_isTouchingWall) {
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        protected override void DoChecks() {
            base.DoChecks();

            _isGrounded = Player.CheckIfGrounded();
            _isTouchingWall = Player.CheckIfTouchingWall();
        }
    }
}