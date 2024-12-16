// File: Controllers/PropertyPricePredictionController.cs
using Application.AIML;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Real_Estate_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PropertyPricePredictionController : ControllerBase
    {
        private readonly PropertyPricePredictionModel propertyPricePredictionModel;

        public PropertyPricePredictionController(PropertyPricePredictionModel propertyPricePredictionModel, PropertyDataParser propertyDataParser)
        {
            this.propertyPricePredictionModel = propertyPricePredictionModel;

            string trainingDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "houses_Madrid.csv"); 


            if (System.IO.File.Exists(trainingDataFilePath))
            {
                var trainingData = propertyDataParser.Parse(trainingDataFilePath).ToList();
                propertyPricePredictionModel.Train(trainingData, numberOfTrees: 200);
            }
            else
            {
                throw new FileNotFoundException("Training data file not found.", trainingDataFilePath);
            }
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictPrice([FromBody] PropertyData propertyData)
        {
            return propertyPricePredictionModel.Predict(propertyData);
        }
    }
}


