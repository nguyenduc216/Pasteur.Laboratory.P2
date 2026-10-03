SweetSoft_Settings.Properties.RegisterDialogScript = true;
function OpenDialog(id) {
    currentOpenDialog = id;
}
var currentOpenDialog = "";
var currentCloseDialog = "";
function ReOpenDialog() {
    if (currentOpenDialog.length > 0) {
        var div = $('div#' + currentOpenDialog);
        var dialog = $('div.modal-dialog', div);

        var header = $('div#' + currentOpenDialog + '_header', div);
        div.modal("show");
        div.css("padding-right", "0px !important;");
        if (header.length > 0) {
            if (header.hasClass("bootstrap-dialog-draggable")) {
                var restrictionzone = header.attr("restrictionzone");
                var opt = {};
                opt.handle = 'div#' + currentOpenDialog + '_header';
                if (typeof (restrictionzone) !== 'undefined' && restrictionzone.length > 0) {
                    opt.containment = restrictionzone;
                    opt.scroll = true;
                }
                dialog.draggable(opt);
            }

            var maximize = $("button.maximize", header);
            if (typeof (maximize) !== 'undefined' && maximize.length > 0) {
                var i = $("i:eq(0)", maximize);
                maximize.unbind("click");
                maximize.bind("click", function () {

                    var mdialog = $('div.modal-dialog', div);
                    console.log("maximize click: ", mdialog, i);
                    if (i.hasClass("fa-square-o")) {
                        i.removeClass();
                        i.addClass("fa fa-clone");
                        div.addClass("  ");
                        if (mdialog.length > 0) {

                            mdialog.addClass("modal-fullscreen");
                            var div0 = $("div:eq(0)", mdialog);
                            if (div0.length > 0) div0.addClass("modal-fullscreen");

                            var modal_content = $("div.modal-content", mdialog);
                            if (modal_content.length > 0) modal_content.addClass("modal-fullscreen");
                        }
                    } else {
                        i.removeClass();
                        i.addClass("fa fa-square-o");
                        div.removeClass("modal-fullscreen");
                        $(".modal-fullscreen").removeClass("modal-fullscreen");
                    }
                });
            }
        }

        var dialog = $(SweetSoft_Settings.Dialog.Items).filter(function (idx, item) {
            return item.ID === currentOpenDialog;
        });

        if (dialog == null || dialog.length == 0) {
            dialog = { ID: currentOpenDialog, OpenState: true }
            SweetSoft_Settings.Dialog.Items.push(dialog);
        }
        else dialog[0].OpenState = true;
        currentOpenDialog = "";
    }
}

function ReCloseDialog() {
    if (currentCloseDialog.length > 0) {
        var div = $('div#' + currentCloseDialog);
        var span = $('span#' + currentCloseDialog);

        div.modal('hide');
        div.removeClass("in");

        var dialog = $(SweetSoft_Settings.Dialog.Items).filter(function (idx, item) {
            return item.ID === currentCloseDialog;
        });
        //console.log("dialog: ", dialog);
        if (dialog == null || dialog.length == 0) {
            dialog = { ID: currentCloseDialog, OpenState: false }
            SweetSoft_Settings.Dialog.Items.push(dialog);
        }
        else dialog[0].OpenState = false;

        var modal_backdrop = $(".modal-backdrop");
        if (modal_backdrop.length > 0) {
            if (modal_backdrop.length == 1)
                $("body").removeClass("modal-open");
            modal_backdrop[0].remove();
        }
    }
    currentCloseDialog = "";
}

function CloseDialogServer(id) {
    currentCloseDialog = id;
}

function CloseDialogClient(id) {
    currentCloseDialog = id;
    ReCloseDialog();
    return false;
}

function OpenDialogClient(id) {
    currentOpenDialog = id;
    ReOpenDialog();
}

function RegisterDialog(id) {
    var dialog = SweetSoft_Settings.Dialog.Items.filter(function (item, idx) {
        console.log(item, idx);
    });
    if (dialog == null) {
        dialog = { ID: id, OpenState: false }
        SweetSoft_Settings.Dialog.Items.push(dialog);
    }
}

function RegisterDialogScript() {


    var modal = $("div.modal");
    if (modal.length > 0) {
        modal.each(function () {
            var id = $(this).attr("id");
            if (!SweetSoft_Settings.Properties.Postback) {
                var showonload = $(this).attr("data-showonload");
                if (typeof (showonload) !== ' undefined' && showonload === "true") {
                    currentOpenDialog = id;
                    ReOpenDialog();
                }
            }
            else {
                var id = $(this).attr("id");
                var dialog = $(SweetSoft_Settings.Dialog.Items).filter(function (idx, item) {
                    return item.ID === id;
                });
                if (dialog != null && dialog.length > 0 && dialog[0].OpenState) $(this).addClass("in");
            }
        });
    }
}

