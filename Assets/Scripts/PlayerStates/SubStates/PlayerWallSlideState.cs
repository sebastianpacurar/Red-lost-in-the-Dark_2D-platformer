using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;
using UnityEngine;

namespace PlayerStates.SubStates {
    public class PlayerWallSlideState : PlayerTouchingWallState {
        private bool _isAutoClimbOn;

        public PlayerWallSlideState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();

            Player.FlipSpriteX();
            Player.SetVelocityY(0f);
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            if (Player.CurrentVelocity.y > 0f) Player.SetVelocityX(0f);

            if (XInput == -Player.FacingDirection) {
                Player.CheckIfShouldFlip(XInput);
                StateMachine.ChangeState(Player.InAirState);
            }

            if (JumpInput && !_isAutoClimbOn) {
                StateMachine.ChangeState(Player.WallJumpState);
            } else if (_isAutoClimbOn && JumpInput) {
                XInput = 0;

                //TODO: handle 0.2f dynamically
                if (Time.time >= StartTime + 0.2f) {
                    StateMachine.ChangeState(Player.WallJumpState);
                }
            }
        }

        public override void Exit() {
            base.Exit();

            Player.FlipSpriteX();
        }

        public override void DoChecks() {
            base.DoChecks();

            _isAutoClimbOn = Player.CheckAutoClimbOn();
        }
    }
}