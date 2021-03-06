﻿using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Incidents
{
    public class Crime : IHasId<string>, IHasBounds, IGeocodable
    {
        public string GetId() => this.IncidentNum;

        //[Column(TypeName = "geometry")]
        public Point Point { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MinLat { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MaxLat { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MinLng { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MaxLng { get; set; }

        public GeometryAccuracy? Accuracy { get; set; }
        public GeometrySource? Source { get; set; }
        public DateTime? LastGeocodeAttempt { get; set; }

        [MaxLength(20)]
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string IncidentNum { get; set; }

        public DateTime ReportedDateTime { get; set; }
        public decimal ReportedYear { get; set; }
        public decimal ReportedMonth { get; set; }

        [MaxLength(128)]
        public string Location { get; set; }

        [MaxLength(128)]
        public string WeaponUsed { get; set; }

        // TODO: These floats should be ints?
        public decimal ALD { get; set; }
        public decimal NSP { get; set; }
        public decimal POLICE { get; set; }
        public decimal TRACT { get; set; }
        public decimal WARD { get; set; }
        public decimal ZIP { get; set; }
        public double RoughX { get; set; }
        public double RoughY { get; set; }
        public int Arson { get; set; }
        public int AssaultOffense { get; set; }
        public int Burglary { get; set; }
        public int CriminalDamage { get; set; }
        public int Homicide { get; set; }
        public int LockedVehicle { get; set; }
        public int Robbery { get; set; }
        public int SexOffense { get; set; }
        public int Theft { get; set; }
        public int VehicleTheft { get; set; }
    }
}
