using Commons;
using Patterns;
using UnityEngine;


namespace Game.JumpDash.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCollision : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Constants.STR_ENEMY_TAG))
            {
                LogUtility.ValidInfo("OnCollisionEnter2D", $"Hit enemy");
                this.Broadcast(EventID.HitBlock);
            }
        }
    }
}
