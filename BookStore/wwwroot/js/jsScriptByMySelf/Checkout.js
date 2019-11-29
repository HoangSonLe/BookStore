function validate(data) {

    let isValid = true;
    let Name = data.find('input[name="Name"]').val();
    let Address = data.find('input[name="Address"]').val();
    let Phone = data.find('input[name="Phone"]').val();
    let Email = data.find('input[name="Email"]').val();

    //validate name
    if (Name.length > 0) {
        $("#errorName").css('display', 'none');
    }
    else {
        $("#errorName").css('display', 'block');
        isValid = false;
    }

    //validate phone
    let pattern = /0[3789][0-9]{8}$/;
    if (pattern.test(Phone)) {
        $("#errorPhone").css('display', 'none');
    }
    else {
        $("#errorPhone").css('display', 'block');
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

    //validate address
    if (Address.length > 0) {
        $("#errorAddress").css('display', 'none');
    }
    else {
        $("#errorAddress").css('display', 'block');
        isValid = false;
    }

    return isValid;
}

$("#Payment").click(function (e) {
    e.preventDefault();
    if (validate($("#InfoOrder"))) {
        var order = $("#InfoOrder").serialize();
        var payMethod = $("input[name='PayMethod']:checked").val();
        if (payMethod == "Paypal") {
            window.location = "https://localhost:44300/Paypal/Checkout?" + order;
        }
        else if (payMethod == "OnePay") {

        }
        else if (payMethod == "DirectPay") {
            window.location = "https://localhost:44300/Checkout/Invoice?" + order;
            //$.ajax({
            //    url: "/Checkout/Invoice",
            //    type: "POST",
            //    data: order,
            //    success: function (response) {
            //        $(".checkout-area").html("");
            //        $(".checkout-area").html(response);
            //    }
            //});
        }
    }
});

