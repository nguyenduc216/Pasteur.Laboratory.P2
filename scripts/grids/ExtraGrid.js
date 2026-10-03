SweetSoft_Settings.Properties.RegisterGridScript = true;

function RegisterGridScript() {
    try {
        var tables = $("table.dataTable.extra-grid");
        
        if (tables.length > 0) {

            tables.each(function () {
                var cr_table = $(this);
                // try {
                var trCol = $("tr", cr_table);
               
                cr_table.prepend($("<thead></thead>").append($(this).find("tr:first")));

                $("<tfoot></tfoot>").append($(this).find("tr:last")).insertAfter($("tbody", cr_table));
               
                var fixHeader = cr_table.attr("data-fix-header");
                var table = cr_table.DataTable({
                    responsive: cr_table.hasClass("responsive"),
                    paging: false,
                    "bPaginate": false,
                    "bLengthChange": false,
                    "bFilter": false,
                    "bSortable": true,
                    "bInfo": false,
                    "bAutoWidth": true
                    ,
                    "oLanguage": { "sZeroRecords": "", "sEmptyTable": "" }
                });
              
                //if (typeof (fixHeader) !== 'undefined' && fixHeader == "true") {
                //    var header = new $.fn.dataTable.FixedHeader(table);
                //    var it = {
                //        id: cr_table.attr("id"),
                //        tbl: header
                //    };
                //    SweetSoft_Settings.Grid.Items.push(it);
                //}

                var filterColumnsClientId = cr_table.attr("filterClientId");

                //console.log("filterColumnsClientId: ", filterColumnsClientId);
                if (filterColumnsClientId != null && filterColumnsClientId.length > 0) {
                    var filterControl = $("#" + filterColumnsClientId);
                   
                    if (filterControl.length > 0) {
                        var sl = '#' + filterColumnsClientId + ' option';
                        var slObj = $(sl);
                        slObj.each(function (i, selected) {
                            var vl = $(selected).val();
                            //console.log("vl: ", vl, $(selected).is(":selected"));
                            table.column(vl).visible($(selected).is(":selected"));
                            //console.log("table.column(vl): ", table.column(vl), table);
                            //var sort = table.column(vl).data("data-sorting-sw");
                            //console.log("sort: ", sort);
                            //if (sort.length > 0) table.column(vl).attr("class", sort);
                        });

                        filterControl.on("change", function () {
                            console.log("change: ", slObj);
                            slObj.each(function (i, selected) {
                                var vl = $(selected).val();
                                table.column(vl).visible($(selected).is(":selected"));
                            });
                        });

                        // Thêm 1 icon list vào filter
                        var btSelect = filterControl.next();
                       
                        if (btSelect.length > 0) {
                            var btn = $(".btn", btSelect);
                            if (btn.length > 0) {
                                var icon = $("<i class='ace-icon fa fa-bars'></i>")
                                icon.appendTo(btn);
                            }
                        }
                    }
                    cr_table.removeAttr("filterClientId");
                }

                var sort_col = $("th[data-sorting-sw]");
                if (sort_col.length > 0) {
                    $(sort_col).each(function () {
                        var sort_vl = $(this).attr("data-sorting-sw");
                        if (sort_vl.length > 0) $(this).attr("class", sort_vl);
                    });
                }

                //  } catch (ex2) { console.log("ex2: ", ex2); }
                var loading = $("#loading_" + cr_table.attr("id"));
              
                if (loading.length > 0) loading.remove();
                cr_table.css("display", "table");
            });
        }
    } catch (ex) {

    }
}