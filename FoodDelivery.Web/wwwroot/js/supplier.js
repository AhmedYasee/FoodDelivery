var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Supplier/GetAll' },
        "columns": [
            { data: 'name', "width": "20%" },
            { data: 'email', "width": "20%" },
            { data: 'phone', "width": "20%" },
            { data: 'address', "width": "30%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="btn-group">
                            <a href="/Admin/Supplier/AddSupplier/${data}" class="btn btn-primary btn-sm">Edit</a>
                            <a href="/Admin/Supplier/SupplierInfo/${data}" class="btn btn-info btn-sm">View</a>
                            <a onClick=Delete('/Admin/Supplier/Delete/${data}') class="btn btn-danger btn-sm">Delete</a>
                        </div>
                    `;
                },
                "width": "10%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
