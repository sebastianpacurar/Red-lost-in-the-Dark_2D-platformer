using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;
using UnityEngine;

namespace PlayerStates.SubStates {
    public class PlayerWallJumpState : PlayerAbilityState {
        private Vector2 _wallJumpDirection;

        public PlayerWallJumpState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            SetWallJumpDirection();
            Player.SetVelocityY(0f);
            Player.AddWallJumpForce(Player.WallJumpForce * _wallJumpDirection);
            Player.CheckIfShouldFlip((int)_wallJumpDirection.x);
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            Player.Anim.SetFloat("yVelocity", Player.CurrentVelocity.y);

            if (Time.time >= StartTime + Player.WallJumpDuration) {
                IsAbilityDone = true;
                StateMachine.ChangeState(Player.InAirState);
            } else if (Player.CheckIfTouchingWall()) {
                IsAbilityDone = true;
                StateMachine.ChangeState(Player.WallSlideState);
            }
        }

        private void SetWallJumpDirection() {
            _wallJumpDirection = new Vector2(-Player.FacingDirection, 1);
        }
    }
}