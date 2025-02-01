using Commons;
using Game.KnifeHit.KnifeTarget;
using Patterns;
using UnityEngine;

namespace Game.KnifeHit.Player
{
    public class PlayerLaunchKnife : MonoBehaviour
    {
        public Target Target {  get; private set; }
        public Transform spawner;

        private void Awake()
        {
            Target = FindObjectOfType<Target>();
        }

        public void OnPlayerLaunch()
        {
            if (Target == null) return;

            var knife = ObjectPooling.Instance.GetPool(Constants.STR_KNIFE_TAG)
                //.Get(spawner.position, parent: transform.parent);
                .Find(gameObject => !gameObject.GetComponent<Knife>().Hit);
            knife.transform.position = spawner.position;
            knife.transform.parent = transform.parent;
            knife.SetActive(true);
            knife.GetComponent<Knife>().Launch(Target.transform);
        }
    }
}

