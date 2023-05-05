using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;

namespace PlayerStates.SubStates {
    public class PlayerFallDamageState : PlayerAbilityState {
        public PlayerFallDamageState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            Player.FreezePlayer();
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (!IsAnimationFinished) return;
            Player.HealthPoints -= PlayerData.fallDamage;

            if (Player.CheckIfDead()) {
                StateMachine.ChangeState(Player.DeathState);
            } else {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        protected internal override void Exit() {
            base.Exit();
            Player.UnFreezePlayer();
        }
    }
}