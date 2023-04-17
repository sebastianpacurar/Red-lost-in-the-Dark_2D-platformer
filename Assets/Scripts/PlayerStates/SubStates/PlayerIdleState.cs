using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;

namespace PlayerStates.SubStates {
    public class PlayerIdleState : PlayerGroundedState {
        public PlayerIdleState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();
            Player.SetVelocityX(0f);
        }

        public override void Exit() {
            base.Exit();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (XInput != 0f) {
                StateMachine.ChangeState(Player.MoveState);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
        }

        public override void DoChecks() {
            base.DoChecks();
        }
    }
}