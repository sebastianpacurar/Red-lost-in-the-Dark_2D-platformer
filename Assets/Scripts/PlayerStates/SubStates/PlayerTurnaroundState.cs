using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;

namespace PlayerStates.SubStates {
    public class PlayerTurnaroundState : PlayerGroundedState {
        public PlayerTurnaroundState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();

            Player.FlipSpriteX();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (!IsAnimationFinished) {
                Player.SetVelocityX(0f);
            } else if (IsAnimationFinished) {
                Player.FlipScale();
                StateMachine.ChangeState(XInput != 0 ? Player.MoveState : Player.IdleState);
            }
        }

        public override void Exit() {
            base.Exit();

            Player.FlipSpriteX();
        }
    }
}