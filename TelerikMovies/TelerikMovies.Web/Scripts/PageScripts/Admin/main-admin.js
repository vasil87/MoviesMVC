$(function () {
    $('#loader').hide();
    $('.button-collapse').sideNav({
        menuWidth: 150,
        edge: 'right', // Choose the horizontal origin
        closeOnClick: true, // Closes side-nav on <a> clicks, useful for Angular/Meteor
        draggable: true// Choose whether you can drag to open on touch screens,
    });
    $('form.show-loader').on('submit', function () {
        $('#loader').show();
    })
})