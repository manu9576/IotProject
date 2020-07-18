$(document).ready(function() 
{
    $('#bt_envoyer').click(function() 
    {
        $('#fr1').attr('action',
                       'mailto:manu9576@free.fr?subject=' +
                       $('#email_mail').val() + '&body=' + $('#txt_mail').val());
        $('#fr1').submit();
    });
});