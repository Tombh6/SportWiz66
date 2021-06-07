// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var cartNum = 0;
$(function () {

    
    //searchBox
    $("#searchBox").autocomplete({
        source: "/Products/Search",
        minLength: 2,
        select: function (event, ui) {
            location.href = '/Products/Details/' + ui.item.id;
        }

    });



    //shoppingCart Items Counter, add to css where attr data-content
    var jqxhr = $.get("/ShoppingCarts/GetNumOfItems")
        .done(function (data) {
            $('.shopping-card').addClass('change').attr('data-content', data);
        })
        .fail(function () {
            $('.shopping-card').addClass('change').attr('data-content', "Error");
        });


});


