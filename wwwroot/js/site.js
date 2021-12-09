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
    $('.select2').select2();
});