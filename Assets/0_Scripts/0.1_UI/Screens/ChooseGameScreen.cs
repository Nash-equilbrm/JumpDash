using Commons;
using DG.Tweening;
using Patterns;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;


namespace Game.UI
{
    public class ChooseGameScreen : BaseScreen
    {
        public RectTransform jumpDashBtn;
        public RectTransform knifeHitBtn;
        public float selectScale;
        public float duration;
        public Ease ease;

        private int _gameIndex = -1;

        public override void Hide()
        {
            base.Hide();
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            OnKnifeHitClicked();
        }


        public void OnJumpDashClicked()
        {
            jumpDashBtn.DOScale(Vector3.one * selectScale, duration).SetEase(ease);
            knifeHitBtn.DOScale(Vector3.one, duration).SetEase(ease);
            knifeHitBtn.SetAsFirstSibling();
            _gameIndex = 1;
        }


        public void OnKnifeHitClicked()
        {
            knifeHitBtn.DOScale(Vector3.one * selectScale, duration).SetEase(ease);
            jumpDashBtn.DOScale(Vector3.one, duration).SetEase(ease);
            jumpDashBtn.SetAsFirstSibling();
            _gameIndex = 2;
        }


        public void OnPlayBtnClicked()
        {
            LogUtility.ValidInfo("MainMenuScreen", "OnPlayBtnClicked");
            this.Broadcast(EventID.OnPlayBtnClicked, _gameIndex);
            Hide();
        }
    }

}
