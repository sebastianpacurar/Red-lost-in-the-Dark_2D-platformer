using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;

namespace PlayerStates.SubStates {
    public class PlayerLandState : PlayerGroundedState {
        public PlayerLandState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (XInput != 0) {
                StateMachine.ChangeState(Player.MoveState);
            } else if (IsAnimationFinished) {
                StateMachine.ChangeState(Player.IdleState);
            }
        }
    }
}