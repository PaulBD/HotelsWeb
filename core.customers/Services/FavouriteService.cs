using core.customers.dtos;
using library.couchbase;
using System.Collections.Generic;
using System.Linq;

namespace core.customers.services
{
    public class FavouriteService : IFavouriteService
    {
        private CouchBaseHelper _couchbaseHelper;
        private CustomerService _customerService;

        public FavouriteService()
        {
            _couchbaseHelper = new CouchBaseHelper();
            _customerService = new CustomerService();
        }

        public void InsertNewFavourite(string token, FavouriteDto favourite)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if ((customer != null) && (customer.TriperooCustomers != null))
            {
                favourite.Id = customer.TriperooCustomers.Favourites.Count + 1;
                customer.TriperooCustomers.Favourites.Add(favourite);
                _customerService.InsertUpdateCustomer(customer.TriperooCustomers.Reference, customer.TriperooCustomers);
            }
        }

        /// <summary>
        /// Return Favourite By Id
        /// </summary>
        public FavouriteDto ReturnFavouriteById(int id, string token)
        {
            var list = ReturnFavouritesByToken(token);

            if (list != null)
            {
                return list.FirstOrDefault(q => q.Id == id);
            }

            return null;
        }

        /// <summary>
        /// Archive Favourite Id
        /// </summary>
        public void ArchiveFavouriteById(int id, string token)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if ((customer != null) && (customer.TriperooCustomers != null))
            {
                customer.TriperooCustomers.Favourites[id - 1].IsArchived = true;
                var newCustomer = _customerService.InsertUpdateCustomer(customer.TriperooCustomers.Reference, customer.TriperooCustomers);
            }
        }

        /// <summary>
        /// Return Favourites by token
        /// </summary>
        public List<FavouriteDto> ReturnFavouritesByToken(string token)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if ((customer != null) && (customer.TriperooCustomers != null))
            {
                return customer.TriperooCustomers.Favourites;
            }

            return null;
        }
    }
}
