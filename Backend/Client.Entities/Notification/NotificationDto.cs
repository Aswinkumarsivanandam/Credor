using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }        
        public string Title { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
        public string CreatedByFirstName { get; set; }
        public string CreatedByLastName { get; set; }
        public string CreatedByProfileImageURL { get; set; }
        public DateTime CreatedOn { get; set; }
        public double DaysDifference { get; set; }
        public int MonthDifference { get; set; }
        public string DisplayTime { get; set; }
        public string CreatedBy { get; set; }
        public bool IsCreatedByAdmin { get; set; }

    }
}
