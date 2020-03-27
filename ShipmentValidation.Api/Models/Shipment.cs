using System;
using CsvHelper.Configuration.Attributes;

namespace ShipmentValidation.Api.Models
{
    
    public class Shipment
    {
        [Name("Shipment Origin")]
        public Location ShipmentOrigin { get; set; }
        [Name("Shipment Destination")]
        public Location ShipmentDestination { get; set; }
        [Name("Pickup Date")]
        public DateTime PickUpDate { get; set; }
        [Name("Weight")]
        public decimal Weight { get; set; }
        [Name("Cube")]
        public decimal Cube { get; set; }
        [Name("Service Type")]
        public ServiceType serviceType { get; set; }
    }

    public enum Location{
        none,
        ABL,
        CCI,
        CGO,
        CLV,
        CMB,
        CRD
    }
    public enum ServiceType{
        none,
        Standard
    }
}