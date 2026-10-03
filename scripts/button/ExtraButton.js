SweetSoft_Settings.Properties.RegisterButtonScript = true;
var lsrw_42qqr = "";
function ClickSnip(id) {
    lsrw_42qqr = id;
    SweetSoft_Settings.Properties.showPostBackPanel = false;
    var icon = $("#" + id + "_icon");
    var snip = $("#" + id + "_snip");
    var span = $("#" + id + "_text");
    if (typeof (icon) !== 'undefined' && icon.length > 0 &&
        typeof (snip) !== 'undefined' && snip.length > 0) {
        if (typeof (SweetSoft_Settings.Properties.panelPostBackProcessingID) !== 'undefined' &&
            SweetSoft_Settings.Properties.panelPostBackProcessingID.length > 0)
            $("#" + SweetSoft_Settings.Properties.panelPostBackProcessingID).css("display", "none");
        icon.css("display", "none");
        snip.css("display", "inline-block");
        if (typeof (span) !== 'undefined' && span.length > 0) {
            var sniptext = span.attr("sniptext");
            span.attr("default-text", span.val());
            if (typeof (sniptext) !== 'undefined' && sniptext.length > 0) span.html(sniptext);
        }
        $(SweetSoft_Settings.Properties.modal).css("display", "block");

    }
}