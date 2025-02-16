namespace UI
{
    public class BaseOverlap : BaseUIElement
    {
        public override void Hide()
        {
            base.Hide();
        }

        public override void Init()
        {
            base.Init();
            uiType = UIType.Overlap;
        }

        public override void Show(object data)
        {
            base.Show(data);
        }
    }
}