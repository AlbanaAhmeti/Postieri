namespace Postieri.Interfaces
{
    public interface IDashboard
    {
        double GetTotal();
        int AvailableCouriers();
        int OrdersInLastThreeMonths();
        int OrdersAccepted();
        int OrdersAtTheCourier();
        int LastSevenDays();
    }
}
