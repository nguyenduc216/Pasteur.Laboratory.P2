
SweetSoft_Settings.Properties.RegisterMathTextboxScript = true;

function RegisterMathTextboxScript() {
    var math_outer = $(".math-outer");

    if (math_outer.length > 0) {
        math_outer.each(function () {
            var d_out = $(this);
            var d_out_id = d_out.attr("id");
            var d_content = $(".math-content", d_out);
            var h_id = d_out.data("hid");


            var h_control = $("input[type='hidden'][id$='" + h_id + "']");


            var ct = h_control.val();
            while (ct.indexOf("&lt;") > -1)
                ct = ct.replace("&lt;", "<");
            while (ct.indexOf("&gt;") > -1)
                ct = ct.replace("&gt;", ">");


            d_content.html(ct);
            d_content.html(ct);

            d_content.unbind("keydown");
            d_content.bind("keydown", function (e) {
                var d = $(this);
                var isCtrl = d.attr("data-ctrl") === "1";
                var isShift = d.attr("data-shift") === "1";
                var isAlt = d.attr("data-alt") === "1";
                var isUp = d.attr("data-up") === "1";
                var isDown = d.attr("data-down") === "1";
                var isSup = d.attr("data-sup") === "1";
                var isSub = d.attr("data-sub") === "1";


                var k = e.key;
                if (e.keyCode == 17) {
                    isCtrl = true;
                    d.attr("data-ctrl", "1");
                }

                if (e.keyCode == 16) {
                    isShift = true;
                    d.attr("data-shift", "1");
                }

                if (e.keyCode == 18) {
                    isAlt = true;
                    d.attr("data-alt", "1");
                }

                if (e.keyCode == 38) {
                    isUp = true;
                    d.attr("data-up", "1");
                }

                if (e.keyCode == 40) {
                    isDown = true;
                    d.attr("data-down", "1");
                }
                //console.log(" isCtrl: ", isCtrl, " sAlt: ", isAlt, " isUp: ", isUp, " isDown: ", isDown, "e.keyCode:", e.keyCode);

                var k_ok = false;
                k_ok |= (e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105);
                k_ok |= (e.keyCode >= 65 && e.keyCode <= 90);
                k_ok |= (e.keyCode === 38 || e.keyCode === 40);

                if (e.keyCode === 27) {
                    d.attr("data-sup", "0");
                    d.attr("data-sub", "0");
                    d.attr("data-up", "0");
                    d.attr("data-down", "0");
                    d.attr("data-ctrl", "0");
                    d.attr("data-alt", "0");

                    var html2 = d.html();
                    d.html(html2 + "&zwnj;" + "&zwnj;" + "&zwnj;");
                    while (html2.indexOf("<sup>") > -1)
                        html2 = html2.replace("<sup>", "");
                    while (html2.indexOf("</sup>") > -1)
                        html2 = html2.replace("</sup>", "");
                    while (html2.indexOf("<sub>") > -1)
                        html2 = html2.replace("<sub>", "");
                    while (html2.indexOf("</sub>") > -1)
                        html2 = html2.replace("</sub>", "");
                    while (html2.indexOf('<label class="math-underline">') > -1)
                        html2 = html2.replace('<label class="math-underline">', '');
                    while (html2.indexOf("</label>") > -1)
                        html2 = html2.replace("</label>", "");
                    while (html2.indexOf("&zwnj;") > -1)
                        html2 = html2.replace("&zwnj;", "");
                    while (html2.indexOf("&nbsp;") > -1)
                        html2 = html2.replace("&nbsp;", "");

                    SetCursorEnd(d[0], html2.length + 1);
                    cr_activeE = null;
                }
                //else {
                //    if (isCtrl && isUp) { // ctrl + up
                //        console.log(" k_ok2: ", k_ok);
                //        e.preventDefault();

                //        if (k_ok) {

                //            isSup = true;
                //            d.attr("data-sup", "1");
                //            var sup = $("<sup>");
                //            sup.appendTo(d);

                //            sup.html("&zwnj;");
                //            SetCursorEnd(sup[0], 1);

                //        }
                //    }
                //    if (isCtrl && isDown) {// ctrl + shift + =
                //        e.preventDefault();

                //        if (k_ok) {
                //            isSub = true;
                //            var sub = $("<sub>");
                //            sub.appendTo(d);
                //            sub.html("&zwnj;");
                //            SetCursorEnd(sub[0], 1);
                //            d.attr("data-sub", "1");

                //        }
                //    }
                //}
                var ct = d.html();

                if (k_ok || e.keyCode === 231) ct = ct + k;

            });
            d_content.unbind("focusout");
            d_content.bind("focusout", function (e) {

                SetValueHiddenField(d_out, 1);
            });

            d_out.unbind("focusin");
            d_out.bind("focusin", function (e) {

                var showkeyboard = d_content.attr("data-showkeyboard");
                if (showkeyboard === "1")
                    ShowMathBox(d_out_id);
            });

            //d_out.unbind("focusout");
            //d_out.bind("focusout", function (e) {
            //    CloseMathBox();
            //});
            d_content.unbind("keyup");
            d_content.bind("keyup", function (e) {
                if (e.keyCode == 17) {
                    d_content.attr("data-ctrl", "0");
                }

                if (e.keyCode == 16) {
                    d_content.attr("data-shift", "0");
                }

                if (e.keyCode == 18) {
                    d_content.attr("data-alt", "0");
                }

                if (e.keyCode == 38) {
                    d_content.attr("data-up", "0");
                }

                if (e.keyCode == 40) {
                    d_content.attr("data-down", "0");
                }
            });
        });
    }
}

