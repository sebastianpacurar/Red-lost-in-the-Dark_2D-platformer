using Data;
using PlayerFiniteStateMachine;

namespace PlayerStates.SubStates {
    public class PlayerInAirState : PlayerState {
        private int _xInput;
        private bool _isGrounded;
        private bool _jumpInput;

        public PlayerInAirState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();
        }

        public override void Exit() {
            base.Exit();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            _xInput = Player.InputHandler.MovementInput;
            _jumpInput = Player.InputHandler.JumpInput;

            if (_isGrounded && Player.CurrentVelocity.y < 0.01f) {
                StateMachine.ChangeState(Player.LandState);
            } else if (_jumpInput && Player.JumpState.CanJump()) {
                Player.InputHandler.SetJumpInputFalse();
                StateMachine.ChangeState(Player.JumpState);
            } else {
                Player.CheckIfShouldFlip(_xInput);
                Player.SetVelocityX(PlayerData.movementVelocity * _xInput);
                Player.Anim.SetFloat("yVelocity", Player.CurrentVelocity.y);
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