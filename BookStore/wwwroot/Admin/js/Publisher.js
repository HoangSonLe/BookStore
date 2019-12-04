var table = $('#tablePublishers').DataTable();
$('#tablePublishers tbody').on('click', '.btnDelete', function () {
    var idItem = $(this).data("id");
    var nameItem = $(this).data("name");
    var item = $(this).parents('tr');
    Swal.fire({
        html: '<h2>Bạn có chắc chắn xóa <b>' + nameItem + "</b>?</h2>",
        text: "Bạn không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đúng, xóa.!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Admin/Publisher/Delete",
                type: "POST",
                data: {
                    id: idItem
                },
                success: function (data) {
                    if (data == 1) {
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
                    else if (data==0) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi',
                            text: 'Không thành công!',
                            showConfirmButton: false,
                            timer: 1000
                        });
                    }
                }
            });

        }
    })

});
$('#tablePublishers tbody').on('click', '.btnDetail', function () {
    var idItem = $(this).data("id");
    $.ajax({
        url: "/Admin/Publisher/Detail",
        type: "GET",
        data: {
            id: idItem
        },
        success: function (data) {
            $("#modalBody").html("");
            $("#modalBody").html(data);
            //$("#modalBody").modal("show");
        }
    });

});
$('#tablePublishers tbody').on('click', '.btnEdit', function () {
    var idItem = $(this).data("id");
    $.ajax({
        url: "/Admin/Publisher/Edit",
        type: "GET",
        data: {
            id: idItem
        },
        success: function (data) {
            $("#modalBody").html("");
            $("#modalBody").html(data);
            //$("#modalBody").modal("show");
        }
    });

});
$(".btnAdd").click(function () {
    $.ajax({
        url: "/Admin/Publisher/Create",
        type: "GET",

        success: function (data) {
            $("#modalBody").html("");
            $("#modalBody").html(data);
            //$("#modalBody").modal("show");
        }
    });
})

