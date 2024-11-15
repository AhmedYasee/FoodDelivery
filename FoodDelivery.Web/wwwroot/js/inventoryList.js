var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "responsive": true,
        "ajax": {
            url: '/Admin/Inventory/GetInventoryItems',  // API endpoint to get items with quantity > 0
            dataSrc: 'data'
        },
        "columns": [
            { data: 'name', "width": "15%" },  // Item Name
            { data: 'typeName', "width": "10%" },  // Type Name (updated)
            { data: 'categoryName', "width": "10%" },  // Category Name (updated)
            { data: 'unitOfMeasurementName', "width": "10%" },  // Unit of Measurement Name (updated)
            { data: 'batchNumber', "width": "10%" },  // Batch Number
            { data: 'quantity', "width": "10%" },  // Quantity
            {
                data: 'expirationDate',
                "render": function (data) {
                    return data ? new Date(data).toLocaleDateString() : 'N/A';  // Format expiration date
                },
                "width": "10%"
            },  // Expiration Date
            { data: 'reorderLevel', "width": "10%" },  // Reorder Level
            {
                data: 'productID',  // Product Image
                "render": function (data) {
                    return `<span id="display-images" onclick=DisplayImages('${data}') style="cursor: pointer; text-decoration: underline;">View</span>`;
                },
                "width": "10%"
            },
            {
                data: 'productID',  // Actions (Edit/Delete)
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/admin/inventory/upsert/${data}" class="btn btn-primary mr-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                                <a onClick=Delete('/admin/inventory/delete/${data}') class="btn btn-danger "> <i class="bi bi-trash-fill"></i> Delete</a>
                            </div>`;
                },
                "width": "15%"
            }
        ]
    });
}


function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                },
                error: function (data) {
                    toastr.error(data.message);
                }
            })
        }
    })
}

function DisplayImages(id) {
    $.ajax({
        url: "/Admin/Inventory/GetImages/" + id,
        success: function (result) {
            $("#show-images").html(result);
        }
    });
}

document.addEventListener("click", function (e) {
    if (e.target.getAttribute('id') == 'display-images') {
        globalThis.scrollTo(0, 0);
    }
    if (e.target.className == "bi bi-x") {
        let parent = e.target.parentElement.parentElement;
        parent.remove();
    }
});
