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

            if (XInput == 0f) {
                StateMachine.ChangeState(Player.IdleState);
            } else if (!Player.CheckIfFacingInputDirection(XInput)) {
                StateMachine.ChangeState(Player.TurnaroundState);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();

            Player.SetVelocityX(PlayerData.movementVelocity * XInput);
        }

        public override void DoChecks() {
            base.DoChecks();
        }
    }
}