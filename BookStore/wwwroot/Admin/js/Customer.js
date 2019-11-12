var table = $('#tableCustomers').DataTable();

$(".btnAdd").click(function () {
    $.ajax({
        url: "/Admin/Customer/Add",
        type: "GET",
        success: function (data) {
            $("#modalBody").html("");
            $("#modalBody").html(data);
        }
    });
});


$(".btnCreate").on('click', function (e) {
    e.preventDefault();
    var customer = $("#formAdd").serialize();
    $.ajax({
        url: "/Admin/Customer/Add",
        type: "POST",
        data: customer,
        success: function (data) {
            Swal.fire({
                icon: 'success',
                title: 'Thành công!',
                text: 'Thêm thành công!',
                showConfirmButton: false,
                timer: 1000
            });
            $("#tbodyDatatable").html("");
            $("#tbodyDatatable").html(data);
        },
        error: function () {
            Swal.fire({
                icon: 'fail',
                title: 'Thất bại!',
                text: 'Thêm không thành công!',
                showConfirmButton: false,
                timer: 1000
            });
        }
    });

});

$(".btnDetail").click(function () {
    var id = $(this).data("id");
    $.ajax({
        url: "/Admin/Customer/Details",
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

$(".btnEdit").click(function () {
    var id = $(this).data("id");
    $.ajax({
        url: "/Admin/Customer/Edit",
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

$(".btnEditSaveChange").on('click', function (e) {
    e.preventDefault();
    var customer = $("#formEdit").serialize();
    $.ajax({
        url: "/Admin/Customer/Edit",
        type: "POST",
        data: customer,
        success: function (data) {
            Swal.fire({
                icon: 'success',
                title: 'Thành công!',
                text: 'Sửa thành công!',
                showConfirmButton: false,
                timer: 1000
            });
            $("#tbodyDatatable").html("");
            $("#tbodyDatatable").html(data);
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Thất bại!',
                text: 'Sửa không thành công!',
                showConfirmButton: false,
                timer: 1000
            });
        }
    });
});

$(".btnDelete").click(function () {
    var id = $(this).data("id");
    var item = $(this).parents('tr');
    Swal.fire({
        html: '<h2>Bạn có chắn xóa thành viên có mã <b>' + id + "</b>?</h2>",
        text: "Bạn không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đúng, xóa.!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Admin/Customer/Delete",
                type: "POST",
                data: {
                    id: id
                },
                success: function () {
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
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Xóa thất bại!',
                        showConfirmButton: false,
                        timer: 1000
                    });
                }
            });

        }
    })
});
