using Commons;
using DG.Tweening;
using Game.LevelDesign;
using Patterns;
using System;
using System.Collections;
using UnityEngine;


namespace Game.JumpDash.LevelController
{
    public class LevelController : MonoBehaviour
    {
        private JumpDash _gameController;
        private Tween _spawnTween;
        private Transform _spawner;
        public float speed;
        public float interval;

        private void Start()
        {
            if (!_spawner)
            {
                _spawner = new GameObject("Spawner").transform;
                _spawner.SetParent(transform);
                _spawner.position = new(0f, Common.ScreenHeightInUnit / 2 + 1f, 0f);
            }
        }

        private void OnEnable()
        {
            this.Register(EventID.InitBlockSpanwer, OnInitGameplay);
            this.Register(EventID.HitBlock, OnHitBlock);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.InitBlockSpanwer, OnInitGameplay);
            this.Unregister(EventID.HitBlock, OnHitBlock);
        }


        private void OnHitBlock(object obj)
        {
            if (_spawner == null) return;
            _spawnTween.Kill();
        }

        private void OnInitGameplay(object obj = null)
        {
            if (_gameController is null)
            {
                if (obj is not JumpDash gameController) return;
                _gameController = gameController;
                speed = _gameController.CurrentLevel.blockSpeed;
                interval = _gameController.CurrentLevel.spawnIntervalInSeconds;
            }
            _spawnTween = DOVirtual.DelayedCall(_gameController.CurrentLevel.spawnIntervalInSeconds, () =>
            {
                if(speed != _gameController.CurrentLevel.blockSpeed)
                {
                    speed = _gameController.CurrentLevel.blockSpeed;
                    interval = _gameController.CurrentLevel.spawnIntervalInSeconds;
                    _spawnTween.Kill();
                    ReinitGamePlay();
                }
                else
                {
                    SpawnOneBlock(_gameController.CurrentLevel.blockSpeed);
                    speed = _gameController.CurrentLevel.blockSpeed;
                    interval = _gameController.CurrentLevel.spawnIntervalInSeconds;
                }


            }, true)
            .SetLoops(-1, LoopType.Restart);
        }

        private void ReinitGamePlay()
        {
            DOTween.Sequence()
                .AppendInterval(.1f)
                .AppendCallback(() =>
                {
                    OnInitGameplay();
                });
        }

        private void SpawnOneBlock(float speed)
        {
            var randomPosition = new Vector3(
                UnityEngine.Random.Range(-Common.ScreenWidthInUnit / 2, Common.ScreenWidthInUnit / 2),
                _spawner.position.y,
                _spawner.position.z);

            var jumpDashConfig = GameManager.Instance.GameConfig.jumpDash;
            int randomLength = UnityEngine.Random.Range(jumpDashConfig.minBlockLength, jumpDashConfig.maxBlockLength);

            var blockObj = ObjectPooling.Instance.GetPool(Constants.STR_BLOCK_TAG)
                                                .Get(position: randomPosition, parent: transform);
            if (blockObj.TryGetComponent<Block>(out var block))
            {
                block.SetBlockLength(randomLength);
                block.SetSpeed(speed);
                blockObj.SetActive(true);
            }
        }

    }
}
