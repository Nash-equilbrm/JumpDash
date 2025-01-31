using Commons;
using Patterns;
using TMPro;
using UI;


namespace Game.UI
{
    public class GameplayScreen : BaseScreen
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
        }

        private void DisplayData()
        {
            bestScoreText.text = $"{Strings.BestScore}: {GameManager.Instance.GameData.bestScore}";
            currentScoreText.text = GameManager.Instance.CurrentScore.ToString();

        }
    }
}
