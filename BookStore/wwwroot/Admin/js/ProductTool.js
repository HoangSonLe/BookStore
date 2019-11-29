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
$('#formProduct').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});

//Hàm cho bút xóa button trên hình mô tả
$(".btnDeleteImage").on("click", function () {
    var id = $(this).data("id");
    var str = $("[name='ArrDeleteImage']").val().toString();
    $("[name='ArrDeleteImage']").val(str + id + ",");
    $(this).closest("div").remove();
});
//Hàm cho bút xóa button trên hình bìa
$(".btnDeleteImageCover").on("click", function () {
    var id = $(this).data("id");
    $(this).closest("div").hide();
    $(".divAddImageCover").html('<h5>Upload Ảnh Bìa</h5>');
});
//Hàm undo hình bìa
$("#undoDeleteImage").on("click", function (e) {
    e.preventDefault();
    $(".btnDeleteImageCover").closest("div").show();
});
//Hàm để hiện ảnh phóng to
function viewImage(image) {
    // Get the modal
    var modal = document.getElementById("myModal");

    // Get the image and insert it inside the modal - use its "alt" text as a caption
    var img = document.getElementById(image);
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    modal.style.display = "block";
    modalImg.src = img.src;
    captionText.innerHTML = img.alt;

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];

    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }
    modal.onclick = function () {
        modal.style.display = "none";
    }
}
ClassicEditor
    .create(document.querySelector('#editor'))
    .then(editor => {
        console.log(editor);
    })
    .catch(error => {
        console.error(error);
    });
