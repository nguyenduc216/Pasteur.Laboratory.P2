Room = function (info, hotel) {
    if (typeof (info) !== 'undefined') {
        this.Hotel = hotel;
        this.RoomName = info.RoomName;
        this.RoomID = info.RoomID;
        this.IsAvailable = info.IsAvailable;
        this.HotelName = info.HotelName;
        this.AreaName = info.AreaName;
        this.AreaID = info.AreaID;
        this.FloorName = info.FloorName;
        this.FloorID = info.FloorID;
        this.RoomTypeName = info.RoomTypeName;
        this.RoomTypeID = info.RoomTypeID;
        this.GuestCount = info.GuestCount;
        this.ChildrenCount = info.ChildrenCount;
        this.PersonNumber = info.PersonNumber;
        this.CheckInBillID = info.CheckInBillID;
        this.Price = info.Price;
        this.RoomPrice = info.RoomPrice;
        this.ArrivalTime = info.ArrivalTime;
        this.ArrivalTimeString = info.ArrivalTimeString;
        this.RoomViewUrl = info.RoomViewUrl;
        this.DomElement = null;

        if (this.ChildrenCount == null) this.ChildrenCount = 0;
        this.OnInit();

    }
}

Room.prototype.OnInit = function () {
    var room = "";

    var classExt = "rid-" + this.RoomID;
    classExt = classExt + " vailable-" + this.IsAvailable;
    classExt = classExt + " fid-" + this.FloorID;
    classExt = classExt + " aid-" + this.AreaID;
    classExt = classExt + " rtid-" + this.RoomTypeID;
    if (!this.IsAvailable) {
        room = "<div class=\"col-lg-2 room " + classExt + "\">" +
                "<div class=\"room-box row no-margin active\">" +
                    "<i style=\"float: left; margin-right: 0px; color: blue;display:none;\" class=\"fa fa-spinner fa-pulse fa-spin-custom\"></i><h3 class=\"room-title\">" + this.RoomName + "</h3>" +
                    "<div class=\"detail\">" +
                        "<div class=\"col-lg-12 no-padding arrival-time\"><i class=\"fa fa-calendar\"></i>" + this.ArrivalTimeString + "</div>" +
                        "<div class=\"col-lg-12 no-padding \"><i class=\"fa fa-money\"></i>" + this.RoomPrice + "</div>" +
                        "<div class=\"col-lg-12 no-padding\">" +
                            "<div style=\"cursor:pointer;\">" +
                                "<span class=\"label label-info view-bill\" role='1' data-checkinbill='" + this.CheckInBillID + "'>" +
                                    "<i class=\"fa fa-male\"></i>: <span>" + this.GuestCount + "</span></span>" +
                                "<span class=\"label label-success view-bill\"  role='2' data-checkinbill='" + this.CheckInBillID + "'>" +
                                    "<i class=\"fa fa-child\"></i>: <span>" + this.ChildrenCount + "</span></span>" +
                            "</div>" +
                             "<div class=\"cmd\" >" +
                               "<a class=\"btn btn-warning view-bill\">" +
                               "<i class=\"fa fa-eye\"></i>DS khách</a>" +
                             "</div>" +
                            "<div class=\"active-cmd\">" +
                                "<div class=\"col-lg-6 no-padding\"><a class=\"btn btn-primary btn-bill\" target='_blank' href='" + this.RoomViewUrl + "'>" +
                                    "<i class=\"fa fa-eye\"></i>Xem</a></div>" +
                                    "<div class=\"col-lg-6 no-padding\"><a class=\"btn btn-success btn-bill\" target='_blank' href='" + this.RoomCheckoutUrl + "'>" +
                                    "<i class=\"fa fa-share\"></i>Trả</a></div></div></div></div></div></div>";
    } else
        room = "<div class=\"col-lg-2 room " + classExt + "\">" +
                "<div class=\"room-box row no-margin\">" +
                    "<i style=\"float: left; margin-right: 0px; color: blue;display:none;\" class=\"fa fa-spinner fa-pulse fa-spin-custom\"></i><h3 class=\"room-title\">" + this.RoomName + "</h3>" +
                    "<div class=\"detail\">" +
                        "<div class=\"col-lg-12 no-padding\"><i class=\"fa fa-calendar\"></i>Phòng trống</div>" +
                        "<div class=\"col-lg-12 no-padding \"><i class=\"fa fa-money\"></i>" + this.RoomPrice + "</div>" +
                        "<div class=\"col-lg-12 no-padding\">" +
                            "<div class=\"\">" +
                                "<span class=\"label label-info\">" +
                                    "<i class=\"fa fa-male\"></i>: <span>" + this.GuestCount + "</span></span>" +
                                "<span class=\"label label-success\">" +
                                    "<i class=\"fa fa-child\"></i>:<span> " + this.ChildrenCount + "</span></span>" +
                            "</div>" +
                            "<div class=\"cmd\" >" +
                               "<a class=\"btn btn-bill\" target='_blank' href='" + this.RoomViewUrl + "'>" +
                               "<i class=\"fa fa-plus\"></i>Phiếu ĐP</a>" +
                             "</div>" +
                            "<div class=\"active-cmd\" style='display:none;'>" +
                                "<div class=\"col-lg-6 no-padding\"><a class=\"btn btn-primary btn-bill\" target='_blank' href='" + this.RoomViewUrl + "'>" +
                                    "<i class=\"fa fa-eye\"></i>Xem</a></div>" +
                                    "<div class=\"col-lg-6 no-padding\"><a class=\"btn btn-success btn-bill\" target='_blank' href='" + this.RoomCheckoutUrl + "'>" +
                                    "<i class=\"fa fa-share\"></i>Trả</a></div></div></div></div></div></div>";
    this.DomElement = $(room);

    var view_bill = $(".view-bill", this.DomElement);
    if (view_bill.length > 0) {
        view_bill.click(function () {
            var bill = $(this);
            var role = bill.attr("role");
            var checkinid = bill.attr("data-checkinbill");
            console.log("checkinid: ", checkinid);
            var data = { "CheckInBillID": checkinid };
            var result = AjaxPost(data, "/QuanLy/ViewRoom.aspx/GetCheckInBill");

            if (result != null && result.MSG === "success") {
                if (result.GData != null) {
                    var table1 = $("#guest_table");
                    var body1 = $("tbody", table1);
                    body1.html("");

                    $(result.GData).each(function (idx, item) {

                        var record = item;

                        var row = "<tr>";
                        row += "<td>" + (idx + 1) + "</td>";
                        row += "<td><a class='guest-info-detail' data-gid='" + record.GuestID + "'>" + record.GuestName + "</a></td>";
                        row += "<td>" + record.IdentificationNumber + "</td>";
                        row += "<td>" + record.BirthdayToString + "</td>";
                        row += "<td>" + record.Sex + "</td>";
                        row += "<td>" + record.GuestTypeName + "</td>";
                        row += "<td>" + record.IsInRoom + "</td>";
                        row += "<td>" + record.ArrivalTimeToString + "</td>";
                        row += "<td>" + record.DepartTimeToString + "</td>";
                        row += "<td>" + record.RealDepartTimeToString + "</td>";
                        row += "</tr>";


                        $(row).appendTo(body1);
                    });


                    var table2 = $("#children_table");
                    var body2 = $("tbody", table2);
                    body2.html("");

                    $(result.CData).each(function (idx, item) {

                        var record = item;

                        var row = "<tr>";
                        row += "<td>" + (idx + 1) + "</td>";
                        row += "<td>" + record.ChildName + "</td>";
                        row += "<td>" + record.BirthdayToString + "</td>";
                        row += "<td>" + record.Sex + "</td>";
                        row += "<td>" + record.GuestTypeName + "</td>";
                        row += "<td>" + record.GuestName + "</td>";
                        row += "</tr>";


                        $(row).appendTo(body2);
                    });

                }

                $("#liGuest").removeClass("active");
                $("#guest_info").removeClass("active");
                $("#liChildren").removeClass("active");
                $("#children_info").removeClass("active");

                if (role === '1') {
                    $("#liGuest").addClass("active");
                    $("#guest_info").addClass("in active");
                } else {
                    $("#liChildren").addClass("active");
                    $("#children_info").addClass("in active");
                }

                if (typeof (BindGuestDetail) === "function") BindGuestDetail();
                $("[id$='btnRoomDetail']").click();
            }

            //bill.removeAttr("data-checkinbill");
            //bill.removeAttr("role");
        });
    }
}

