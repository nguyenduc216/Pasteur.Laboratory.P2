SweetSoft_Settings.Properties.RegisterCheckBoxScript = true;
function RegisterCheckboxScript() {
    var allCheckbox = $("input[type='checkbox'][data-render!='false']:not(.ace)");
    //  console.log("allCheckbox: ", allCheckbox);
    if (allCheckbox.length > 0) {
        allCheckbox.each(function () {
            var checkbox = $(this);
            if (!checkbox.hasClass("ace")) checkbox.addClass("ace");
            //    console.log("checkbox: ", checkbox);
            var label = $("label[for='" + checkbox.attr("id") + "']");
            if (label != null && label.length > 0) label.addClass("lbl");
            else label = $("<label class='lbl' for='" + checkbox.attr("id") + "'></label>");
            //    console.log("label: ", label);
            var parent = checkbox.parent();
            //    console.log("parent: ", parent, typeof (prev), parent[0].tagName);
            if (typeof (parent) === 'undefined' || parent[0].tagName !== "LABEL") {
                var boundLabel = $("<label class='ext-control cb pos-rel'></label>");
                checkbox.wrap(boundLabel);
                label.insertAfter(checkbox);
            }
        });
    }

    var clickGroup = $("input[type='checkbox'][data-click-group='true'][data-group-name!='']");
    //  console.log("clickGroup: ", clickGroup);
    if (clickGroup.length > 0) {
        clickGroup.each(function () {
            var checkbox = $(this);
            checkbox.unbind("change");
            checkbox.bind("change", function () {
                var checked = checkbox.is(":checked");
                // console.log("onchange: ", checkbox, checked);
                var groupName = checkbox.attr("data-group-name");
                var itemGroup = $("input[type='checkbox'][id!='" + checkbox.attr("id") + "'][disabled!='disabled']." + groupName);
                //   console.log("groupName: ", groupName, itemGroup);
                if (itemGroup.length > 0) {
                    if (checked) {
                        itemGroup.attr("checked", true);
                        itemGroup.prop("checked", true);
                    }
                    else itemGroup.removeAttr("checked");
                }
            });
        });
    }
}