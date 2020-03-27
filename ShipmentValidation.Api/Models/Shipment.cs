using System;

namespace ShipmentValidation.Api.Models
{
    
    public class Shipment
    {
        public Location ShipmentOrigin { get; set; }
        public Location ShipmentDestination { get; set; }
        public DateTime PickUpDate { get; set; }
        public decimal Weight { get; set; }
        public decimal Cube { get; set; }
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