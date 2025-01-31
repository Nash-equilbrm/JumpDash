using Commons;
using DG.Tweening;
using Game.LevelDesign;
using Patterns;
using System;
using System.Collections;
using UnityEngine;


namespace Game.LevelController
{
    public class LevelController : MonoBehaviour
    {
        private Tween _spawnTween;
        private Transform _spawner;

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
            this.Register(EventID.InitGamePlay, OnInitGameplay);
            this.Register(EventID.HitBlock, OnHitBlock);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.InitGamePlay, OnInitGameplay);
            this.Unregister(EventID.HitBlock, OnHitBlock);
        }

        private void OnHitBlock(object obj)
        {
            if (_spawner == null) return;
            _spawnTween.Kill();
        }

        private void OnInitGameplay(object obj)
        {
            _spawnTween = DOVirtual.DelayedCall(GameManager.Instance.LevelDesignConfig.spawnIntervalInSeconds, () =>
            {
                SpawnOneBlock();
            }, true)
            .SetLoops(-1, LoopType.Restart);
        }

        private void SpawnOneBlock()
        {
            var randomPosition = new Vector3(
                UnityEngine.Random.Range(-Common.ScreenWidthInUnit / 2, Common.ScreenWidthInUnit / 2),
                _spawner.position.y,
                _spawner.position.z);

            var levelConfig = GameManager.Instance.LevelDesignConfig;
            int randomLength = UnityEngine.Random.Range(levelConfig.minBlockLength, levelConfig.maxBlockLength);

            var blockObj = ObjectPooling.Instance.GetPool(Strings.Blocks)
                                                .Get(position: randomPosition, parent: GameManager.Instance.World.transform);
            if (blockObj.TryGetComponent<Block>(out var block))
            {
                block.SetBlockLength(randomLength);
                block.SetSpeed(GameManager.Instance.LevelDesignConfig.initialBlockSpeed);
                blockObj.SetActive(true);
            }
        }
    }
}
