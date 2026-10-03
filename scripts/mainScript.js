

function RegisterScript() {
    //try {
    if (typeof (NProgress) !== 'undefined') NProgress.done();

    var systemModal = $(SweetSoft_Settings.Properties.modal);
    if (typeof (systemModal) === 'undefined' || systemModal === null || systemModal.length === 0) {
        systemModal = $('<div>');
        systemModal.addClass('systemModal');
        systemModal.appendTo($('body'));
        systemModal.css("display", "block");
    }
    $("#" + SweetSoft_Settings.Properties.aspnetFormID).validationEngine();
    HeaderCollaspe();

    if (SweetSoft_Settings.Properties.RegisterComboboxScript)
        RegisterComboboxScript();
    if (SweetSoft_Settings.Properties.RegisterMathTextboxScript)
        RegisterMathTextboxScript();
    if (SweetSoft_Settings.Properties.RegisterDateTimeScript)
        RegisterDateTimePicker();

    if (SweetSoft_Settings.Properties.RegisterDialogScript) {
        RegisterDialogScript();
        ReOpenDialog();
        ReCloseDialog();
    }

    if (SweetSoft_Settings.Properties.RegisterGridScript)
        RegisterGridScript();

    if (SweetSoft_Settings.Properties.RegisterCheckBoxScript)
        RegisterCheckboxScript();

    if (SweetSoft_Settings.Properties.RegisterRadioScript)
        RegisterRadioScript();

    // Keypress các control
    $("[data-input-enter='true']").keypress(function (e) {
        if (e.which === 13) {
            var item = $(this);
            var databtn = item.data("enterId");
            if (typeof (databtn) !== 'undefined' && databtn.length > 0) {
                databtn = $("#" + databtn);
                if (typeof (databtn) !== 'undefined') {
                    databtn.click();
                    if (databtn[0].tagName === "A") eval(databtn.attr("href"));
                }
            }
        }
    });

    if (SweetSoft_Settings.Sidebar.linkItemEffect) {
        var linkCol = $(".link-show-processing");
        if (linkCol.length > 0) {
            linkCol.each(function () {
                var a = $(this);
                a.unbind("click");
                a.bind("click", function () {
                    var div = $("<div class='goto-progress'><div class='progress-box'><img class='image' src='/images/loading-gears-animation-3.gif' /></div></div>");
                    console.log("div: ", div);
                    div.appendTo($("body"));
                });
            });
        }
    }

    if (typeof (SweetSoft_Settings.Events.OnInit) === 'function' && SweetSoft_Settings.Events.OnInit != null) { SweetSoft_Settings.Events.OnInit(); }

    //if (SweetSoft_Settings.Properties.RegisterCheckBoxScript)
    //    $('input[type=checkbox][data-toggle^=toggle]').bootstrapToggle();

    $('[data-toggle="tooltip"]').tooltip();

    SweetSoft_Settings.Properties.Postback = true;

    if (SweetSoft_Settings.Properties.modal.length > 0) {
        var modal = $(SweetSoft_Settings.Properties.modal);
        if (modal.length > 0) modal.css("display", "none");
    }

    maskInput();
    //} catch (ex) {
    //    if (SweetSoft_Settings.Properties.modal.length > 0) {
    //        var modal = $(SweetSoft_Settings.Properties.modal);
    //        if (modal.length > 0) modal.css("display", "none");
    //    }
    //}

    BindGuestDetail();
}

function BindGuestDetail() {
    var guest_detail = $(".guest-info-detail");
    console.log("guest_detail: ", guest_detail);
    if (guest_detail.length > 0) {
        $(guest_detail).each(function () {
            var item = $(this);

            item.click(function () {
                var gid = $(this).attr("data-gid");
                console.log("gid: ", gid);
                if (typeof (ShowSpinner) === "function") ShowSpinner();
                if (typeof (PreBindGuestInfo) === "function") PreBindGuestInfo();
                if (typeof (BindGuestInfo) === "function") BindGuestInfo(gid, 0);
            });
        });
    }
}

function maskInput() {
    try {
        $("input.MaskDate[TimeFormat='None'],input.MaskDate:not([TimeFormat])").inputmask("date", { placeholder: "__/__/____" });
        $("input.MaskDate[TimeFormat='Full']").inputmask("datetime", { placeholder: "__/__/____ __:__" });
        $("input.MaskDate[TimeFormat='Half']").inputmask("datetime12", { placeholder: "__/__/____ __:__ _m" });
        $("input.monthandyear").inputmask("monthandyear", { placeholder: "__/____" });
        $(".mask-currency").inputmask("decimal", { radixPoint: ",", autoGroup: true, groupSeparator: ".", groupSize: 3 });
        $(".mask-float").inputmask("decimal", { radixPoint: ",", autoGroup: true, groupSeparator: ".", groupSize: 3 });
        $(".mask-onlyNumberSp").inputmask("integer");
        $(".mask-email").inputmask('Regex', { regex: "[a-zA-Z0-9._%-]+@[a-zA-Z0-9-]+\\.[a-zA-Z]{2,4}" });
    } catch (ex) { console.log("maskInput err: ", ex); }
}

