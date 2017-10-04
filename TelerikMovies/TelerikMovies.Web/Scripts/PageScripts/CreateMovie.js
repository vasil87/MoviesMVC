$(() =>{
    $('.datepicker').pickadate({
        selectMonths: true, // Creates a dropdown to control month
        selectYears: 15, // Creates a dropdown of 15 years to control year,
        today: 'Today',
        clear: 'Clear',
        close: 'Ok',
        closeOnSelect: true,// Close upon selecting a date
    });
    $('#AddGenre').click(function () {
        var $allGenres = $('.genre');
        var count = $allGenres.length;
        if (count < 3) {
            var elementToAdd = $(`<div class="form-group genre">
            <label class ="control-label col-md-2" for="Genre1">Genre${count + 1}</label>
            <div class="col-md-10">
                <input class ="form-control text-box single-line" data-val="true" data-val-maxlength="The field Genre must be a string." data-val-maxlength-max="500" data-val-required="The Genre field is required." id="Genre${count + 1}" name="Genres[${count}].Name" type="text" value="">
                <span class ="field-validation-valid text-danger" data-valmsg-for="Genres[${count}].Name" data-valmsg-replace="true"></span>
            </div>
        </div>`)
            $allGenres.last().after(elementToAdd);
            $(`#Genre${count + 1}`).rules('add', {
                required: true,
                messages: {
                    required: "The field Genre must be a string."
                }
                });
            if (count === 2) {
                $('#AddGenre').prop('disabled', true);
            }
        } else {
            return false;
        }

    });
    $('.create-movie-form').on('submit', function () {
        $(this).valid();
    })

});


    