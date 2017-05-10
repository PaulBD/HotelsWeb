using Newtonsoft.Json;
using System.Collections.Generic;

namespace core.extras.dtos
{
	public class LoungeAttributes
	{
		public string Product { get; set; }
		public int RequestCode { get; set; }
		public string Result { get; set; }
		public bool cached { get; set; }
		public string expires { get; set; }
	}

	public class LoungeFilter
	{
		public int contact_centre_lounge_notes { get; set; }
		public string landside { get; set; }
	}


	public class LoungeImages
	{
	    public List<object> image { get; set; }
	}

    public class Lounge
	{
		public List<object> Attributes { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string BookingURL { get; set; }
		public string MoreInfoURL { get; set; }
		public double NonDiscPrice { get; set; }
		public double Price { get; set; }
		public string Terminal { get; set; }
		public string Logo { get; set; }
		public string TripappImages { get; set; }
		public LoungeImages Images { get; set; }
		public int Option_count { get; set; }
		public string Introduction { get; set; }
		public string OpeningTime { get; set; }
		public string ClosingTime { get; set; }
		public string Why_BookOne { get; set; }
		public string Address { get; set; }
		public string[] TripappImagesList
		{
			get
			{
				if (!string.IsNullOrEmpty(TripappImages))
				{
					return TripappImages.Split(';');
				}

				return null;
			}
		}
	}

	public class LoungeCancellationWaiver
	{
		public double Waiver { get; set; }
	}

	public class LoungeRates
	{
		public int GBP { get; set; }
		public double EUR { get; set; }
		public double USD { get; set; }
	}

	public class LoungePricing
	{
		public int CCardSurchargePercent { get; set; }
		public string CCardSurchargeMin { get; set; }
		public int CCardSurchargeMax { get; set; }
		public string DCardSurchargePercent { get; set; }
		public string DCardSurchargeMin { get; set; }
		public string DCardSurchargeMax { get; set; }
		public List<LoungeCancellationWaiver> CancellationWaiver { get; set; }
		public LoungeRates Rates { get; set; }
	}

	public class LoungeRequest
	{
		public string ABTANumber { get; set; }
		public string ArrivalDate { get; set; }
		public string ArrivalTime { get; set; }
		public int Adults { get; set; }
		public int Children { get; set; }
		public string key { get; set; }
		public int token { get; set; }
		public int v { get; set; }
		public string format { get; set; }
	}

	public class LoungeAPIHeader
	{
		public LoungeRequest Request { get; set; }
	}

	public class LoungeApiError
	{
		public string Code { get; set; }
		public string Message { get; set; }
	}

	public class LoungeAPIReply
	{
		public LoungeAPIReply()
		{
			Attributes = new LoungeAttributes();
			API_Header = new LoungeAPIHeader();
			Lounge = new List<Lounge>();
			Pricing = new LoungePricing();
			Error = new LoungeApiError();
		}

		public LoungeApiError Error { get; set; }
		public LoungeAttributes Attributes { get; set; }
		public List<Lounge> Lounge { get; set; }
		public LoungePricing Pricing { get; set; }
		public string SepaID { get; set; }
		public LoungeAPIHeader API_Header { get; set; }
	}

	public class LoungeAvailabilityResponseDto
	{
		public LoungeAPIReply API_Reply { get; set; }
	}
}
