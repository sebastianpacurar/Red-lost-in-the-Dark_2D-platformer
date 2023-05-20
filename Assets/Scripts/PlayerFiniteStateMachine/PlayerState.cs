using Data;
using UnityEngine;

namespace PlayerFiniteStateMachine {
    // base class for all our states
    public class PlayerState {
        protected readonly PlayerScript Player;
        protected readonly PlayerStateMachine StateMachine;
        protected readonly PlayerData PlayerData;
        protected bool IsAnimationFinished;

        protected float StartTime;
        private readonly string _animBoolName;

        protected PlayerState(PlayerScript player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) {
            Player = player;
            StateMachine = stateMachine;
            PlayerData = playerData;
            _animBoolName = animBoolName;
        }

        protected internal virtual void Enter() {
            DoChecks();
            Player.Anim.SetBool(_animBoolName, true);
            StartTime = Time.time;
            IsAnimationFinished = false;
        }

        protected internal virtual void Exit() {
            Player.Anim.SetBool(_animBoolName, false);
        }

        protected internal virtual void LogicUpdate() { }

        protected internal virtual void PhysicsUpdate() {
            DoChecks();
        }

        protected virtual void DoChecks() { }

        public virtual void AnimationTrigger() { }
        public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;
    }
}