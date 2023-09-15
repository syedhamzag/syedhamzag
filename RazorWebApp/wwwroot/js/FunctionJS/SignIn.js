

var urlquestion = params = new URLSearchParams(window.location.search);
var _callback = '';
var language = localStorage.getItem("language") == null ? "en" : "ar";
if (urlquestion.has('callback')) {
    _callback = getParams();//urlquestion.get('callback');
}
if (urlquestion.has('I')) {
    _id = urlquestion.get('I');
}
var btnsub = $("#btn_sub");
var btnsubopt = $("#btnsubotp");
var imgload = $("#img_load");
var dataip = "_";
var strkey = "";
var ApiForm = '';
var apiUrl_View = window.location.origin
var _UserTimeZone = -(new Date().getTimezoneOffset() / 60);
$(document).ready(function () {
    imgload.hide();
});

$("#signinForm").on("submit", function (e) {
    e.preventDefault();

    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,10})+$/;
    var txt_nam = $("#txtusername").val();
    var txt_pwd = $("#txtuserpassword").val();

    if (txt_nam == "") {
        Swal.fire({
            title: "Please enter email address",

            icon: 'warning',
            showConfirmButton: true,

            showClass: {
                popup: 'animated fadeInDown faster'
            },
            hideClass: {
                popup: 'animated fadeOutUp faster'
            }

        })
        return false;
    } else if (!txt_nam.match(mailformat)) {
        Swal.fire({
            title: "Invalid email format",

            icon: 'warning',
            showConfirmButton: true,

            showClass: {
                popup: 'animated fadeInDown faster'
            },
            hideClass: {
                popup: 'animated fadeOutUp faster'
            }

        })
        return true;

    } else if (txt_pwd == "") {
        Swal.fire({
            title: "Please enter password",

            icon: 'warning',
            showConfirmButton: true,

            showClass: {
                popup: 'animated fadeInDown faster'
            },
            hideClass: {
                popup: 'animated fadeOutUp faster'
            }

        })
        return false;
    }
    else if (txt_pwd.length < 8) {
        Swal.fire({
            title: "Password minimum length should be 8 characters",

            icon: 'warning',
            showConfirmButton: true,

            showClass: {
                popup: 'animated fadeInDown faster'
            },
            hideClass: {
                popup: 'animated fadeOutUp faster'
            }

        })
        return false;
    }
    var rememberMe = $('#rememberme').is(":checked");
    console.log(rememberMe);
    var _cre = JSON.stringify({
        "email": txt_nam,
        "password": txt_pwd,
        "rememberMe": rememberMe
    });
    var dataip = "_";
    //checklogin();
    $.ajax({
        type: "POST",
        cache: false,
        url: "/SignIn?handler=Login",
        contentType: "application/json",
        dataType: "json",
        data: _cre,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
            imgload.show();
            btnsub.hide();
        },


        success: function (response) {
            if (response.statusCode == 200) {
                imgload.hide();
                btnsub.show();
                var phone = response["data"]["phoneNumber"];
                var Id = response["data"]["id"];
                var phonereplace = "";
                for (let index = 0; index < phone.length; index++) {
                    phonereplace = phonereplace + "*";
                }
                generateOTP(Id);
                $('#otpphone').html(phone.substring(0, 3) + "" + phonereplace.substring(3, phone.length))
                $("#otpModal").modal("show");

            }
            else if (response.statusCode == 203) {
                imgload.hide();
                btnsub.show();

                _title = response.message;

                var _title = new Date(_title);

                _title1 = 'Acount is locked, Try after ';
                _title = _title1 + moment(_title).format('h:mm A');
                Swal.fire({
                    title: _title,

                    icon: 'warning',
                    showConfirmButton: true,

                    showClass: {
                        popup: 'animated fadeInDown faster'
                    },
                    hideClass: {
                        popup: 'animated fadeOutUp faster'
                    }

                })
            }
            else {
                imgload.hide();
                btnsub.show();
                _title = response.statusCode == 405 ? "Error # <a href='" + apiUrl_View + "/Configuration/Report/ErrorLog?I=" + response.message + "' target='_blank'>" + " " + response.message + "</a>" : response.message;
                Swal.fire({
                    title: _title,

                    icon: 'warning',
                    showConfirmButton: true,

                    showClass: {
                        popup: 'animated fadeInDown faster'
                    },
                    hideClass: {
                        popup: 'animated fadeOutUp faster'
                    }

                })

            }
        },
        error: function (xhr, status, err) {
            imgload.hide();
            btnsub.show();
            Swal.fire({
                title: xhr.status.toString() + ' ' + err.toString(),

                icon: 'warning',
                showConfirmButton: true,

                showClass: {
                    popup: 'animated fadeInDown faster'
                },
                hideClass: {
                    popup: 'animated fadeOutUp faster'
                }

            })
            return;
        }
    })

    return true;
});
function generateOTP(Id) {
    $.ajax({
        type: "Get",
        url: "/SignIn?handler=GenerateOTP",
        contentType: "application/json",
        dataType: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
            xhr.setRequestHeader("UserId", Id);
            imgload.show();
        },

        success: function (response) {
            if (response.statusCode == 200) 
            {
                imgload.hide();

            }
            else if (response.statusCode == 203) {
                imgload.hide();
                btnsubopt.show();

                Swal.fire({
                    title: response.message,

                    icon: 'warning',
                    showConfirmButton: true,

                    showClass: {
                        popup: 'animated fadeInDown faster'
                    },
                    hideClass: {
                        popup: 'animated fadeOutUp faster'
                    }

                })
            }
            else {
                imgload.hide();
                btnsubopt.show();
                _title = response.statusCode == 405 ? "Error # <a href='" + apiUrl_View + "/Configuration/Report/ErrorLog?I=" + response.message + "' target='_blank'>" + " " + response.message + "</a>" : response.message;

            }
        },
        error: function (xhr, status, err) {
            imgload.hide();
            btnsubopt.show();
            Swal.fire({
                title: xhr.status.toString() + ' ' + err.toString(),

                icon: 'warning',
                showConfirmButton: true,

                showClass: {
                    popup: 'animated fadeInDown faster'
                },
                hideClass: {
                    popup: 'animated fadeOutUp faster'
                }

            })
            return;
        }
    })

}


