var table = $('#tableEmployees').DataTable();
var NameImage = "";

async function ReadImage(input) {
    if (input.files && input.files[0]) {
        
        var data = new FormData();
        data.append('file', input.files[0], input.files[0].name);
        $.ajax({
            url: "/Admin/Employee/UploadImage",
            type: "POST",
            data: data,
            contentType: false,
            processData: false,
            success: function (message) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#image_upload_preview').attr('src', e.target.result).width('200').height('200'); // setting ur image here		
                };
                reader.readAsDataURL(input.files[0]);
                NameImage = message.name;
            },
            error: function () {
                alert("there was error uploading files!");
            }
        });
    }
}

$("#Role").change(function () {
    var id = $("#Role").val();
    console.log(id);
    $.ajax({
        url: "/Admin/Employee/GetManagers",
        type: "POST",
        data: {
            role: id
        },
        success: function (data) {
            $(".ManagerList").html("")
            
            $(".ManagerList").html(data);
        }
    });

});

function validate(data) {

    let isValid = true;
    let FirstName = data.find('input[name="FirstName"]').val();
    let UserName = data.find('input[name="UserName"]').val();
    let Password = data.find('input[name="Password"]').val();
    let Address = data.find('input[name="Address"]').val();
    let Phone = data.find('input[name="Phone"]').val();
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
    let patternPassword = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$/;
    if (patternPassword.test(Password)) {
        $("#errorPassword").css('display', 'none');
    }
    else {
        isValid = false;
        $("#errorPassword").css('display', 'block');
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
    if (Email.length > 0) {
        let patternEmail = /^[a-z][a-z0-9_\.]{3,32}@[a-z0-9]{2,}(\.[a-z0-9]{2,4}){1,2}$/gm;
        if (patternEmail.test(Email)) {
            $("#errorEmail").css('display', 'none');
        }
        else {
            isValid = false;
            $("#errorEmail").css('display', 'block');
        }
    }
    
    return isValid;
}

$(".btnAdd").click(function () {
    $.ajax({
        url: "/Admin/Employee/Add",
        type: "GET",
        success: function (data) {
            $("#modalBody").html("");
            $('#modalDetail').modal({
                show: 'false'
            });
            $("#modalBody").html(data);
        },
        error: function (response) {
            $("#modalDetail").modal("hide");
            Swal.fire({
                icon: 'warning',
                title: 'Cảnh báo!',
                text: JSON.parse(response.responseText).message,
                showConfirmButton: false,
                timer: 1000
            });
        }
    });
});

$(".btnCreate").on('click', function (e) {
    e.preventDefault();
    if (validate($("#formAdd"))) {
        var employee = $("#formAdd").serialize();
        var nameImage = "&NameImage=" + NameImage;
        $.ajax({
            url: "/Admin/Employee/Add",
            type: "POST",
            data: employee + nameImage,
            success: function (data) {
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công!',
                    text: 'Thêm thành công!',
                    showConfirmButton: false,
                    timer: 1000
                });
                $(".tableEmployee").html("");
                $(".tableEmployee").html(data);
                $("#modalDetail").modal("hide");
            },
            error: function (response) {
                Swal.fire({
                    icon: 'fail',
                    title: 'Thất bại!',
                    text: JSON.parse(response.responseText).message,
                    showConfirmButton: false,
                    timer: 1000
                });
            }
        });

        nameImage = "";
    }
});

$('#tableEmployees tbody').on('click', '.btnDetail', function () {
    var id = $(this).data("id");
    $.ajax({
        url: "/Admin/Employee/Details",
        type: "GET",
        data: {
            id: id
        },
        success: function (data) {
            $("#modalBody").html("");
            $('#modalDetail').modal({
                show: 'false'
            });
            $("#modalBody").html(data);
        },
        error: function (response) {
            Swal.fire({
                icon: 'warning',
                title: 'Cảnh báo!',
                text: JSON.parse(response.responseText).message,
                showConfirmButton: false,
                timer: 1000
            });
        }
    });
});

$('#tableEmployees tbody').on('click', '.btnEdit', function () {
    var id = $(this).data("id");
    $.ajax({
        url: "/Admin/Employee/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (data) {
            $("#modalBody").html("");
            $('#modalDetail').modal({
                show: 'false'
            });
            $("#modalBody").html(data);
        },
        error: function (response) {
            Swal.fire({
                icon: 'warning',
                title: 'Cảnh báo!',
                text: JSON.parse(response.responseText).message,
                showConfirmButton: false,
                timer: 1000
            });
        }
    });
});

$(".btnEditSaveChange").click(function (e) {
    e.preventDefault();
    if (validate($("#formEdit"))) {
        let employee = $("#formEdit").serialize();
        let nameImage = "&NameImage=" + NameImage;
        $.ajax({
            url: "/Admin/Employee/Edit",
            type: "POST",
            data: employee + nameImage,
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

$('#tableEmployees tbody').on('click', '.btnDelete', function () {
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
                error: function (response) {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Cảnh báo',
                        text: JSON.parse(response.responseText).message,
                        showConfirmButton: false,
                        timer: 1000
                    });
                }
            });

        }
    })
});
