function onClickAddToCart(id, quantity) {
    $.ajax({
        url: "/Cart/AddToCart",
        data: {
            id: id,
            quantity: quantity
        },
        success: function (data) {
            Swal.fire({
                icon: 'success',
                title: 'Thành công...',
                text: 'Thêm sản phẩm vào giỏ hàng thành công!',
                showConfirmButton: false,
                timer: 1500
            })
            $("#cartList").html("");
            $("#cartList").html(data);
        },
        error: function (data) {
            alert("error");
        }
    });
};
function ReturnViewProductCart() {
    $.ajax({
        url: "/Cart/ViewProductCart",
        success: function (data) {
            $("#cartList").html("");
            $("#cartList").html(data);
        },
        error: function (data) {
            alert("error");
        }
    });
}
function onClickUpdateToCart(id, quantity) {
    $.ajax({
        url: "/Cart/UpdateToCart",
        data: {
            id: id,
            quantity: quantity
        },
        success: function (data) {
            $("#productCart").html("");
            $("#productCart").html(data);
            ReturnViewProductCart();
            Swal.fire({
                icon: 'success',
                title: 'Cập nhật thành công!',
                text: 'Sản phẩm đã được cập nhật trong giỏ hàng!',
                showConfirmButton: false,
                timer: 1000
            });
        },
        error: function (data) {
            alert("error");
        }
    });
};
function onClickUpdateNumberToCart(id, quantity) {
    $.ajax({
        url: "/Cart/UpdateNumberToCart",
        data: {
            id: id,
            quantity: quantity
        },
        success: function (data) {
            $("#productCart").html("");
            $("#productCart").html(data);
            ReturnViewProductCart();
            Swal.fire({
                icon: 'success',
                title: 'Cập nhật thành công!',
                text: 'Sản phẩm đã được cập nhật trong giỏ hàng!',
                showConfirmButton: false,
                timer: 1000
            });
        },
        error: function (data) {
            alert("error");
        }
    });
};
function onClickRemoveInCart(id) {
    Swal.fire({
        title: 'Bạn có chắn xóa sản phẩm khỏi giỏ hàng?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xóa sản phẩm!',
        cancelButtonText: 'Hủy!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Cart/DeleteInCart",
                data: {
                    id: id,
                },
                success: function (data) {
                    $("#productCart").html("");
                    $("#productCart").html(data);
                    ReturnViewProductCart();
                    Swal.fire(
                        'Đã xóa!',
                        'Sản phẩm đã xóa khỏi giỏ hàng.',
                        'success'
                    )
                },
                error: function (data) {
                    alert("error");
                }
            });
        }
    })
};