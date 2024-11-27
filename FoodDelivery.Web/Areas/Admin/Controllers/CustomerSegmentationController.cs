using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerSegmentationController : Controller
    {
        private readonly string connectionString = "Data Source=.;Initial Catalog=FoodDel;Integrated Security=True;Encrypt=False;";

        public IActionResult Index()
        {
            DataTable segmentationResults = new DataTable();
            Dictionary<string, int> clusterDistribution = new Dictionary<string, int>();
            Dictionary<string, int> churnDistribution = new Dictionary<string, int>();
            Dictionary<string, int> lifetimeValueDistribution = new Dictionary<string, int>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Fetch segmentation results
                    string query = "SELECT * FROM CustomerSegmentationResults";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(segmentationResults);

                    // Calculate Cluster Distribution
                    clusterDistribution = segmentationResults.AsEnumerable()
                        .GroupBy(row => row["Segment"])
                        .ToDictionary(group => group.Key.ToString(), group => group.Count());

                    // Calculate Churn Risk Distribution
                    churnDistribution = segmentationResults.AsEnumerable()
                        .GroupBy(row => row["ChurnRisk"].ToString())
                        .ToDictionary(group => group.Key, group => group.Count());

                    // Calculate Lifetime Value Distribution
                    lifetimeValueDistribution = new Dictionary<string, int>
                    {
                        { "< $1000", segmentationResults.AsEnumerable().Count(row => Convert.ToDouble(row["LifetimeValue"]) < 1000) },
                        { "$1000-$5000", segmentationResults.AsEnumerable().Count(row => Convert.ToDouble(row["LifetimeValue"]) >= 1000 && Convert.ToDouble(row["LifetimeValue"]) <= 5000) },
                        { "> $5000", segmentationResults.AsEnumerable().Count(row => Convert.ToDouble(row["LifetimeValue"]) > 5000) }
                    };
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error fetching segmentation results: {ex.Message}";
                return View(new DataTable());
            }

            ViewData["ClusterDistribution"] = clusterDistribution.Values.ToList();
            ViewData["ChurnDistribution"] = churnDistribution.Values.ToList();
            ViewData["LifetimeValueDistribution"] = lifetimeValueDistribution.Values.ToList();

            return View(segmentationResults);
        }

        [HttpPost]
        public IActionResult ProcessSegmentation()
        {
            try
            {
                // Call the Python script
                var pythonService = new FoodDelivery.Services.PythonScriptService();
                pythonService.RunPythonScript("apply_model.py");
                TempData["Message"] = "Customer segmentation process completed successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Segmentation process failed: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
