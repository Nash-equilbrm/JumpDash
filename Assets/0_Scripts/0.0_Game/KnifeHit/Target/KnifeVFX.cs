using Commons;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.KnifeHit.KnifeTarget
{
    public class KnifeVFX : MonoBehaviour
    {
        public ParticleSystem[] _impactParticles;
        private void OnEnable()
        {
            this.Register(EventID.HitTarget, OnHitTarget);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.HitTarget, OnHitTarget);
        }

        private void OnHitTarget(object obj)
        {
            if (obj is not Knife knife) return;
            if (knife.gameObject != gameObject) return;
            var vfx = Common.GetRandomItem(_impactParticles);
            vfx.Play();
        }
    }
}

