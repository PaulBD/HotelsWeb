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
        public void InsertNewBookmark(string token, BookmarkDto bookmark)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if ((customer != null) && (customer.TriperooCustomers != null))
            {
                var foundBookmark = customer.TriperooCustomers.Bookmarks.FirstOrDefault(q => q.LocationId == bookmark.LocationId);

                if (foundBookmark != null)
                {
                    customer.TriperooCustomers.Bookmarks.Remove(foundBookmark);
                }

				bookmark.Id = customer.TriperooCustomers.Bookmarks.Count + 1;
				bookmark.DateCreated = DateTime.Now;
                customer.TriperooCustomers.Bookmarks.Add(bookmark);

                _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
            }
        }

        /// <summary>
        /// Return Bookmarks By Id
        /// </summary>
        public BookmarkDto ReturnBookmarkById(int id, string token)
        {
            var list = ReturnBookmarksByToken(token);

            if (list != null)
            {
                return list.FirstOrDefault(q => q.Id == id);
            }

            return null;
        }

        /// <summary>
        /// Archive Bookmark Id
        /// </summary>
        public void ArchiveBookmarkById(int id, string token)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if ((customer != null) && (customer.TriperooCustomers != null))
            {
                customer.TriperooCustomers.Bookmarks[id - 1].IsArchived = true;
                var newCustomer = _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
            }
        }

        /// <summary>
        /// Return Favourites by token
        /// </summary>
        public List<BookmarkDto> ReturnBookmarksByToken(string token)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if ((customer != null) && (customer.TriperooCustomers != null))
            {
                return customer.TriperooCustomers.Bookmarks;
            }

            return null;
        }
    }
}
