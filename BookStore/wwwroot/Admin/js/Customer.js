var table = $('#tableCustomers').DataTable();
var NameImage = "";
var NameFolder = "";
async function ReadImage(input) {
    var prefix = "";
    if ($("#Image").data("name") == "Add") {
        prefix = 'A_' + $(".DateAdd").html() + "_";
    }
    else if ($("#Image").data("name") == "Edit") {
        prefix = 'E_' + $("#Image").data("id") + "_";
    }

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#image_upload_preview').attr('src', e.target.result).width('200').height('200'); // setting ur image here		
        };
        reader.readAsDataURL(input.files[0]);
        var data = new FormData();
        data.append('file', input.files[0], prefix + input.files[0].name);
        $.ajax({
            url: "/Admin/Customer/UploadImage",
            type: "POST",
            data: data,
            contentType: false,
            processData: false,
            success: function (message) {
                NameImage = message.name;
                NameFolder = message.folder;
            },
            error: function () {
                alert("there was error uploading files!");
            }
        });
    }
}

function validate(data) {
   
    let isValid = true;
    let FirstName = data.find('input[name="FirstName"]').val();
    let UserName = data.find('input[name="UserName"]').val();
    let Password = data.find('input[name="Password"]').val();
    let Address = data.find('input[name="Address"]').val();
    let Phone = data.find('input[name="PhoneNumber"]').val();
    let Email = data.find('input[name="Email"]').val();

    //validate firstname
    if (FirstName.length > 0) {
        $("#errorFirstName").css('display', 'none');
    }
    else {
        $("#errorFirstName").css('display', 'block');
        isValid = false;
    }
    //validate username
    if (UserName.length > 0) {
        $("#errorUserName").css('display', 'none');
    }
    else {
        $("#errorUserName").css('display', 'block');
        isValid = false;
    }
    //validate password
    if (Password[0] == "'") {
        isValid = false;
        $("#errorPassword").css('display', 'block');
    }
    else {
        $("#errorPassword").css('display', 'none');
    }
    //validate address
    if (Address.length > 0) {
        $("#errorAddress").css('display', 'none');
    }
    else {
        $("#errorAddress").css('display', 'block');
        isValid = false;
    }
    //validate phone
    let pattern = /0[3789][0-9]{8}$/;
    if (pattern.test(Phone)) {
        $("#errorPhoneNumber").css('display', 'none');
    }
    else {
        $("#errorPhoneNumber").css('display', 'block');
        isValid = false;
    }
    //validate email
    let patternEmail = /^[a-z][a-z0-9_\.]{3,32}@[a-z0-9]{2,}(\.[a-z0-9]{2,4}){1,2}$/gm;
    if (patternEmail.test(Email)) {
        $("#errorEmail").css('display', 'none');
    }
    else {
        isValid = false;
        $("#errorEmail").css('display', 'block');
    }
    return isValid;
}


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
    if (validate($("#formAdd"))) {
        var customer = $("#formAdd").serialize();
        var nameImage = "&NameImage=" + NameImage;
        var nameFolder = "&NameFolder=" + NameFolder;
        $.ajax({
            url: "/Admin/Customer/Add",
            type: "POST",
            data: customer + nameImage + nameFolder,
            success: function (data) {
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công!',
                    text: 'Thêm thành công!',
                    showConfirmButton: false,
                    timer: 1000
                });
                $(".tableCustomer").html("");
                $(".tableCustomer").html(data);
                $("#modalDetail").modal("hide");
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

        nameImage = "";
    }
});

$('#tableCustomers tbody').on('click', '.btnDetail', function () {
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

$('#tableCustomers tbody').on('click', '.btnEdit', function () {
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

$(".btnCancel").click(function () {
    $.ajax({
        url: "/Admin/Customer/DeleteFolderTmp",
        type: "POST",
        data: { NameFolder: NameFolder }
    });
    NameFolder = "";
});

$(".btnEditSaveChange").click(function (e) {
    e.preventDefault();
    if (validate($("#formEdit"))) {
        var customer = $("#formEdit").serialize();
        var nameImage = "&NameImage=" + NameImage;
        var nameFolder = "&NameFolder=" + NameFolder;
        console.log(nameFolder);
        $.ajax({
            url: "/Admin/Customer/Edit",
            type: "POST",
            data: customer + nameImage + nameFolder,
            success: function (data) {
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công!',
                    text: 'Sửa thành công!',
                    showConfirmButton: false,
                    timer: 1000
                });
                $(".tableCustomer").html("");
                $(".tableCustomer").html(data);
                $("#modalDetail").modal("hide");
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
        NameImage = "";
    }
});

$('#tableCustomers tbody').on('click', '.btnDelete', function () {
    var id = $(this).data("id");
    var item = $(this).parents('tr');
    Swal.fire({
        html: '<h2>Bạn có chắn xóa thành viên có mã <b>' + id + "</b>?</h2>",
        text: "Bạn không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Hủy',
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
