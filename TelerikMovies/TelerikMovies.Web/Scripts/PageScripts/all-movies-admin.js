$(() => {
    var urlForDataTableData = $('#movies-data-table-admin').data('url');
    var urlToDeleteRows = $('#delete-movie').data('url');
    var urlToEdit = $('#edit-movie').data('url');
    var undoUrl = $('#undo-delete-movie').data('url');

    var timeFormat = function (data, type, row, meta) {
        if (data.indexOf('-62135596800000') !== -1) {
            return "";
        }
        return new Date(parseInt(data.replace(/\/Date\((.*?)\)\//gi, "$1"))).toISOString().split('T')[0];

    }

    $('#movies-data-table-admin').dataTable({
        dom: 'flrtpBi',
        responsive: true,
        searching: true,
        ordering: true,
        "processing": true,
        "serverSide": true,
        "info": true,
        "stateSave": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50]],
        "ajax": {
            "url": urlForDataTableData,
            "type": "POST",
        },
        "columns": [
                { "data": "Name", "orderable": true },
                {
                    "data": "ReleaseDate",
                    "orderable": true,
                    "render": timeFormat
                },
                {
                    "data": "CreatedOn", "orderable": true, "render": timeFormat
                },
                {
                    "data": "ModifiedOn", "orderable": true, "render": timeFormat
                },
                {
                    "data": "DeletedOn", "orderable": true, "render": timeFormat
                },
                { "data": "isDeleted", "className": "is-deleted","orderable": true },
                { "data": "Likes", "orderable": true },
                { "data": "Dislikes", "orderable": true },
                { "data": "Id", "className": "movie-id", "orderable": false, "searchable": false },

        ],
        "order": [[0, "asc"]],
        buttons: [
                'selectAll',
                'selectNone'
        ],
        select: true,
    });


    $('#delete-movie').click(function () {
        var ids = [];
        

        $('.selected').each((i, el) =>
        {
            if ($(el).children('.is-deleted').text().toLowerCase()!='true')
            ids.push($(el).children('.movie-id').text())
        });

        if (ids.length === 0) {
            Materialize.toast('Please select rows first', 3000)
            return false;
        }

        $.ajax({
            url: urlToDeleteRows,
            type: "POST",
            data: JSON.stringify({ ids: ids }),
            contentType: "application/json; charset=utf-8",
        }).done(function (result) {
            $('#movies-data-table-admin').DataTable()
            .draw();
            Materialize.toast('Succesfully removed', 3000)
        });
    });

    $('#undo-delete-movie').click(function () {
        var ids = [];

        $('.selected').each((i, el) => {
            if ($(el).children('.is-deleted').text().toLowerCase() != 'false')
                ids.push($(el).children('.movie-id').text())
        });

        if (ids.length === 0) {
            Materialize.toast('Please select rows first', 3000)
            return false;
        }

        $.ajax({
            url: undoUrl,
            type: "POST",
            data: JSON.stringify({ ids: ids }),
            contentType: "application/json; charset=utf-8",
        }).done(function (result) {
            $('#movies-data-table-admin').DataTable()
            .draw();
            Materialize.toast('Succesfully removed', 3000)
        });
    });

    $('#edit-movie').click(function () {
        var ids = [];
        $('.selected').each((i, el) => {
                ids.push($(el).children('.movie-id').text())
        });

        if (ids.length === 0) {
            Materialize.toast('Please select row first', 3000)
            return false;
        }
        else if (ids.length > 1)
        {
            Materialize.toast('Please select single row', 3000)
            return false;
        }

        window.location.replace(urlToEdit + ids[0]);

    });


    $('#movies-data-table-admin tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
    });

});


