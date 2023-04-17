using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;

namespace PlayerStates.SubStates {
    public class PlayerMoveState : PlayerGroundedState {
        public PlayerMoveState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();
        }

        public override void Exit() {
            base.Exit();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            
            Player.CheckIfShouldFlip(XInput);
            Player.SetVelocityX(PlayerData.movementVelocity * XInput);

            if (XInput == 0f) {
                StateMachine.ChangeState(Player.IdleState);
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