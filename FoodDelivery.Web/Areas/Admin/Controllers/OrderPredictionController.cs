using FoodDelivery.Models.ViewModels;
using FoodDelivery.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using FoodDelivery.Repository.Data;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderPredictionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderPredictionService _predictionService;

        public OrderPredictionController(ApplicationDbContext context, OrderPredictionService predictionService)
        {
            _context = context;
            _predictionService = predictionService;
        }

        // GET: AI Dashboard
        public IActionResult Dashboard()
        {
            // Get current month's sales data along with product prices
            var currentMonthSales = _context.OrderDetails
                .Where(od => od.OrderHeader.OrderDate.Month == DateTime.Now.Month && od.OrderHeader.OrderDate.Year == DateTime.Now.Year)
                .GroupBy(od => new { od.ProductId, od.Product.Name, od.Product.Price }) // Include product price
                .Select(group => new ProductSoldViewModel
                {
                    ProductName = group.Key.Name,
                    TotalQuantity = group.Sum(g => g.Quantity),
                    Profit = (float)group.Sum(g => g.Quantity * group.Key.Price) // Explicit conversion from decimal to double
                }).ToList();

            // Check if there is any data
            if (!currentMonthSales.Any())
            {
                ViewBag.ErrorMessage = "No sales data available for this month.";
                return View();
            }

            // Train the model using the current month's sales data
            _predictionService.TrainModel(currentMonthSales);

            // Forecast next month's profits based on the model
            var forecastedProfits = ForecastNextMonthProfits(currentMonthSales);

            // Prepare data for the chart
            ViewBag.CurrentMonthProfits = currentMonthSales.Select(p => p.Profit).ToList();
            ViewBag.ForecastedProfits = forecastedProfits.Select(p => p.Profit).ToList();
            ViewBag.ProductNames = currentMonthSales.Select(p => p.ProductName).ToList();

            // Pass the product data to the view
            ViewBag.ProductsSold = currentMonthSales;

            return View();
        }

        // Forecast next month's profits using the AI model
        private List<ProductSoldViewModel> ForecastNextMonthProfits(List<ProductSoldViewModel> currentMonthSales)
        {
            var forecastedProfits = new List<ProductSoldViewModel>();

            foreach (var product in currentMonthSales)
            {
                // Use the AI model to predict next month's profit based on quantity
                var predictedProfit = _predictionService.PredictProfit(product);

                forecastedProfits.Add(new ProductSoldViewModel
                {
                    ProductName = product.ProductName,
                    TotalQuantity = product.TotalQuantity,
                    Profit = (float)predictedProfit
                });
            }

            return forecastedProfits;
        }
    }
}