Room.prototype.AddToPlaceHolder = function (placeHolderElement) {
    if (this.DomElement !== null) {
        this.DomElement.appendTo($(placeHolderElement));
    }
}

Room.prototype.OpenLoading = function () {
    var room = this.DomElement;
    if (room.length > 0) {
        var room_box = $(".room-box", room);
        if (room_box.length > 0) {
            var snipper = $("i.fa-spinner", room_box);
            if (snipper.length > 0) snipper.css("display", "block");
        }
    }
}

Room.prototype.Rebuild = function (newInfo) {
    this.Hotel = newInfo;
    this.RoomName = newInfo.RoomName;
    this.RoomID = newInfo.RoomID;
    this.IsAvailable = newInfo.IsAvailable;
    this.HotelName = newInfo.HotelName;
    this.AreaName = newInfo.AreaName;
    this.AreaID = newInfo.AreaID;
    this.FloorName = newInfo.FloorName;
    this.FloorID = newInfo.FloorID;
    this.RoomTypeName = newInfo.RoomTypeName;
    this.RoomTypeID = newInfo.RoomTypeID;
    this.GuestCount = newInfo.GuestCount;
    this.ChildrenCount = newInfo.ChildrenCount;
    this.PersonNumber = newInfo.PersonNumber;
    this.CheckInBillID = newInfo.CheckInBillID;
    this.ArrivalTime = newInfo.ArrivalTime;
    this.ArrivalTimeString = newInfo.ArrivalTimeString;
    this.RoomViewUrl = newInfo.RoomViewUrl;

    var classExt = "rid-" + this.RoomID;
    classExt = classExt + " vailable-" + this.IsAvailable;
    classExt = classExt + " fid-" + this.FloorID;
    classExt = classExt + " aid-" + this.AreaID;
    classExt = classExt + " rtid-" + this.RoomTypeID;

    var room = this.DomElement;

    if (this.RoomName === "102") console.log("room: ", room);
    if (room.length > 0) {
        room.attr("class", "col-lg-2 room " + classExt);
        var room_box = $(".room-box", room);
        if (this.RoomName === "102") console.log("room_box: ", room_box);
        if (room_box.length > 0) {
            room_box.removeClass("active");
            if (!newInfo.IsAvailable) room_box.addClass("active");
            var snipper = $("i.fa-spinner", room_box); if (snipper.length > 0) snipper.css("display", "block");
            var detail = $(".detail", room_box);
            if (this.RoomName === "102") console.log("detail: ", detail);
            if (detail.length > 0) {
                var arrival_time = $(".arrival-time", detail);
                if (this.RoomName === "102") console.log("arrival_time: ", arrival_time);
                if (arrival_time.length > 0) {
                    var i_calendar = $("i.fa", arrival_time);
                    if (this.RoomName === "102") console.log("i_calendar: ", i_calendar);
                    if (i_calendar.length > 0) {
                        i_calendar.removeClass("fa-calendar");
                        if (!newInfo.IsAvailable && this.ArrivalTimeString.length > 0) {
                            i_calendar.addClass("fa-calendar");
                            arrival_time.contents(":not(i)").text(": " + this.ArrivalTimeString);
                        }
                    }

                    var label_info = $(".label-info", detail);
                    if (this.RoomName === "102") console.log("label_info: ", label_info, this.GuestCount, this.ChildrenCount);
                    if (label_info.length > 0) $("span", label_info).html(this.GuestCount);
                    var label_success = $(".label-success", detail); if (label_success.length > 0) $("span", label_success).html(this.ChildrenCount);;
                    var cmd = $(".cmd", detail);
                    var activecmd = $(".active-cmd", detail);
                    if (!newInfo.IsAvailable) {
                        activecmd.css("display", "block");
                        cmd.css("display", "none");
                    } else {
                        activecmd.css("display", "none");
                        cmd.css("display", "block");
                    }
                }
            }
            if (snipper.length > 0) snipper.css("display", "none");
        }
    }
}
