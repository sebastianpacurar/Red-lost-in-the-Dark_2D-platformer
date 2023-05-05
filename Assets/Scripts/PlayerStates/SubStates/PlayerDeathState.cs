using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;

namespace PlayerStates.SubStates {
    public class PlayerDeathState : PlayerAbilityState {
        public PlayerDeathState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            Player.FreezePlayer();
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();
            if (!IsAnimationFinished) return;

            Player.RestartFromCheckpoint();
            StateMachine.ChangeState(Player.InAirState);
        }

        protected internal override void Exit() {
            base.Exit();
            Player.UnFreezePlayer();
        }
    }
}