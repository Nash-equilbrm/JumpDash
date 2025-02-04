using Commons;
using Game.KnifeHit.KnifeTarget;
using Patterns;
using System;
using UnityEngine;

namespace Game.KnifeHit.Player
{
    public class PlayerLaunchKnife : MonoBehaviour
    {
        public Target Target {  get; private set; }
        public bool CanLaunch { get; internal set; } = true;

        public Transform spawner;

        private void Awake()
        {
            Target = FindObjectOfType<Target>();
        }

        private void OnEnable()
        {
            this.Register(EventID.HitObstacle, OnHitObstacle);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.HitObstacle, OnHitObstacle);
        }

        private void OnHitObstacle(object obj)
        {
            CanLaunch = false;
        }

        public void OnPlayerLaunch()
        {
            if (!CanLaunch || Target == null) return;
            var pool = ObjectPooling.Instance.GetPool(Constants.STR_KNIFE_TAG);
            var knife = pool
                //.Get(spawner.position, parent: transform.parent);
                .Find(gameObject => gameObject.GetComponent<Knife>().Status == Knife.KnifeStatus.Idle);
            if( knife == null)
            {
                knife = pool.Get(spawner.position, parent: transform.parent);
            }
            knife.transform.SetPositionAndRotation(spawner.position, Quaternion.identity);
            knife.transform.parent = transform.parent;
            knife.SetActive(true);
            knife.GetComponent<Knife>().Launch(Target.transform);
            this.Broadcast(EventID.PlayerFinishLaunch);
        }
    }
}

