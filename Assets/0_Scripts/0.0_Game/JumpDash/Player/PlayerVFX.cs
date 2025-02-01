using DG.Tweening;
using Patterns;
using System;
using UnityEngine;

namespace Game.JumpDash.Player
{
    public class PlayerVFX : MonoBehaviour
    {
        public ParticleSystem _jumpVFX;
        public ParticleSystem _landingVFX;
        public ParticleSystem _hitBlockVFX;


        private void OnEnable()
        {
            this.Register(EventID.PlayerFinishMovement, OnPlayerFinishMovement);
            this.Register(EventID.HitBlock, OnHitBlock);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.PlayerFinishMovement, OnPlayerFinishMovement);
            this.Unregister(EventID.HitBlock, OnHitBlock);
        }

        private void OnHitBlock(object obj)
        {
            _hitBlockVFX?.Play();
        }

        public void OnPlayerMove()
        {
            _jumpVFX?.Play();
        }

        private void OnPlayerFinishMovement(object obj)
        {
            _landingVFX?.Play();
        }
    }
}
