using Data;
using PlayerFiniteStateMachine;

namespace PlayerStates.SuperStates {
    public class PlayerGroundedState : PlayerState {
        protected int XInput;
        protected bool GroundSlideInput;
        protected bool JumpInput;
        protected bool DashInput;
        private bool _isGrounded;

        protected PlayerGroundedState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            XInput = Player.InputHandler.MovementInput;
            GroundSlideInput = Player.InputHandler.GroundSlideInput;
            JumpInput = Player.InputHandler.JumpInput;
            DashInput = Player.InputHandler.DashInput;

            if (JumpInput) {
                StateMachine.ChangeState(Player.JumpState);
            } else if (!_isGrounded) {
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        protected override void DoChecks() {
            base.DoChecks();
            _isGrounded = Player.CheckIfGrounded();
        }
    }
}