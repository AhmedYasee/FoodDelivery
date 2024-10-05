function Plus(id) {
    $.ajax({
        type: "POST",
        url: '/Customer/Cart/Plus',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#cart-container').load(document.URL + ' #cart');
            }
        }
    });
}

function Minus(id) {
    $.ajax({
        type: "POST",
        url: '/Customer/Cart/Minus',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#cart-container').load(document.URL + ' #cart');
            }
        }
    });
}
function Delete(id) {
    url = '/Customer/Cart/Delete/' + id
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
                    toastr.success(data.message);
                    $('#cart-container').load(document.URL + ' #cart');
                    $('#shopping-link').load(document.URL + ' #shopping-cart')
                },
            })
        }
    })
}
$(document).ready(function () {
    $("#coupun1-btn").click(e => {
        e.preventDefault();
        let coupunCode = document.querySelector("#coupun-code");
        if (coupunCode.value) {
            let name = coupunCode.value;
            let btnValue = document.querySelector("#coupun-btn").value;
            if (btnValue.trim() == "Apply") {
                $.ajax({
                    type: "POST",
                    url: '/Customer/Cart/Coupun',
                    data: JSON.stringify(name),
                    contentType: "application/json",
                    success: function (data) {
                        if (data.success) {
                            coupunCode.disabled = true;
                            let coupunSpan = document.querySelector(".coupun-span");
                            coupunSpan.innerHTML = data.message;
                            coupunSpan.classList.add("found");
                            $("#coupun-btn").value = "Remove";
                            $('#cart-container').load(document.URL + ' #cart');
                            $.ajax({
                                type: "POST",
                                url: '/Customer/Cart/Summary',
                                data: JSON.Stringify(name),
                                contentType: "application/json",
                            });
                        }
                        else {
                            toastr.error(data.message);
                        }
                    }
                });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: '/Customer/Cart/CoupunRemove',
                    contentType: "application/json",
                    success: function (data) {
                        if (data.success) {
                            coupunCode.disabled = false;
                            coupunCode.value = "";
                            let coupunSpan = document.querySelector(".coupun-span");
                            coupunSpan.classList.remove("found");
                            $("#coupun-btn").value = "Apply";
                            $('#cart-container').load(document.URL + ' #cart');
                        }
                    }
                });
            }
        }
    });
});