using Commons;
using DG.Tweening;
using Patterns;
using UI;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    public class ContinuePopup : BasePopup
    {
        public RectTransform pRectTransform;
        public CanvasGroup pCanvasGroup;
        public float duration = 3f;
        public Ease ease = Ease.OutCubic;


        public override void Hide()
        {
            pCanvasGroup.DOFade(0, duration).SetEase(ease).OnComplete(() =>
            {
                base.Hide();
            });
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            DOTween.Sequence()
                .AppendInterval(.5f)
                .Append(pCanvasGroup.DOFade(1, duration).SetEase(ease));
        }

        public void OnCloseBtnClicked()
        {
            pCanvasGroup.DOFade(0, duration).SetEase(ease).OnComplete(() =>
            {
                this.Broadcast(EventID.OnBackToMainMenuBtnClicked);
                base.Hide();
            });
        }

        public void OnContinueBtnClicked()
        {
            pCanvasGroup.DOFade(0, duration).SetEase(ease).OnComplete(() =>
            {
                this.Broadcast(EventID.OnContinueClicked);
                base.Hide();
            });
        }
    }
}
