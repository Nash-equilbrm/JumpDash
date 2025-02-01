using Commons;
using DG.Tweening;
using Patterns;
using System;
using UnityEngine;


namespace Game.LevelDesign
{
    public class Block : MonoBehaviour
    {
        public Transform _spriteTransform;
        [SerializeField] private float _speed;
        private Tween _movementTween;


        private void Awake()
        {
            gameObject.tag = Constants.STR_ENEMY_TAG;
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.tag = Constants.STR_ENEMY_TAG;
            }
        }

        private void OnEnable()
        {
            this.Register(EventID.HitBlock, OnHitBlock);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.HitBlock, OnHitBlock);
        }

        private void OnHitBlock(object obj)
        {
            SetSpeed(0f);
        }

        public void SetBlockLength(int length)
        {
            var jumpDashConfig = GameManager.Instance.GameConfig.jumpDash;
            length = Mathf.Clamp(length,
                jumpDashConfig.minBlockLength,
                jumpDashConfig.maxBlockLength);
            var newScale = _spriteTransform.localScale;
            newScale.x = length;
            _spriteTransform.localScale = newScale;
        }



        public void SetSpeed(float value)
        {
            value = value > 0f ? value : 0f;

            _movementTween?.Kill();

            if (value == 0) return;

            _speed = value;
            Vector3 dest = new(transform.position.x, -Common.ScreenHeightInUnit / 2 - 1, transform.position.z);
            float distance = transform.position.y - dest.y;
            _movementTween = transform.DOMove(dest, distance / _speed).SetEase(Ease.Linear).OnComplete(OnReachDestination);
        }


        private void OnReachDestination()
        {
            ObjectPooling.Remove(gameObject);
        }
    }
}
