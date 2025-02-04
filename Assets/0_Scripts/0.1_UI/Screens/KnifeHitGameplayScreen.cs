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
        public float iconSizeActive;
        public float iconSizeInactive;
        public float iconSizeDisabled;


        public override void Hide()
        {
            base.Hide();
            this.Unregister(EventID.GainPoint, OnGainPoint);
            this.Unregister(EventID.PlayerFinishLaunch, OnFinishLaunch);
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

            if (data is KnifeHitLevel level)
            {
                DisplayData(level);
            }
        }


        private void OnFinishLaunch(object obj)
        {
            Transform firstIconActive = knifeCounter.Cast<Transform>()
               .FirstOrDefault(child => child.GetComponent<Image>().color == iconColorActive);
            if (firstIconActive == null) return;
            firstIconActive.GetComponent<Image>().DOColor(iconColorInactive, duration).SetEase(ease);
            firstIconActive.transform.DOScale(Vector3.one * iconSizeInactive, duration).SetEase(ease);
        }

        private void OnGainPoint(object obj)
        {
            if (obj is not KnifeHit game) return;
            bestScoreText.text = $"{Constants.STR_BEST_SCORE}: {GameManager.Instance.GameData.bestScore.knifeHit}";
            currentScoreText.text = $"{Constants.STR_SCORE}: {game.CurrentScore}";

            // Reset knife counter icons
            foreach (Transform t in knifeCounter) {
                t.gameObject.SetActive(false);
            }

            DisplayKnifeCounter(game.CurrentLevel.knifeCount);
        }

        private void DisplayData(KnifeHitLevel level)
        {
            bestScoreText.text = $"{Constants.STR_BEST_SCORE}: {GameManager.Instance.GameData.bestScore.knifeHit}";
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
                image.transform.DOScale(iconSizeActive, duration).SetEase(ease);
            }
        }
    }

}
