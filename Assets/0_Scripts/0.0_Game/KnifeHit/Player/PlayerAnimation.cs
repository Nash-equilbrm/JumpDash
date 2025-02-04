using Commons;
using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.KnifeHit.Player 
{
    public class PlayerAnimation : MonoBehaviour
    {
        public Animator animator;

        public void OnLaunchKnife()
        {
            animator.Play(Common.GetRandomItem(new string[] { "NinjaThrow", "NinjaJumpThrow" }));
        }
    }

}
