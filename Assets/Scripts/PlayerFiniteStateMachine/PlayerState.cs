using Data;
using UnityEngine;

namespace PlayerFiniteStateMachine {
    // base class for all our states
    public class PlayerState {
        protected PlayerScript Player;
        protected PlayerStateMachine StateMachine;
        protected PlayerData PlayerData;
        protected bool IsAnimationFinished;

        protected float StartTime;
        private string _animBoolName;

        public PlayerState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) {
            Player = player;
            StateMachine = stateMachine;
            PlayerData = playerData;
            _animBoolName = animBoolName;
        }

        public virtual void Enter() {
            DoChecks();
            Player.Anim.SetBool(_animBoolName, true);
            StartTime = Time.time;
            IsAnimationFinished = false;
        }

        public virtual void Exit() {
            Player.Anim.SetBool(_animBoolName, false);
        }

        public virtual void LogicUpdate() { }

        public virtual void PhysicsUpdate() {
            DoChecks();
        }

        public virtual void DoChecks() { }

        public virtual void AnimationTrigger() { }

        public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;
    }
}