var table = $('#tablePublishers').DataTable();

$('#tablePublishers tbody').on('click', '.btnDelete', function () {
    var FeedbackId = $(this).data("id");
    var nameItem = $(this).data("name");
    var item = $(this).parents('tr');
    Swal.fire({
    html: '<h2>Bạn có chắc chắn xóa phản hồi của <b>' + nameItem + "</b>?</h2>",
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
$(".btnDetail").click(function () {
    let feedbackID = $(this).data("id");
    $.ajax({
        url: "/Admin/Feedback/Detail",
        type: "GET",
        data: {
            id: feedbackID
        },
        success: function (data) {
            $("#modalBody").html("");
            $("#modalBody").html(data);
            //$("#modalBody").modal("show");
        }
    });
})
$("#opFilter").change(function () {
    let stt = document.getElementById(this.id).value;
    $.ajax({
        url: "/Admin/Feedback/Filter",
        type: "GET",
        data: {
            status: stt
        },
        success: function (data) {
            $(".bodyContent").html("");
            $(".bodyContent").html(data);
        }
    });
})
