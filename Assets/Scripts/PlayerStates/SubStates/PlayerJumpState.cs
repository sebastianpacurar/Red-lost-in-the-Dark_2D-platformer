using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;

namespace PlayerStates.SubStates {
    public class PlayerJumpState : PlayerAbilityState {
        public PlayerJumpState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            Player.SetVelocityY(0f);
            Player.AddJumpForce(PlayerData.jumpVelocity);
            IsAbilityDone = true;
        }
    }
}