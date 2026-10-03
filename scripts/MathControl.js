var cr_MathControlID = null;
var cr_activeE = null;
function ShowMathBox(controlClientID) {
    
    $(".math-box-clone").remove();
    $(".open-math-box").removeClass("open-math-box");
    cr_MathControlID = controlClientID;
    //$(".math-control-box").show();
    var math_control_box = $(".math-control-box");
    var math_control_box_clone = math_control_box.clone();
    math_control_box_clone.addClass("math-box-clone");
    var d_out = $("#" + cr_MathControlID);
    math_control_box_clone.appendTo(d_out);
    d_out.addClass("open-math-box");
    cr_activeE = null;

    //alert(controlClientID);
}

function CloseMathBox() {
    cr_MathControlID = null;
    $(".open-math-box").removeClass("open-math-box");
    $(".math-box-clone").remove();
    cr_activeE = null;
}

function CancelMath() {
    var d_out = $("#" + cr_MathControlID);
    var d_content = $(".math-content", d_out);
    var html2 = d_content.html();
    d_content.html(html2 + "&zwnj;");
    html2 = html2.replace("<sup>", "");
    html2 = html2.replace("</sup>", "");
    html2 = html2.replace("<sub>", "");
    html2 = html2.replace("</sub>", "");
    html2 = html2.replace('<label class="math-underline">', '');
    html2 = html2.replace("</label>", "");
    html2 = html2.replace("&zwnj;", "");
    html2 = html2.replace("&nbsp;", "");
  
    d_content.focus();
    SetCursorEnd(d_content[0], html2.length + 1);
}

function m_supscript() {
    var d_out = $("#" + cr_MathControlID);
    var d_content = $(".math-content", d_out);
    if (d_content.length > 0) {
        var sup = $("<sup>");
        sup.appendTo(d_content);
        sup.html("&zwnj;");
        SetCursorEnd(sup[0], 1);
        cr_activeE = sup;
    }
}

function m_subscript() {
    var d_out = $("#" + cr_MathControlID);
    var d_content = $(".math-content", d_out);
    if (d_content.length > 0) {
        var sub = $("<sub>");
        sub.appendTo(d_content);
        sub.html("&zwnj;");
        SetCursorEnd(sub[0], 1);
        cr_activeE = sub;
    }
}

function m_underline(c) {
    var item = null;
    if (cr_activeE != null) item = cr_activeE;
    else {
        var d_out = $("#" + cr_MathControlID);
        var d_content = $(".math-content", d_out);
        cr_activeE = d_content;
    }

    if (cr_activeE != null) {
        var html2 = cr_activeE.html();
        cr_activeE.html(html2 + "&nbsp;");

        var span = $("<label class='math-underline'>" + c + "</label>");

        span.appendTo(cr_activeE);
        html2 = cr_activeE.html();
        cr_activeE.html(html2 + "&nbsp;");
        SetValueHiddenField(d_out, 0);
    }
}
function m_htmlChar(c) {
    var item = null;
    if (cr_activeE != null) item = cr_activeE;
    else {
        var d_out = $("#" + cr_MathControlID);
        var d_content = $(".math-content", d_out);
        cr_activeE = d_content;
    }

    if (cr_activeE != null) {
        var html2 = cr_activeE.html();
        cr_activeE.html(html2 + "&nbsp;" + c + "&nbsp;");
        SetValueHiddenField(d_out, 0);
    }
}



