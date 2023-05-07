using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;

namespace PlayerStates.SubStates {
    public class PlayerIdleState : PlayerGroundedState {
        public PlayerIdleState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        private bool _sameDirAsInput;

        protected internal override void Enter() {
            base.Enter();
            Player.SetVelocityX(0f);
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (XInput != 0f && Player.CheckIfFacingInputDirection(XInput)) {
                StateMachine.ChangeState(Player.MoveState);
            } else if (XInput != 0f && !Player.CheckIfFacingInputDirection(XInput)) {
                StateMachine.ChangeState(Player.TurnaroundState);
            } else if (DashInput && Player.CanDash) {
                StateMachine.ChangeState(Player.DashState);
            }
        }
    }
}