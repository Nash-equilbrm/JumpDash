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
                this.Register(EventID.HitBlock, OnHitBlock);
            });
        }

        private void OnDisable()
        {
            this.Unregister(EventID.HitBlock, OnHitBlock);
        }

        private void OnHitBlock(object obj)
        {
            ShakeCamera();
        }

        public void ShakeCamera()
        {
            LogUtility.NotificationInfo("Shake Camera");
            transform.DOShakePosition(duration, strength, vibrato, randomness)
                .OnComplete(() => transform.position = originalPos);
        }
    }
}