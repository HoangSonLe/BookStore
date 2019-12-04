var table = $('#tableRoles').DataTable();

$(".btnAdd").click(function () {
    $.ajax({
        url: "/Admin/Roles/Create",
        type: "GET",
        
        success: function (data) {
            $("#modalBody").html("");
            $("#modalBody").html(data);
            //$("#modalBody").modal("show");
        }
    });
})
$('#tableRoles tbody').on('click', '.btnEdit', function () {
    var idItem = $(this).data("id");
    //var nameItem = $(this).data("name");
    $.ajax({
        url: "/Admin/Roles/Edit",
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

$(".btnDelete").click(function () {
    let idItem = $(this).data("id");
    let item = $(this).parents('tr');
    var roleName = $(this).data("name");
    Swal.fire({
        html: '<h2>Bạn có chắc chắn muốn xóa vai trò <b>' + roleName + "</b>?</h2>",
        text: "Bạn không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đúng, xóa.!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Admin/Roles/Delete",
                type: "POST",
                data: {
                    id: idItem
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
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Thất bại!',
                        text: "Đã xảy ra lỗi. Xin vui lòng thử lại",
                        showConfirmButton: false,
                        timer: 2000
                    });
                }
            });
        }
    })
})
