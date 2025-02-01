using Commons;
using Patterns;

namespace UI
{
    public class MainMenuScreen : BaseScreen
    {
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
        }

        public void OnPlayBtnClicked()
        {
            LogUtility.ValidInfo("MainMenuScreen", "OnPlayBtnClicked");
            this.Broadcast(EventID.OnPlayBtnClicked);
            Hide();
        }
    }
}

