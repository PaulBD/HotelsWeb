using System.Linq;
using core.customers.dtos;
using core.places.services;
using library.caching;
using library.couchbase;

namespace core.customers.services
{
    public class PhotoService : IPhotoService
    {
        private CouchBaseHelper _couchbaseHelper;
        private ICustomerService _customerService;
        private ILocationService _locationService;
        private readonly ICacheProvider _cache;

        public PhotoService(ICacheProvider cache)
        {
            _cache = cache;
            _couchbaseHelper = new CouchBaseHelper();
            _customerService = new CustomerService();
            _locationService = new LocationService(_cache);
        }

        /// <summary>
        /// Returns the photos by token.
        /// </summary>
        public CustomerPhotos ReturnPhotosByReference(string reference)
        {
            var customer = _customerService.ReturnCustomerByReference(reference);
            return customer.TriperooCustomers.CustomerPhotos;
        }

        /// <summary>
        /// Archives photo By Id
        /// </summary>
        public void ArchivePhotoById(string token, string photoReference)
        {
            var customer = _customerService.ReturnCustomerByToken(token);
            var item = customer.TriperooCustomers.CustomerPhotos.PhotoList.FirstOrDefault(q => q.photoReference.ToLower() == photoReference.ToLower());
            customer.TriperooCustomers.CustomerPhotos.PhotoList.Remove(item);
            _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);

            if (item.regionID > 0)
            {
                var location = _locationService.ReturnLocationById(item.regionID, false);
                var locationItem = location.Photos.PhotoList.FirstOrDefault(q => q.photoReference == photoReference);
                location.Photos.PhotoList.Remove(locationItem);
                _locationService.UpdateLocation(location, false);
            }
        }

        /// <summary>
        /// Insert New Photo
        /// </summary>
        public void InsertNewPhoto(int regionID, places.dtos.PhotoList photo, string reference)
        {
            var customer = _customerService.ReturnCustomerByReference(reference);

            customer.TriperooCustomers.CustomerPhotos.PhotoList.Add(new dtos.PhotoList { photoReference = photo.photoReference, url = photo.prefix + photo.suffix, regionID = regionID });

            _customerService.InsertUpdateCustomer(reference, customer.TriperooCustomers);
        }
    }
}
