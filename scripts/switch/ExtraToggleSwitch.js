SweetSoft_Settings.Properties.RegisterCheckBoxScript = true;
function RegisterCheckboxScript() {
    var inputgroup = $("input[type='checkbox'][data-change-group='true']");

    if (inputgroup.length > 0) {
        inputgroup.each(function () {
            var item = $(this);
            item.unbind("click");
            item.bind("click", function () {
                var groupname = item.attr("data-group-name");
                if (groupname.length > 0) {
                    var checked = item.is(":checked");
                    var subItem = $("input[type='checkbox'][data-group-name='" + groupname + "'][data-change-group='false'],input[type='checkbox'][data-group-name='" + groupname + "']:not([data-change-group])");
                    if (subItem.length > 0) subItem.attr("checked", checked);
                }
            });
        });
    }
}
