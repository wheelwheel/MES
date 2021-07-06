$(document).ready(function () {
        $("#show_hide_password_CurrentPassword a").on('mouseout', function (event) {
            event.preventDefault();
            if ($('#show_hide_password_CurrentPassword input').attr("type") == "text") {
                $('#show_hide_password_CurrentPassword input').attr('type', 'password');
                $('#show_hide_password_CurrentPassword i').addClass("fa-eye-slash");
                $('#show_hide_password_CurrentPassword i').removeClass("fa-eye");
            }
        });

        $("#show_hide_password_CurrentPassword a").on('mouseover', function (event) {
            event.preventDefault();
            if ($('#show_hide_password_CurrentPassword input').attr("type") == "password") {
                $('#show_hide_password_CurrentPassword input').attr('type', 'text');
                $('#show_hide_password_CurrentPassword i').removeClass("fa-eye-slash");
                $('#show_hide_password_CurrentPassword i').addClass("fa-eye");
            }
        });

        $("#show_hide_password_NewPassword a").on('mouseout', function (event) {
            event.preventDefault();
            if ($('#show_hide_password_NewPassword input').attr("type") == "text") {
                $('#show_hide_password_NewPassword input').attr('type', 'password');
                $('#show_hide_password_NewPassword i').addClass("fa-eye-slash");
                $('#show_hide_password_NewPassword i').removeClass("fa-eye");
            }
        });

        $("#show_hide_password_NewPassword a").on('mouseover', function (event) {
            event.preventDefault();
            if ($('#show_hide_password_NewPassword input').attr("type") == "password") {
                $('#show_hide_password_NewPassword input').attr('type', 'text');
                $('#show_hide_password_NewPassword i').removeClass("fa-eye-slash");
                $('#show_hide_password_NewPassword i').addClass("fa-eye");
            }
        });

        $("#show_hide_password_ConfirmPassword a").on('mouseout', function (event) {
            event.preventDefault();
            if ($('#show_hide_password_ConfirmPassword input').attr("type") == "text") {
                $('#show_hide_password_ConfirmPassword input').attr('type', 'password');
                $('#show_hide_password_ConfirmPassword i').addClass("fa-eye-slash");
                $('#show_hide_password_ConfirmPassword i').removeClass("fa-eye");
            }
        });

        $("#show_hide_password_ConfirmPassword a").on('mouseover', function (event) {
            event.preventDefault();
            if ($('#show_hide_password_ConfirmPassword input').attr("type") == "password") {
                $('#show_hide_password_ConfirmPassword input').attr('type', 'text');
                $('#show_hide_password_ConfirmPassword i').removeClass("fa-eye-slash");
                $('#show_hide_password_ConfirmPassword i').addClass("fa-eye");
            }
        }); 
    });