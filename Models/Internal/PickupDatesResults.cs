﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Internal
{
    public class PickupDatesResults
    {
        public DateTime? NextGarbagePickupDate { get; set; }
        public DateTime? NextRecyclingPickupDate { get; set; }
    }
}
