﻿var table = $('#tableEmployees').DataTable();
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
        var data = new FormData();
        data.append("file", input.files[0], prefix + input.files[0].name);
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
                NameFolder = message.folder;
            },
            error: function (response) {
                if (response.responseJSON.message == "login") {
                    alert("Vui lòng đăng nhập lại!");
                    window.location = window.location.protocol + "//" + window.location.host + "/Admin/Login";
                }
                else {
                    alert("there was error uploading files!");
                }
            }
        });
    }
}

$("#Role").change(function () {
    var role = $("#Role").val();
    var id = $("#Role").data("id");
    $.ajax({
        url: "/Admin/Employee/GetManagers",
        type: "POST",
        data: {
            role: role,
            idEmployee: id
        },
        success: function (data) {
            $(".ManagerList").html("")
            
            $(".ManagerList").html(data);
        },
        error: function (response) {
            if (response.responseJSON.message == "login") {
                alert("Vui lòng đăng nhập lại!");
                window.location = window.location.protocol + "//" + window.location.host + "/Admin/Login";
            }
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
    if (Password[0] == "'") {
        isValid = false;
        $("#errorPassword").css('display', 'block');
    }
    else {
        $("#errorPassword").css('display', 'none');
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
            if (response.responseJSON.message == "login") {
                alert("Vui lòng đăng nhập lại!");
                window.location = window.location.protocol + "//" + window.location.host + "/Admin/Login";
            }
            else {
                $("#modalDetail").modal("hide");
                Swal.fire({
                    icon: 'warning',
                    title: 'Cảnh báo!',
                    text: response.responseJSON.content,
                    showConfirmButton: false,
                    timer: 1000
                });
            }
        }
    });
});

$(".btnCreate").on('click', function (e) {
    e.preventDefault();
    if (validate($("#formAdd"))) {
        var employee = $("#formAdd").serialize();
        var nameImage = "&NameImage=" + NameImage;
        var nameFolder = "&NameFolder=" + NameFolder;
        $.ajax({
            url: "/Admin/Employee/Add",
            type: "POST",
            data: employee + nameImage + nameFolder,
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
                nameImage = "";
            },
            error: function (response) {
                if (response.responseJSON.message == "login") {
                    alert("Vui lòng đăng nhập lại!");
                    window.location = window.location.protocol + "//" + window.location.host + "/Admin/Login";
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Thất bại!',
                        text: response.responseJSON.content,
                        showConfirmButton: false,
                        timer: 1000
                    });
                }
            }
        });

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
            console.log(response);
            if (response.responseJSON.message == "login") {
                alert("Vui lòng đăng nhập lại!");
                window.location = window.location.protocol + "//" + window.location.host + "/Admin/Login";
            }
            else {
                Swal.fire({
                    icon: 'warning',
                    title: 'Cảnh báo!',
                    text: response.responseJSON.content,
                    showConfirmButton: false,
                    timer: 1000
                });
            }
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
            if (response.responseJSON.message == "login") {
                alert("Vui lòng đăng nhập lại!");
                window.location = window.location.protocol + "//" + window.location.host + "/Admin/Login";
            }
            else {
                Swal.fire({
                    icon: 'warning',
                    title: 'Cảnh báo!',
                    text: response.responseJSON.content,
                    showConfirmButton: false,
                    timer: 1000
                });
            }
        }
    });
});

$(".btnCancel").click(function () {
    $.ajax({
        url: "/Admin/Employee/DeleteFolderTmp",
        type: "POST",
        data: { NameFolder: NameFolder }
    });
    NameFolder = "";
});


$(".btnEditSaveChange").click(function (e) {
    e.preventDefault();
    if (validate($("#formEdit"))) {
        let employee = $("#formEdit").serialize();
        let nameImage = "&NameImage=" + NameImage;
        var nameFolder = "&NameFolder=" + NameFolder;
        $.ajax({
            url: "/Admin/Employee/Edit",
            type: "POST",
            data: employee + nameImage + nameFolder,
            success: function (data) {
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công!',
                    text: 'Sửa thành công!',
                    showConfirmButton: false,
                    timer: 1000
                });
                $(".tableEmployee").html("");
                $(".tableEmployee").html(data);
                $("#modalDetail").modal("hide");
            },
            error: function (response) {
                if (response.responseJSON.message == "login") {
                    alert("Vui lòng đăng nhập lại!");
                    window.location = window.location.protocol + "//" + window.location.host + "/Admin/Login";
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Thất bại!',
                        text: response.responseJSON.content,
                        showConfirmButton: false,
                        timer: 1000
                    });
                }
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
        cancelButtonText: 'Hủy',
        confirmButtonText: 'Đúng, xóa.!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Admin/Employee/Delete",
                type: "POST",
                data: {
                    id: id
                },
                success: function (data) {
                    $(".tableEmployee").html("");
                    $(".tableEmployee").html(data);
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
                    if (response.responseJSON.message == "login") {
                        alert("Vui lòng đăng nhập lại!");
                        window.location = window.location.protocol + "//" + window.location.host + "/Admin/Login";
                    }
                    else {
                        Swal.fire({
                            icon: 'warning',
                            title: 'Cảnh báo',
                            text: response.responseJSON.content,
                            showConfirmButton: false,
                            timer: 1000
                        });
                    }
                }
            });

        }
    })
});
