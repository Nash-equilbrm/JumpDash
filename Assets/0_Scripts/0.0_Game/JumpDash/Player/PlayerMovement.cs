using Commons;
using DG.Tweening;
using Patterns;
using System;
using UnityEngine;

namespace Game.JumpDash.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Tooltip("Player position's offset from screen boundaries in unity-unit (x for left and right, y for lower)")]
        public Vector2 initOffset;
        public Ease movementEase = Ease.InOutCubic;

        [field: SerializeField] public bool CanMove { get; private set; }



        private Tween _moveTween;
        private Tween _scaleTween;


        private void OnEnable()
        {
            this.Register(EventID.HitBlock, OnHitBlock);
            this.Register(EventID.OnContinueClicked, OnContinueClicked);
            SetPlayerOnRightSide();
            CanMove = true;
        }

        private void OnDisable()
        {
            this.Unregister(EventID.HitBlock, OnHitBlock);
            this.Unregister(EventID.OnContinueClicked, OnContinueClicked);
        }

        private void OnContinueClicked(object obj)
        {
            SetPlayerOnRightSide();
            CanMove = true;
        }

        private void OnHitBlock(object obj)
        {
            CanMove = false;
            if (_moveTween != null) { 
               _moveTween.Kill();
            }
        }

        private void SetPlayerOnRightSide()
        {
            transform.position = new(Common.ScreenWidthInUnit / 2 - initOffset.x, -Common.ScreenHeightInUnit / 2 + initOffset.y);
            transform.localScale = Vector3.one;
        }

        public void Move()
        {
            if (!CanMove) return;
            CanMove = false;
            Vector2 newPosition = transform.position;
            newPosition.x *= -1;

            _moveTween = transform.DOMove(newPosition, GameManager.Instance.GameConfig.jumpDash.playerJumpDuration)
                .SetEase(movementEase)
                .OnComplete(() =>
                {
                    CanMove = true;
                    this.Broadcast(EventID.PlayerFinishMovement);
                    _moveTween = null;
                });

            var newScale = transform.localScale;
            newScale.x *= -1;
            _scaleTween = transform.DOScale(newScale, GameManager.Instance.GameConfig.jumpDash.playerJumpDuration)
                .SetEase(movementEase)
                .OnComplete(() => {
                    transform.localScale = newScale;
                    _scaleTween = null;
                });

        }
    }
}
