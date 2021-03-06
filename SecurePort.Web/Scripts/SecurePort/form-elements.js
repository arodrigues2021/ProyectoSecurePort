var FormElements = function () {
    //function to initiate jquery.inputlimiter
    var runInputLimiter = function () {
        $('.limited').inputlimiter({
            remText: 'You only have %n character%s remaining...',
            remFullText: 'Stop typing! You\'re not allowed any more characters!',
            limitText: 'You\'re allowed to input %n character%s into this field.'
        });
    };
    //function to initiate query.autosize    
    var runAutosize = function () {
        $("textarea.autosize").autosize();
    };
    //function to initiate Select2
    //var runSelect2 = function () {
    //    $(".search-select").select2({
    //        placeholder: "Select a State",
    //        allowClear: true
    //    });
    //};
    //function to initiate jquery.maskedinput
    var runMaskInput = function () {
        $.mask.definitions['~'] = '[+-]';
        $('.input-mask-number').mask('99999');
        $('.input-mask-date').mask('99/99/9999');
        $('.input-mask-phone').mask('99-999-9999');
        $('.input-mask-eyescript').mask('~9.99 ~9.99 999');
        $(".input-mask-product").mask("a*-999-a999", {
            placeholder: " ",
            completed: function () {
                alert("You typed the following: " + this.val());
            }
        });
    };
    //var runMaskMoney = function () {
    //    $(".currency").maskMoney();
    //};
    //function to initiate bootstrap-datepicker
    var runDatePicker = function () {
        $('.date-picker').datepicker({
            autoclose: true,
            weekStart: 1
        });
    };
    //function to initiate bootstrap-timepicker
    var runTimePicker = function () {
        $('.time-picker').timepicker();
    };
    //function to initiate daterangepicker
    var runDateRangePicker = function () {
        $('.date-range').daterangepicker();
        $('.date-time-range').daterangepicker({
            timePicker: true,
            timePickerIncrement: 15,
            format: 'DD/MM/YYYY h:mm A'
        });
    };
    //function to initiate bootstrap-colorpicker
    var runColorPicker = function () {
        $('.color-picker').colorpicker({
            format: 'hex'
        });
        $('.color-picker-rgba').colorpicker({
            format: 'rgba'
        });
        $('.colorpicker-component').colorpicker();
    };
    //function to initiate jquery.tagsinput
    var runTagsInput = function () {
        $('#tags_1').tagsInput({
            width: 'auto'
        });
    };
    //function to initiate summernote
    var runSummerNote = function () {
        $('.summernote').summernote({
            height: 300,
            tabsize: 2
        });
    };
    //function to initiate ckeditor
    //var runCKEditor = function () {
    //    CKEDITOR.disableAutoInline = true;
    //    $('textarea.ckeditor').ckeditor();
    //};
    return {
        //main function to initiate template pages
        init: function () {
            runInputLimiter();
            runAutosize();
            //runSelect2();
            //runMaskInput();
            //runMaskMoney();
            //runDatePicker();
            //runTimePicker();
            //runDateRangePicker();
            //runColorPicker();
            //runTagsInput();
            //runSummerNote();
            //runCKEditor();
        }
    };
}();