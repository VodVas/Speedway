public class NextCarButton : CarButtonBase
{
    protected override void OnButtonClicked()
    {
        GetCarShop().SwitchNextCar();
    }
}