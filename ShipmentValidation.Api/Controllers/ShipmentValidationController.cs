using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using ShipmentValidation.Api.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ShipmentValidation.Api.Dtos;
using System.Threading.Tasks;

namespace ShipmentValidation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentValidationController : ControllerBase
    {
        private  List<Shipment> _shipments;
        private List<ShipmentErrorLog>  _errorLogs = new List<ShipmentErrorLog>();
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
            try
            {
                bool isRecordBad = false;
                using (var reader = new StreamReader("C:\\Users\\archana.mishra\\Documents\\SampleData\\ShipmentFile.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    
                    csv.Configuration.BadDataFound = context =>
                        {
                            isRecordBad = true;

                            var errorLog = new ShipmentErrorLog();
                            errorLog.ErrorType = "File Error";
                            errorLog.ErrorValue = context.RawRecord;
                            _errorLogs.Add(errorLog);
                        };
                    csv.Configuration.HeaderValidated = (isValid, headerNames, headerNameIndex, context) =>
                    {
                        foreach (var header in headerNames)
                        {
                            if (!isValid)
                            {
                                isRecordBad = true;
                                var errorLog = new ShipmentErrorLog();
                                errorLog.ErrorType = "File header Error in "  + header;
                                errorLog.ErrorValue =  header + " is either missing or wrong entry";
                                _errorLogs.Add(errorLog);
                            }
                        }

                    } ;
                   _shipments= csv.GetRecords<Shipment>().ToList();                   

                }
            }
            catch(CsvHelperException ex)
            {
                var errorLog = new ShipmentErrorLog();
                errorLog.ErrorType = "Shipment load Error";
                errorLog.ErrorValue = ex.Message;
                _errorLogs.Add(errorLog);
            }
            
        }

        [HttpGet]
        public IActionResult GetAllShipments(bool includeError = false)
        {
            LoadShipments();
            if(_errorLogs.Count>0)
            return Ok(_errorLogs);
            
            return Ok(_shipments);
        }  
        
    }
}