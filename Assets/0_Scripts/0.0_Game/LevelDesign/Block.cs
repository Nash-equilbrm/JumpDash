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
            gameObject.tag = Strings.EnemyTag;
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.tag = Strings.EnemyTag;
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
            length = Mathf.Clamp(length,
                GameManager.Instance.LevelDesignConfig.minBlockLength,
                GameManager.Instance.LevelDesignConfig.maxBlockLength);
            var newScale = _spriteTransform.localScale;
            newScale.x = length;
            _spriteTransform.localScale = newScale;
        }



        public void SetSpeed(float value)
        {
            value = Mathf.Clamp(value, 0f, GameManager.Instance.LevelDesignConfig.maxBlockSpeed);

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