function HeaderCollaspe() {

    var header = $(".card-header.collapse");
    if (header.length > 0) {
        header.each(function () {
            var head = $(this);
            var left = $(".pull-left", head);
            var block = $(".card-block", head);
            left.on("click", function () {
                var i = $("i.fa", left);
                if (i.length > 0) {
                    if (i.hasClass("fa-caret-square-o-down")) {
                        i.removeClass("fa-caret-square-o-down");
                        i.addClass("fa-caret-square-o-right");
                        head.addClass("closed");
                    } else {
                        i.removeClass("fa-caret-square-o-right");
                        i.addClass("fa-caret-square-o-down");
                        block.css("display", "none");
                        head.removeClass("closed");
                    }
                }
            });
        });
    }
}

function __doPostBackAsync(eventName, eventArgs) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    if (!Array.contains(prm._asyncPostBackControlIDs, eventName)) {
        prm._asyncPostBackControlIDs.push(eventName);
    }

    if (!Array.contains(prm._asyncPostBackControlClientIDs, eventName)) {
        prm._asyncPostBackControlClientIDs.push(eventName);
    }

    __doPostBack(eventName, eventArgs);
}


function openLoaderBeforePostback(postbackId) {
    SweetSoft_Settings.Properties.showPostBackPanel = true;
    if (postbackId.length > 0) {
        var postbackElemet = $("#" + postbackId);
        if (typeof (postbackElemet) !== 'undefined' && postbackElemet !== null && postbackElemet.length > 0) {
            var runload = postbackElemet.data("runLoader");
            if (typeof (runload) !== 'undefined' && runload.length > 0) SweetSoft_Settings.Properties.showPostBackPanel = (runload === "true" || runload === true);
        }
    }

    if (typeof (NProgress) !== 'undefined' && SweetSoft_Settings.Properties.showPostBackPanel)
        NProgress.start();

    var systemModal = $(SweetSoft_Settings.Properties.modal);
    if (typeof (systemModal) === 'undefined' || systemModal === null || systemModal.length === 0) {
        systemModal = $('<div>');
        systemModal.addClass('systemModal');
        systemModal.appendTo($('body'));
    }

    if (systemModal.length > 0) systemModal.css("display", "block");
}

function prm_InitializeRequest(sender, args) {
    SweetSoft_Settings.Validation.Items = [];
    var postbackId = args.get_postBackElement().id;
    openLoaderBeforePostback(postbackId);
}

function beginRequestHandler(sender, args) {
    var postbackId = args.get_postBackElement().id;
    openLoaderBeforePostback(postbackId);
}

function endREquestHandler(sender, args) {
    if (args.get_error() !== undefined && args.get_error() !== null) {
        if (lsrw_42qqr.length > 0) {
            var snip = $("#" + lsrw_42qqr + "_snip"); if (snip.length > 0) snip.css("display", "none");
            var icon = $("#" + lsrw_42qqr + "_icon"); if (icon.length > 0) icon.css("display", "inline-block");
            var span = $("#" + lsrw_42qqr + "_text"); if (span.length > 0) span.val(span.attr("default-text"));
        }
        $(SweetSoft_Settings.Properties.modal).css("display", "none");

        console.log("System Script Error: ", args.get_error());

        if (typeof (SPnotify) !== 'undefined')
            SPnotify.Show('error', "System Error", args.get_error(), "slideInRight", "slideOutRight");
        args.set_errorHandled(true);
    }
}

function OpenPageLink(link) {
    ShowLoadingMode(false);
    window.location.href = link;
}

function ShowLoadingMode(embed_background) {
    if (embed_background)
        $("#" + SweetSoft_Settings.Properties.panelPostBackProcessingID).addClass("embackground").css("display", "block");
    else
        $("#" + SweetSoft_Settings.Properties.panelPostBackProcessingID).removeClass("embackground").css("display", "block");
}



$(function () {
    if (SweetSoft_Settings.Events.OnInit == null) {
        SweetSoft_Settings.Events.OnInit = function () {
            $("#" + SweetSoft_Settings.Properties.aspnetFormID).validationEngine({ focusInvalid: true, focusFirstField: true });
        };
    }
    if (SweetSoft_Settings.Properties.RegisterSearchBoxScript) {
        $('body').on('click', function (event) {
            var target = $(event.target);
            if (target.parents('.bootstrap-select').length) {
                event.stopPropagation();
                $(target.parents('.bootstrap-select')).toggleClass('open');
            }
        });

        $(document).on('click', '.dropdown-menu.popup', function (e) {
            e.stopPropagation();
        });
    }

    RegisterScript();
    if (typeof (Sys) !== 'undefined') {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) prm.add_initializeRequest(prm_InitializeRequest);
        if (prm != null) prm.add_endRequest(RegisterScript);
        if (prm != null) prm.add_endRequest(endREquestHandler);
    }

    var goto_progress = $(".goto-progress");
    if (typeof (goto_progress) !== 'undefined' && goto_progress.length > 0) {
        goto_progress.fadeOut(1000, function () {
            $(this).remove();
        });
    }
});





