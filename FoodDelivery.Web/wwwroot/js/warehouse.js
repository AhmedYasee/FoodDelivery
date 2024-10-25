var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "responsive": true,
        "ajax": { url: '/Admin/Warehouse/GetAll' },
        "columns": [
            { data: 'warehouseName', "width": "30%" },
            { data: 'description', "width": "40%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="btn-group">
                            <a href="/admin/warehouse/addwarehouse/${data}" class="btn btn-primary btn-sm">Edit</a>
                            <a onClick=Delete('/admin/warehouse/delete/${data}') class="btn btn-danger btn-sm">Delete</a>
                        </div>
                    `;
                },
                "width": "30%"
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
        confirmButtonText: 'Yes, delete it!'
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
