$(() =>{
    $.get("/genres/GetAllNamesNotExpired", function (resultGenres) {
     
        var setAutoComplete = function (selector) {
            $(selector).autocomplete({
                data:  resultGenres ,
                limit: 20, 
                minLength: 1, 
            });
        }

        setAutoComplete(`#genre`);

        $('#loader').hide();

        $('.create-genre-form').on('submit', function () {

            var result = $(this).valid();
            if (result) {
                $('#loader').show();
            }
        })

    });

});


    