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

// stop users from entering source code
//document.addEventListener("contextmenu", function (e) {
//    e.preventDefault();
//}, false);

//$(function () {
//    $("body").on('click keypress', function () {
//        ResetThisSession();
//    });
//});

//document.addEventListener("keydown", (e) => {
//    // USE THIS TO DISABLE CONTROL AND ALL FUNCTION KEYS
//    // if (e.ctrlKey || (e.keyCode>=112 && e.keyCode<=123)) {
//    // THIS WILL ONLY DISABLE CONTROL AND F12
//    if (e.ctrlKey || e.keyCode == 123) {
//        e.stopPropagation();
//        e.preventDefault();
//    }
//});
// stop users from entering source code end

// toggle full screen
function toggleFullScreen() {
    var a = $(window).height() - 10;

    if (!document.fullscreenElement && // alternative standard method
        !document.mozFullScreenElement && !document.webkitFullscreenElement) { // current working methods
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    } else {
        if (document.cancelFullScreen) {
            document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
            document.webkitCancelFullScreen();
        }
    }
}
function display_ct6() {
    var x = new Date()
    var ampm = x.getHours() >= 12 ? ' PM' : ' AM';
    hours = x.getHours() % 12;
    hours = hours ? hours : 12;
    var x1 = x.getMonth() + 1 + "/" + x.getDate() + "/" + x.getFullYear();
    var minutes = x.getMinutes();
    var seconds = x.getSeconds();

    if (hours < 10) {
        hours = '0' + hours;
    }
    if (minutes < 10) {
        minutes = '0' + minutes;
    }

    if (seconds < 10) {
        seconds = '0' + seconds;
    }
    x1 = hours + ":" + minutes + ":" + seconds + " " + ampm;
    document.getElementById('ct6').innerHTML = x1;
    display_c6();
}

function display_c6() {
    var refresh = 1000; // Refresh rate in milli seconds
    mytime = setTimeout('display_ct6()', refresh)
}
display_c6()

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

function display_timer() {

    document.getElementById('timer').innerHTML = timeInSecondsAfterSessionOut;
    StartThisSessionTimer();
}
/*StartThisSessionTimer();*/

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

    $("#datepicker").datepicker({
        changeMonth: true,
        changeYear: false,
    })
        .hide()
        .click(function () {
            $(this).hide();
        });

    $("#datepickerImage").click(function () {
        $("#datepicker").show();
    });

});

function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}

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


