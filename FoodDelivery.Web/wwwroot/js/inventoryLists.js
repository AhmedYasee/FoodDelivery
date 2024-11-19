var dataTable;

$(document).ready(function () {
    loadDataTable();
});

// Function to load the DataTable with inventory items
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "responsive": true,
        "ajax": {
            url: '/Admin/Inventory/GetInventoryItems',
            type: 'GET',
            dataSrc: 'data'
        },
        "columns": [
            { data: 'productName', "width": "15%" },   // Product Name
            { data: 'typeName', "width": "10%" },      // Type
            { data: 'categoryName', "width": "10%" },  // Category
            { data: 'unitOfMeasurementName', "width": "10%" }, // Unit of Measurement
            { data: 'batchNumber', "width": "10%" },   // Batch Number
            { data: 'quantity', "width": "10%" },      // Quantity
            {
                data: 'expirationDate',
                "render": function (data) {
                    return data ? new Date(data).toLocaleDateString() : 'N/A';
                },
                "width": "10%"
            },  // Expiration Date
            { data: 'reorderLevel', "width": "10%" },  // Reorder Level
            { data: 'warehouseName', "width": "10%" }  // Warehouse
        ]
    });
}


