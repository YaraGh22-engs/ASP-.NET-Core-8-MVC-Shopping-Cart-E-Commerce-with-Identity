using Microsoft.CodeAnalysis;
using System;

namespace Shopping_Cart_2.Services
{
    public class RatingService :IRatingService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        public RatingService(IUserService userService, ApplicationDbContext db )
        {

            _userService = userService;
            _db = db;
             
        }

        public int RateProduct(int rate, int itemId, string userId)
        {
             var rated = _db.Ratings?.Include(x => x.Item).Where(r => r.UserId == userId && r.ItemId==itemId).FirstOrDefault();
            if (rated != null)
            {
                UpdateRateProduct(rate, itemId, userId);
                return 1;
            }
            else
            {
                var rating = new Rating()
                {
                    RatingValue = rate,
                    UserId = userId,
                    ItemId = itemId, 
                };
                //rating.Item.ProductAverageRate = GetProductRate(itemId);
                _db.Ratings.Add(rating);
            } 
            return _db.SaveChanges();
        }
        public int UpdateRateProduct(int rate, int itemId, string userId)
        {
            var rating = _db.Ratings.Include(x => x.Item).Where(r => r.UserId == userId && r.ItemId == itemId).SingleOrDefault();
            if (rating == null)
            {
                return 0;
            }
            rating.RatingValue = rate;
            rating.Item.ProductAverageRate = GetProductRate(itemId);
            _db.Ratings.Update(rating);
            return _db.SaveChanges(); 
        }
        

        public int GetUserRate(string userId, int itemId)
        {
            var rate = _db.Ratings?.Include(x => x.Item).Where(r => r.UserId == userId && r.ItemId == itemId).SingleOrDefault();
            if (rate == null)
            {
                return -1;
            }
            return rate.RatingValue;
        }
        public double GetProductRate(int itemId)
        {
            var productRate = _db.Ratings.Include(x=>x.Item).Where(r => r.ItemId == itemId).ToList();
            if (productRate.Count()==0 ) { 
                return -1;
                 
            }
            var av = productRate.Average(r => r.RatingValue);
            return av;
        }





    }
}
