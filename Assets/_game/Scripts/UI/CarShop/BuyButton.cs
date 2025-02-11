public class BuyButton : CarButtonBase
{
    protected override void OnButtonClicked()
    {
        GetCarShop().BuyCurrentCar();
    }
}