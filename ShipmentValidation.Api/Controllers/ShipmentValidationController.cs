using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using ShipmentValidation.Api.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ShipmentValidation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentValidationController : ControllerBase
    {
        private  List<Shipment> _shipments;
        //public ShipmentValidationController(List<Shipment> shipments)
        //{
         //   _shipments = shipments;

        //}
        /// Load shipments from file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private void LoadShipments()
        {
            using (var reader = new StreamReader("C:\\Users\\archana.mishra\\Documents\\SampleData\\Shipment_NoError.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                _shipments = csv.GetRecords<Shipment>().ToList();
            }
        }

        [HttpGet]
        public IEnumerable<Shipment> Get()
        {
            using (var reader = new StreamReader("C:\\Users\\archana.mishra\\Documents\\SampleData\\Shipment_NoError.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                _shipments = csv.GetRecords<Shipment>().ToList();                
            }
            return _shipments; 

        }



    }
}