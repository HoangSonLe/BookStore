var table = $('#tablePublishers').DataTable();

//$('#tablePublishers tbody').on('click', '.btnDetail', function (){
    
//});
function onDetailProduct(id){
    //var id = $(this).data("id");
    $.ajax({
        url: "/Admin/Products/Details",
        type: "GET",
        data: {
            id: id
        },
        success: function (data) {
            $("#modalBody").html("");
            $("#modalBody").html(data);
        }
    });
}
$('#tablePublishers tbody').on('click', '.btnDelete', function () {
    var idItem = $(this).data("id");
    var nameItem = $(this).data("name");
    var item = $(this).parents('tr');
    Swal.fire({
        html: '<h2>Bạn có chắn xóa <b>' + nameItem + "</b>?</h2>",
        text: "Bạn không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đúng, xóa.!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Admin/ProductCategory/Delete",
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


