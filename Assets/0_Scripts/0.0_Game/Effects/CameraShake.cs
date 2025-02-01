using Commons;
using DG.Tweening;
using Patterns;
using UnityEngine;


namespace Game.Effect
{
    public class CameraShake : MonoBehaviour
    {
        public float duration = 0.5f;
        public float strength = 1f;
        public int vibrato = 10;
        public float randomness = 90f;

        private Vector3 originalPos;

        void Start()
        {
            originalPos = transform.position;
        }

        private void OnEnable()
        {
            PubSub.WaitForInstance(this, () =>
            {
                this.Register(EventID.HitBlock, HeavyShake);
                this.Register(EventID.PlayerFinishMovement, LightShake);
            });
        }

        private void OnDisable()
        {
            this.Unregister(EventID.HitBlock, HeavyShake);
            this.Unregister(EventID.PlayerFinishMovement, LightShake);
        }

        private void LightShake(object obj)
        {
            strength = 0.2f;
            ShakeCamera();
        }

        private void HeavyShake(object obj)
        {
            strength = 0.5f;
            ShakeCamera();
        }


        public void ShakeCamera()
        {
            transform.DOShakePosition(duration, strength, vibrato, randomness)
                .OnComplete(() => transform.position = originalPos);
        }
    }
}