function ButtonCheckValid(sender) {
    var button = $("#" + sender);
    if (button.length > 0) {
        var group_name = button.attr("data-group-name");
        var bound = $(group_name);

        SweetSoft_Settings.Validation.Items = bound;

        var result = RunValidElement();
        var data = group_name.replace(".vlg-", "");
        var divBound = $("[validation-group-bound='" + data + "']");
        if (divBound.length === 0) divBound = $("[validation-group-bound='default']");
        if (divBound.length > 0 && SweetSoft_Settings.Validation.Items.length > 0) {
            var position = $(SweetSoft_Settings.Validation.Items[0]).position();
            divBound.animate({ scrollTop: position.top - 40 }, 'slow');
        }
        if (!result) {
            setTimeout(function () {
                var snip = $("#" + sender + "_snip"); if (snip.length > 0) snip.css("display", "none");
                var icon = $("#" + sender + "_icon"); if (icon.length > 0) icon.css("display", "inline-block");
                var span = $("#" + sender + "_text"); if (span.length > 0) span.val(span.attr("default-text"));
                $(SweetSoft_Settings.Properties.modal).css("display", "none");
            }, 700);

        }
        return result;
    }
    return false;
}

function RunValidElement() {
    var result = true;

    if (SweetSoft_Settings.Validation.Items.length > 0) {
        SweetSoft_Settings.Validation.Items.each(function () {
            var item = $(this);

            var rs = item.validationEngine('validate');
            result = result && rs;
        });
    }
    return result;
}