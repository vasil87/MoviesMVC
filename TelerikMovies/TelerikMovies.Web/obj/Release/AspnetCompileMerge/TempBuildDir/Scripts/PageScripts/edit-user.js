$(() => {
    var defaultPhotoUrl = $('#user-photo').data('defaulturl');
    $('#ImgUrl').keyup(function () {
        var imgUrl = $('#ImgUrl').val().trim();
        if (!(typeof imgUrl === 'undefined')) {
            if (imgUrl === '') {
                let img = $('#user-photo');
                img.attr('src', defaultPhotoUrl);
            } else {
                let img = $('#user-photo');
                img.attr('src', imgUrl);
            }      
        }
    });
});