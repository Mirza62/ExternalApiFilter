using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonateKartWebApiProject.Models
{
    public class CampaignResult
    {
        public double totalAmount { get; set; }
        public string title { get; set; }
        public double backersCount { get; set; }
        public DateTime endDate { get; set; }
    }
}
