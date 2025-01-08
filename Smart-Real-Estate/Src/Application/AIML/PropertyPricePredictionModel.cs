// File: ../Application/AIML/PropertyPricePredictionModel.cs
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Org.BouncyCastle.Ocsp;
using System.Collections.Generic;

namespace Application.AIML
{
    public class PropertyPricePredictionModel
    {
        private readonly MLContext mlContext;
        private ITransformer? model;

        public PropertyPricePredictionModel()
        {
            mlContext = new MLContext();
        }

        public void Train(List<PropertyData> trainingData, int numberOfTrees = 100)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(trainingData);
            // Split the data into training and test sets
            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = split.TrainSet;
            var testData = split.TestSet;

            var pipeline = mlContext.Transforms.CopyColumns("Label", nameof(PropertyData.Price))
                .Append(mlContext.Transforms.Conversion.ConvertType("Label", outputKind: DataKind.Single))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("AddressEncoded", nameof(PropertyData.Address)))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("DescriptionEncoded", nameof(PropertyData.Description)))
                .Append(mlContext.Transforms.Conversion.ConvertType(nameof(PropertyData.Surface), outputKind: DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType(nameof(PropertyData.Rooms), outputKind: DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType(nameof(PropertyData.Floor), outputKind: DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType(nameof(PropertyData.Year), outputKind: DataKind.Single))
                .Append(mlContext.Transforms.Conversion.ConvertType(nameof(PropertyData.Parking), outputKind: DataKind.Single))
                .Append(mlContext.Transforms.Concatenate("Features", nameof(PropertyData.Surface), nameof(PropertyData.Rooms), nameof(PropertyData.Floor), nameof(PropertyData.Year), nameof(PropertyData.Parking), "AddressEncoded", "DescriptionEncoded"))
                .Append(mlContext.Regression.Trainers.FastTree(new FastTreeRegressionTrainer.Options
                {
                    NumberOfTrees = numberOfTrees
                }));

            model = pipeline.Fit(dataView);
            // Evaluate the model on the test set
            var predictions = model.Transform(testData);
            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
            // Output the evaluation metrics
            Console.WriteLine($"R^2: {metrics.RSquared:0.##}");
            Console.WriteLine($"RMS: {metrics.RootMeanSquaredError:0.##}");
        }
        public float Predict(PropertyData propertyData)
        {
            if (model == null)
            {
                throw new InvalidOperationException("Model has not been trained.");
            }

            var predictionEngine = mlContext.Model.CreatePredictionEngine<PropertyData, PropertyPricePrediction>(model);
            var prediction = predictionEngine.Predict(propertyData);
            return prediction.Price;
        }

        public class PropertyPricePrediction
        {
            [ColumnName("Score")]
            public float Price { get; set; }
        }
    }
}




















