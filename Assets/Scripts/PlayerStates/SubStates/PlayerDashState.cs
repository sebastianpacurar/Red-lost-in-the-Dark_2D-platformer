using Data;
using Player;
using PlayerFiniteStateMachine;
using PlayerStates.SuperStates;
using UnityEngine;

namespace PlayerStates.SubStates {
    public class PlayerDashState : PlayerAbilityState {
        private float _dashXForce;
        private int _dashDirection;
        private Vector2 _lastAfterImgPos;

        public PlayerDashState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        protected internal override void Enter() {
            base.Enter();
            _dashDirection = Player.SetDashDirection();
            _dashXForce = PlayerData.dashSpeed * _dashDirection;

            Player.AddDashForce(_dashXForce);
        }

        protected internal override void LogicUpdate() {
            base.LogicUpdate();
            
            CheckIfShouldPlaceAfterImg();
            Player.SetVelocityY(0);
            
            if (Player.CheckIfTouchingWall()) {
                Player.SetCanDashFalse();
                StateMachine.ChangeState(Player.WallSlideState);
            } else if (Time.time >= StartTime + PlayerData.dashTime) {
                Player.SetCanDashFalse();

                if (Player.CheckIfGrounded()) {
                    StateMachine.ChangeState(Player.IdleState);
                } else {
                    StateMachine.ChangeState(Player.InAirState);
                }
            }
        }

        private void PlaceAfterImage() {
            var img = PlayerAfterImagePool.Instance.GetFromPool();
            var playerTransform = Player.transform;
            img.transform.localScale = playerTransform.localScale;
            _lastAfterImgPos = playerTransform.position;
        }

        private void CheckIfShouldPlaceAfterImg() {
            if (Vector2.Distance(Player.transform.position, _lastAfterImgPos) >= PlayerData.distBetweenAfterImgs) {
                PlaceAfterImage();
            }
        }
    }
}