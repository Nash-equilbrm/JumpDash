using Commons;
using Game.KnifeHit.Player;
using Game.KnifeHit.UI;
using Game.UI;
using Patterns;
using UI;


namespace Game.KnifeHit
{
    public class KnifeHit : MiniGame
    {
        public KnifeHitLevel CurrentLevel { get; private set; } = GameManager.Instance.GameConfig.knifeHit.levels[0];
        private int _currentLevelIndex = 0;

        private int _knifeLeft = GameManager.Instance.GameConfig.knifeHit.levels[0].knifeCount;

        internal override void OnMinigameEndEnter()
        {
            base.OnMinigameEndEnter();

            UIManager.Instance.ShowPopup<ContinuePopup>(forceShowData: true);
            this.Register(EventID.OnBackToMainMenuBtnClicked, OnBackToMainMenuClicked);
            this.Register(EventID.OnContinueClicked, OnContinueClicked);
            this.Register(EventID.UnloadMiniGame, UnloadMiniGame);
        }

        internal override void OnMinigameEndExit()
        {
            base.OnMinigameEndExit();

            this.Unregister(EventID.OnBackToMainMenuBtnClicked, OnBackToMainMenuClicked);
            this.Unregister(EventID.OnContinueClicked, OnContinueClicked);
            this.Unregister(EventID.UnloadMiniGame, UnloadMiniGame);
        }

        internal override void OnMinigameInitEnter()
        {
            base.OnMinigameInitEnter();

            InstantitateGameObjects();

            this.ChangeState(MiniGame.GameState.Gameplay);
        }

        internal override void OnMinigameInitExit()
        {
            base.OnMinigameInitExit();
        }

        internal override void OnMinigamePlayEnter()
        {
            base.OnMinigamePlayEnter();
            this.Register(EventID.HitObstacle, OnHitObstacle);
            this.Register(EventID.HitTarget, OnHitTarget);
            UIManager.Instance.ShowScreen<KnifeHitGameplayScreen>(CurrentLevel, forceShowData: true);
            ObjectPooling.Instance.GetPool(Constants.STR_KNIFE_TAG).RecycleAll();
            _knifeLeft = CurrentLevel.knifeCount;
            this.Broadcast(EventID.InitTarget, this);
        }

        internal override void OnMinigamePlayExit()
        {
            base.OnMinigamePlayExit();

            this.Unregister(EventID.HitObstacle, OnHitObstacle);
            this.Unregister(EventID.HitTarget, OnHitTarget);
            var gamePlayScreen = UIManager.Instance.GetExistScreen<KnifeHitGameplayScreen>();
            gamePlayScreen.Hide();
        }


        private void InstantitateGameObjects()
        {
            if (Player == null)
            {
                Player = Instantiate(playerPrefab, transform);
            }
            else
            {
                Player.SetActive(true);
                Player.GetComponent<PlayerLaunchKnife>().CanLaunch = true;
            }
            ObjectPooling.Instance.GetPool(Constants.STR_KNIFE_TAG).Prepare(1);
        }

        private void OnHitObstacle(object obj)
        {
            this.ChangeState(MiniGame.GameState.GameEnd);
        }

        private void OnHitTarget(object obj)
        {
            _knifeLeft--;
            if (_knifeLeft == 0)
            {
                CurrentScore++;
                UpdateBestScore();
                UpdateCurrentLevel();
                _knifeLeft = CurrentLevel.knifeCount;
                this.Broadcast(EventID.GainPoint, this);
            }
        }


        private void OnBackToMainMenuClicked(object obj)
        {
            this.Broadcast(EventID.ExitMiniGame);
        }

        private void OnContinueClicked(object obj)
        {
            ChangeState(GameState.GameInit);
        }

        private void UnloadMiniGame(object data)
        {
            this.Unregister(EventID.OnBackToMainMenuBtnClicked, OnBackToMainMenuClicked);
            this.Unregister(EventID.OnContinueClicked, OnContinueClicked);
            this.Unregister(EventID.UnloadMiniGame, UnloadMiniGame);
            ObjectPooling.Instance.GetPool(Constants.STR_KNIFE_TAG).DestroyAll();
            Common.UnloadSceneAsync(this, Constants.SCENE_KNIFE_HIT, gcCollect: true);
        }


        private void UpdateBestScore()
        {
            GameManager.Instance.GameData.bestScore.knifeHit =
                GameManager.Instance.GameData.bestScore.knifeHit > CurrentScore
                ? GameManager.Instance.GameData.bestScore.knifeHit
                : CurrentScore;
        }

        private void UpdateCurrentLevel()
        {
            if (CurrentLevel.upperBound != -1 && CurrentScore > CurrentLevel.upperBound)
            {
                _currentLevelIndex++;
                CurrentLevel = GameManager.Instance.GameConfig.knifeHit.levels[_currentLevelIndex];
            }
        }
    }

}
