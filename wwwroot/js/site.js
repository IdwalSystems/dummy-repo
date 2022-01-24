////const myTimeout = setTimeout(myGreeting, 5000);

////function myGreeting() {
////    toastr.options = {
////        "closeButton": true,
////        "timeOut": "0",
////        "extendedTimeOut": "0"
////    }
////    // for warning - orange box
////    toastr.warning("Masa anda telah tamat. Sila tekan butang 'Refresh' untuk log in semula.");
////}
$(function () {
    $("body").on('click keypress', function () {
        ResetThisSession();
    });
});

var timeInSecondsAfterSessionOut = 600; // change this to change session time out (in seconds).
var secondTick = 0;

function ResetThisSession() {
    secondTick = 0;
}

function StartThisSessionTimer() {
    secondTick++;
    var timeLeft = ((timeInSecondsAfterSessionOut - secondTick) / 60).toFixed(0); // in minutes
    timeLeft = timeInSecondsAfterSessionOut - secondTick; // override, we have 30 secs only 

    $("#spanTimeLeft").html(timeLeft);

    if (secondTick > timeInSecondsAfterSessionOut) {
        clearTimeout(tick);
        
        toastr.options = {
            "closeButton": true,
            "timeOut": "500000",
            "extendedTimeOut": "100000"
        };
        toastr.warning("Masa anda telah tamat. Sila log in semula.");
        /*window.location = '/Account/Login';*/
        return ;
    }
    tick = setTimeout("StartThisSessionTimer()", 1000);
}

StartThisSessionTimer();

function showDate(d) {
    var s = new Date(d);
    var month = s.getMonth() + 1;
    var day = s.getDate();
    var year = s.getFullYear();

    return (day < 10 ? '0' + day : day) + "/" + (month < 10 ? '0' + month : month) + "/" + year;
}

$(document).ready(function () {
    $(".select2").select2({
        theme: "bootstrap"
    });  

});



//ToTopBtn------------------------------------------------------------------------
//Get the button
var mybutton = document.getElementById("myBtn");

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}
//ToTopBtn------------------------------------------------------------------------


