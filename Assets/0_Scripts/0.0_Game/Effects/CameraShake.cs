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
                this.Register(EventID.HitBlock, Shake);
                this.Register(EventID.PlayerFinishMovement, LightShake);
                this.Register(EventID.HitTarget, LightShake);
                this.Register(EventID.HitObstacle, LightShake);
            });
        }

        private void OnDisable()
        {
            this.Unregister(EventID.HitBlock, Shake);
            this.Unregister(EventID.PlayerFinishMovement, LightShake);
            this.Unregister(EventID.HitTarget, LightShake);
            this.Unregister(EventID.HitObstacle, LightShake);
        }

        private void LightShake(object obj)
        {
            strength = Constants.CAMERA_SHAKE_LIGHT;
            ShakeCamera();
        }

        private void Shake(object obj)
        {
            strength = Constants.CAMERA_SHAKE_MEDIUM;
            ShakeCamera();
        }


        public void ShakeCamera()
        {
            transform.DOShakePosition(duration, strength, vibrato, randomness)
                .OnComplete(() => transform.position = originalPos);
        }
    }
}