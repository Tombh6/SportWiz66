// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {


    var result = 
    
    $(document).on("click", '.tweet', function (event) {
        var productId = $(this).attr("id");

        $.ajax({
            url: '/Products/Tweet',
            data: { id: productId },
            success: function (data) {

                $('<div></div>').dialog({
                    modal: true,
                    title: "Confirmation",
                    open: function () {
                        $(this).html(data);
                    },
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });

            },
            error: function () {
                alert("error");
            }
        });



    });




});

