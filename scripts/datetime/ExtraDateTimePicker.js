
if (typeof BootStrap_NET === 'undefined')
    BootStrap_NET = {};
BootStrap_NET.ExtraDateTimePicker = {
    data: {
        laterInit: [],
        cacheObjectFunc: undefined,
        cacheData: [],
        tempCall: undefined
    },
    cacheObject: {
        initCacheObjectFunc: function () {

            function localCache(id, maxCacheNum, cacheOnLoad) {

                this._id = id || '';
                this._data = {};
                this._count = 0;

                this._maxCache = -1;
                if (typeof maxCacheNum === 'number')
                    this._maxCache = maxCacheNum;

                this._cacheOnLoad = false;
                if (typeof cacheOnLoad === 'boolean')
                    this._cacheOnLoad = cacheOnLoad;
            };

            localCache.prototype.remove = function (url) {
                if (this.exist(url) === true) {
                    delete this._data[url];
                    this._count--;
                }
            };
            localCache.prototype.exist = function (url) {
                return this._data.hasOwnProperty(url) && this._data[url] !== null;
            };
            localCache.prototype.get = function (url) {
                console.log('Getting in cache for url' + url);
                return this._data[url];
            };
            localCache.prototype.set = function (url, cachedData, callback) {
                if (this.exist(url))
                    this._data[url] = cachedData;
                else {
                    if ((this._maxCache === -1) || (this._count < this._maxCache)) {
                        this._count++;
                        this._data[url] = cachedData;
                    }
                    else if (this._maxCache > 0)
                        console.log('Can not save data because of maximum cache reach.');
                }
                if ($.isFunction(callback))
                    callback(cachedData);
            };
            localCache.prototype.callAjax = function () {
                if ((this._maxCache === -1) || (this._count < this._maxCache)) {
                    this._count++;
                    this._data[url] = cachedData;
                }
            }

            localCache.prototype.countItem = function () { return this._count; }

            BootStrap_NET.ExtraDateTimePicker.data.cacheObjectFunc = localCache;
        },
        findCache: function (id) {
            if (typeof id === 'undefined' || id === null || id.length === 0 ||
                BootStrap_NET.ExtraDateTimePicker.data.cacheData.length === 0)
                return undefined;
            var found = undefined;
            $.each(BootStrap_NET.ExtraDateTimePicker.data.cacheData, function (i, o) {
                if (o._id === id) {
                    found = o;
                    return false;
                }
            });
            return found;
        },
        addCacheForElement: function (id, numPage, cacheOnLoad) {
            if (typeof id === 'undefined' || id === null || id.length === 0) {
                console.log('Can not create cache for element not has id.');
                return null;
            }

            var indx = -1;
            $.each(BootStrap_NET.ExtraDateTimePicker.data.cacheData, function (i, o) {
                if (o._id === id) {
                    indx = i;
                    return false;
                }
            });

            var obj = new BootStrap_NET.ExtraDateTimePicker.data.cacheObjectFunc(id, numPage, cacheOnLoad);
            if (indx !== -1)
                BootStrap_NET.ExtraDateTimePicker.data.cacheData[indx] = obj;
            else
                BootStrap_NET.ExtraDateTimePicker.data.cacheData.push(obj);
            return obj;
        }
    },
    commonFunction: {
        isIE: function () {
            var myNav = navigator.userAgent.toLowerCase();
            return (myNav.indexOf('msie') != -1) ? parseInt(myNav.split('msie')[1]) : false;
        },
        getIEVersion: function () {
            var rv = -1;
            if (navigator.appName == 'Microsoft Internet Explorer') {
                var ua = navigator.userAgent;
                var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
                if (re.exec(ua) != null)
                    rv = parseFloat(RegExp.$1);
            }
            else if (navigator.appName == 'Netscape') {
                var ua = navigator.userAgent;
                var re = new RegExp("Trident/.*rv:([0-9]{1,}[\.0-9]{0,})");
                if (re.exec(ua) != null)
                    rv = parseFloat(RegExp.$1);
            }
            return rv;
        },
        getPropertyNotCaseInsensitive: function (obj, property) {
            property = (property + "").toLowerCase();
            if (property.length > 0) {
                for (var p in obj) {
                    if (obj.hasOwnProperty(p) && property == (p + "").toLowerCase()) {
                        return obj[p];
                    }
                }
            }
            return null;
        },
        countProperties: function (obj) {
            var count = "__count__", hasOwnProp = Object.prototype.hasOwnProperty;

            if (typeof obj[count] === "number" && !hasOwnProp.call(obj, count)) {
                return obj[count];
            }
            count = 0;
            for (var prop in obj) {
                if (hasOwnProp.call(obj, prop)) {
                    count++;
                }
            }
            return count;
        },
        getDataQuery: function (name, ss) {
            if (typeof ss !== 'undefined' && ss.length > 0) {
                if (ss.indexOf('?') !== 0)
                    ss = '?' + ss;
            }
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)", 'i'),
                results = regex.exec(ss || location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        },
        toASPNetTicks: function (date) {
            var currentTime = date.getTime();

            // 10,000 ticks in 1 millisecond
            // jsTicks is number of ticks from midnight Jan 1, 1970
            var jsTicks = currentTime * 10000;

            // add 621355968000000000 to jsTicks
            // netTicks is number of ticks from midnight Jan 1, 01 CE
            var netTicks = jsTicks + 621355968000000000;

            return netTicks;
        },

        findEventHandlers: function (eventType, jqSelector) {
            var results = [];
            var jQ = jQuery;// to avoid conflict between others frameworks like Mootools

            var arrayIntersection = function (array1, array2) {
                return jQ(array1).filter(function (index, element) {
                    return jQ.inArray(element, jQ(array2)) !== -1;
                });
            };

            var haveCommonElements = function (array1, array2) {
                return arrayIntersection(array1, array2).length !== 0;
            };


            var addEventHandlerInfo = function (element, event, $elementsCovered) {
                var extendedEvent = event;
                if ($elementsCovered !== void 0 && $elementsCovered !== null) {
                    jQ.extend(extendedEvent, { targets: $elementsCovered.toArray() });
                }
                var eventInfo;
                var eventsInfo = jQ.grep(results, function (evInfo, index) {
                    return element === evInfo.element;
                });

                if (eventsInfo.length === 0) {
                    eventInfo = {
                        element: element,
                        events: [extendedEvent]
                    };
                    results.push(eventInfo);
                } else {
                    eventInfo = eventsInfo[0];
                    eventInfo.events.push(extendedEvent);
                }
            };


            var $elementsToWatch = jQ(jqSelector);
            if (jqSelector === "*")//* does not include document and we might be interested in handlers registered there
                $elementsToWatch = $elementsToWatch.add(document);
            var $allElements = jQ("*").add(document);

            jQ.each($allElements, function (elementIndex, element) {
                var allElementEvents = jQ._data(element, "events");
                if (allElementEvents !== void 0 && allElementEvents[eventType] !== void 0) {
                    var eventContainer = allElementEvents[eventType];
                    jQ.each(eventContainer, function (eventIndex, event) {
                        var isDelegateEvent = event.selector !== void 0 && event.selector !== null;
                        var $elementsCovered;
                        if (isDelegateEvent) {
                            $elementsCovered = jQ(event.selector, element); //only look at children of the element, since those are the only ones the handler covers
                        } else {
                            $elementsCovered = jQ(element); //just itself
                        }
                        if (haveCommonElements($elementsCovered, $elementsToWatch)) {
                            addEventHandlerInfo(element, event, $elementsCovered);
                        }
                    });
                }
            });

            return results;
        }
    },
    mainFunction: {
        getFormat: function (opt) {
            var fm = opt.format;
            var locale = moment.localeData('vi');
            var temp = moment.localeData(opt.locale);

            if (typeof temp !== 'undefined' && temp !== null)
                locale = temp;
            if (typeof fm === 'undefined' || fm.length === 0 || fm === null) {
                if (typeof opt.useMomentShortDay !== 'undefined'
                    && opt.useMomentShortDay !== null && opt.useMomentShortDay === true
                    && typeof locale._longDateFormat !== 'undefined')
                    fm = locale._longDateFormat.L;
            }
            if (typeof fm === 'undefined' || fm.length === 0 || fm === null)
                fm = 'DD/MM/YYYY';

            return fm;
        },
        init: function (sender, args) {
            BootStrap_NET.ExtraDateTimePicker.data.tempCall = undefined;
            if (typeof BootStrap_NET.ExtraDateTimePicker.data.cacheObjectFunc === 'undefined')
                BootStrap_NET.ExtraDateTimePicker.cacheObject.initCacheObjectFunc();

            var $inp = $("[data-datetimepicker='true']");
            if ($inp.length > 0) {
                BootStrap_NET.ExtraDateTimePicker.data.laterInit = [];

                var data = undefined;
                $inp.each(function (i, el) {
                    data = $(el).attr('data-initAfterLoad');
                    if (typeof data !== 'undefined' && data.length > 0
                        && $.trim(data).toLowerCase() === 'true') {
                        $(el).removeAttr('data-initAfterLoad');
                        BootStrap_NET.ExtraDateTimePicker.data.laterInit.push(el);
                    }
                    else
                        BootStrap_NET.ExtraDateTimePicker.mainFunction.initForElement(el);
                });

                if (typeof args !== 'undefined') {
                    BootStrap_NET.ExtraDateTimePicker.mainFunction.initWindowLoad();
                }
            }
        },
        initWindowLoad: function () {
            if ($.isArray(BootStrap_NET.ExtraDateTimePicker.data.laterInit) === true) {
                $.each(BootStrap_NET.ExtraDateTimePicker.data.laterInit, function (i, o) {
                    BootStrap_NET.ExtraDateTimePicker.mainFunction.initForElement(o);
                });
                BootStrap_NET.ExtraDateTimePicker.data.laterInit = [];
            }
        },
        initForElement: function (o) {
            if (typeof o === 'undefined' || o === null)
                return;

            var elparse = o.tagName.toLowerCase() === 'div' ? $(o).find('input:text:eq(0)') : $(o);
            if (elparse.length === 0)
                elparse = $(o);
            var opts = {};

            /*
            //remove api init
            var dataAPI = $(o).data('DateTimePicker');
            if (typeof dataAPI !== 'undefined') {
                var oldOpt = dataAPI.originalOpt;
                if (typeof oldOpt !== 'undefined')
                    opts = oldOpt;

                dataAPI.destroy();
            }
            */
            var defDate = undefined;
            var cache = undefined;
            if ($.isEmptyObject(opts)) {

                /*#region parse setting */

                /*#region prop*/

                var data = undefined;

                /*#region set value*/

                data = elparse.attr('data-hdf');
                if (typeof data !== 'undefined' && data.length > 0) {
                    var hdfid = $.trim(data);
                    opts['hdf'] = hdfid;

                    if (typeof hdfid !== 'undefined' && hdfid.length > 0) {
                        var val = $('#' + hdfid).val();

                        if (val.length > 0) {
                            try {
                                var objDate = JSON.parse(val.split('|')[0]);
                                //console.log('objDate : ', objDate);
                                var ndate = moment(objDate.toString());
                                //console.log('ndate : ', ndate.toDate());
                                defDate = ndate;
                            }
                            catch (ex) {
                                var ndate = moment(val.split('|')[0]);
                                //console.log('ndate : ', ndate.toDate());
                                defDate = ndate;
                            }
                        }
                    }
                }

                /*#region Range date*/

                data = elparse.attr('data-linkDateIsStartDate');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['linkDateIsStartDate'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }

                data = elparse.attr('data-linkDateGroup');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['linkDateGroup'] = $.trim(data);
                }

                /*#endregion*/

                /*#region Code block*/

                data = elparse.attr('data-useCurrent');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['useCurrent'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-useCurrent');



                data = elparse.attr('data-collapse');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['collapse'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-collapse');



                data = elparse.attr('data-useStrict');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['useStrict'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-useStrict');



                data = elparse.attr('data-sideBySide');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['sideBySide'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-sideBySide');



                data = elparse.attr('data-calendarWeeks');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['calendarWeeks'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-calendarWeeks');



                data = elparse.attr('data-showTodayButton');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showTodayButton'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-showTodayButton');



                data = elparse.attr('data-showClear');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showClear'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-showClear');



                data = elparse.attr('data-showClose');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showClose'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-showClose');



                data = elparse.attr('data-ignoreReadonly');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['ignoreReadonly'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-ignoreReadonly');



                data = elparse.attr('data-keepOpen');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['keepOpen'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-keepOpen');


                data = elparse.attr('data-focusOnShow');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['focusOnShow'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-focusOnShow');



                data = elparse.attr('data-inline');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['inline'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-inline');



                data = elparse.attr('data-keepInvalid');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['keepInvalid'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-keepInvalid');



                data = elparse.attr('data-debug');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['debug'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-debug');



                data = elparse.attr('data-allowInputToggle');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['allowInputToggle'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                }
                elparse.removeAttr('data-allowInputToggle');



                data = elparse.attr('data-timeZone');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['timeZone'] = $.trim(data);
                }
                elparse.removeAttr('data-timeZone');


                data = elparse.attr('data-format');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['format'] = $.trim(data);
                }
                elparse.removeAttr('data-format');


                data = elparse.attr('data-locale');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['locale'] = $.trim(data);
                }
                elparse.removeAttr('data-locale');

                if (typeof opts['locale'] === 'undefined'
                    || opts['locale'] === null || opts['locale'].length === 0)
                    opts['locale'] = moment.locale();


                data = elparse.attr('data-useMomentShortDay');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['useMomentShortDay'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    var fm = BootStrap_NET.ExtraDateTimePicker.mainFunction.getFormat(opts);
                    opts['format'] = fm;
                }
                elparse.removeAttr('data-useMomentShortDay');


                data = elparse.attr('data-dayViewHeaderFormat');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['dayViewHeaderFormat'] = $.trim(data);
                }
                elparse.removeAttr('data-dayViewHeaderFormat');

                if (typeof opts['dayViewHeaderFormat'] === 'undefined'
                     || opts['dayViewHeaderFormat'] === null || opts['dayViewHeaderFormat'].length === 0)
                    opts['dayViewHeaderFormat'] = 'MMMM YYYY';


                data = elparse.attr('data-extraFormats');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['extraFormats'] = eval($.trim(data));
                }
                elparse.removeAttr('data-extraFormats');


                data = elparse.attr('data-stepping');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['stepping'] = $.trim(data);
                }
                elparse.removeAttr('data-stepping');


                data = elparse.attr('data-minDate');
                if (typeof data !== 'undefined' && data.length > 0) {
                    var strMinDate = $.trim(data);
                    opts['minDate'] = moment(JSON.parse(strMinDate));
                }
                elparse.removeAttr('data-minDate');


                data = elparse.attr('data-maxDate');
                if (typeof data !== 'undefined' && data.length > 0) {
                    var strMaxDate = $.trim(data);
                    opts['maxDate'] = moment(JSON.parse(strMaxDate));
                }
                elparse.removeAttr('data-maxDate');


                data = elparse.attr('data-disabledDates');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['disabledDates'] = eval($.trim(data));
                    //console.log(" opts['disabledDates'] ", opts['disabledDates']);
                }
                elparse.removeAttr('data-disabledDates');


                data = elparse.attr('data-enabledDates');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['enabledDates'] = eval($.trim(data));
                    //console.log(" opts['enabledDates'] ", opts['enabledDates']);
                }
                elparse.removeAttr('data-enabledDates');

                /*#endregion*/

                /*#region icons*/

                var icons = {};
                data = elparse.attr('data-icons_Time');
                if (typeof data !== 'undefined' && data.length > 0) {
                    icons['time'] = $.trim(data);
                }
                elparse.removeAttr('data-icons_Time');


                data = elparse.attr('data-icons_Date');
                if (typeof data !== 'undefined' && data.length > 0) {
                    icons['date'] = $.trim(data);
                }
                elparse.removeAttr('data-icons_Date');


                data = elparse.attr('data-icons_Up');
                if (typeof data !== 'undefined' && data.length > 0) {
                    icons['up'] = $.trim(data);
                }
                elparse.removeAttr('data-icons_Up');


                data = elparse.attr('data-icons_Down');
                if (typeof data !== 'undefined' && data.length > 0) {
                    icons['down'] = $.trim(data);
                }
                elparse.removeAttr('data-icons_Down');


                data = elparse.attr('data-icons_Previous');
                if (typeof data !== 'undefined' && data.length > 0) {
                    icons['previous'] = $.trim(data);
                }
                elparse.removeAttr('data-icons_Previous');


                data = elparse.attr('data-icons_Next');
                if (typeof data !== 'undefined' && data.length > 0) {
                    icons['next'] = $.trim(data);
                }
                elparse.removeAttr('data-icons_Next');


                data = elparse.attr('data-icons_Today');
                if (typeof data !== 'undefined' && data.length > 0) {
                    icons['today'] = $.trim(data);
                }
                elparse.removeAttr('data-icons_Today');


                data = elparse.attr('data-icons_Clear');
                if (typeof data !== 'undefined' && data.length > 0) {
                    icons['clear'] = $.trim(data);
                }
                elparse.removeAttr('data-icons_Clear');


                data = elparse.attr('data-icons_Close');
                if (typeof data !== 'undefined' && data.length > 0) {
                    icons['close'] = $.trim(data);
                }
                elparse.removeAttr('data-icons_Close');

                /*#region Force Awesome icons*/
                icons = {
                    time: "fa fa-clock-o",
                    date: "fa fa-calendar",
                    up: "fa fa-arrow-up",
                    down: "fa fa-arrow-down",

                    previous: 'fa fa-chevron-left',
                    next: 'fa fa-chevron-right',
                    today: 'fa fa-crosshairs',
                    clear: 'fa fa-trash',
                    close: 'fa fa-times-circle'
                }
                /*#endregion*/

                if ($.isEmptyObject(icons) === false)
                    opts['icons'] = icons;

                /*#endregion*/


                /*#region Code block*/

                data = elparse.attr('data-daysOfWeekDisabled');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['daysOfWeekDisabled'] = eval($.trim(data));
                }
                elparse.removeAttr('data-daysOfWeekDisabled');


                data = elparse.attr('data-viewMode');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['viewMode'] = $.trim(data);
                }
                elparse.removeAttr('data-viewMode');


                data = elparse.attr('data-toolbarPlacement');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['toolbarPlacement'] = $.trim(data);
                }
                elparse.removeAttr('data-toolbarPlacement');


                var wg = {};
                data = elparse.attr('data-widgetPositioning_Horizontal');
                if (typeof data !== 'undefined' && data.length > 0) {
                    wg['horizontal'] = $.trim(data);
                }
                elparse.removeAttr('data-widgetPositioning_Horizontal');


                data = elparse.attr('data-widgetPositioning_Vertical');
                if (typeof data !== 'undefined' && data.length > 0) {
                    wg['vertical'] = $.trim(data);
                }
                elparse.removeAttr('data-widgetPositioning_Vertical');

                if ($.isEmptyObject(wg) === false)
                    opts['widgetPositioning'] = wg;

                data = elparse.attr('data-widgetParent');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['widgetParent'] = $.trim(data);
                }
                elparse.removeAttr('data-widgetParent');


                data = elparse.attr('data-datepickerInputClass');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['datepickerInput'] = $.trim(data);
                }
                elparse.removeAttr('data-datepickerInputClass');

                if (typeof opts['datepickerInput'] === 'undefined'
                   || opts['datepickerInput'] === null || opts['datepickerInput'].length === 0)
                    opts['datepickerInput'] = '.datepickerinput';

                data = elparse.attr('data-keyBindsObject');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] !== 'undefined') {
                    opts['keyBinds'] = window[$.trim(data)];
                }
                elparse.removeAttr('data-keyBindsObject');

                data = elparse.attr('data-toolTipObject');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] !== 'undefined') {
                    opts['tooltips'] = window[$.trim(data)];
                }
                elparse.removeAttr('data-toolTipObject');

                data = elparse.attr('data-disabledTimeIntervalsObject');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] !== 'undefined') {
                    opts['disabledTimeIntervals'] = window[$.trim(data)];
                }
                elparse.removeAttr('data-disabledTimeIntervalsObject');

                data = elparse.attr('data-disabledHours');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['disabledHours'] = eval($.trim(data));
                }
                elparse.removeAttr('data-disabledHours');


                data = elparse.attr('data-enabledHours');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['enabledHours'] = eval($.trim(data));
                }
                elparse.removeAttr('data-enabledHours');

                /*#endregion*/

                /*
                data = elparse.attr('data-toolTip');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['toolTip'] = $.trim(data);
                }
                elparse.removeAttr('data-toolTip');
                */

                /*#endregion*/

                /*#endregion*/

                /*#region event binding*/

                data = elparse.attr('data-onchange');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['fireOnChange'] = $.trim(data);
                }
                elparse.removeAttr('data-onchange');


                data = elparse.attr('data-onClientChangedFunc');
                if (typeof data !== 'undefined' && data.length > 0
                && typeof window[$.trim(data)] === 'function') {
                    opts['onClientChangedFunc'] = window[$.trim(data)];
                }
                elparse.removeAttr('data-onClientChangedFunc');


                data = elparse.attr('data-onClientClosedFunc');
                if (typeof data !== 'undefined' && data.length > 0
                && typeof window[$.trim(data)] === 'function') {
                    opts['onClientClosedFunc'] = window[$.trim(data)];
                }
                elparse.removeAttr('data-onClientClosedFunc');


                data = elparse.attr('data-onClientErrorFunc');
                if (typeof data !== 'undefined' && data.length > 0
                && typeof window[$.trim(data)] === 'function') {
                    opts['onClientErrorFunc'] = window[$.trim(data)];
                }
                elparse.removeAttr('data-onClientErrorFunc');


                data = elparse.attr('data-onClientOpenedFunc');
                if (typeof data !== 'undefined' && data.length > 0
                && typeof window[$.trim(data)] === 'function') {
                    opts['onClientOpenedFunc'] = window[$.trim(data)];
                }
                elparse.removeAttr('data-onClientOpenedFunc');


                data = elparse.attr('data-onClientUpdateFunc');
                if (typeof data !== 'undefined' && data.length > 0
                && typeof window[$.trim(data)] === 'function') {
                    opts['onClientUpdateFunc'] = window[$.trim(data)];
                }
                elparse.removeAttr('data-onClientUpdateFunc');


                /*#endregion*/

                /*#endregion*/

            }

            $(o).datetimepicker(opts);


            /*#region bind event*/

            $(o).bind('dp.change', function (e) {

                //set hidden field
                BootStrap_NET.ExtraDateTimePicker.mainFunction.setHiddenValue(o, e);

                BootStrap_NET.ExtraDateTimePicker.mainFunction.setMinDate(o, e.date);
                BootStrap_NET.ExtraDateTimePicker.mainFunction.setMaxDate(o, e.date);


                if (typeof opts['onClientChangedFunc'] !== 'undefined'
                    && opts['onClientChangedFunc'].length > 0)
                    opts['onClientChangedFunc'](e);

                if (typeof opts['fireOnChange'] !== 'undefined'
                    && opts['fireOnChange'].length > 0)
                    eval(opts['fireOnChange']);

            });

            if (typeof opts['onClientClosedFunc'] !== 'undefined'
                && opts['onClientClosedFunc'].length > 0) {
                $(o).bind('dp.hide', function (e) {
                    /*
                    var elp = this.tagName.toLowerCase() === 'div' ? $(this).find('input:text:eq(0)') : $(this);
                    if (elp.length === 0)
                        elp = $(this);
                    var dataR = elp.attr('required');
                    if (typeof dataR !== 'undefined' && dataR.toLowerCase() !== 'false')
                        BootStrap_NET.ExtraDateTimePicker.mainFunction.CheckValid(this);
                    */
                    opts['onClientClosedFunc'](e);
                });
            }
            else {

                data = elparse.attr('required');
                if (typeof data !== 'undefined' && data.toLowerCase() !== 'false') {
                    $(o).bind('dp.hide', function (e) {
                        BootStrap_NET.ExtraDateTimePicker.mainFunction.CheckValid(this);
                    });
                }
            }

            if (typeof opts['onClientErrorFunc'] !== 'undefined'
                && opts['onClientErrorFunc'].length > 0) {
                $(o).bind('dp.error', function (e) {
                    opts['onClientErrorFunc'](e);
                });
            }

            if (typeof opts['onClientOpenedFunc'] !== 'undefined'
                && opts['onClientOpenedFunc'].length > 0) {
                $(o).bind('dp.show', function (e) {
                    opts['onClientOpenedFunc'](e);
                });
            }

            if (typeof opts['onClientUpdateFunc'] !== 'undefined'
                && opts['onClientUpdateFunc'].length > 0) {
                $(o).bind('dp.update', function (e) {
                    opts['onClientUpdateFunc'](e);
                });
            }

            if (typeof opts['allowInputToggle'] !== 'undefined' && opts['allowInputToggle'] === true)
            {
                if ($(o).is('input')) {
                    var addonColl = $(o).closest('.input-group').find('.input-group-addon');
                    if (addonColl.length > 0) {
                        addonColl.click(function () {
                            $(o).focus();
                        });
                    }
                }
            }
            //if (opts['defaultDate'] !== false)
            //    BootStrap_NET.ExtraDateTimePicker.mainFunction.setHiddenValue(o, { date: $(o).data('DateTimePicker').defaultDate() });

            /*#endregion*/

            if (typeof defDate !== 'undefined') {
                var dataAPI = $(o).data('DateTimePicker');
                if (typeof dataAPI !== 'undefined') {
                    dataAPI.date(defDate);
                    BootStrap_NET.ExtraDateTimePicker.mainFunction.setHiddenValue(o, { date: defDate });
                }
            }
            else {
                //BootStrap_NET.ExtraDateTimePicker.mainFunction.setMinDate(o, defDate);
                //BootStrap_NET.ExtraDateTimePicker.mainFunction.setMaxDate(o, defDate);
            }

            if (typeof cache !== 'undefined' && cache._cacheOnLoad === true) {
                setTimeout(function () {
                    console.log('call cache');
                }, 150);
            }
        },
        setMinDate: function (el, date) {
            if (typeof el !== 'undefined') {
                var elparse = $(el)[0].tagName.toLowerCase() === 'div' ? $(el).find('input:text:eq(0)') : $(el);
                if (elparse.length === 0)
                    elparse = $(el);
               
                var lnkgr = elparse.attr('data-linkDateGroup');
                if (lnkgr && lnkgr.length > 0) {
                    var str = elparse.attr('data-linkDateIsStartDate');
                    var isStart = $.trim(str).toLowerCase() === 'true' ? true : false;
                    if (isStart === true) {
                        var elColl = $("[data-hdf][data-linkDateGroup='" + lnkgr + "']");
                        if (elColl.length > 1) {
                            var elend = elColl.not(elparse).eq(0);
                            if (elend.length > 0) {
                                str = elend.attr('data-linkDateIsStartDate');
                                isStart = $.trim(str).toLowerCase() === 'true' ? true : false;
                                if (isStart === false) {
                                    //console.log('set min date : ', elend);

                                    var api = $(elend).data("DateTimePicker");
                                    if (typeof api === 'undefined')
                                        api = elend.closest('[data-datetimepicker="true"]').data("DateTimePicker");

                                    if (typeof api !== 'undefined') {
                                        //console.log('set min date with data ', date);
                                        api.minDate(date);
                                    }
                                    else {
                                        //console.log('not init api');
                                        clearTimeout(BootStrap_NET.ExtraDateTimePicker.data.tempCall);
                                        BootStrap_NET.ExtraDateTimePicker.data.tempCall = setTimeout(function () {
                                            BootStrap_NET.ExtraDateTimePicker.mainFunction.setMinDate(el, date);
                                        }, 200);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        },
        setMaxDate: function (el, date) {
            if (typeof el !== 'undefined') {
                var elparse = $(el)[0].tagName.toLowerCase() === 'div' ? $(el).find('input:text:eq(0)') : $(el);
                if (elparse.length === 0)
                    elparse = $(el);
                var lnkgr = elparse.attr('data-linkDateGroup');
                if (lnkgr && lnkgr.length > 0) {
                    var str = elparse.attr('data-linkDateIsStartDate');
                    var isStart = $.trim(str).toLowerCase() === 'true' ? true : false;
                    if (isStart === false) {
                        var elColl = $("[data-hdf][data-linkDateGroup='" + lnkgr + "']");
                        if (elColl.length > 1) {
                            var elstart = elColl.not(elparse).eq(0);
                            if (elstart.length > 0) {
                                str = elstart.attr('data-linkDateIsStartDate');
                                isStart = $.trim(str).toLowerCase() === 'true' ? true : false;
                                if (isStart === true) {
                                    //console.log('set max date : ', elstart);

                                    var api = $(elstart).data("DateTimePicker");
                                    if (typeof api === 'undefined')
                                        api = elstart.closest('[data-datetimepicker="true"]').data("DateTimePicker");

                                    if (typeof api !== 'undefined') {
                                        //console.log('set max date with data ',date);
                                        api.maxDate(date);
                                    }
                                    else {
                                        //console.log('not init api');
                                        clearTimeout(BootStrap_NET.ExtraDateTimePicker.data.tempCall);
                                        BootStrap_NET.ExtraDateTimePicker.data.tempCall = setTimeout(function () {
                                            BootStrap_NET.ExtraDateTimePicker.mainFunction.setMaxDate(el, date);
                                        }, 200);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        },
        setHiddenValue: function (o, ev) {
            var elparse = o.tagName.toLowerCase() === 'div' ? $(o).find('input:text:eq(0)') : $(o);
            if (elparse.length === 0)
                elparse = $(o);
            var hdfid = elparse.attr('data-hdf');

            if (typeof ev !== 'undefined' && $.isEmptyObject(ev) === false) {

                var hasData = false;
                var da = undefined;

                /*#region check date*/
                if (moment.isMoment(ev.date)) {
                    da = ev.date.toDate();
                    hasData = true;
                }
                else {
                    if (Object.prototype.toString.call(ev.date) === "[object Date]") {
                        // it is a date
                        if (isNaN(ev.date.getTime())) {  // d.valueOf() could also work
                            // date is not valid
                        }
                        else {
                            da = ev.date;
                            // date is valid
                            hasData = true;
                        }
                    }
                    else {
                        // not a date
                    }
                }

                /*#endregion*/

                if (hasData === true) {
                    var str = moment(da).format();

                    $('#' + hdfid).val(str + '|' + elparse.val());
                }
                else
                    $('#' + hdfid).val('|');
                /*
                if (elparse.val().length === 0) {
                    if (typeof hdfid !== 'undefined' && hdfid.length > 0)
                        $('#' + hdfid).val('|');
                }
                */
            }
            else {
                if (typeof hdfid !== 'undefined' && hdfid.length > 0)
                    $('#' + hdfid).val('|');
            }
        },
        CheckValid: function (el) {
            if (typeof $.fn.validate === 'function') {
                var elparse = $(el)[0].tagName.toLowerCase() === 'div' ? $(el).find('input:text:eq(0)') : $(el);
                if (elparse.length === 0)
                    elparse = $(el);
                return elparse.valid();
            }
            else {
                var val = $(el).val();
                if (val === null || val.length === 0) {
                    if ($(el).parent().hasClass('input-group') === false)
                        $(el).parent().addClass('has-error');
                    else
                        $(el).parent().parent().addClass('has-error');
                    return false;
                }
                else {
                    if ($(el).parent().hasClass('input-group') === false)
                        $(el).parent().removeClass('has-error');
                    else
                        $(el).parent().parent().removeClass('has-error');
                }
                return true;
            }
        }
    }
};

$(function () {
    moment.updateLocale('vi', {
        months: [
            "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7",
            "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
        ],
        weekdays: [
            'Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'
        ]
    });
    BootStrap_NET.ExtraDateTimePicker.mainFunction.init();
    if (typeof Sys !== 'undefined' && typeof Sys.WebForms !== 'undefined')
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(BootStrap_NET.ExtraDateTimePicker.mainFunction.init);
});

$(window).on("load", function () {
    setTimeout(function () {
        BootStrap_NET.ExtraDateTimePicker.mainFunction.initWindowLoad();
    }, 100);
});

// Format string
/*
  Month
  -----------------------------
  M: 1 2 ... 11 12
  Mo: 1st 2nd ... 11th 12th
  MM: 01 02 ... 11 12
  MMM Jan Feb ... Nov Dec
  MMMM: January February ... November December

  Day of Month
  -----------------------------
  D: 1 2 ... 30 31
  Do: 1st 2nd ... 30th 31st
  DD: 01 02 ... 30 31
  Day of Year DDD 1 2 ... 364 365
  DDDo: 1st 2nd ... 364th 365th
  DDDD: 001 002 ... 364 365

  Day of Week
  -----------------------------
  d: 0 1 ... 5 6
  do: 0th 1st ... 5th 6th
  dd: Su Mo ... Fr Sa
  ddd: Sun Mon ... Fri Sat
  dddd: Sunday Monday ... Friday Saturday
  Day of Week (Locale): e 0 1 ... 5 6
  Day of Week (ISO): E 1 2 ... 6 7

  Week of Year
  -----------------------------
  w: 1 2 ... 52 53
  wo: 1st 2nd ... 52nd 53rd
  ww: 01 02 ... 52 53

  Week of Year (ISO)
  -----------------------------
  W: 1 2 ... 52 53
  Wo: 1st 2nd ... 52nd 53rd
  WW: 01 02 ... 52 53

  Year
  -----------------------------
  YY: 70 71 ... 29 30
  YYYY: 1970 1971 ... 2029 2030

  Week Year 
  -----------------------------
  gg: 70 71 ... 29 30
  gggg: 1970 1971 ... 2029 2030

  Week Year (ISO) 
  -----------------------------
  GG: 70 71 ... 29 30
  GGGG: 1970 1971 ... 2029 2030

  AM/PM 
  -----------------------------
  A: AM PM
  a: am pm

  Hour: 
  -----------------------------
  H: 0 1 ... 22 23
  HH: 00 01 ... 22 23
  h: 1 2 ... 11 12
  hh: 01 02 ... 11 12

  Minute
  -----------------------------
  m: 0 1 ... 58 59
  mm: 00 01 ... 58 59

  Second
  -----------------------------
  s: 0 1 ... 58 59
  ss: 00 01 ... 58 59

  Fractional Second 
  -----------------------------
  S: 0 1 ... 8 9
  SS: 0 1 ... 98 99
  SSS 0 1 ... 998 999

  Timezone
  -----------------------------
  z or zz: EST CST ... MST PST 
  Z: -07:00 -06:00 ... +06:00 +07:00
  ZZ: -0700 -0600 ... +0600 +0700

  Unix Timestamp
  -----------------------------
  X: 1360013296
*/
