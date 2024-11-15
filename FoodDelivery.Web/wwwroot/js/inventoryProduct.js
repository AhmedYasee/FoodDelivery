var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $.ajax({
        url: '/Admin/Product/GetAll',  // This is your endpoint for getting the product data
        success: function (data) {
            console.log(data);  // Log the returned data to inspect its structure

            // Now initialize the DataTable with the fetched data
            dataTable = $('#tblData').DataTable({
                "responsive": true,
                "data": data.data,  // Use the data returned from the AJAX call
                "columns": [
                    { data: 'name', "width": "15%" },  // Product Name
                    { data: 'category.name', "width": "15%" },  // Category Name
                    { data: 'type.typeName', "width": "15%" },  // Product Type Name
                    { data: 'unitOfMeasurement.uoMName', "width": "15%" },  // Unit of Measurement
                    { data: 'reorderLevel', "width": "10%" },  // Reorder Level
                    {
                        data: 'productID',  // Handle image viewing
                        "render": function (data) {
                            return `<span id="display-images" onclick=DisplayImages('${data}') style="cursor: pointer; text-decoration: underline;">View</span>`;
                        },
                        "width": "10%"
                    },
                    {
                        data: 'productID',  // Edit and delete actions
                        "render": function (data) {
                            return `<div class="text-center align-baseline">
                                        <a href="/admin/product/upsert/${data}" class="btn btn-primary mr-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                                        <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger "> <i class="bi bi-trash-fill"></i> Delete</a>
                                    </div>`;
                        },
                        "width": "20%"
                    }
                ]
            });
        }
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
        url: "/Admin/Product/GetImages/" + id,
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

function close() {
    let parent = document.getElementById("close").parentElement;
    parent.remove();
}
