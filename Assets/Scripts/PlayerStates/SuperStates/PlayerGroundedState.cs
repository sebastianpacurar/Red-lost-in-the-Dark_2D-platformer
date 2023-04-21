using Data;
using PlayerFiniteStateMachine;

namespace PlayerStates.SuperStates {
    public class PlayerGroundedState : PlayerState {
        protected int XInput;
        private bool _jumpInput;
        private bool _isGrounded;

        public PlayerGroundedState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();
            Player.JumpState.ResetAmountOfJumpsLeft();
        }

        public override void Exit() {
            base.Exit();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            XInput = Player.InputHandler.MovementInput;
            _jumpInput = Player.InputHandler.JumpInput;

            if (_jumpInput && Player.JumpState.CanJump()) {
                Player.InputHandler.SetJumpInputFalse();
                StateMachine.ChangeState(Player.JumpState);
            } else if (!_isGrounded) {
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
        }

        public override void DoChecks() {
            base.DoChecks();
            _isGrounded = Player.CheckIfGrounded();
        }
    }
}