using System;
using ServiceStack.Text;

namespace library.wikipedia.dtos
{
    public class PageDto
	{
		public int pageid { get; set; }
		public int ns { get; set; }
		public string title { get; set; }
		public string extract { get; set; }
	}

	public class Query
	{
		public JsonObject pages { get; set; }
	}

	public class ContentDto
	{
		public string batchcomplete { get; set; }
		public Query query { get; set; }
	}
}
