using Data;
using PlayerFiniteStateMachine;

namespace PlayerStates.SuperStates {
    public class PlayerTouchingWallState : PlayerState {
        protected bool IsGrounded;
        protected bool IsTouchingWall;
        protected int XInput;
        protected bool JumpInput;

        public PlayerTouchingWallState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();
        }

        public override void Exit() {
            base.Exit();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            XInput = Player.InputHandler.MovementInput;
            JumpInput = Player.InputHandler.JumpInput;

            if (IsGrounded) {
                StateMachine.ChangeState(Player.IdleState);
            } else if (!IsTouchingWall) {
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
        }

        public override void DoChecks() {
            base.DoChecks();

            IsGrounded = Player.CheckIfGrounded();
            IsTouchingWall = Player.CheckIfTouchingWall();
        }

        public override void AnimationTrigger() {
            base.AnimationTrigger();
        }

        public override void AnimationFinishTrigger() {
            base.AnimationFinishTrigger();
        }
    }
}