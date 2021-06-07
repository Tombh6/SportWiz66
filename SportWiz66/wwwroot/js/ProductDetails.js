// Please see documentation at https://docs.microsoft.com/aspnet/dvmanshri/client-side/bundling-and-minification
//Made By: David Manshari! at 4am :(

// Write your JavaScript code.


$(function ()
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Show More Reviews Button CODE
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Show Reviews AREA CODE
    //vars to keep reviews we loaded 5 reviews
    var skipCount = 0;
    var takeCount = 5;
    var hasMoreRecords = true;

    function show() {
        $.ajax({
            url: '/Reviews/Index',
            data: { productId: location.href.substr(location.href.lastIndexOf('/') + 1), skipCount: skipCount, takeCount: takeCount },
            success: function (data) {
                if (data == 0) {
                    hasMoreRecords = false; // signal no more records to display
                    $('#ShowMoreReviews').text("No More Reviews To Show").css('background-color', 'red');
                }
                else {
                    $('#results').tmpl(data).appendTo('.comment-list').hide().fadeIn(1000);;
                    skipCount += takeCount; // update for next iteration
                }
            },
            error: function () {
                alert("error");
            }
        });
    }
    show();

    //show more reviews button
    $(document).on("click", '#ShowMoreReviews', function (event) {
        $([document.documentElement, document.body]).animate({
            scrollTop: $("#results").offset().top
        } ,2000);
        show();
    });



    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //SUBMIT Review Button CODE
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    $(document).on("click", '#CommentSubmit', function (event) {
        $.post({
            url: "/Reviews/Create",
            data: { Title: $('#Title').val(), Content: $('#Content').val(), productId: location.href.substr(location.href.lastIndexOf('/') + 1) },
        })
            .done(function () {
                runEffect("#writeReview", "explode");
                //we want to reload all reviews and show the new one on top!
                skipCount = 0;
                takeCount = 5;
                hasMoreRecords = true;
                $(".single-comment").remove();
                $("#counter").load(window.location.href + " #counter"); //update counter
                
   
                show();

                //scrollup
                $([document.documentElement, document.body]).animate({
                    scrollTop: $(".comments-area").offset().top
                }, 2000);

            })
            .fail(function () {
                
            });
    });


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Delete Review Button code
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    $(document).on("click", ".genric-btn", function (event) {
        var reviewId = $(this).attr("id");
        $.ajax({
            url: '/Reviews/Delete',
            data: { id: reviewId },
            success: function (data) {
                if (data == 0) //if try to delete review that already gone
                    alert("You alredy delete!");
                else {
                    var result = "div#" + reviewId; //delete <div with id=reviewID ===> <div class="single-comment justify-content-between d-flex"....
                    runEffect(result,"explode");
                    $(result).remove();
                    $("#counter").load(window.location.href + " #counter");
 
                }             
            },
            error: function () {
                alert("error");
            }
        });
    });


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Add To Cart Button code, Max 5 items per customer
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    $(document).on("click", '#addToCart', function (event) {
        let numOfItems = $('#quantity').val(); //items quantity
        $.ajax({
            url: '/ShoppingCarts/AddToCart',
            data: { productId: location.href.substr(location.href.lastIndexOf('/') + 1) },
            success: function (data) {

                //Display Message from server under the "ADD TO CART" button and run effect
                $("#serverAnswer").html(data);
                ShowEffect("#serverAnswer", "highlight");

                //update cart logo counter
                var jqxhr = $.get("/ShoppingCarts/GetNumOfItems")
                    .done(function (data) {
                        $('.shopping-card').addClass('change').attr('data-content', data);
                    })
                    .fail(function () {
                        $('.shopping-card').addClass('change').attr('data-content', "Error");
                    });

            },
            error: function () {
                alert("error");
            }
        });
    });


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //RunEffects code
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //used for delete 
    function runEffect(ExplodingArea, effectType) {
        // get effect type from
        var selectedEffect = effectType;
        // Most effect types need no options passed by default
        var options = {};
        // some effects have required parameters
        if (selectedEffect === "scale") {
            options = { percent: 50 };
        } else if (selectedEffect === "size") {
            options = { to: { width: 200, height: 60 } };
        }

        // Run the effect
        $(ExplodingArea).toggle(selectedEffect, options, 1000);
    };


    //used for show
    function ShowEffect(Area, effectType) {
        // get effect type from
        var selectedEffect = effectType;

        // Most effect types need no options passed by default
        var options = {};
        // some effects have required parameters
        if (selectedEffect === "scale") {
            options = { percent: 50 };
        } else if (selectedEffect === "transfer") {
            options = { to: "#button", className: "ui-effects-transfer" };
        } else if (selectedEffect === "size") {
            options = { to: { width: 200, height: 60 } };
        }

        // Run the effect
        $(Area).effect(selectedEffect, options, 500, callback);
    };

    // Callback function to bring a hidden box back
    function callback() {
        setTimeout(function () {
            $(Area).removeAttr("style").hide().fadeIn();
        }, 1000);
    };


});









