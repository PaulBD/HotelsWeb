using library.zomato.dtos;

namespace library.zomato.services
{
    public interface IRestaurantService
    {
        RestaurantDto ReturnRestaurantByLocation(double latitude, double longitude);
    }
}
