using library.couchbase;
using core.hotels.dtos;
using ServiceStack.Text;
using System.Collections.Generic;
using System;

namespace core.hotels.services
{
    public class HotelService : IHotelService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooHotels";

        public HotelService()
        {
            _couchbaseHelper = new CouchBaseHelper();
        }

        /// <summary>
        /// Return a list of hotels by place id
        /// </summary>
        public List<HotelDto> ReturnHotelsByPlaceId(int locationId)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE regionID = " + locationId;

            return ProcessQuery(q);
        }

        /// <summary>
        /// Return a list of hotels by proximity
        /// </summary>
        public List<HotelDto> ReturnHotelsByProximity(double longitude, double latitude, double radius)
        {
			var degLat = utility.Location.Deg2rad(latitude);
			var degLon = utility.Location.Deg2rad(longitude);

            var mapPoint = new utility.Location.MapPoint { Latitude = latitude, Longitude = longitude };

            var boundingBox = utility.Location.GetBoundingBox(mapPoint, radius);

            var meridian180condition = " AND ";

            if (boundingBox.MinPoint.Longitude > boundingBox.MaxPoint.Longitude) 
            {
                meridian180condition = " OR ";
            }

            var q = "SELECT * FROM TriperooHotels WHERE (RADIANS(locationCoordinates.latitude) >= " + boundingBox.MinPoint.Latitude + " and RADIANS(locationCoordinates.latitude) <= " + boundingBox.MaxPoint.Latitude + ") and ";
            q += "(RADIANS(locationCoordinates.longitude) >= " + boundingBox.MinPoint.Longitude + meridian180condition + " RADIANS(locationCoordinates.longitude) <= " + boundingBox.MaxPoint.Longitude + ")";
            q += " AND acos(sin( RADIANS(" + degLat + ")) * sin (RADIANS(locationCoordinates.latitude)) + cos( RADIANS(" + degLat + " )) ";
            q += " * cos(RADIANS(locationCoordinates.latitude)) * cos (RADIANS(locationCoordinates.longitude) - RADIANS( " + degLon + "))) <= " + radius / 6371.0;

            return ProcessQuery(q);
        }

        /// <summary>
        /// Process Query
        /// </summary>
        private List<HotelDto> ProcessQuery(string q)
        {
            return _couchbaseHelper.ReturnQuery<HotelDto>(q, _bucketName);
        }

        /// <summary>
        /// Return a single hotel By id
        /// </summary>
        public HotelDetailDto ReturnHotelById(string guid)
        {
            if (!guid.Contains("hotel"))
            {
                guid = "hotel:" + guid;
            }

            //TODO: Change query so its based upon latitude & longitude
            var q = "SELECT * FROM " + _bucketName + " WHERE Reference = '" + guid + "'";

            var result = ProcessQuery(q);

            if (result.Count > 0)
            {
                return result[0].TriperooHotels;
            }

            return null;
        }
    }
}
