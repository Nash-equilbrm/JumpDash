using DG.Tweening;
using Patterns;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public TrailRenderer trailRenderer;

    private Sequence _lineRendererSeq;

    private void Awake()
    {
        ResetLinePosition();
    }

    private void OnEnable()
    {
        this.Register(EventID.PlayerFinishMovement, OnPlayerFinishMovement);
        ResetLinePosition();

        DOVirtual.Float(transform.position.y, transform.position.y - 2, trailRenderer.time, value =>
        {
            lineRenderer.SetPosition(1, new(transform.position.x, value, transform.position.z));
        });
    }

    private void OnDisable()
    {
        this.Unregister(EventID.PlayerFinishMovement, OnPlayerFinishMovement);
    }


    public void ResetLinePosition()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    public void OnMove()
    {
        if (_lineRendererSeq != null && _lineRendererSeq.active) _lineRendererSeq.Kill();
        ResetLinePosition();
        trailRenderer.enabled = true;
        lineRenderer.enabled = false;
    }

    public void OnPlayerFinishMovement(object _)
    {
        if (_lineRendererSeq != null && _lineRendererSeq.active) _lineRendererSeq.Kill();
        _lineRendererSeq = DOTween.Sequence()
            .AppendInterval(trailRenderer.time)
            .AppendCallback(() =>
            {
                ResetLinePosition();
                trailRenderer.enabled = false;
                lineRenderer.enabled = true;


                DOVirtual.Float(transform.position.y, transform.position.y - 2, trailRenderer.time, value =>
                {
                    lineRenderer.SetPosition(1, new(transform.position.x, value, transform.position.z));
                });
            });
    }
}
