using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;
using UnityEngine;

namespace PlayerStates.SubStates {
    public class PlayerWallJumpState : PlayerAbilityState {
        private int _wallJumpDirection;
        private float _abilityDuration;
        private bool _isAutoClimbOn;

        public PlayerWallJumpState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void Enter() {
            base.Enter();

            SetWallJumpDirection();
            SetWallJumpDuration();
            Player.SetVelocityY(0f);
            Player.AddWallJumpForce(GetClimbOrJumpForce());
            Player.CheckIfShouldFlip(_wallJumpDirection);
            Player.JumpState.DecreaseAmountOfJumpsLeft();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            Player.Anim.SetFloat("yVelocity", Player.CurrentVelocity.y);
            
            

            if (Time.time >= StartTime + _abilityDuration) {
                IsAbilityDone = true;
                StateMachine.ChangeState(Player.InAirState);
            } else if (Player.CheckIfTouchingWall()) {
                IsAbilityDone = true;
                StateMachine.ChangeState(Player.WallSlideState);
            }
        }

        public override void DoChecks() {
            base.DoChecks();

            _isAutoClimbOn = Player.CheckAutoClimbOn();
        }

        public void SetWallJumpDirection() {
            _wallJumpDirection = -Player.FacingDirection;
        }

        private Vector2 GetClimbOrJumpForce() {
            var force = PlayerData.wallJumpForce;

            if (Player.HitRight && Player.HitLeft) {
                force = PlayerData.wallClimbForce;
            }

            return new Vector2(force.x * _wallJumpDirection, force.y);
        }

        private void SetWallJumpDuration() {
            _abilityDuration = PlayerData.wallJumpDefaultDuration;

            if (Player.HitRight && Player.HitLeft) {
                _abilityDuration = Player.WallClimbCurrDuration;
            }
        }
    }
}