function SetValueHiddenField(d_out, checkspan) {
    var d_content = $(".math-content", d_out);
    var h_id = d_out.data("hid");


    var h_control = $("input[type='hidden'][id$='" + h_id + "']");
    var ct = d_content.html();

    var rs = '';
    if (checkspan === 1 && typeof (ct) !== 'undefined' && ct.indexOf("span") > -1) {
        var span = $(ct);

        if (span.length > 0) {
            span.each(function (e) {
                var sp = $(this);

                var content = sp.html();
                var style = sp.attr("style");
                //console.log("content:", content, style);
                if (style.indexOf("top: -0.5em") > -1)
                    rs = rs + '<sup>' + content + '</sup>';
                else if (style.indexOf("bottom: -0.5em") > -1)
                    rs = rs + '<sub>' + content + '</sub>';
                else
                    rs = rs + content;
            });
        }
    } else rs = ct;


    while (rs.indexOf("<") > -1)
        rs = rs.replace("<", "&lt;");
    while (rs.indexOf(">") > -1)
        rs = rs.replace(">", "&gt;");


    h_control.val(rs);

}

function InitMathTextbox(divContent, element, subType) {
    element.bind("focusin", function () {
        divContent.attr(subType, "1");
    });

    element.bind("focusout", function () {
        divContent.attr(subType, "0");
    });
}

function SetCursorEnd(element, c) {
    var selection = window.getSelection();

    range = createRange(element, { count: c });

    if (range) {
        range.collapse(false);
        selection.removeAllRanges();
        selection.addRange(range);
    }
    //var range = document.createRange();
    //var sel = window.getSelection();
    //range.setStart(element, 1);
    //range.collapse(true);
    //sel.removeAllRanges();
    //sel.addRange(range);
}

function createRange(node, chars, range) {
    if (!range) {
        range = document.createRange()
        range.selectNode(node);
        range.setStart(node, 0);
    }

    if (chars.count === 0) {
        range.setEnd(node, chars.count);
    } else if (node && chars.count > 0) {
        if (node.nodeType === Node.TEXT_NODE) {
            if (node.textContent.length < chars.count) {
                chars.count -= node.textContent.length;
            } else {
                range.setEnd(node, chars.count);
                chars.count = 0;
            }
        } else {
            for (var lp = 0; lp < node.childNodes.length; lp++) {
                range = createRange(node.childNodes[lp], chars, range);

                if (chars.count === 0) {
                    break;
                }
            }
        }
    }

    return range;
};