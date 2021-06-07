// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {

    var form = $('#filterform');
    var url = form.attr('action');

    //filterbOX
    $('#filter').keyup(function () {
        $('table').fadeOut(300, function () {
            $('tbody').empty()
            $.ajax({
                url: url,
                data: form.serialize(),
                success: function (data) {
                    $('tbody').empty()
                    $('table').fadeIn(300);
                    $('#restuls').tmpl(data).appendTo('tbody');
                    
                }
            });
        });
    });


    //prevent pressing enter so we dont go to jSON View
    $("#filter").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
        }
    });



});

