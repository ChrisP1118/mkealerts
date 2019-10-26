﻿using GeoAPI.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Places
{
    public class ParcelDTO
    {
        [MaxLength(10)]
        public string TAXKEY { get; set; }

        public Guid CommonParcelId { get; set; }
        public CommonParcelDTO CommonParcel { get; set; }
    }
}