//Submit OTP
$("#otpmodalform").on("submit", function (e) {
    e.preventDefault();
    var _Otp1 = $("#txt_otp-1").val()
    var _Otp2 = $("#txt_otp-2").val()
    var _Otp3 = $("#txt_otp-3").val()
    var _Otp4 = $("#txt_otp-4").val()
    var _Otp5 = $("#txt_otp-5").val()
    var _Otp6 = $("#txt_otp-6").val()
 
    var txt_otp = _Otp1 + _Otp2 + _Otp3 + _Otp4 + _Otp5 + _Otp6;

    if (txt_otp == "") {
        Swal.fire({
            title: "Please enter OTP",

            icon: 'warning',
            showConfirmButton: true,

            showClass: {
                popup: 'animated fadeInDown faster'
            },
            hideClass: {
                popup: 'animated fadeOutUp faster'
            }

        })
        return false;
    }

    $.ajax({
        type: "GET",
        url: "/SignIn?handler=ConfirmOTP",
        contentType: "application/json",
        dataType: "json",
        beforeSend: function (xhr) 
        {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
            xhr.setRequestHeader("OTP", txt_otp);
            imgload.show();
            btnsubopt.hide();
        },


        success: function (response) {
            if (response.statusCode == 200) 
            {
                imgload.hide();

                var _UrlView = 'Index';

                window.location.assign(_UrlView);

            } 
            else if (response.statusCode == 203) {
                imgload.hide();
                btnsubopt.show();

                _title = response.message;

                var _title = new Date(_title);

                _title1 = 'Acount is locked, Try after ';
                _title = _title1 + moment(_title).format('h:mm A');
                $('#message').html(response.message)
            }
            else {
                imgload.hide();
                btnsubopt.show();
                _title = response.statusCode == 405 ? "Error # <a href='" + apiUrl_View + "/Configuration/Report/ErrorLog?I=" + response.message + "' target='_blank'>" + " " + response.message + "</a>" : response.message;
                $('#message').html(response.message)

            }
        },
        error: function (xhr, status, err) {
            imgload.hide();
            btnsubopt.show();
            var _title = "";
            return;
        }
    })

    return true;
});
