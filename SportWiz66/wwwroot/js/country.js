// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//this write the country name next to special offer at HOME Dorian Yates Banner
$(function () {


    $.getJSON("https://api.ipgeolocation.io/ipgeo?apiKey=8fa9497c971f42118416e2a72d640cd4&fields=country_name&output=json", function (data) {
    })
        .done(function (data) {
            var country = data["country_name"];
            var result = "We have special Offers for " + country +"!";
            $("#offers").text(result);
        })
        .fail(function () {
            $("#offers").text("Special Offers");
        });

});


