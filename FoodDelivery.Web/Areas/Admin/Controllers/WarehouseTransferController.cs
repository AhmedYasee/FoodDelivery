using FoodDelivery.Models;
using FoodDelivery.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WarehouseTransferController : Controller
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IInventoryListRepository _inventoryListRepository;

        public WarehouseTransferController(
            IBranchRepository branchRepository,
            IWarehouseRepository warehouseRepository,
            IInventoryListRepository inventoryListRepository)
        {
            _branchRepository = branchRepository;
            _warehouseRepository = warehouseRepository;
            _inventoryListRepository = inventoryListRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Modules/Inventory/WarehouseTransfer/Index.cshtml");
        }

        [HttpGet]
        public IActionResult GetBranches()
        {
            var branches = _branchRepository.GetAll()
                .Select(b => new
                {
                    b.Id,
                    b.BranchName,
                    Warehouses = b.Warehouses.Select(w => new { w.Id, w.WarehouseName })
                }).ToList();

            return Json(branches);
        }

        [HttpGet]
        public IActionResult GetWarehouseItems(int warehouseId)
        {
            var items = _inventoryListRepository.GetByWarehouse(warehouseId)
                .GroupBy(i => i.Product.Name)
                .Select(g => new
                {
                    ProductId = g.First().ProductID, // Use the first item's ProductId
                    ProductName = g.Key,            // Group key is the product name
                    TotalQuantity = g.Sum(i => i.Quantity) // Sum quantities for each product
                })
                .Where(item => item.TotalQuantity > 0) // Exclude items with zero quantity
                .ToList();

            return Json(items);
        }


        [HttpGet]
        public IActionResult GetBatchesByItem(int warehouseId, int productId)
        {
            // Fetch batches for the selected item in the warehouse
            var batches = _inventoryListRepository.GetByWarehouse(warehouseId)
                .Where(i => i.ProductID == productId)
                .Select(i => new
                {
                    BatchNumber = i.BatchNumber,
                    Quantity = i.Quantity
                })
                .Where(i => i.Quantity > 0) // Exclude items with zero quantity
                .ToList();

            return Json(batches);
        }


        [HttpPost]
        public IActionResult SubmitTransfer([FromBody] WarehouseTransferViewModel transferData)
        {
            if (transferData == null || !transferData.TransferItems.Any())
            {
                Console.WriteLine("No transfer items provided.");
                return BadRequest("No transfer items provided.");
            }

            Console.WriteLine($"Processing transfer: SourceWarehouseId={transferData.SourceWarehouseId}, DestinationWarehouseId={transferData.DestinationWarehouseId}");

            foreach (var item in transferData.TransferItems)
            {
                Console.WriteLine($"Processing item: ProductId={item.ProductId}, BatchNumber={item.BatchNumber}, Quantity={item.Quantity}");

                // Fetch the source inventory item
                var sourceInventory = _inventoryListRepository.GetAllWithIncludes()
                    .FirstOrDefault(i => i.WarehouseID == transferData.SourceWarehouseId &&
                                         i.ProductID == item.ProductId &&
                                         i.BatchNumber == item.BatchNumber);

                if (sourceInventory == null)
                {
                    Console.WriteLine($"Source item not found: ProductId={item.ProductId}, BatchNumber={item.BatchNumber}");
                    return BadRequest($"Source item with Product ID {item.ProductId} and Batch {item.BatchNumber} not found.");
                }

                // Check if quantity exceeds availability
                if (sourceInventory.Quantity < item.Quantity)
                {
                    Console.WriteLine($"Insufficient quantity for ProductId={item.ProductId}, BatchNumber={item.BatchNumber}. Available={sourceInventory.Quantity}, Requested={item.Quantity}");
                    return BadRequest($"Not enough quantity for Product: {sourceInventory.Product.Name} (Batch: {item.BatchNumber}). Available: {sourceInventory.Quantity}, Requested: {item.Quantity}");
                }

                // Deduct quantity from source
                Console.WriteLine($"Deducting {item.Quantity} from SourceInventoryId={sourceInventory.InventoryListID}");
                sourceInventory.Quantity -= item.Quantity;

                // Fetch or create the destination inventory item
                var destinationInventory = _inventoryListRepository.GetAllWithIncludes()
                    .FirstOrDefault(i => i.WarehouseID == transferData.DestinationWarehouseId &&
                                         i.ProductID == item.ProductId &&
                                         i.BatchNumber == item.BatchNumber);

                if (destinationInventory != null)
                {
                    Console.WriteLine($"Updating existing destination inventory for ProductId={item.ProductId}, BatchNumber={item.BatchNumber}");
                    destinationInventory.Quantity += item.Quantity;
                }
                else
                {
                    Console.WriteLine($"Creating new destination inventory for ProductId={item.ProductId}, BatchNumber={item.BatchNumber}");
                    _inventoryListRepository.Add(new InventoryList
                    {
                        ProductID = item.ProductId,
                        WarehouseID = transferData.DestinationWarehouseId,
                        BatchNumber = item.BatchNumber,
                        Quantity = item.Quantity,
                        ExpirationDate = sourceInventory.ExpirationDate, // Use the same expiration date
                    });
                }
            }

            // Commit all changes
            Console.WriteLine("Saving changes to the database.");
            _inventoryListRepository.Save();

            Console.WriteLine("Transfer completed successfully."); // Log for debugging
            return Ok("Transfer completed successfully."); // Ensure this response is returned
        }

    }
}
