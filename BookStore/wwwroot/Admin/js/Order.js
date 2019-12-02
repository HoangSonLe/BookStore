var table = $('#tablePublishers').DataTable();

$(".btnDetail").click(function () {
    var id = $(this).data("id");
    $.ajax({
        url: "/Admin/Order/Details",
        type: "GET",
        data: {
            id: id
        },
        success: function (data) {
            $("#modalBody").html("");
            $("#modalBody").html(data);
        }
    });
});

// delete order
$(".btnDelete").click(function () {
    var id = $(this).data("id");
    var item = $(this).parents('tr');
    Swal.fire({
        html: '<h2>Bạn có chắn xóa <b>' + "</b>?</h2>",
        text: "Bạn không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đúng, xóa.!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Admin/Order/Delete",
                type: "POST",
                data: {
                    id: id
                },
                success: function (data) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Thành công!',
                        text: 'Xóa thành công!',
                        showConfirmButton: false,
                        timer: 1000
                    });
                    table
                        .row(item)
                        .remove()
                        .draw();
                }
            });

        }
    })
})
var OrderDetail = [];
// add order Detai
$(".add-detail").click(function () {

    let productQuantity = $("#ProQuantity").val();
    if (Number(productQuantity) > 0) {
        let productId = $("#ProductId option:selected").val();
        let productName = $("#ProductId option:selected").text();

        OrderDetail.push(
            { id: productId, quantity: productQuantity }
        );

        $.ajax({
            url: "/Admin/Order/AddOrderDetailTemp",
            type: "POST",
            data: {
                id: productId,
                quantity: productQuantity,
                productName: productName
            },
            success: function (data) {
                $(".add-order-detail").html(data);
            }
        });

        //    $(".add-order-detail").append(`
        //    <tr id="rows-${productId}">
        //        <th scope="row">${productName}</th>
        //        <td>${productId}</td>
        //        <td>${productQuantity}</td>
        //        <td><span class="glyphicon glyphicon-remove" aria-hidden="true" onclick="removeThis(${productId})"></span></td>
        //    </tr>
        //`);
    } else {
        Swal.fire({
            icon: 'warning',
            title: 'Lỗi!',
            text: 'Số lượng không hợp lệ!',
            showConfirmButton: false,
            timer: 1000
        });
    }

})

function removeThis(id) {
    $("#rows-" + id).remove();
    $.ajax({
        url: "/Admin/Order/DeleteOrderDetailTemp",
        type: "DELETE",
        data: {
            id: id
        },
        success: function (data) {
            $(".add-order-detail").html(data);
        }
    })
}


// Create order using ajax
$(".create-order").click(function () {
    var data = $("#formOrder").serialize();
    $.ajax({
        url: "/Admin/Order/Create",
        type: "POST",
        data: data,
        success: function (data) {
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Hóa đơn đã được tạo mới thành công',
                showConfirmButton: false,
                timer: 1500
            });
            window.location.href = "/Admin/Order";
        }
    })
})
