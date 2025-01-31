using Commons;
using DG.Tweening;
using Patterns;
using System;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Tooltip("Player position's offset from screen boundaries in unity-unit (x for left and right, y for lower)")]
        public Vector2 initOffset;
        public float moveDuration = 1f;
        public Ease movementEase = Ease.InOutCubic;

        [field: SerializeField] public bool CanMove { get; private set; }

        private Tween _moveTween;


        private void OnEnable()
        {
            this.Register(EventID.HitBlock, OnHitBlock);
            SetPlayerOnLeftSide();
            CanMove = true;
        }

        private void OnDisable()
        {
            this.Unregister(EventID.HitBlock, OnHitBlock);
        }

        private void OnHitBlock(object obj)
        {
            CanMove = false;
            if (_moveTween == null) return;
            _moveTween.Kill();
        }

        private void SetPlayerOnLeftSide()
        {
            transform.position = new(-Common.ScreenWidthInUnit / 2 + initOffset.x, -Common.ScreenHeightInUnit / 2 + initOffset.y);
        }

        public void Move()
        {
            if (!CanMove) return;
            CanMove = false;
            Vector2 newPosition = transform.position;
            newPosition.x *= -1;
            _moveTween = transform.DOMove(newPosition, moveDuration)
                .SetEase(movementEase)
                .OnComplete(() =>
                {
                    CanMove = true;
                    this.Broadcast(EventID.PlayerFinishMovement);
                    _moveTween = null;
                });

        }
    }
}
