using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;


namespace PlayerStates.SubStates {
    public class PlayerJumpState : PlayerAbilityState {
        private int _amountOfJumpsLeft;

        public PlayerJumpState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {
            _amountOfJumpsLeft = playerData.amountOfJumps;
        }

        public override void Enter() {
            base.Enter();

            Player.SetVelocityYToZero();
            Player.AddJumpForce(PlayerData.jumpVelocity);
            IsAbilityDone = true;
            _amountOfJumpsLeft--;
        }

        public bool CanJump() {
            return _amountOfJumpsLeft > 0;
        }

        public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = PlayerData.amountOfJumps;

        public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;
    }
}