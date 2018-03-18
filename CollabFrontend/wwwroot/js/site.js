// Write your Javascript code.
$(function() {

    $('#login-form-link').click(function(e) {
    	$("#login-form").delay(100).fadeIn(100);
 		$("#register-form").fadeOut(100);
		$('#register-form-link').removeClass('active');
		$(this).addClass('active');
		e.preventDefault();
	});
	$('#register-form-link').click(function(e) {
		$("#register-form").delay(100).fadeIn(100);
 		$("#login-form").fadeOut(100);
		$('#login-form-link').removeClass('active');
		$(this).addClass('active');
		e.preventDefault();
	});

});

(function() {
    $('#login-form input').keyup(function() {

        var empty = false;
        $('#login-form input').each(function() {
            if ($(this).val() == '') {
                empty = true;
            }
        });

        if (empty) {
            $('#login-submit').attr('disabled', 'disabled'); 
        } else {
            $('#login-submit').removeAttr('disabled'); 
        }
	});
	
	$('#register-form input').keyup(function() {

        var empty = false;
        $('#register-form input').each(function() {
            if ($(this).val() == '') {
                empty = true;
            }
        });

        if($("#password-register").val().valueOf() == $("#confirm-password").val().valueOf() && $("#password-register").val().valueOf()!=="") {
            empty = false;
            $("#message").html("");
        } else if($("#password-register").val().valueOf() != $("#confirm-password").val().valueOf()) {
            $("#message").html("Passwörter stimmen nicht überein");
            empty = true;
        } else if($("#password-register").val().valueOf()=="") {
            $("#message").html("");
            empty = true;
        }

        if (empty) {
            $('#register-submit').attr('disabled', 'disabled');
        } else {
            $('#register-submit').removeAttr('disabled');
        }
	});
	
	$('.newlistitemname').keyup(function() {
        var empty = false;
		if ($(this).val() == '') {
			empty = true;
        }

        if (empty) {
            $('.createNewItem').attr('disabled', 'disabled'); 
        } else {
            $('.createNewItem').removeAttr('disabled'); 
        }
	});
	
	$('#newListName').keyup(function() {
        var empty = false;
		if ($("#newListName").val() == '') {
			empty = true;
		}

        if (empty) {
            $('#createNewListButton').attr('disabled', 'disabled'); 
        } else {
            $('#createNewListButton').removeAttr('disabled'); 
        }
    });

    $('#contact-form-input input').keyup(function() {
        var empty = false;

        if($("#email-contact").val().valueOf() == '') {
            empty = true;
        } else if($("#firstname-contact").val().valueOf() == '') {
            empty = true;
        } else if($("#lastname-contact").val().valueOf() == '') {
            empty = true;
        }

        if (empty) {
            $('#save_button').attr('disabled', 'disabled');
        } else {
            $('#save_button').removeAttr('disabled'); 
        }
    });

    $('#contact-password-input input').keyup(function() {
        var empty = false;

        if ($("#password-contact").val().valueOf() != $("#confirm-password-contact").val().valueOf()) {
            empty = true;
        }

        if (empty) {
            $('#changePassword').attr('disabled', 'disabled');
        } else {
            $('#changePassword').removeAttr('disabled'); 
        }
    });
})()