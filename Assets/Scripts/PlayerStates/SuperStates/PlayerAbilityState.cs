using Data;
using PlayerFiniteStateMachine;

namespace PlayerStates.SuperStates {
    public class PlayerAbilityState : PlayerState {
        protected bool IsAbilityDone;
        private bool _isGrounded;

        public PlayerAbilityState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();
            IsAbilityDone = false;
        }

        public override void Exit() {
            base.Exit();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (IsAbilityDone) {
                if (_isGrounded && Player.CurrentVelocity.y < 0.01f) {
                    StateMachine.ChangeState(Player.IdleState);
                } else {
                    StateMachine.ChangeState(Player.InAirState);
                }
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