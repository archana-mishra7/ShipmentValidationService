using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using ShipmentValidation.Api.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ShipmentValidation.Api.Dtos;
using System.Threading.Tasks;
using System;

namespace ShipmentValidation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentValidationController : ControllerBase
    {
        private  List<Shipment> _shipments = new List<Shipment>();
        private List<ShipmentErrorLog>  _errorLogs = new List<ShipmentErrorLog>();
        bool isRecordBad = false;
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
           try{
                
                using (var reader = new StreamReader("C:\\Users\\archana.mishra\\Documents\\SampleData\\ShipmentFormat.csv"))
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
                    csv.Configuration.ReadingExceptionOccurred = new System.Func<CsvHelperException, bool> (HandleReadException);
                    while (csv.Read()){
                        
                        var record = csv.GetRecord<Shipment>();
                        if (!(record == null))
                        {
                            //good.Add(record);
                            _shipments.Add(record);
                        }

                        isRecordBad = false;
                    }
		
                   //_shipments= csv.GetRecords<Shipment>().ToList();                   

                }
            }
            catch(CsvHelperException ex)
            {
                var errorLog = new ShipmentErrorLog();
                errorLog.ErrorType = "Shipment load Error";
                errorLog.ErrorValue = ex.Message + ex.InnerException;
                _errorLogs.Add(errorLog);
            }
            
        }

        private bool HandleReadException(CsvHelperException arg)
        {
            //isRecordBad = true;
            var errorLog = new ShipmentErrorLog();
            errorLog.ErrorType =arg.Message;
            if(!(arg.InnerException == null))
            errorLog.ErrorValue = arg.InnerException.ToString();
            _errorLogs.Add(errorLog);
            return false;
        }

        [HttpGet]
        public IEnumerable<Shipment> GetAllShipments(bool includeError = false)
        {
            LoadShipments();
           
            //if(_shipments.Count>0)
            return _shipments;
        } 

        
        [HttpGet("{error}")]
        public IEnumerable<ShipmentErrorLog> GetAllErrors(string error)
        {
            LoadShipments();
           
            //if(_shipments.Count>0)
            return _errorLogs;
        } 
        
    }
}