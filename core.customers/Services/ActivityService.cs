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
                    var activities = trip.TripDetails.TripSummary.SelectMany(p => p.Activities);

                    if (activities != null)
                    {
                        var l = activities.Where(c => c.RegionID == activity.RegionID);

                        if (l.Any())
                        {
                            foundActivity = l.FirstOrDefault();
                        }
                    }
                }

                if (foundActivity == null)
                {
                    if (!string.IsNullOrEmpty(activity.RegionName))
                    {
                        var isTravelActivity = activity.ActivityType.ToLower() == "transfers & ground transport" || activity.ActivityType.ToLower() == "hotel";

                        if (isTravelActivity)
                        {
                            activity.Id = -1;
                        }
                        else
                        {
                            activity.Id = trip.TripDetails.TripSummary.SelectMany(p => p.Activities).ToList().Count + 1;
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

                        TripSummary nextAvailableDay = null;

                        if (activity.ActivityType == "Restaurants")
                        {    
                            nextAvailableDay = trip.TripDetails.TripSummary.FirstOrDefault(q => q.RestaurantCount == 0);
                        }
                        else {
                            nextAvailableDay = trip.TripDetails.TripSummary.FirstOrDefault(q => q.TotalDuration >= 0 && q.TotalDuration < totalDuration && q.ActivitiesCount < maxItems);
                        }

                        var startTimePeriod = ReturnStartTimePeriod(trip.TripDetails.TripSummary, nextAvailableDay.Date);
                        var activityLengthInMinutes = ReturnActivityLengthInMinutes(activity.Length);

                        //TODO Do I need to revevailute next Available Day if duratation is full day??

                        activity.Day = nextAvailableDay.Day;
                        activity.Date = nextAvailableDay.Date;
                        activity.Length = activityLengthInMinutes.ToString();
                        activity.Price = activity.Price;
                        activity.BookingUrl = activity.BookingUrl;
                        activity.StartTimePeriod = startTimePeriod;

                        if (activity.ActivityType == "Restaurants")
                        {
                            trip.TripDetails.TripSummary.FirstOrDefault(q => q.Date == nextAvailableDay.Date).RestaurantCount += 1;
                        }
                        else {
                            trip.TripDetails.TripSummary.FirstOrDefault(q => q.Date == nextAvailableDay.Date).ActivitiesCount += 1;
                        }
                        trip.TripDetails.TripSummary.FirstOrDefault(q => q.Date == nextAvailableDay.Date).TotalDuration += activityLengthInMinutes;
                        trip.TripDetails.TripSummary.FirstOrDefault(q => q.Date == nextAvailableDay.Date).Activities.Add(activity);
                    }
                }

                _tripService.InsertUpdateTrip(customer.TriperooCustomers.CustomerReference, trip);
            }
        }

        private string ReturnStartTimePeriod(List<TripSummary> summary, string currentDate)
        {
            var selectedDay = summary.Where(q => q.Date == currentDate).ToList();

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
                    return list.SelectMany(p => p.Activities).FirstOrDefault(c => c.RegionID == locationId);
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
                    foreach(var t in existingTrip.TripDetails.TripSummary)
                    {
                        foreach (var s in t.Activities)
                        {
                            if (s.RegionID == locationId)
                            {
                                t.Activities.Remove(s);
                            }
                        }

                    }

                    _tripService.InsertUpdateTrip(customer.TriperooCustomers.CustomerReference, existingTrip);
                }
            }
        }

        /// <summary>
        /// Return Activities by token
        /// </summary>
        public List<TripSummary> ReturnActivitiesByToken(string token, int tripId)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                var existingTrip = _tripService.ReturnTripByCustomerReferenceAndId(customer.TriperooCustomers.CustomerReference, tripId);

                if (existingTrip != null)
                {
                    return existingTrip.TripDetails.TripSummary;
                }
            }

            return null;
        }
    }
}
