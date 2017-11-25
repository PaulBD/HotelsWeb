using core.customers.dtos;
using library.couchbase;
using System.Linq;
using System.Collections.Generic;
using System;

namespace core.customers.services
{
    public class TripService : ITripService
    {
		private CouchBaseHelper _couchbaseHelper;
        private CustomerService _customerService;
        private ActivityService _activityService;
        private readonly string _bucketName = "TriperooCustomers";

		public TripService()
		{
			_couchbaseHelper = new CouchBaseHelper();
			_customerService = new CustomerService();
            _activityService = new ActivityService(_customerService, this);
		}

        /// <summary>
        /// Archive existing trip
        /// </summary>
        public void ArchiveExistingTrip(int tripId, string token)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
				customer.TriperooCustomers.Trips.FirstOrDefault(q => q.Id == tripId).IsArchived = true;
				_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);

                var existingTrip = ReturnTripByCustomerReferenceAndId(customer.TriperooCustomers.CustomerReference, tripId);

                if (existingTrip != null)
                {
                    existingTrip.IsArchived = true;
                    InsertUpdateTrip(customer.TriperooCustomers.CustomerReference, existingTrip);
                }
            }
        }

        /// <summary>
        /// Inserts the new trip
        /// </summary>
        public int InsertNewTrip(string token, TripDto trip)
		{
            var tripId = 0;
			var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                tripId = customer.TriperooCustomers.Trips.Count + 1;
                var url = customer.TriperooCustomers.Profile.ProfileUrl + "/trips/" + tripId + "/" + trip.TripName.Replace(" ", "-").ToLower();
                trip.Id = tripId;
                trip.Url = url;
                trip.CustomerReference = customer.TriperooCustomers.CustomerReference;

                // Insert into Customer
                customer.TriperooCustomers.Trips.Add(new CustomerTripDto(){
                    DateCreated = new DateTime(),
                    Id = tripId,
                    Url = url
                });
				_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);

                if ((!System.String.IsNullOrEmpty(trip.TripDetails.TripStart)) && (!System.String.IsNullOrEmpty(trip.TripDetails.TripEnd)))
                {
                    var dtStart = Convert.ToDateTime(trip.TripDetails.TripStart);
                    var dtEnd = Convert.ToDateTime(trip.TripDetails.TripEnd);
                    trip.TripDetails.TripLength = (dtEnd - dtStart).TotalDays;

                    for (var i = 0; i < trip.TripDetails.TripLength; i++)
                    {
                        trip.TripDetails.TripSummary.Add(new TripSummary
                        {
                            RestaurantCount = 0,
                            ActivitiesCount = 0,
                            Day = i,
                            TotalDuration = 0,
                            Date = dtStart.AddDays(i).ToString("yyyy/MM/dd")
                        });
                    }
                }
                var activity = new ActivityDto();

                if (trip.TripDetails.TripSummary.Count > 0)
                {
                    if (trip.TripDetails.TripSummary[0].Activities != null)
                    {
                        if (trip.TripDetails.TripSummary[0].Activities.Count > 0)
                        {
                            activity = trip.TripDetails.TripSummary[0].Activities[0];
                            trip.TripDetails.TripSummary[0].Activities.RemoveAt(0);
                        }
                    }
                }

                //Insert into Trip
                InsertUpdateTrip(customer.TriperooCustomers.CustomerReference, trip);

                //Insert new activity
                _activityService.InsertNewActivity(token, trip.Id, activity);
            }

            return tripId;
        }

        /// <summary>
        /// Edits the existing trip.
        /// </summary>
        public void EditExistingTrip(string token, int tripId, TripDto trip)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                var existingTrip = ReturnTripByCustomerReferenceAndId(customer.TriperooCustomers.CustomerReference, tripId);

                if (existingTrip != null)
                {
                    var url = customer.TriperooCustomers.Profile.ProfileUrl.ToLower() + "/trips/" + existingTrip.Id + "/" + trip.TripName.Replace(" ", "-").ToLower();
                    var existingCustomerTrip = customer.TriperooCustomers.Trips.FirstOrDefault(q => q.Id == tripId);

                    existingTrip.TripDetails = trip.TripDetails;
                    //existingTrip.Days = trip.Days;
                    existingTrip.TripName = trip.TripName;
                    existingTrip.Url = url;
                    InsertUpdateTrip(customer.TriperooCustomers.CustomerReference, existingTrip);

                    existingCustomerTrip.Url = url;
                    _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
                } 
            }
        }

        /// <summary>
        /// Inserts the update trip.
        /// </summary>
        public void InsertUpdateTrip(string customerReference, TripDto trip)
        {
            var tripReference = customerReference + ":trip:" + trip.Id;
            _couchbaseHelper.AddRecordToCouchbase(tripReference, trip, _bucketName);
        }

        /// <summary>
        /// Return Customer by reference
        /// </summary>
        public TripDto ReturnTripByCustomerReferenceAndId(string customerReference, int tripId)
        {
            if (!customerReference.Contains("customer"))
            {
                customerReference = "customer:" + customerReference;
            }

            var q = "SELECT id, customerReference, url, type, tripName, tripDetails, days FROM " + _bucketName + " WHERE customerReference = '" + customerReference + "' AND id = " + tripId;
            var result = ProcessQuery(q);

            if (result.Count > 0)
            {
                return result[0];
            }

            return null;
        }

        /// <summary>
        /// Return Customer by reference
        /// </summary>
        public List<TripDto> ReturnTripsByCustomerReference(string customerReference)
        {
            if (!customerReference.Contains("customer"))
            {
                customerReference = "customer:" + customerReference;
            }

            var q = "SELECT id, customerReference, url, type, tripName, tripDetails, days FROM " + _bucketName + " WHERE customerReference = '" + customerReference + "' AND type = 'trip'";
            return ProcessQuery(q);
        }


        /// <summary>
        /// Process Query
        /// </summary>
        private List<TripDto> ProcessQuery(string q)
        {
            var result = _couchbaseHelper.ReturnQuery<TripDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }
    }
}
