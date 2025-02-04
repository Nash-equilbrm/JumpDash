using Commons;
using DG.Tweening;
using Game;
using Patterns;
using UnityEngine;


public class Knife : MonoBehaviour
{
    public enum KnifeStatus
    {
        Idle,
        Launching,
        Hit,
        Missed,
        Obstacle
    }
    public Collider2D collider2d;
    public KnifeStatus Status { get; set; } = KnifeStatus.Idle;
    public Ease launchEase;
    private Tween _launchTween;
    private float _fallDuration = 1.2f;
    private Transform _target;

    private void OnEnable()
    {
        collider2d.enabled = true;
    }

    private void OnDisable()
    {
        Status = KnifeStatus.Idle;
    }


    public void Launch(Transform target)
    {
        if (target == null) return;
        _target = target;
        Status = KnifeStatus.Launching;
        _launchTween = transform.DOMove(target.position, GameManager.Instance.GameConfig.knifeHit.knifeLaunchDuration)
            .SetEase(launchEase);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.STR_KNIFE_TAG))
        {
            OnHitKnife();
        }
        else if (collision.gameObject.CompareTag(Constants.STR_TARGET_TAG))
        {
            OnHitTarget();
        }
        else if (collision.gameObject.CompareTag(Constants.STR_BONUS_TAG))
        {
            LogUtility.ValidInfo("OnCollisionEnter2D", $"Hit Bonus");
        }

    }


    private void OnHitTarget()
    {
        LogUtility.ValidInfo("OnCollisionEnter2D", $"Hit Target");
        if (Status == KnifeStatus.Obstacle) return;
        _launchTween.Kill();
        Status = KnifeStatus.Hit;
        this.Broadcast(EventID.HitTarget, this);
    }

    private void OnHitKnife()
    {
        LogUtility.ValidInfo("OnCollisionEnter2D", $"Hit knife");
        if (Status == KnifeStatus.Hit || Status == KnifeStatus.Obstacle) return;
        collider2d.enabled = false;
        _launchTween.Kill();
        Status = KnifeStatus.Missed;
        this.Broadcast(EventID.HitObstacle, this);
        var underScreenPosition = new Vector3(transform.position.x,
            -Common.ScreenHeightInUnit / 2 - 1f, transform.position.z);
        transform.DOMove(underScreenPosition, _fallDuration).SetEase(launchEase).OnComplete(() =>
        {
            Status = KnifeStatus.Idle;
            ObjectPooling.Remove(gameObject);
        });
    }
}
