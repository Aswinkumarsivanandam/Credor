using System;
using System.Collections.Generic;
using System.Text;

namespace Credor.Client.Entities
{
    public class PortfolioReservationsSummaryDto
    {
        public int Id { get; set; }
        public int OfferingId { get; set; }
        public decimal TotalReserved { get; set; }       
        public int Reservations { get; set; }
        public decimal Converted { get; set; }
        public decimal Remaining { get; set; }        
    }
    public class ReservationsSummaryDto
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public decimal TotalReserved { get; set; }
        public decimal ReservationSize { get; set; }
        public int Reservations { get; set; }      
        public decimal Remaining { get; set; }
        public int NonAccredited { get; set; }
    }
}
