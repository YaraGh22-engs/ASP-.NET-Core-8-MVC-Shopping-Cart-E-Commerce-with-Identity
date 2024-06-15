namespace Shopping_Cart_2.Services
{
    public interface IRatingService
    {
        int RateProduct(int rate, int itemId, string userId);
        int UpdateRateProduct(int rate, int itemId, string userId);
        int GetUserRate(string userId, int itemId);
        double GetProductRate(int itemId);
    }
}
