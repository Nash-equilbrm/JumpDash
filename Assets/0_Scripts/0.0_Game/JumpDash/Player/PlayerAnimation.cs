using UnityEngine;
using Commons;
using Patterns;
using System;
using DG.Tweening;

namespace Game.JumpDash.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        public Ease movementEase = Ease.InOutCubic;

        public Animator animator;


        private void Start()
        {
            animator.Play(Constants.JUMP_DASH_PLAYER_RUN_ANIM);
        }

        private void OnEnable()
        {
            this.Register(EventID.PlayerFinishMovement, OnPlayerFinishMovement);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.PlayerFinishMovement, OnPlayerFinishMovement);
        }

        private void OnPlayerFinishMovement(object obj)
        {
            animator.Play(Constants.JUMP_DASH_PLAYER_RUN_ANIM);
        }

        public void OnPlayerMove()
        {
            animator.Play(Constants.JUMP_DASH_PLAYER_JUMP_ANIM);
        }
    }
}

