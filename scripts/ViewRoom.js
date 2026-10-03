
var Hotel = {
    Info: {
        Id: null,
        Time: function () { return $("input[type='text'][id$='dateCheckIn']").val(); },
        PlaceHolderID: '#roomPlaceHolder',
        InitDataSource: "/Default.aspx/GhtrTD",
        RoomItemClass: null,
        FilterExpression: '',
        TimeDelay: 1000,//1000 * 60 * 2,
        AutoRefresh: false
    },
    Rooms: [],
    Area: [],
    Floor: [],
    Guest: {},
    Roles: {
        View: true,
        Create: true
    },
    Events: {
        OnInit: function (delay) {
            if (typeof (delay) !== 'undefined')
                Hotel.Info.TimeDelay = delay;
            if (Hotel.Info.Id != null) {
                console.log("hid: ", Hotel.Info.Id, Hotel.Info.Time);
                var data = { "hid": Hotel.Info.Id, "time": $("input[type='text'][id$='dateCheckIn']").val() };
                var result = AjaxPost(data, Hotel.Info.InitDataSource);
                if (result.Message.length > 0) {

                } else {
                    $(result.List).each(function (idx, item) {
                        var room = new Room(item, Hotel);
                        Hotel.Events.AddNewRoom(room);
                    });
                }

                //if (Hotel.Info.AutoRefresh && Hotel.Info.TimeDelay > 0)
                //    Hotel.Events.Refresh(Hotel.Info.TimeDelay);
            }
            var txtdateCheckIn = $("input[type='text'][id$='dateCheckIn']");
            if (typeof (dateCheckIn) !== 'undefined' && dateCheckIn !== null && dateCheckIn.length > 0) {
                dateCheckIn.change(function () {
                    $(Hotel.Info.PlaceHolderID).html("");
                    Hotel.Events.OnInit(0);
                });
            }
        },
        AddNewRoom: function (room) {
            Hotel.Rooms.push(room);
            room.AddToPlaceHolder(Hotel.Info.PlaceHolderID);
        },
        GetRoom: function (roomID) {
            if (Hotel.Rooms.length > 0) {
                var room = $(Hotel.Rooms).filter(function (idx, room) { return room.RoomID === roomID; });
                if (room.length > 0) return room[0];
            }
            return null;
        },
        FilterRoom: function (filterClassValue, delay) {
            var $grid = $(Hotel.Info.PlaceHolderID).isotope({
                itemSelector: ".room", "layoutMode": "fitRows"
            });

            if (filterClassValue !== 'refresh') {
                $grid.isotope({ filter: filterClassValue });
                Hotel.Info.FilterExpression = filterClassValue;
            } else {
                if (typeof (delay) !== 'undefined')
                    setTimeout(function () { $grid.isotope({ filter: Hotel.Info.FilterExpression }); }, delay);
                else $grid.isotope({ filter: Hotel.Info.FilterExpression });
            }
        },
        Refresh: function (delay) {
            if (typeof (delay) !== 'undefined') Hotel.Info.TimeDelay = delay;

            var fN = function () {
                if (Hotel.Info.Id != null) {
                    $(Hotel.Rooms).each(function (idx, item) { item.OpenLoading(); });
                    var data = { "hid": Hotel.Info.Id, "time": Hotel.Info.Time };
                    var result = AjaxPost(data, "/Default.aspx/GhtrTD");
                    if (typeof (result) !== 'undefined') {
                        if (result.Message.length > 0) {

                        } else {
                            $(result.List).each(function (idx, item) {
                                var room = Hotel.Events.GetRoom(item.RoomID);

                                if (room === null || typeof (room) === 'undefined') {
                                    room = new Room(item, Hotel);
                                    Hotel.Events.AddNewRoom(room);
                                } else {
                                    room.Rebuild(item);
                                }
                            });
                        }
                        var txtdateCheckIn = $("input[type='text'][id$='dateCheckIn']");
                        txtdateCheckIn.val("");
                    }
                }
            };

            if (Hotel.Info.AutoRefresh) {
                setTimeout(function () {
                    fN();
                    Hotel.Events.Refresh(Hotel.Info.TimeDelay);
                }, Hotel.Info.TimeDelay);
            } else {
                fN();
            }
        }
    }
}





