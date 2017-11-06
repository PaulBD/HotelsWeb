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
                    var isTravelActivity = activity.Type.ToLower() == "transfers & ground transport" || activity.Type.ToLower() == "hotel";

                    if (isTravelActivity)
                    {
                        activity.Id = -1;
                    }
                    else
                    {
                        activity.Id = trip.Days.Count + 1;
                    }

                    activity.DateCreated = DateTime.Now;

                    var maxItems = 0;
                    var totalDuration = 0;

                    switch (trip.TripDetails.TripPace)
                    {
                        case "easy going":
                            maxItems = 1;
                            totalDuration = 120;
                            break;
                        case "balanced":
                            maxItems = 2;
                            totalDuration = 240;
                            break;
                        case "fast paced":
                            maxItems = 3;
                            totalDuration = 360;
                            break;
                    }

                    if (!isTravelActivity)
                    {
                        var nextAvailableDay = trip.TripDetails.TripSummary.FirstOrDefault(q => q.TotalDuration >= 0 && q.TotalDuration < totalDuration && q.Count < maxItems);
                        var startTimePeriod = ReturnStartTimePeriod(trip.Days, nextAvailableDay.Date);
                        var activityLengthInMinutes = ReturnActivityLengthInMinutes(activity.Length);

                        //TODO Do I need to revevailute next Available Day if duratation is full day??

                        activity.Day = nextAvailableDay.Day;
                        activity.VisitDate = nextAvailableDay.Date;
                        activity.Length = activity.Length;
                        activity.Price = activity.Price;
                        activity.BookingUrl = activity.BookingUrl;
                        activity.StartTimePeriod = startTimePeriod;

                        trip.TripDetails.TripSummary.FirstOrDefault(q => q.Date == nextAvailableDay.Date).Count += 1;
                        trip.TripDetails.TripSummary.FirstOrDefault(q => q.Date == nextAvailableDay.Date).TotalDuration += activityLengthInMinutes;

                        trip.Days.Add(activity);
                    }
                    else
                    {
                        activity.Day = -1;
                        activity.VisitDate = "";
                        activity.Length = activity.Length;
                        activity.Price = activity.Price;
                        activity.BookingUrl = activity.BookingUrl;
                        activity.StartTimePeriod = "";
                        trip.TripExtras.Add(activity);
                    }
                }

                _tripService.InsertUpdateTrip(customer.TriperooCustomers.CustomerReference, trip);
            }
        }

        private string ReturnStartTimePeriod(List<ActivityDto> days, string currentDate)
        {
            var selectedDay = days.Where(q => q.VisitDate == currentDate).ToList();

            if (selectedDay != null)
            {
                if (selectedDay.Count == 0)
                {
                    return "AM";
                }
            }

            return "PM";
        }

        private int ReturnActivityLengthInMinutes(string activityLength)
        {
            var activityMinutes = 120;

            if (activityLength != null)
            {
                if (activityLength.Length > 0)
                {
                    var h = activityLength.ToLower().Split(' ');

                    if (h.Length > 0)
                    {
                        activityMinutes = (Convert.ToInt32(h[0].Trim().Replace("h", "").Replace("hours", "").Replace("hour", "")) * 60);

                        if (h.Length > 2)
                        {
                            activityMinutes += (Convert.ToInt32(h[2].Trim().Replace("m", "").Replace("minutes", "")));
                        }
                    }
                    else {
                        if (h.Contains("h") || h.Contains("hours") || h.Contains("hour"))
                        {
                            activityMinutes = (Convert.ToInt32(h[0].Trim().Replace("h", "").Replace("hours", "").Replace("hour", "")) * 60);
                        }
                        else {
                            activityMinutes = (Convert.ToInt32(h[1].Trim().Replace("m", "").Replace("minutes", "")));
                        }
                    }
                }
            }

            return activityMinutes;
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
