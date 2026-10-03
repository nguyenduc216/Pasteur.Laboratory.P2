SweetSoft_Settings.Properties.RegisterComboboxScript = true;
function RegisterComboboxScript() {
    var selectpicker = $(".selectpicker");
    if (typeof (selectpicker) !== 'undefined' && selectpicker.length > 0) {
        selectpicker.each(function (idx) {
            var item = $(this);
            var multipleText = item.attr("data-mutiple-text-format");
            var singleText = item.attr("data-single-text-format");
            $(this).selectpicker({
                countSelectedText: function (numSelected, numTotal) {
                    return (numSelected == 1) ? singleText : multipleText;
                }
            });
        });
    }
}