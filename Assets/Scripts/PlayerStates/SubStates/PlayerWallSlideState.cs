using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;
using UnityEngine;

namespace PlayerStates.SubStates {
    public class PlayerWallSlideState : PlayerTouchingWallState {
        private bool _isAutoClimbOn;

        public PlayerWallSlideState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            Player.FlipSpriteX();
            Player.SetVelocityY(0f);
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            if (_isAutoClimbOn) {
                XInput = 0;
            }

            if (Time.time >= StartTime + Player.WallSlideHangDuration) {
                // if left is pressed while wall sliding on the right side, and the other way around
                if (XInput == -Player.FacingDirection) {
                    Player.CheckIfShouldFlip(XInput);
                    StateMachine.ChangeState(Player.InAirState);
                }

                if (JumpInput) {
                    StateMachine.ChangeState(Player.WallJumpState);
                }
            }
        }

        protected internal override void Exit() {
            base.Exit();

            Player.FlipSpriteX();
        }

        protected override void DoChecks() {
            base.DoChecks();

            _isAutoClimbOn = Player.CheckAutoClimbOn();
        }
    }
}