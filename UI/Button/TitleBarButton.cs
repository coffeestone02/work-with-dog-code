public class TitleBarButton : BaseButton
{
    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        switch (name)
        {
            case "RestoreButton":
                Managers.Window.Restore();
                break;
            case "PIPButton":
                Managers.Window.PIP();
                break;
            case "HideButton":
                Managers.Window.Hide();
                break;
            case "ExitButton":
                Managers.Window.Exit();
                break;
        }
    }
}
