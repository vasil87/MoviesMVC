$(() => {
    $.get("/genres/GetAllNamesNotExpired", function (resultGenres) {

        var setAutoComplete = function (selector) {
            $(selector).autocomplete({
                data: resultGenres,
                limit: 20, // The max amount of results that can be shown at once. Default: Infinity.
                minLength: 1, // The minimum length of the input for the autocomplete to start. Default: 1.
            });
        }

        setAutoComplete(`input.autocomplete`);

        if ($('.genre').length==3)
        {
            $('#AddGenre').hide();
        }
       
        $('#loader').hide();

        $(document).on('click', '.delete-genre', function () {
            if ($($(this).prevAll('.autocomplete')[0]).attr('name')=='Genres[1].Name' && $('input[name="Genres[2].Name"]').length > 0)
            {
                var $input = $('input[name="Genres[2].Name"]')
                $input.attr('name', 'Genres[1].Name');
                $input.attr('id', 'Genre2');
                $input.prev().attr('for', 'Genre2');
            }
            $(this).parents('div.genre').remove();
            if (!$('#AddGenre').is(":visible")) {
                $('#AddGenre').show();
            }
        });

        $input = $('.datepicker').pickadate({
            selectMonths: true, // Creates a dropdown to control month
            selectYears: 15, // Creates a dropdown of 15 years to control year,
            today: 'Today',
            clear: 'Clear',
            close: 'Ok',
            closeOnSelect: true,// Close upon selecting a date
        });

        var picker = $input.pickadate('picker');
        picker.set('select', Date.now())

        $('#AddGenre').click(function () {
            var $allGenres = $('.genre');
            var count = $allGenres.length;
            if (count < 3) {
                var elementToAdd = $(`<div class="input-field col s4 genre">
            <label for="Genre${count + 1}">Genre</label>
            <input class ="autocomplete"  autocomplete="off" data-val="true" data-val-maxlength="The field Genre must be a string." data-val-maxlength-max="500" data-val-required="The Genre field is required." id="Genre${count + 1}" name="Genres[${count}].Name" type="text" value="">
             <button class ="delete-genre btn-floating  waves-effect waves-light red accent-4 right">
                       <i class ="material-icons small right" >delete </i>
              </button>
            <span class ="field-validation-valid red-text text-darken-4"" data-valmsg-for="Genres[${count}].Name" data-valmsg-replace="true"></span>
        </div>`)

                $allGenres.last().after(elementToAdd);

                setAutoComplete(`#Genre${count + 1}`);

                $(`#Genre${count + 1}`).rules('add', {
                    required: true,
                    messages: {
                        required: "The Genre field is required."
                    }
                });
                if (count === 2) {
                    $('#AddGenre').hide();
                }
            } else {
                return false;
            }

        });

        $('.edit-movie-form').on('submit', function () {

            var result = $(this).valid();
            if (result) {
                $('#loader').show();
            }
        })

    });

});


