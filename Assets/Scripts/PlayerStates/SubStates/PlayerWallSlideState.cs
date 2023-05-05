using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;
using UnityEngine;

namespace PlayerStates.SubStates {
    public class PlayerWallSlideState : PlayerTouchingWallState {
        private bool _isAutoClimbOn;
        private bool _isAutoWallJumpOn;

        public PlayerWallSlideState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            Player.FlipSpriteX();
            Player.SetVelocityY(0f);
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();
            
            // needed to prevent XInput from interfering with the wallJump Force:
            // NOTE:  in case Left (towards left wall) OR Right (towards right wall) AND Jump is pressed
            _isAutoWallJumpOn = Player.CheckIfAutoWallJumpOn();

            if (_isAutoClimbOn || _isAutoWallJumpOn) {
                XInput = 0;
                Player.SetVelocityY(0f);
            }

            if (Time.time <= StartTime + Player.WallSlideHangDuration) return;
            
            // perform one side wall jump
            if (_isAutoWallJumpOn || JumpInput) {
                StateMachine.ChangeState(Player.WallJumpState);
            }
            
            // if left is pressed while wall sliding on the right side, and the other way around
            if (XInput == -Player.FacingDirection) {
                Player.CheckIfShouldFlip(XInput);
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        protected internal override void Exit() {
            base.Exit();

            Player.FlipSpriteX();
        }

        protected override void DoChecks() {
            base.DoChecks();

            _isAutoClimbOn = Player.CheckIfAutoClimbOn();
        }
    }
}