﻿var table = $('#tableComment').DataTable();
$("#opFilter").change(function () {
    let id = document.getElementById(this.id).value;
    $.ajax({
        url: "/Admin/Comment/Filter",
        type: "GET",
        data: {
            id: id
        },
        success: function (data) {
            $(".bodyContent").html("");
            $(".bodyContent").html(data);
        }
    });
})
$('#tableComment tbody').on('click', '.btnDelete', function () {
    var idItem = $(this).data("id");
    var nameItem = $(this).data("name");
    var item = $(this).parents('tr');
    Swal.fire({
        html: '<h2>Bạn có chắc chắn muốn xóa bình luận của <b>' + nameItem + "</b>?</h2>",
        text: "Bạn không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đúng, xóa.!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Admin/Comment/Delete",
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
                    else {
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
//$('#tableComment tbody').on('click', '.btnDetail', function () {
//    var idItem = $(this).data("id");
//    alert(idItem);
//    //var nameItem = $(this).data("name");
//    var item = $(this).parents('tr');
//    $.ajax({
//        url: "/Admin/Comment/Detail",
//        type: "GET",
//        data: {
//            id: idItem
//        },
//        success: function (data) {
//            $("#modalBody").html("");
//            $("#modalBody").html(data);
//        }
//    });

//});
function detail(idItem) {
    $.ajax({
        url: "/Admin/Comment/Detail",
        type: "GET",
        data: {
            id: idItem
        },
        success: function (data) {
            $("#modalBody").html("");
            $("#modalBody").html(data);
        }
    });
}

