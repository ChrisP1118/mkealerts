﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.PickupDates
{
    public class PickupDatesResultsDTO
    {
        public DateTime? NextGarbagePickupDate { get; set; }
        public DateTime? NextRecyclingPickupDate { get; set; }
    }
}
