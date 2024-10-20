using Microsoft.ML;
using Microsoft.ML.Data;
using FoodDelivery.Models.ViewModels;
using System.Collections.Generic;

namespace FoodDelivery.Services
{
    public class OrderPredictionService
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public OrderPredictionService()
        {
            _mlContext = new MLContext();
        }

        public void TrainModel(List<ProductSoldViewModel> salesData)
        {
            var dataView = _mlContext.Data.LoadFromEnumerable(salesData);

            // Define the data process pipeline
            var pipeline = _mlContext.Transforms.Concatenate("Features", "TotalQuantity")
                          .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Profit"));

            // Train the model
            _model = pipeline.Fit(dataView);
        }

        // Predict profits for next month based on quantity
        public double PredictProfit(ProductSoldViewModel productData)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductSoldViewModel, ProfitPrediction>(_model);

            var prediction = predictionEngine.Predict(productData);

            return prediction.PredictedProfit;
        }
    }

    public class ProfitPrediction
    {
        [ColumnName("Score")]
        public float PredictedProfit { get; set; }
    }
}
