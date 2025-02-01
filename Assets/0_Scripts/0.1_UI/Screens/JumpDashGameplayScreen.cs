using Commons;
using Patterns;
using TMPro;
using UI;


namespace Game.JumpDash.UI
{
    public class JumpDashGameplayScreen : BaseScreen
    {
        public TMP_Text bestScoreText;
        public TMP_Text currentScoreText;

        public override void Hide()
        {
            base.Hide();
            this.Unregister(EventID.GainPoint, OnGainPoint);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            this.Register(EventID.GainPoint, OnGainPoint);
            DisplayData();
        }

        private void OnGainPoint(object obj)
        {
            if (obj is not int score) return;
            currentScoreText.text = score.ToString();
            bestScoreText.text = $"{Constants.STR_BEST_SCORE}: {GameManager.Instance.GameData.bestScore.jumpDash}";
        }

        private void DisplayData()
        {
            bestScoreText.text = $"{Constants.STR_BEST_SCORE}: {GameManager.Instance.GameData.bestScore.jumpDash}";
            currentScoreText.text = "0";
        }
    }
}
