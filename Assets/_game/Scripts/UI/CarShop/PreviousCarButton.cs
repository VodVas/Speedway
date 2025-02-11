public class PreviousCarButton : CarButtonBase
{
    protected override void OnButtonClicked()
    {
        GetCarShop().SwitchPreviousCar();
    }
}