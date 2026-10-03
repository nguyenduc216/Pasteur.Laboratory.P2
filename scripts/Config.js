var SweetSoft_Settings = {
    Properties: {
        hiddenPropertiesClientID: "",
        updatePanelPropertiesID: "",
        Postback: false,
        panelPostBackProcessingID: "",
        modal: ".systemModal",
        showPostBackPanel: true,
        RegisterComboboxScript: false,
        RegisterDateTimeScript: false,
        RegisterDialogScript: false,
        RegisterCheckBoxScript: false,
        RegisterRadioScript: false,
        RegisterButtonScript: false,
        RegisterSearchBoxScript: false,
        RegisterGridScript: false,
        RegisterMathTextboxScript: false,
        aspnetFormID: "aspnetForm"
    },
    Modal: {
        ProgressInnerHtml: ""
    },
    Validation: {
        Items: []
    },
    Dialog: {
        Items: []
    },
    Grid: {
        Items: []
    },
    Events: {
        OnInit: null
    },
    Sidebar: {
        linkItemEffect: true
    }
}

function OpenMainModal(color) {

    if (SweetSoft_Settings.Properties.modal.length > 0) {
        var modal = $(SweetSoft_Settings.Properties.modal);
        if (color) {
            modal.addClass("background");
        }
        modal.css("display", "block");
    }
}

function CloseMainModal() {
    if (SweetSoft_Settings.Properties.modal.length > 0) {
        var modal = $(SweetSoft_Settings.Properties.modal);
        modal.css("display", "none");
    }
}

function addRequestHanlde(f) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(f);
}

function removeRequestHanlde(f) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(clearRequestHandle);
}

function AjaxGet(url) {
    var datareturn;
    $.ajax({
        type: "Get",
        async: false,
        url: url,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            datareturn = data;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            datareturn = null;
        }
    });
    return datareturn;
}

function AjaxPost(data, url) {
    var datareturn;
    $.ajax({
        type: "post",
        async: false,
        data: JSON.stringify(data),
        url: url,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            datareturn = data.d;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            datareturn = null;
        }
    });
    return datareturn;
}


//function AjaxPostCallback(data, url, callback) {
//    var datareturn;
//    $.ajax({
//        type: "post",
//        async: false,
//        data: JSON.stringify(data),
//        url: url,
//        contentType: 'application/json; charset=utf-8',
//        dataType: 'json',
//        success: function (data) {
//            datareturn = data.d;
//            if (typeof (callback) === "function")
//                callback(datareturn);
//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            datareturn = null;
//        }
//    });
//}
var SPnotify = {};

SPnotify.ShowSuccess = function (title, message, showicon, position) {
    SPnotify.Show('success', title, message, showicon, position);
}

SPnotify.ShowError = function (title, message, showicon, position) {
    SPnotify.Show('error', title, message, showicon, position);
}

SPnotify.ShowInfo = function (title, message, showicon, position) {
    SPnotify.Show('info', title, message, showicon, position);
}

SPnotify.Show = function (vtype, vtitle, message, animate_in, animate_out) {
   
    var opts = {
        title: vtitle,
        text: message,
        addclass: "stack-custom2",
        type: vtype,
        animate: {
            animate: true,
            in_class: animate_in,
            out_class: animate_out,
        },
        width: "420px",
    };
    //console.log("typeof PNotify ", typeof (PNotify));
    if (typeof (PNotify) !== 'undefined')
        new PNotify(opts);
}

var ACEnotify = {};

ACEnotify.ShowSuccess = function (vTitle, vMessage, vImage, vSticky, vCenter, vLight) {
    ACEnotify.Show(vTitle, vMessage, vImage, 'gritter-success ', vCenter, vLight);
}

ACEnotify.ShowError = function (vTitle, vMessage, vImage, vSticky, vCenter, vLight) {
    ACEnotify.Show(vTitle, vMessage, vImage, 'gritter-error ', vCenter, vLight);
}

ACEnotify.ShowWarning = function (vTitle, vMessage, vImage, vSticky, vCenter, vLight) {
    ACEnotify.Show(vTitle, vMessage, vImage, 'gritter-warning ', vCenter, vLight);
}

ACEnotify.Show = function (vTitle, vMessage, vImage, vSticky, vClass, vCenter, vLight) {
    if (vClass.length == 0) vClass = 'gritter-info';
    var opt = {
        position: 'bottom-left',
        title: vTitle,
        text: vMessage,
        sticky: vSticky,
        class_name: vClass + (vCenter ? ' gritter-center' : '') + (vLight ? ' gritter-light' : '')
    };
    if (vImage.length > 0)
        opt.image = vImage;
    $.gritter.add(opt);
}
