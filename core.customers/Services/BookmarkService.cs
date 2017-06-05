using core.customers.dtos;
using library.couchbase;
using System.Collections.Generic;
using System.Linq;
using System;

namespace core.customers.services
{
    public class BookmarkService : IBookmarkService
    {
        private CouchBaseHelper _couchbaseHelper;
        private CustomerService _customerService;

        public BookmarkService()
        {
            _couchbaseHelper = new CouchBaseHelper();
            _customerService = new CustomerService();
        }

        /// <summary>
        /// Inserts the new bookmark.
        /// </summary>
        public void InsertNewBookmark(string token, int tripId, CustomerLocationDto bookmark)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                CustomerLocationDto foundBookmark = null;

                var trips = customer.TriperooCustomers.Trips.FirstOrDefault(q => q.Id == tripId);

                if (trips != null)
                {
                    foundBookmark = trips.Bookmarks.FirstOrDefault(q => q.RegionID == bookmark.RegionID);
                }

                if (foundBookmark == null)
                {
                    bookmark.Id = customer.TriperooCustomers.Trips.FirstOrDefault(q => q.Id == tripId).Bookmarks.Count + 1;
                    bookmark.DateCreated = DateTime.Now;
                    customer.TriperooCustomers.Trips.FirstOrDefault(q => q.Id == tripId).Bookmarks.Add(bookmark);
                }

                _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
            }
        }

        /// <summary>
        /// Return Bookmarks By Id
        /// </summary>
        public CustomerLocationDto ReturnBookmarkByLocationId(int locationId, int tripId, string token)
        {
            var list = ReturnBookmarksByToken(token, tripId);

            if (list != null)
            {
                return list.FirstOrDefault(q => q.RegionID == locationId);
            }

            return null;
        }

        /// <summary>
        /// Archive Bookmark Id
        /// </summary>
        public void ArchiveBookmarkByLocationId(int locationId, int tripId, string token)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                customer.TriperooCustomers.Trips.FirstOrDefault(q => q.Id == tripId).Bookmarks.FirstOrDefault(q => q.RegionID == locationId).IsArchived = true;
                var newCustomer = _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
            }
        }

        /// <summary>
        /// Return Favourites by token
        /// </summary>
        public List<CustomerLocationDto> ReturnBookmarksByToken(string token, int tripId)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                return customer.TriperooCustomers.Trips.FirstOrDefault(q => q.Id == tripId).Bookmarks.Where(q => q.IsArchived == false).ToList();
            }

            return null;
        }
    }
}
