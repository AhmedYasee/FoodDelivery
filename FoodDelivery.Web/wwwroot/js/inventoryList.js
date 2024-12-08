$(document).ready(function () {
    $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Inventory/GetInventoryItems",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "productName" },
            { "data": "typeName" },
            { "data": "categoryName" },
            { "data": "unitOfMeasurementName" },
            { "data": "batchNumber" },
            { "data": "quantity" },
            {
                "data": "expirationDate",
                "render": function (data) {
                    if (typeof moment !== 'undefined' && data) {
                        return moment(data).format('YYYY-MM-DD');
                    } else {
                        return data ? new Date(data).toISOString().split('T')[0] : "N/A";
                    }
                }

            },
            { "data": "reorderLevel" },
            { "data": "warehouseName" },
            { "data": "branchName" }
        ],
        "language": {
            "emptyTable": "No inventory items available",
            "loadingRecords": "Loading...",
            "processing": "Processing...",
            "lengthMenu": "Show _MENU_ entries",
            "zeroRecords": "No matching items found",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoEmpty": "No entries available",
            "infoFiltered": "(filtered from _MAX_ total entries)"
        },
        "processing": true,
        "serverSide": false,
        "order": [[0, "asc"]],
        "paging": true,
        "searching": true,
        "autoWidth": false,
        "responsive": true
    });
});
