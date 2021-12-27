setTimeout(function () {
    $(".alert").fadeOut();
}, 2000); 

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

