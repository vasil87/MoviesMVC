$(() =>{
   $input= $('.datepicker').pickadate({
        selectMonths: true, // Creates a dropdown to control month
        selectYears: 15, // Creates a dropdown of 15 years to control year,
        today: 'Today',
        clear: 'Clear',
        close: 'Ok',
        closeOnSelect: true,// Close upon selecting a date
    });
    var picker = $input.pickadate('picker');
    picker.set('select',  Date.now())

    $('#AddGenre').click(function () {
        var $allGenres = $('.genre');
        var count = $allGenres.length;
        if (count < 3) {
            var elementToAdd = $(`<div class="input-field col s4 genre">
            <label for="Genre1">Genre${count + 1}</label>
            <input data-val="true" data-val-maxlength="The field Genre must be a string." data-val-maxlength-max="500" data-val-required="The Genre field is required." id="Genre${count + 1}" name="Genres[${count}].Name" type="text" value="">
            <span class ="field-validation-valid red-text text-darken-4"" data-valmsg-for="Genres[${count}].Name" data-valmsg-replace="true"></span>
        </div>`)
            $allGenres.last().after(elementToAdd);
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
    $('.create-movie-form').on('submit', function () {
        $(this).valid();
    })

});


    