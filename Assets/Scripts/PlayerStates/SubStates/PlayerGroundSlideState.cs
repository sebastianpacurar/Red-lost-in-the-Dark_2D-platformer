using Data;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;
using UnityEngine;

namespace PlayerStates.SubStates {
    public class PlayerGroundSlideState : PlayerGroundedState {
        public PlayerGroundSlideState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();

            Player.FlipSpriteX();
            Player.SetCapsuleToGroundSlide();
        }

        // TODO: fix the branching
        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            Player.SetVelocityX(PlayerData.groundSlideVelocity * Player.FacingDirection);

            if (Time.time >= StartTime + PlayerData.groundSlideMinTime) {
                if (JumpInput) {
                    Player.SetCapsuleToNormal();

                    if (Player.CheckIfShouldDieFromNoRoom()) {
                        Player.HealthPoints = 0f;
                    } else {
                        StateMachine.ChangeState(Player.JumpState);
                    }
                }

                if (!GroundSlideInput) {
                    Player.SetCapsuleToNormal();

                    if (Player.CheckIfShouldDieFromNoRoom()) {
                        Player.HealthPoints = 0f;
                    } else if (XInput != 0) {
                        StateMachine.ChangeState(Player.IdleState);
                    } else {
                        StateMachine.ChangeState(Player.MoveState);
                    }
                }
            }
        }

        protected internal override void Exit() {
            base.Exit();

            Player.FlipSpriteX();
        }
    }
}