using Commons;
using DG.Tweening;
using Patterns;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.LightAnchor;
using static UnityEngine.UI.Image;


namespace Game.KnifeHit.KnifeTarget
{
    public class Target : MonoBehaviour
    {
        [Tooltip("Set knife transform at this position before set it as Target's children")]
        public Vector3 knifeHitPosition;
        public Transform rotator;
        public Transform placeKnifePosition;

        private float _lerpSpeed = 2f;
        private float _targetSpinSpeed;
        private float _spinSpeed;

        private KnifeHit _gameController;

        private Tween _setSpeedTween;


        private void Awake()
        {
            _lerpSpeed = Constants.KNIFE_HIT_TARGET_SPIN_LERP_SPEED;
            SetNewTargetValue();
            placeKnifePosition.localPosition = knifeHitPosition;
        }


        private void OnEnable()
        {
            this.Register(EventID.InitTarget, InitTarget);
            this.Register(EventID.GainPoint, OnReinitTarget);
            this.Register(EventID.HitTarget, OnHitTarget);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.InitTarget, InitTarget);
            this.Unregister(EventID.GainPoint, OnReinitTarget);
            this.Unregister(EventID.HitTarget, OnHitTarget);
        }


        void Update()
        {
            transform.Rotate(0, 0, _spinSpeed * Time.deltaTime);
            _spinSpeed = Mathf.Lerp(_spinSpeed, _targetSpinSpeed, Time.deltaTime * _lerpSpeed);
        }

        private void SetNewTargetSpeed()
        {
            var changeTargetSpeedEverySeconds = _gameController == null 
                ? GameManager.Instance.GameConfig.knifeHit.levels[0].changeTargetSpeedEverySeconds
                : _gameController.CurrentLevel.changeTargetSpeedEverySeconds;
            if (changeTargetSpeedEverySeconds == 0)
            {
                _targetSpinSpeed = _gameController.CurrentLevel.targetSpeedRange[0];
                return;
            }

            _setSpeedTween?.Kill();
            _setSpeedTween = StartRepeatingAction(() =>
            {
                if(Mathf.Abs(_targetSpinSpeed - _spinSpeed) < 10f)
                {
                    SetNewTargetValue();
                }
            }, changeTargetSpeedEverySeconds);
        }

        private Tween StartRepeatingAction(Action callback, float t)
        {
            return DOVirtual.DelayedCall(t, () =>
            {
                callback?.Invoke();
            }).SetLoops(-1, LoopType.Restart);
        }


        private void InitTarget(object obj)
        {
            if(_gameController is null)
            {
                if (obj is not KnifeHit gameController) return;
                _gameController = gameController;
            }

            SetObstacles(_gameController.CurrentLevel.obstacle);
            SetNewTargetSpeed();
        }


        private void SetObstacles(int[] obstacleRange)
        {
            var obstacleCount = UnityEngine.Random.Range(obstacleRange[0], obstacleRange[^1]);
            LogUtility.NotificationInfo("SetObstacles", obstacleCount.ToString());
            Vector2[] randomPoses = new Vector2[obstacleCount];
            for (int i = 0; i < obstacleCount; i++)
            {
                Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized; // Random unit direction
                Vector2 randomPos;
                do
                {
                    randomPos = new Vector2(transform.position.x, transform.position.y) + randomDirection * Mathf.Abs(knifeHitPosition.y);
                }
                while (randomPoses.Contains(randomPos));
                LogUtility.Info("New random pos", randomPos.ToString());
                randomPoses[i] = randomPos;
                var knifeObj = ObjectPooling.Instance.GetPool(Constants.STR_KNIFE_TAG).Find((g) =>

                    g.GetComponent<Knife>().Status == Knife.KnifeStatus.Idle
                );
                if(knifeObj.TryGetComponent<Knife>(out var k))
                {
                    k.Status = Knife.KnifeStatus.Obstacle;
                    knifeObj.transform.localPosition = randomPos;
   
                    knifeObj.gameObject.SetActive(true);

                    var randomRotationZ = UnityEngine.Random.Range(0f, 360f);
                    rotator.localRotation = Quaternion.Euler(0f, 0f, randomRotationZ);
                    knifeObj.transform.SetParent(placeKnifePosition, false);

                    knifeObj.transform.localPosition = Vector3.zero;
                    knifeObj.transform.localRotation = Quaternion.identity;
                    knifeObj.transform.SetParent(transform, true);

                }
            }
        }

        private void SetNewTargetValue()
        {
            if (_gameController != null)
            {
                _targetSpinSpeed = Common.GetRandomItem(_gameController.CurrentLevel.targetSpeedRange);
            }
            else
            {
                _targetSpinSpeed = GameManager.Instance.GameConfig.knifeHit.levels[0].targetSpeedRange[0];
            }
            LogUtility.Info("SetTargetValue", _targetSpinSpeed.ToString());
        }

        private void OnReinitTarget(object obj)
        {
            ObjectPooling.Instance.GetPool(Constants.STR_KNIFE_TAG).RecycleAll();
            InitTarget(obj);
        }

        private void OnHitTarget(object obj)
        {
            if (obj is not Knife knife) return;
            knife.transform.position = transform.position + knifeHitPosition;
            knife.transform.SetParent(transform);
        }
    }

}
