using Commons;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.KnifeHit.KnifeTarget
{
    public class TargetVFX : MonoBehaviour
    {
        public ParticleSystem reinitVFX;
        public ParticleSystem missVFX;
        public ParticleSystem[] hitVFX;

        private void OnEnable()
        {
            this.Register(EventID.GainPoint, OnGainPoint);
            this.Register(EventID.HitTarget, OnHitTarget);
            this.Register(EventID.HitObstacle, OnHitObstacle);

        }

        private void OnDisable()
        {
            this.Unregister(EventID.GainPoint, OnGainPoint);
            this.Unregister(EventID.HitTarget, OnHitTarget);
            this.Unregister(EventID.HitObstacle, OnHitObstacle);

        }

        private void OnHitObstacle(object obj)
        {
            missVFX?.Play();
        }

        private void OnHitTarget(object obj)
        {
            var vfx = Common.GetRandomItem(hitVFX);
            vfx?.Play();
        }

        private void OnGainPoint(object obj)
        {
            reinitVFX?.Play();
            this.Broadcast(EventID.TargetExplode);
        }
    }

}
