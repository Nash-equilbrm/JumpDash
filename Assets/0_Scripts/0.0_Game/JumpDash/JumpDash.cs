using Commons;
using Game.JumpDash.UI;
using Game.UI;
using Patterns;
using UI;


namespace Game.JumpDash
{
    public class JumpDash : MiniGame
    {
        public JumpDashLevel CurrentLevel { get; private set; } = GameManager.Instance.GameConfig.jumpDash.levels[0];
        private int _currentLevelIndex = 0;

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
            this.Broadcast(EventID.InitBlockSpanwer, this);

            this.ChangeState(MiniGame.GameState.Gameplay);
        }

        internal override void OnMinigameInitExit()
        {
            base.OnMinigameInitExit();
        }

        internal override void OnMinigamePlayEnter()
        {
            base.OnMinigamePlayEnter();

            this.Register(EventID.PlayerFinishMovement, OnPlayerFinishMovement);
            this.Register(EventID.HitBlock, OnHitBlock);
            UIManager.Instance.ShowScreen<JumpDashGameplayScreen>(forceShowData: true);
        }

        internal override void OnMinigamePlayExit()
        {
            base.OnMinigamePlayExit();

            this.Unregister(EventID.PlayerFinishMovement, OnPlayerFinishMovement);
            this.Unregister(EventID.HitBlock, OnHitBlock);
            var gamePlayScreen = UIManager.Instance.GetExistScreen<JumpDashGameplayScreen>();
            gamePlayScreen.Hide();
        }




        private void InstantitateGameObjects()
        {
            if (this.Player == null)
            {
                this.Player = Instantiate(this.playerPrefab, transform);
            }
            else
            {
                this.Player.SetActive(true);
            }
            ObjectPooling.Instance.GetPool(Constants.STR_BLOCK_TAG).Prepare(10);
        }


        private void OnHitBlock(object obj)
        {
            this.ChangeState(MiniGame.GameState.GameEnd);
        }

        private void OnPlayerFinishMovement(object obj)
        {
            this.CurrentScore++;
            UpdateBestScore();
            UpdateCurrentLevel();
            this.Broadcast(EventID.GainPoint, this.CurrentScore);

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
            ObjectPooling.Instance.GetPool(Constants.STR_BLOCK_TAG).DestroyAll();
            Common.UnloadSceneAsync(this, Constants.SCENE_JUMP_DASH, gcCollect: true);
        }


        private void UpdateBestScore()
        {
            GameManager.Instance.GameData.bestScore.jumpDash =
                GameManager.Instance.GameData.bestScore.jumpDash > CurrentScore
                ? GameManager.Instance.GameData.bestScore.jumpDash
                : CurrentScore;
        }

        private void UpdateCurrentLevel()
        {
            if(CurrentLevel.upperBound != -1 && CurrentScore > CurrentLevel.upperBound)
            {
                _currentLevelIndex++;
                CurrentLevel = GameManager.Instance.GameConfig.jumpDash.levels[_currentLevelIndex];
            }
        }



    }
}

