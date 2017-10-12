$(document).ready(function () {
    $('.carousel').carousel({ dist :-50});
});

let checkForm = function () {
    var value = $('#search-movies-field').val();
    if (typeof value === 'undefined' || value.trim() === '') {
        Materialize.toast("Please enter at least one char",1000);
        return false;
    }
    $('html, body').animate({ scrollTop: 0 }, 'slow');
    return true
};