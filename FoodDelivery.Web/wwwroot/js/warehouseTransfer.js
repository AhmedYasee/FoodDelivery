document.addEventListener("DOMContentLoaded", () => {
    const sourceBranch = $("#sourceBranch");
    const sourceWarehouse = $("#sourceWarehouse");
    const destinationBranch = $("#destinationBranch");
    const destinationWarehouse = $("#destinationWarehouse");

    let itemsData = []; // Stores unique item names for the warehouse

    // Function to load branches with warehouses
    function loadBranches() {
        $.get("/Admin/Branch/GetAllBranchesWithWarehouses", (branches) => {
            sourceBranch.empty().append(`<option value="">Select Branch</option>`);
            destinationBranch.empty().append(`<option value="">Select Branch</option>`);

            branches.forEach((branch) => {
                sourceBranch.append(
                    `<option value="${branch.branchId}">${branch.branchName}</option>`
                );
                destinationBranch.append(
                    `<option value="${branch.branchId}">${branch.branchName}</option>`
                );
            });
        }).fail(() => {
            console.error("Failed to load branches.");
        });
    }

    // Function to load warehouses for a specific branch
    function loadWarehouses(branchId, warehouseDropdown) {
        warehouseDropdown.empty().append(`<option value="">Select Warehouse</option>`);
        if (!branchId) return;

        $.get(`/Admin/Warehouse/GetWarehousesByBranch?branchId=${branchId}`, (warehouses) => {
            warehouses.forEach((warehouse) => {
                warehouseDropdown.append(
                    `<option value="${warehouse.warehouseId}">${warehouse.warehouseName}</option>`
                );
            });
        }).fail(() => {
            console.error("Failed to load warehouses for branch ID:", branchId);
        });
    }

    // Function to load unique items in the selected warehouse
    function loadWarehouseItems(warehouseId) {
        if (!warehouseId) return;

        $.get(`/Admin/WarehouseTransfer/GetWarehouseItems?warehouseId=${warehouseId}`, (data) => {
            itemsData = data; // Store unique items for the warehouse
            $(".item-select").each(function () {
                $(this).empty().append(`<option value="">Select Item</option>`);
                data.forEach((item) => {
                    $(this).append(
                        `<option value="${item.productId}">${item.productName}</option>`
                    );
                });
            });
        }).fail(() => {
            console.error("Failed to load warehouse items for warehouse ID:", warehouseId);
        });
    }

    // Function to load batches for a selected item
    function loadBatches(warehouseId, productId, batchDropdown) {
        batchDropdown.empty().append(`<option value="">Select Batch</option>`);
        if (!warehouseId || !productId) return;

        $.get(
            `/Admin/WarehouseTransfer/GetBatchesByItem?warehouseId=${warehouseId}&productId=${productId}`,
            (batches) => {
                batches.forEach((batch) => {
                    batchDropdown.append(
                        `<option value="${batch.batchNumber}">${batch.batchNumber} (Qty: ${batch.quantity})</option>`
                    );
                });
            }
        ).fail(() => {
            console.error(
                "Failed to load batches for warehouse ID:",
                warehouseId,
                "and product ID:",
                productId
            );
        });
    }



    // Event: Source branch changes
    sourceBranch.change(function () {
        const branchId = $(this).val();
        loadWarehouses(branchId, sourceWarehouse);
    });

    // Event: Destination branch changes
    destinationBranch.change(function () {
        const branchId = $(this).val();
        loadWarehouses(branchId, destinationWarehouse);
    });

    // Event: Source warehouse changes
    sourceWarehouse.change(function () {
        const warehouseId = $(this).val();
        loadWarehouseItems(warehouseId);
    });

    // Event: Item selection changes to load corresponding batches
    $("#transferItemsContainer").on("change", ".item-select", function () {
        const rowId = $(this).attr("id").split("_")[1]; // Get row index
        const selectedProductId = $(this).val();
        const warehouseId = sourceWarehouse.val();
        const batchDropdown = $(`#batchSelect_${rowId}`);
        loadBatches(warehouseId, selectedProductId, batchDropdown);
    });

    // Add new transfer rows dynamically
    $("#transferItemsContainer").on("click", ".add-transfer-item", function () {
        const newIndex = $(".transfer-item").length;
        $("#transferItemsContainer").append(`
            <div class="transfer-item row mb-3">
                <div class="col-md-3">
                    <label for="itemSelect_${newIndex}" class="form-label">Item</label>
                    <select id="itemSelect_${newIndex}" class="form-select item-select" required></select>
                </div>
                <div class="col-md-3">
                    <label for="batchSelect_${newIndex}" class="form-label">Batch</label>
                    <select id="batchSelect_${newIndex}" class="form-select batch-select" required></select>
                </div>
                <div class="col-md-3">
                    <label for="quantity_${newIndex}" class="form-label">Quantity</label>
                    <input type="number" id="quantity_${newIndex}" class="form-control transfer-quantity" min="1" required />
                </div>
                <div class="col-md-3 d-flex align-items-end">
                    <button type="button" class="btn btn-danger remove-transfer-item">-</button>
                </div>
            </div>
        `);

        const warehouseItems = itemsData;
        $(`#itemSelect_${newIndex}`).empty().append(`<option value="">Select Item</option>`);
        warehouseItems.forEach((item) => {
            $(`#itemSelect_${newIndex}`).append(
                `<option value="${item.productId}">${item.productName}</option>`
            );
        });
    });

    // Remove transfer rows dynamically
    $("#transferItemsContainer").on("click", ".remove-transfer-item", function () {
        $(this).closest(".transfer-item").remove();
    });

    // Submit form
    $("#transferForm").on("submit", function (e) {
        e.preventDefault();

        const sourceWarehouseId = sourceWarehouse.val();
        const destinationWarehouseId = destinationWarehouse.val();

        if (!sourceWarehouseId || !destinationWarehouseId) {
            toastr.error("Please select both source and destination warehouses.");
            return;
        }

        const transferItems = [];
        $(".transfer-item").each(function () {
            const productId = $(this).find(".item-select").val();
            const batchNumber = $(this).find(".batch-select").val();
            const quantity = $(this).find(".transfer-quantity").val();

            if (!productId || !batchNumber || !quantity || quantity <= 0) {
                toastr.error("Please complete all fields for each transfer item.");
                return false;
            }

            transferItems.push({
                productId: parseInt(productId),
                batchNumber: batchNumber,
                quantity: parseInt(quantity),
            });
        });

        if (transferItems.length === 0) {
            toastr.error("No valid transfer items to submit.");
            return;
        }

        $.ajax({
            url: "/Admin/WarehouseTransfer/SubmitTransfer",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                sourceWarehouseId: parseInt(sourceWarehouseId),
                destinationWarehouseId: parseInt(destinationWarehouseId),
                transferItems: transferItems,
            }),
            success: function (response) {
                console.log("Server response:", response); // Debugging: Log the response
                toastr.success(response); // Show success notification
                setTimeout(() => {
                    location.reload(); // Reload the page after a slight delay
                }, 2000); // Optional delay for user to read the notification
            },
            error: function (xhr) {
                console.error("Error response:", xhr.responseText); // Debugging: Log the error response
                toastr.error(xhr.responseText || "Failed to submit transfer."); // Show error notification
            },
        });
    });



    // Load branches on page load
    loadBranches();
});
