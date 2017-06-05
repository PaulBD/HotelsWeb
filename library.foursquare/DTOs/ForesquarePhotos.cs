using System;
using System.Collections.Generic;

namespace library.foursquare.dtos
{
    public class ForesquarePhotosMeta
	{
		public int code { get; set; }
		public string requestId { get; set; }
	}

	public class Source
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	public class ForesquarePhoto
	{
		public string prefix { get; set; }
		public string suffix { get; set; }
	}

	public class User
	{
		public User()
		{
			photo = new PhotoDto();
		}

		public string id { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string gender { get; set; }
		public PhotoDto photo { get; set; }
	}

	public class PhotoDto
	{
		public string prefix { get; set; }
		public string suffix { get; set; }
	}

	public class With
	{
		public With()
		{
			photo = new PhotoDto();
		}
		public string id { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string gender { get; set; }
		public PhotoDto photo { get; set; }
	}

	public class Checkin
	{
		public Checkin()
		{
			with = new List<With>();
		}

		public string id { get; set; }
		public int createdAt { get; set; }
		public string type { get; set; }
		public int timeZoneOffset { get; set; }
		public List<With> with { get; set; }
	}

	public class Item
	{
		public Item()
		{
			source = new Source();
			user = new User();
			checkin = new Checkin();
		}

		public string id { get; set; }
		public int createdAt { get; set; }
		public Source source { get; set; }
		public string prefix { get; set; }
		public string suffix { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public User user { get; set; }
		public Checkin checkin { get; set; }
		public string visibility { get; set; }
	}

	public class ForesquarePhotos
	{
		public ForesquarePhotos()
		{
			items = new List<Item>();
		}

		public int count { get; set; }
		public List<Item> items { get; set; }
		public int dupesRemoved { get; set; }
	}

	public class ForesquarePhotosResponse
	{
		public ForesquarePhotosResponse()
		{
			photos = new ForesquarePhotos();
		}

		public ForesquarePhotos photos { get; set; }
	}

	public class ForesquarePhotosDto
	{
		public ForesquarePhotosDto()
		{
			meta = new ForesquarePhotosMeta();
			response = new ForesquarePhotosResponse();
		}

		public ForesquarePhotosMeta meta { get; set; }
		public ForesquarePhotosResponse response { get; set; }
	}
}
