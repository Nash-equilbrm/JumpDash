using Commons;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.KnifeHit.KnifeTarget
{
    public class Target : MonoBehaviour
    {
        public float lerpSpeed = 2f;

        private float _targetSpinSpeed;
        private float _spinSpeed;

        private KnifeHit _gameController;


        private void Awake()
        {
            SetNewTargetValue();
        }

        void Update()
        {
            transform.Rotate(0, 0, _spinSpeed * Time.deltaTime);

            // Smoothly interpolate towards the target value
            _spinSpeed = Mathf.Lerp(_spinSpeed, _targetSpinSpeed, Time.deltaTime * lerpSpeed);

            // If close enough, pick a new target
            if (Mathf.Abs(_spinSpeed - _targetSpinSpeed) < 0.1f)
            {
                SetNewTargetValue();
            }
        }


        private void SetNewTargetValue()
        {
            if (_gameController != null) 
            {
                _targetSpinSpeed = UnityEngine.Random.Range(_gameController.CurrentLevel.targetSpeedRange[0]
                    , _gameController.CurrentLevel.targetSpeedRange[^1]);
            }
            else
            {
                _targetSpinSpeed = GameManager.Instance.GameConfig.knifeHit.levels[0].targetSpeedRange[0];
            }
        }

        private void OnEnable()
        {
            this.Register(EventID.InitTarget, InitTarget);
            this.Register(EventID.ReinitTarget, OnReinitTarget);
            this.Register(EventID.HitTarget, OnHitTarget);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.InitTarget, InitTarget);
            this.Unregister(EventID.ReinitTarget, OnReinitTarget);
            this.Unregister(EventID.HitTarget, OnHitTarget);
        }

        private void InitTarget(object obj)
        {
            if(_gameController is null)
            {
                if (obj is not KnifeHit gameController) return;
                _gameController = gameController;
            }


        }

        private void OnReinitTarget(object obj)
        {
            ObjectPooling.Instance.GetPool(Constants.STR_KNIFE_TAG).RecycleAll();
            InitTarget(obj);
        }

        private void OnHitTarget(object obj)
        {
            if (obj is not Knife knife) return;
            knife.transform.SetParent(transform);
        }
    }

}
