using Commons;
using DG.Tweening;
using Game;
using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Knife : MonoBehaviour
{
    public bool Hit { get; private set; } = false;
    public Ease launchEase;

    private Tween _launchTween;


    private void OnDisable()
    {
        Hit = false;
    }


    public void Launch(Transform target)
    {
        if (target == null) return;
        _launchTween = transform.DOMove(target.position, GameManager.Instance.GameConfig.knifeHit.knifeLaunchDuration)
            .SetEase(launchEase);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.STR_KNIFE_TAG))
        {
            LogUtility.ValidInfo("OnCollisionEnter2D", $"Hit knife");
            _launchTween.Kill();
            this.Broadcast(EventID.HitObstacle, this);
        }
        else if (collision.gameObject.CompareTag(Constants.STR_TARGET_TAG))
        {
            LogUtility.ValidInfo("OnCollisionEnter2D", $"Hit Target");
            _launchTween.Kill();
            Hit = true;
            this.Broadcast(EventID.HitTarget, this);
        }
        else if (collision.gameObject.CompareTag(Constants.STR_BONUS_TAG))
        {
            LogUtility.ValidInfo("OnCollisionEnter2D", $"Hit Bonus");
        }

    }
}
