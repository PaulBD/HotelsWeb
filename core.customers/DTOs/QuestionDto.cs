using library.common;
using System;

namespace core.customers.dtos
{
    public class QuestionDetailDto
    {
        public string Type { get { return "question"; } }
        public string QuestionReference { get; set; }
        public string CustomerReference { get; set; }
        public int InventoryReference { get; set; }
        public string Question { get; set; }
        public bool IsArchived { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class QuestionDto
    {
        public string CustomerImageUrl { get; set; }
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
    }


}
