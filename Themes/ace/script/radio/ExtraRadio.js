SweetSoft_Settings.Properties.RegisterRadioScript = true;
function RegisterRadioScript() {
    var allRadio = $("input[type='radio'][data-render!='false']:not(.ace)");
    //  console.log("allRadio: ", allRadio);
    if (allRadio.length > 0) {
        allRadio.each(function () {
            var radiobox = $(this);
            var fake_name = radiobox.attr("fake-name"); 
            if (typeof (fake_name) === 'undefined') 
                var fake_name = radiobox.parent().attr("fake-name");
            
            if (typeof (fake_name) != 'undefined' && fake_name.length > 0) {

                radiobox.attr("name", fake_name);
            }
            if (!radiobox.hasClass("ace")) radiobox.addClass("ace");
            //    console.log("radiobox: ", radiobox);
            var label = $("label[for='" + radiobox.attr("id") + "']");
            if (label != null && label.length > 0) label.addClass("lbl");
            else label = $("<label class='lbl' for='" + radiobox.attr("id") + "'></label>");
            //    console.log("label: ", label);
            var parent = radiobox.parent();
            //    console.log("parent: ", parent, typeof (prev), parent[0].tagName);
            if (typeof (parent) === 'undefined' || parent[0].tagName !== "LABEL") {
                var boundLabel = $("<label class='ext-control rd pos-rel'></label>");
                radiobox.wrap(boundLabel);
                label.insertAfter(radiobox);
            }
        });
    }
}