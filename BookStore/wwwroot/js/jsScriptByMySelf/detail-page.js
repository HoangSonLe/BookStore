var action = 1;
var commentID = 0;
var productID = $(".btnSendComment").data("id");

    //increasing view count

    $.ajax({
        url: "/Products/IncreaseViewCount",
        type: "POST",
        data: {
            productID: productID
        },
        success: function (data) {

        }
    });
    //Getting comments
    $.ajax({
        url: "/Products/GetComments",
        type: "GET",
        data: {
            productID: productID
        },
        success: function (data) {
            $("#comments").html(data);
        }
    });

    //Show modal 
    $(".quickViewModal").on("click", function () {
        var id = $(this).data("id");
        $.ajax({
            url: "/Products/Detail",
            type: "GET",
            data: {
                id: id
            },
            success: function (data) {
                $(".modalBody").html("");
                $(".modalBody").html(data);
            },
            error: function (data) {
                alert("error");

            }
        });
    });


//Sending Comment
$(".btnSendComment").click(function () {
    let productID = $(this).data("id");
    let context = $("#writeComment").val().trim();
    if (context.length > 0) {
        if (action == 2) handleEditComment(context);
        else {
            //Add comment
            $.ajax({
                url: "/Products/AddComment",
                type: "POST",
                data: {
                    ProductID: productID,
                    Context: context

                },
                success: function (data) {
                    if (data == "error") {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi',
                            text: 'Đã xảy ra lỗi. Xin vui lòng thử lại sau!',
                            showConfirmButton: false,
                            timer: 2000
                        });
                    }
                    else {
                        Swal.fire({
                            icon: 'success',
                            title: 'Thành công',
                            text: 'Thêm bình luận thành công!',
                            showConfirmButton: false,
                            timer: 2000
                        });
                        $("#writeComment").val("");
                        $("#comments").html("");
                        $("#comments").html(data);
                    }
                },
                error: function (data) {
                    alert("error");

                }
            });
        }
    }
})

$(".btnCancel").click(function () {
    action = 1;
    $(".btnCancel").css("display", "none");
    $("#writeComment").val("");
})

function Delete(id) {
    Swal.fire({
        html: '<h2>Bạn có chắc chắn muốn xóa bình luận này không?</h2>',
        text: "Bạn không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đúng, xóa.!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Products/DeleteComment",
                type: "POST",
                data: {
                    CommentID: id,
                    ProductID: productID
                },
                success: function (data) {
                    if (data == "error") {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi',
                            text: 'Đã xảy ra lỗi. Xin vui lòng thử lại sau!',
                            showConfirmButton: false,
                            timer: 2000
                        });
                    }
                    else {
                        Swal.fire({
                            icon: 'success',
                            title: 'Thành công',
                            text: 'Xóa bình luận thành công!',
                            showConfirmButton: false,
                            timer: 2000
                        });
                        $("#writeComment").val("");
                        $("#comments").html("");
                        $("#comments").html(data);
                        $(".btnCancel").css("display", "none");
                    }
                },
                error: function (data) {
                    alert("error");

                }
            });
        }
    });

}

//Edit comment
function Edit(context, id) {
    action = 2;
    commentID = id;
    $(".btnCancel").css("display", "inline");
    $("#writeComment").val(context);
}

function handleEditComment(context) {
    $.ajax({
        url: "/Products/EditComment",
        type: "POST",
        data: {
            CommentID: commentID,
            Context: context,
            ProductID: productID
        },
        success: function (data) {
            if (data == "error") {
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi',
                    text: 'Đã xảy ra lỗi. Xin vui lòng thử lại sau!',
                    showConfirmButton: false,
                    timer: 2000
                });
            }
            else {
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Cập nhật bình luận thành công!',
                    showConfirmButton: false,
                    timer: 2000
                });
                action = 1;
                $("#writeComment").val("");
                $("#comments").html("");
                $("#comments").html(data);
                $(".btnCancel").css("display", "none");
            }
        },
        error: function (data) {
            alert("error");

        }
    })
}

