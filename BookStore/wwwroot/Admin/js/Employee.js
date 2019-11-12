var table = $('#tableEmployees').DataTable();

$("#formAdd").on('submit', function (e) {
    e.preventDefault();
    var employee = $("#formAdd").serialize();
    $.ajax({
        url: "/Admin/Employee/Add",
        type: "POST",
        data: employee,
        success: function () {
            Swal.fire({
                icon: 'success',
                title: 'Thành công!',
                text: 'Thêm thành công!',
                showConfirmButton: false,
                timer: 1000
            });
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
        url: "/Admin/Employee/Details",
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
        url: "/Admin/Employee/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (data) {
            $(".box").html("");
            $(".box").html(data);
        }
    });
});

$("#formEdit").on('submit', function (e) {
    e.preventDefault();
    var employee = $("#formEdit").serialize();
    $.ajax({
        url: "/Admin/Employee/Edit",
        type: "POST",
        data: employee,
        success: function () {
            Swal.fire({
                icon: 'success',
                title: 'Thành công!',
                text: 'Sửa thành công!',
                showConfirmButton: false,
                timer: 1000
            });
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
                url: "/Admin/Employee/Delete",
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
                        text: 'Bạn không được quyền xóa!',
                        showConfirmButton: false,
                        timer: 1000
                    });
                }
            });

        }
    })
});
