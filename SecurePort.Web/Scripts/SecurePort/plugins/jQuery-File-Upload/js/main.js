$(function () {
    
    var jqXHRData;
    
    $('#fileupload').fileupload( {
        dataType: 'json',
        add: function (e, data) {
            jqXHRData = data;
        },
        fail: function (event, data) {
            $("#tbx-file-path").val('');
            document.getElementById("hl-start-upload").disabled = true;
        }
    });
    
    $("#fileupload").on('change', function () {
        $("#tbx-file-path").val(jqXHRData.files[0].name);
        document.getElementById("hl-start-upload").disabled = false;
    });
    
    $("#hl-start-upload").on('click', function () {
        if (jqXHRData) {
            $('#fileupload').addClass('fileupload-processing');
            jqXHRData.submit();
        }
        return false;
    });
    
});
    