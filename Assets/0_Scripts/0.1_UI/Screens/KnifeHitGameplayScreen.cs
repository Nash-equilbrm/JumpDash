using Commons;
using DG.Tweening;
using Patterns;
using System;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;


namespace Game.KnifeHit.UI
{
    public class KnifeHitGameplayScreen : BaseScreen
    {
        public TMP_Text bestScoreText;
        public TMP_Text currentScoreText;
        public Transform knifeCounter;
        public GameObject knifeIconTemplate;
        public float duration = .7f;
        public Ease ease = Ease.Linear;
        public Color iconColorActive;
        public Color iconColorInactive;
        public Color iconColorDisabled;

        public override void Hide()
        {
            base.Hide();
            this.Unregister(EventID.GainPoint, OnGainPoint);
            this.Unregister(EventID.PlayerFinishLaunch, OnFinishLaunch);
            this.Unregister(EventID.ReinitTarget, OnReinitTarget);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            this.Register(EventID.GainPoint, OnGainPoint);
            this.Register(EventID.PlayerFinishLaunch, OnFinishLaunch);
            this.Register(EventID.ReinitTarget, OnReinitTarget);

            if (data is KnifeHitLevel level)
            {
                DisplayData(level);
            }
        }

        private void OnReinitTarget(object obj)
        {
            if (obj is not KnifeHitLevel level) return;
            DisplayKnifeCounter(level.knifeCount);
        }

        private void OnFinishLaunch(object obj)
        {
            Transform firstIconActive = transform.Cast<Transform>()
               .FirstOrDefault(child => child.GetComponent<Image>().color == Color.white);
            firstIconActive.GetComponent<Image>().DOColor(iconColorInactive, duration).SetEase(ease);
        }

        private void OnGainPoint(object obj)
        {
            if (obj is not int score) return;
            bestScoreText.text = $"{Constants.STR_BEST_SCORE}: {GameManager.Instance.GameData.bestScore.jumpDash}";
            currentScoreText.text = $"{Constants.STR_SCORE}: {score}";

            // Reset knife counter icons
            foreach (Transform t in knifeCounter) { 
                if(t.TryGetComponent<Image>(out var image))
                {
                    image.DOColor(iconColorDisabled, duration).SetEase(ease).OnComplete(() => t.gameObject.SetActive(false));
                }
            }
        }

        private void DisplayData(KnifeHitLevel level)
        {
            bestScoreText.text = $"{Constants.STR_BEST_SCORE}: {GameManager.Instance.GameData.bestScore.jumpDash}";
            currentScoreText.text = $"{Constants.STR_SCORE}: 0";

            DisplayKnifeCounter(level.knifeCount);
        }


        private void DisplayKnifeCounter(int knifeCount)
        {
            int currentTotalIcon = knifeCounter.transform.childCount;
            if (currentTotalIcon < knifeCount)
            {
                for (int i = currentTotalIcon + 1; i <= knifeCount; i++)
                {
                    var newIcon = Instantiate(knifeIconTemplate, parent: knifeCounter);
                    newIcon.SetActive(false);
                }
            }
            for (int i = 0; i < knifeCount; ++i)
            {
                var knifeIcon = knifeCounter.GetChild(i);
                knifeIcon.gameObject.SetActive(true);
                var image = knifeIcon.GetComponent<Image>();
                image.color = iconColorDisabled;
                image.DOColor(iconColorActive, duration).SetEase(ease);
            }
        }
    }

}
