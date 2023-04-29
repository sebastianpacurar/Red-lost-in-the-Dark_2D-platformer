using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;
using UnityEngine;

namespace PlayerStates.SubStates {
    public class PlayerWallJumpState : PlayerAbilityState {
        private Vector2 _wallJumpDirection;
        private float _abilityDuration; //TODO: this could be removed

        public PlayerWallJumpState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            SetWallJumpDirection();
            _abilityDuration = Player.WallJumpDuration;

            Player.SetVelocityY(0f);
            Player.AddWallJumpForce(Player.WallJumpForce * _wallJumpDirection); //TODO: asta tre inmultit cu FAcingDirection, BOULE!

            Player.CheckIfShouldFlip((int)_wallJumpDirection.x);
            Player.JumpState.DecreaseAmountOfJumpsLeft();
        }

        protected internal override void LogicUpdate() {
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

        protected override void DoChecks() {
            base.DoChecks();

            // _isAutoClimbOn = Player.CheckAutoClimbOn();
        }

        private void SetWallJumpDirection() {
            _wallJumpDirection = new Vector2(-Player.FacingDirection, 1);
        }
    }
}