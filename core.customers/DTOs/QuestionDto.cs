using library.common;
using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class QuestionDetailDto
    {
        public QuestionDetailDto()
        {
            Answers = new List<AnswerDto>();
        }

        public string Type { get { return "question"; } }
        public string QuestionReference { get; set; }
		public string CustomerReference { get; set; }
		public string CustomerImageUrl { get; set; }
        public int InventoryReference { get; set; }
        public string Question { get; set; }
        public bool IsArchived { get; set; }
        public DateTime DateCreated { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }

	public class AnswerDto
	{
		public string QuestionReference { get; set; }
		public string CustomerReference { get; set; }
		public string CustomerImageUrl { get; set; }
		public DateTime DateCreated { get; set; }
		public string Answer { get; set; }
		public int LikeCount { get; set; }
		public bool IsArchived { get; set; }
	}

    public class QuestionDto
	{
		public QuestionDto()
		{
			Answers = new List<AnswerDto>();
		}

        public string customerImageUrl { get; set; }
        public string CustomerName { get; set; }
        public string CustomerProfileUrl { get; set; }
        public string CustomerReference { get; set; }
        public DateTime DateCreated { get; set; }
        public string FriendlyDate
        {
            get
            {
                return DateHelper.TimeAgo(DateCreated);
            }
        }
        public int InventoryReference { get; set; }
        public bool IsArchived { get; set; }
        public string Question { get; set; }
        public string QuestionReference { get; set; }
		public string Type { get; set; }
		public List<AnswerDto> Answers { get; set; }
    }


}
