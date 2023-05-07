using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;

namespace PlayerStates.SubStates {
    public class PlayerMoveState : PlayerGroundedState {
        public PlayerMoveState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (XInput == 0f) {
                StateMachine.ChangeState(Player.IdleState);
            } else if (!Player.CheckIfFacingInputDirection(XInput)) {
                StateMachine.ChangeState(Player.TurnaroundState);
            } else if (GroundSlideInput) {
                StateMachine.ChangeState(Player.GroundSlideState);
            } else if (DashInput && Player.CanDash) {
                StateMachine.ChangeState(Player.DashState);
            }
        }

        protected internal override void PhysicsUpdate() {
            base.PhysicsUpdate();

            Player.SetVelocityX(PlayerData.movementVelocity * XInput);
        }
    }
}