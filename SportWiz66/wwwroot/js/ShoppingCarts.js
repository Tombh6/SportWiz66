// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
//Made By: David Manshari! at 4am :(

// Write your JavaScript code.


$(function ()
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Change Quantity Selection code
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $(".mdb-select").on('change', function () {  //the dropbox select
        var cartItemId = $(this).attr("id"); //get cartItem Id
        var quantity = $(this).val(); //get selected quantity
        var result = "tr." + cartItemId;
        if (quantity == 0) {
            updateQuantity(cartItemId, quantity); //sends quantity to server
            runEffect(result, "drop");      
        }

        else {
            updateQuantity(cartItemId, quantity); //sends quantity to server
            ShowEffect(result, "highlight");
        }

        
    });

    //send to server the quantity
    function updateQuantity(cartItemId, quantity) {
        $.ajax({
            url: '/ShoppingCarts/UpdateItemQuantity',
            data: { cartItemId: cartItemId, quantity: quantity },
            success: function (data) {

                //if server return this message, display it
                var message = "Sorry it doesn't exists";
                if (data == message)
                    alert("You can't delete it, because it doesn't exsits");

                //update shoppingcart counter, if no items in cart(0) reload page so we get "your cart is empty page"
                var jqxhr = $.get("/ShoppingCarts/GetNumOfItems")
                    .done(function (data) {
                        if (data == 0) {
                            location.reload();
                        }
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
    }








    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //RunEffects code
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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








