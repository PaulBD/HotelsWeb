using core.customers.dtos;
using library.couchbase;
using System.Collections.Generic;
using System.Linq;
using System;

namespace core.customers.services
{
    public class ActivityService : IActivityService
    {
        private CouchBaseHelper _couchbaseHelper;
        private ICustomerService _customerService;
        private ITripService _tripService;

        public ActivityService(ICustomerService customerService, ITripService tripService)
        {
            _couchbaseHelper = new CouchBaseHelper();
            _customerService = customerService;
            _tripService = tripService;
        }

        /// <summary>
        /// Inserts the new activity.
        /// </summary>
        public void InsertNewActivity(string token, int tripId, ActivityDto activity)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                var trip = _tripService.ReturnTripByCustomerReferenceAndId(customer.TriperooCustomers.CustomerReference, tripId);

                ActivityDto foundActivity = null;

                if (trip != null)
                {
                    foundActivity = trip.Days.FirstOrDefault(q => q.RegionID == activity.RegionID);
                }

                if (foundActivity == null)
                {
                    activity.Id = trip.Days.Count + 1;
                    activity.DateCreated = DateTime.Now;
                    trip.Days.Add(activity);
                }

                _tripService.InsertUpdateTrip(customer.TriperooCustomers.CustomerReference, trip);
            }
        }

        /// <summary>
        /// Return Activities By Id
        /// </summary>
        public ActivityDto ReturnActivityByLocationId(int locationId, int tripId, string token)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                var list = ReturnActivitiesByToken(token, tripId);

                if (list != null)
                {
                    return list.FirstOrDefault(q => q.RegionID == locationId);
                }
            }

            return null;
        }

        /// <summary>
        /// Archive Activity Id
        /// </summary>
        public void ArchiveActivityByLocationId(int locationId, int tripId, string token)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                var existingTrip = _tripService.ReturnTripByCustomerReferenceAndId(customer.TriperooCustomers.CustomerReference, tripId);

                if (existingTrip != null)
                {
                    existingTrip.Days.FirstOrDefault(q => q.RegionID == locationId).IsArchived = true;
                    _tripService.InsertUpdateTrip(customer.TriperooCustomers.CustomerReference, existingTrip);
                }
            }
        }

        /// <summary>
        /// Return Activities by token
        /// </summary>
        public List<ActivityDto> ReturnActivitiesByToken(string token, int tripId)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                var existingTrip = _tripService.ReturnTripByCustomerReferenceAndId(customer.TriperooCustomers.CustomerReference, tripId);

                if (existingTrip != null)
                {
                    return existingTrip.Days;
                }
            }

            return null;
        }
    }
}
