var table = $('#tableRoles').DataTable();

$(".delete-role").click(function (event) {
    event.preventDefault();
    let idItem = $(this).data("id");
    let item = $(this).parents('tr');

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
                }
            });
        }
    })
})
