// Hàm live preview cho hình vừa chọn
function livePreview(inputFile, divPreview) {
    if (typeof (FileReader) != "undefined") {
        var dvPreview = $(divPreview);
        dvPreview.html("");
        //debugger
        var t = inputFile.files;
        var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
        $(inputFile).each(function () {
            var file = $(this);
            if (regex.test(file[0].name.toLowerCase())) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = $("<img />");
                    img.attr("style", "height:150px;width: 150px; margin:10px;");
                    img.attr("src", e.target.result);
                    dvPreview.append(img);
                }
                reader.readAsDataURL(file[0]);
            } else {
                alert(file[0].name + " is not a valid image file.");
                dvPreview.html("");
                return false;
            }
        });
    } else {
        alert("This browser does not support HTML5 FileReader.");
    }
}
// Hàm thay thế button click thành input click
function changeSelectFile(idInput, idButton, e) {
    e.preventDefault();
    var fileSelect = document.getElementById(idButton),
        fileElem = document.getElementById(idInput);
    fileElem.click();
}
$(function () {
    // Hàm nhập tên chuyển thành url freindly
    $(".inputName").change(function () {
        var string = $(this).val();
        $(".inputUrl").val(ChangeStringToSlug(string));
    });

    // Hàm nhập % giảm giá chuyển số tiền tương ứng
    $(".inputDiscount").change(function () {
        var discount = parseInt($(this).val());
        var price = parseInt($(".inputPrice").val());
        $(".inputPromotionPrice").val((price * (100 - discount)) / 100);
    });

    //Hàm tắt enter= submit cho form
    $('#formAdd').on('keyup keypress', function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode === 13) {
            e.preventDefault();
            return false;
        }
    });

    //
    $(".submitForm").on("click", function (e) {
        e.preventDefault();
        if ($("#fileuploadCover").val() == '') {
            e.preventDefault();
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Vui lòng chọn hình bìa!',
            });
        }
        else {
            
            $("#formAdd").submit();
        }
    });
})
ClassicEditor
    .create(document.querySelector('#editor'))
    .then(editor => {
        console.log(editor);
    })
    .catch(error => {
        console.error(error);
    });
