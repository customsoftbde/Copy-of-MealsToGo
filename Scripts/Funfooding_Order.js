jQuery(document).ready(function () {
    //Add to cart
    jQuery('#PickUpTime #LastOrderTime').datepicker({ dateFormat: "dd/mm/yy HH:MM tt" });
    
    //remove from cart
    jQuery('#remove').click(function () {
        var id = 1;
        jQuery.ajax("Home/RemoveealFromCart",
         {
             data: { id: id },
             type: "POST",
             success: function (data) {
                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });
    //Update no. of quontity
    jQuery('#remove').click(function () {
        var id = 1, qty = 1;
        jQuery.ajax("Home/UpdatetoCart",
         {
             data: { id: id, qty: qty },
             type: "POST",
             success: function (data) {
                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });
    if (jQuery('#profilepic').length > 0) {
        jQuery('#Photo').hide();
    }
    else {
        jQuery('#Photo').show();
    }
    jQuery('.RemoveProfilePhoto').click(function () {
        var id = jQuery(this).attr('name');
        jQuery.ajax("/User/RemovePhoto",
         {
             data: { id: id },
             type: "POST",
             success: function (data) {
                 jQuery('#Photo').show();
                 jQuery('.RemoveProfilePhoto').hide();
                 jQuery('#profilepic').hide();
                 
                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });

    jQuery('.DeleteMealItem').click(function () {
        var id = jQuery(this).attr('name');
        if (confirm('Are you sure you want to delete this record?')) {
            jQuery.ajax("/MealItem/Delete",
             {
                 data: { id: id },
                 type: "POST",
                 success: function (data) {
                     location.reload(true);
                     //jQuery('#' + id).remove();
                     //if (jQuery('#' + id).length == 0) {
                     //    jQuery('#photos').hide();
                     //}
                     //Remove and Display no. records in cart at the header section
                 },
                 error: function (e, s, t) {
                     alert(e);
                 }
             });
        }
    });
    ///

    jQuery('.RemovePhoto').click(function () {
        var id = jQuery(this).attr('name');
        jQuery.ajax("/MealItem/RemovePhoto",
         {
             data: { id: id },
             type: "POST",
             success: function (data) {
                 jQuery('#' + id).remove();
                 if (jQuery('#' + id).length == 0) {
                     jQuery('#photos').hide();
                 }
                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });
    jQuery('#aMealAdSchedules').click(function () {
        var nextid = jQuery('#tblMealAdSchedules').children().children().length;
        
        var newRow ='<tr><td><input class="text-box single-line" data-val="true" data-val-date="The field PickUpStartDateTime must be a date." data-val-required="The PickUpStartDateTime field is required." id="MealAdSchedules_' + nextid + '__PickUpStartDateTime" name="MealAdSchedules[' + nextid + '].PickUpStartDateTime" type="datetime" value="">' +
        '<span class="field-validation-valid" data-valmsg-for="MealAdSchedules[' + nextid + '].PickUpStartDateTime" data-valmsg-replace="true"></span>&nbsp;' +
        '<input class="text-box single-line" data-val="true" data-val-date="The field PickUpEndDateTime must be a date." data-val-required="The PickUpEndDateTime field is required." id="MealAdSchedules_' + nextid + '__PickUpEndDateTime" name="MealAdSchedules[' + nextid + '].PickUpEndDateTime" type="datetime" value="">' +
        '<span class="field-validation-valid" data-valmsg-for="MealAdSchedules[' + nextid + '].PickUpEndDateTime" data-valmsg-replace="true"></span></td></tr>';

        jQuery('#tblMealAdSchedules tr:last').after(newRow);
        return false;
    });

});