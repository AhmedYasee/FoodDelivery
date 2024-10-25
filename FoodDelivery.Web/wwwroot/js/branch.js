var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "responsive": true,
        "ajax": { url: '/Admin/Branch/GetAll' },
        "columns": [
            { data: 'branchName', "width": "20%" }, // Reduced the width of Branch Name
            {
                data: 'city',
                "render": function (data, type, row) {
                    return row.street + ', ' + row.city + ', ' + row.country;
                },
                "width": "40%" // Location keeps a medium width
            },
            { data: 'manager.name', "width": "20%" }, // Manager remains unchanged
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="btn-group">
                            <a href="/admin/branch/addbranch/${data}" class="btn btn-primary btn-sm">Edit</a>
                            <a onClick=Delete('/admin/branch/delete/${data}') class="btn btn-danger btn-sm">Delete</a>
                        </div>
                    `;
                },
                "width": "20%" // Increased width for Actions
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
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        Swal.fire({
                            title: 'Error!',
                            text: data.message,
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                },
                error: function (data) {
                    toastr.error("An error occurred while processing your request.");
                }
            });
        }
    });
}
