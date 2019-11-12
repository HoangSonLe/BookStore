﻿var table = $('#tablePublishers').DataTable();

$('#tablePublishers tbody').on('click', '.btnDelete', function () {
    var FeedbackId = $(this).data("id");
    var nameItem = $(this).data("name");
    var item = $(this).parents('tr');
    Swal.fire({
    html: '<h2>Bạn có chắn xóa phản hồi của <b>' + nameItem + "</b>?</h2>",
    text: "Bạn không thể hoàn tác!",
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'Đúng, xóa.!'
            }).then((result) => {
                if (result.value) {
        $.ajax({
            url: "/Admin/Feedback/Delete",
            type: "POST",
            data: {
                id: FeedbackId
            },
            success: function (data) {
                if (data == 1) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Thành công!',
                        text: 'Xóa thành công!',
                        showConfirmButton: false,
                        timer: 2000
                    });
                    table
                        .row(item)
                        .remove()
                        .draw();
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Xóa Không thành công!',
                        showConfirmButton: false,
                        timer: 2000
                    });
                }
            }

        });
    }
})
})
