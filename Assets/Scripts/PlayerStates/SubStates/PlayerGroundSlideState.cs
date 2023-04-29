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
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();

            Player.SetVelocityX(PlayerData.movementVelocity * Player.FacingDirection);

            if (Time.time >= StartTime + PlayerData.groundSlideMinTime) {
                if (JumpInput) {
                    StateMachine.ChangeState(Player.JumpState);
                }

                if (!GroundSlideInput) {
                    if (XInput != 0) {
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