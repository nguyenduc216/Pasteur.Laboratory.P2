
if (typeof BootStrap_NET === 'undefined')
    BootStrap_NET = {};
BootStrap_NET.ExtraDateRangePicker = {
    data: {
        color: ['has-info', 'has-danger', 'has-primary', 'has-default',
            'has-warning', 'has-error', 'has-success',
            'select2-info', 'select2-danger', 'select2-primary', 'select2-default',
            'select2-warning', 'select2-error', 'select2-success'],
        laterInit: [],
        cacheObjectFunc: undefined,
        cacheData: []
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

            BootStrap_NET.ExtraDateRangePicker.data.cacheObjectFunc = localCache;
        },
        findCache: function (id) {
            if (typeof id === 'undefined' || id === null || id.length === 0 ||
                BootStrap_NET.ExtraDateRangePicker.data.cacheData.length === 0)
                return undefined;
            var found = undefined;
            $.each(BootStrap_NET.ExtraDateRangePicker.data.cacheData, function (i, o) {
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
            $.each(BootStrap_NET.ExtraDateRangePicker.data.cacheData, function (i, o) {
                if (o._id === id) {
                    indx = i;
                    return false;
                }
            });

            var obj = new BootStrap_NET.ExtraDateRangePicker.data.cacheObjectFunc(id, numPage, cacheOnLoad);
            if (indx !== -1)
                BootStrap_NET.ExtraDateRangePicker.data.cacheData[indx] = obj;
            else
                BootStrap_NET.ExtraDateRangePicker.data.cacheData.push(obj);
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
        deleteDivWrapper: function () {
            var divColl = $('div[id^="dp_"].date-picker-wrapper:hidden');
            if (divColl.length > 0) {
                var id = '';
                var numberPattern = /\d+/g;
                var ar = [];
                for (var i = 0, j = divColl.length; i < j; i++) {
                    id = $(divColl[i]).attr('id');
                    if (id) {
                        ar = id.match(numberPattern);
                        if (ar != null && $.isArray(ar) === true && ar.length === 1 && ar[0].length >= 13)
                            $(divColl[i]).remove();
                    }
                }
            }
        },
        getFormat: function (opt) {
            var fm = opt.format;
            var locale = moment.localeData('en');
            var temp = moment.localeData(opt.language);
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

            BootStrap_NET.ExtraDateRangePicker.mainFunction.deleteDivWrapper();
            if (typeof BootStrap_NET.ExtraDateRangePicker.data.cacheObjectFunc === 'undefined')
                BootStrap_NET.ExtraDateRangePicker.cacheObject.initCacheObjectFunc();

            var $inp = $("input.daterange-picker,span.daterange-picker");
            if ($inp.length > 0) {
                BootStrap_NET.ExtraDateRangePicker.data.laterInit = [];

                var data = undefined;
                $inp.each(function (i, el) {
                    data = $(el).attr('data-initAfterLoad');
                    if (typeof data !== 'undefined' && data.length > 0
                        && $.trim(data).toLowerCase() === 'true') {
                        $(el).removeAttr('data-initAfterLoad');
                        BootStrap_NET.ExtraDateRangePicker.data.laterInit.push(el);
                    }
                    else
                        BootStrap_NET.ExtraDateRangePicker.mainFunction.initForElement(el);
                });

                if (typeof args !== 'undefined') {
                    BootStrap_NET.ExtraDateRangePicker.mainFunction.initWindowLoad();
                }
            }
        },
        initWindowLoad: function () {
            if ($.isArray(BootStrap_NET.ExtraDateRangePicker.data.laterInit) === true) {
                $.each(BootStrap_NET.ExtraDateRangePicker.data.laterInit, function (i, o) {
                    BootStrap_NET.ExtraDateRangePicker.mainFunction.initForElement(o);
                });
                BootStrap_NET.ExtraDateRangePicker.data.laterInit = [];
            }
        },
        initForElement: function (o) {
            if (typeof o === 'undefined' || o === null)
                return;

            var opts = {};

            var hasOpt = false;
            //remove api init
            var dataAPI = $(o).data('dateRangePicker');
            if (typeof dataAPI !== 'undefined') {
                var oldOpt = dataAPI.originalOpt;
                if (typeof oldOpt !== 'undefined')
                    opts = oldOpt;

                dataAPI.destroy();
            }

            if ($.isEmptyObject(opts)) {

                /*#region parse setting */

                /*#region prop*/

                var data = undefined;

                /*#region set value*/

                data = $(o).attr('data-singleDate');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['singleDate'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-singleDate');
                }

                data = $(o).attr('data-language');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['language'] = $.trim(data);
                    $(o).removeAttr('data-language');
                }
                if (typeof opts['language'] === 'undefined'
                      || opts['language'] === null || opts['language'].length === 0)
                    opts['language'] = 'auto';

                data = $(o).attr('data-separator');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['separator'] = data;
                    $(o).removeAttr('data-separator');
                }
                if (typeof opts['separator'] === 'undefined'
                      || opts['separator'] === null || opts['separator'].length === 0)
                    opts['separator'] = ' to ';

                data = $(o).attr('data-format');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['format'] = $.trim(data);
                    $(o).removeAttr('data-format');
                }

                data = $(o).attr('data-hdf');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['hdf'] = $.trim(data);


                    if (typeof opts['hdf'] !== 'undefined' && opts['hdf'].length > 0) {
                        var val = $('#' + opts['hdf']).val();
                        var dataVal = val.split('|');
                        if (dataVal.length > 0) {
                            var ar = dataVal[0].split(',');
                            var text = dataVal[1];
                            if (dataVal.length > 2) {
                                for (var i = 2; i < dataVal.length; i++)
                                    text += '|' + dataVal[i];
                            }

                            //console.log('text : ', text);
                            if (text.length > 0) {
                                $(o).val(text);
                            }
                            else {
                                var date1 = undefined;
                                var date2 = undefined;
                                //console.log('arr :', ar, ar[0]);
                                if (ar.length > 0) {
                                    if ($.trim(ar[0]).length > 0) {
                                        /*
                                        var d1 = ar[0].substring(1);
                                        d1 = d1.substring(0, d1.length - 1);
                                        */
                                        date1 = moment(JSON.parse(ar[0]));
                                    }

                                    if (ar.length > 1) {
                                        if (ar[1] !== null && typeof ar[1] !== 'undefined' && $.trim(ar[1]).length > 0) {
                                            /*
                                            var d2 = ar[1].substring(1);
                                            d2 = d2.substring(0, d2.length - 1);
                                            date2 = moment(d2);
                                            */
                                            date2 = moment(JSON.parse(ar[1]));
                                        }
                                    }
                                }
                                //console.log(date1, date2);
                                var fm = BootStrap_NET.ExtraDateRangePicker.mainFunction.getFormat(opts);
                                //console.log('fm : ', fm, date1.format(fm));
                                if (typeof opts['singleDate'] !== 'undefined' && opts['singleDate'] === true
                                    && typeof date1 !== 'undefined') {
                                    if (o.tagName.toLowerCase() === 'input')
                                        $(o).val(date1.format(fm));
                                    else
                                        $(o).html(date1.format(fm));
                                }
                                else {
                                    if (typeof date1 !== 'undefined' && typeof date2 !== 'undefined') {
                                        if (o.tagName.toLowerCase() === 'input')
                                            $(o).val(date1.format(fm) + opts['separator']
                                                + date2.format(fm));
                                        else
                                            $(o).html(date1.format(fm) + opts['separator']
                                           + date2.format(fm));
                                    }
                                }
                            }
                        }
                    }
                }

                /*#endregion*/

                data = $(o).attr('data-extraClass');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['extraClass'] = $.trim(data);
                    $(o).removeAttr('data-extraClass');
                }

                data = $(o).attr('data-dayDivAttrs');
                if (typeof data !== 'undefined' && data.length > 0
                   && typeof window[$.trim(data)] !== 'undefined'
                   && $.isArray(window[$.trim(data)]) === true) {
                    opts['dayDivAttrs'] = window[$.trim(data)];
                    $(o).removeAttr('data-dayDivAttrs');
                }

                data = $(o).attr('data-dayTdAttrs');
                if (typeof data !== 'undefined' && data.length > 0
                   && typeof window[$.trim(data)] !== 'undefined'
                   && $.isArray(window[$.trim(data)]) === true) {
                    opts['dayTdAttrs'] = window[$.trim(data)];
                    $(o).removeAttr('data-dayTdAttrs');
                }

                data = $(o).attr('data-customShortcuts');
                if (typeof data !== 'undefined' && data.length > 0
                   && typeof window[$.trim(data)] !== 'undefined'
                   && $.isArray(window[$.trim(data)]) === true) {
                    opts['customShortcuts'] = window[$.trim(data)];
                    $(o).removeAttr('data-customShortcuts');
                }

                data = $(o).attr('data-hideGap');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['hideGap'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-hideGap');
                }

                data = $(o).attr('data-showClearButton');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showClearButton'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-showClearButton');
                }

                data = $(o).attr('data-alwaysOpen');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['alwaysOpen'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-alwaysOpen');
                }

                data = $(o).attr('data-batchMode');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['batchMode'] = $.trim(data);
                    $(o).removeAttr('data-batchMode');
                }

                data = $(o).attr('data-useMomentShortDay');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['useMomentShortDay'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-useMomentShortDay');
                }

                data = $(o).attr('data-inline');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['inline'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-inline');
                }

                data = $(o).attr('data-selectBackward');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['selectBackward'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-selectBackward');
                }

                data = $(o).attr('data-selectForward');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['selectForward'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-selectForward');
                }

                data = $(o).attr('data-overrideDataTooltip');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['overrideDataTooltip'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-overrideDataTooltip');
                }

                data = $(o).attr('data-showWeekNumbers');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showWeekNumbers'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-showWeekNumbers');
                }

                data = $(o).attr('data-showDropdownMonth');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showDropdownMonth'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-showDropdownMonth');
                }

                data = $(o).attr('data-showMonthShort');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showMonthShort'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-showMonthShort');
                }

                data = $(o).attr('data-showDropdownYear');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showDropdownYear'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-showDropdownYear');
                }

                data = $(o).attr('data-showTopbar');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showTopbar'] = $.trim(data).toLowerCase() === 'false' ? false : true;
                    $(o).removeAttr('data-showTopbar');
                }

                data = $(o).attr('data-showShortcuts');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showShortcuts'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-showShortcuts');
                }

                data = $(o).attr('data-swapTime');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['swapTime'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-swapTime');
                }

                data = $(o).attr('data-startDate');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['startDate'] = $.trim(data);
                    $(o).removeAttr('data-startDate');
                }

                data = $(o).attr('data-endDate');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['endDate'] = $.trim(data);
                    $(o).removeAttr('data-endDate');
                }

                data = $(o).attr('data-showOtherMonth');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showOtherMonth'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-showOtherMonth');
                }

                data = $(o).attr('data-stickyMonths');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['stickyMonths'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-stickyMonths');
                }

                data = $(o).attr('data-singleMonth');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['singleMonth'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-singleMonth');
                }
                if (typeof opts['singleMonth'] === 'undefined'
                       || opts['singleMonth'] === null || opts['singleMonth'].length === 0)
                    opts['singleMonth'] = 'auto';


                data = $(o).attr('data-autoClose');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['autoClose'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-autoClose');
                }

                data = $(o).attr('data-lookBehind');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['lookBehind'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-lookBehind');
                }

                data = $(o).attr('data-defaultDate');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['defaultDate'] = $.trim(data);
                    $(o).removeAttr('data-defaultDate');
                }

                data = $(o).attr('data-calculateWraperWith');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['calculateWraperWith'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-calculateWraperWith');
                }

                data = $(o).attr('data-getWeekNumber');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['getWeekNumber'] = window[$.trim(data)];
                    $(o).removeAttr('data-getWeekNumber');
                }

                data = $(o).attr('data-showDateFilterFunc');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['showDateFilterFunc'] = window[$.trim(data)];
                    $(o).removeAttr('data-showDateFilterFunc');
                }

                data = $(o).attr('data-beforeShowDayFunc');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['beforeShowDayFunc'] = window[$.trim(data)];
                    $(o).removeAttr('data-beforeShowDayFunc');
                }

                data = $(o).attr('data-showDifferenceFunc');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['showDifferenceFunc'] = window[$.trim(data)];
                    $(o).removeAttr('data-showDifferenceFunc');
                }

                data = $(o).attr('data-firstDateSelectedFunc');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['firstDateSelectedFunc'] = window[$.trim(data)];
                    $(o).removeAttr('data-firstDateSelectedFunc');
                }

                data = $(o).attr('data-lastDateSelectedFunc');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['lastDateSelectedFunc'] = window[$.trim(data)];
                    $(o).removeAttr('data-lastDateSelectedFunc');
                }

                data = $(o).attr('data-onOpenedFunc');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['onOpenedFunc'] = window[$.trim(data)];
                    $(o).removeAttr('data-onOpenedFunc');
                }

                data = $(o).attr('data-onClosedFunc');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['onClosedFunc'] = window[$.trim(data)];
                    $(o).removeAttr('data-onClosedFunc');
                }

                data = $(o).attr('data-setValue');
                var mainSetValue = undefined;
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    mainSetValue = window[$.trim(data)];
                    $(o).removeAttr('data-setValue');
                }/*
            else {
                opts['setValue'] = function (s) {
                    if (s !== $(this).val()) {
                        $(this).val(s);
                    }
                }
            }*/

                opts['setValue'] = function (valstr, s1, s2, d1, d2) {
                    //console.log('SetValue', valstr, s1, s2, d1, d2);
                    var curval = $(this).val();
                    if (valstr !== curval) {
                        $(this).val(valstr);
                    }
                    var obj = {
                        value: valstr,
                        date1: d1,
                        date2: d2
                    };

                    //set hidden field
                    BootStrap_NET.ExtraDateRangePicker.mainFunction.setHiddenValue(o, obj);
                    if ($.type(mainSetValue) === 'function')
                        mainSetValue(this, valstr, d1, d2);
                }

                data = $(o).attr('data-getValue');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['getValue'] = window[$.trim(data)];
                    $(o).removeAttr('data-getValue');
                }

                data = $(o).attr('data-hoveringTooltip');
                if (typeof data !== 'undefined' && data.length > 0) {
                    if (typeof window[$.trim(data)] === 'function')
                        opts['hoveringTooltip'] = window[$.trim(data)];
                    else
                        opts['hoveringTooltip'] = false;
                    $(o).removeAttr('data-hoveringTooltip');
                }

                data = $(o).attr('data-onChangeMonthYear');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['onChangeMonthYear'] = window[$.trim(data)];
                    $(o).removeAttr('data-onChangeMonthYear');
                }
                else {
                    if ($.isEmptyObject(opts['ajaxObj']) === false
                        && typeof opts['ajaxObj'].url !== 'undefined'
                        && opts['ajaxObj'].url.length > 0) {
                        opts['onChangeMonthYear'] = function (el, monthObj, isMonth2, callback) {
                            //console.log('onChangeMonthYear : ', this, el, monthObj, isMonth2);
                            /*#region check and modify success function*/

                            if ($.isFunction(opts['ajaxObj']['success']) === true) {
                                var old = opts['ajaxObj']['success'];
                                opts['ajaxObj']['success'] = function (data, textStatus, jqXHR) {
                                    old.call(this, data, textStatus, jqXHR);
                                    console.log('modify call', data);
                                    if ($.isFunction(callback) === true)
                                        callback(data);
                                }
                            }
                            else {
                                //console.log('define success call');

                                opts['ajaxObj']['success'] = function (res) {
                                    if (typeof res !== 'undefined' && $.isArray(res) === true)
                                        dataMonth = res;

                                    if ($.isFunction(callback) === true)
                                        callback(res);
                                }
                            }

                            /*#endregion*/

                            /*#region check and modify data function*/

                            /*#endregion*/

                            /*
                             data = $(o).attr('data-ajaxDataRequest');
                             if (typeof data !== 'undefined' && data.length > 0) {
                                 if (typeof window[$.trim(data)] === 'function')
                                     ajaxObj['data'] = window[$.trim(data)]();
                                 else
                                     ajaxObj['data'] = window[$.trim(data)];
                                 $(o).removeAttr('data-ajaxDataRequest');
                             }
                             else {
                
                                 if (ajaxObj['contentType'].toLowerCase().indexOf('application/json') >= 0)
                                     ajaxObj['data'] = JSON.stringify({ ms: monthObj.m1[0], ys: monthObj.m1[1] });
                                 else
                                     ajaxObj['data'] = { ms: monthObj.m1[0], ys: monthObj.m1[1] };
                             }
                             */

                            $.ajax(opts['ajaxObj']);
                        }
                    }
                }

                data = $(o).attr('data-maxDays');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['maxDays'] = parseInt($.trim(data)) || 0;
                    $(o).removeAttr('data-maxDays');
                }

                data = $(o).attr('data-duration');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['duration'] = isNaN(parseInt($.trim(data))) ? 200 : parseInt($.trim(data));
                    $(o).removeAttr('data-duration');
                }

                data = $(o).attr('data-minDays');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['minDays'] = parseInt($.trim(data)) || 0;
                    $(o).removeAttr('data-minDays');
                }

                data = $(o).attr('data-shortcuts');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'object') {
                    opts['shortcuts'] = window[$.trim(data)];
                    $(o).removeAttr('data-shortcuts');
                }

                data = $(o).attr('data-enableTime');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['time'] = {
                        enabled: $.trim(data).toLowerCase() === 'true' ? true : false
                    };
                    $(o).removeAttr('data-enableTime');
                }

                /*#region custom values*/

                data = $(o).attr('data-showCustomValues');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['showCustomValues'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-showCustomValues');
                }

                data = $(o).attr('data-customValueLabel');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['customValueLabel'] = $.trim(data);
                    $(o).removeAttr('customValueLabel');
                }

                data = $(o).attr('data-customValues');
                if (typeof data !== 'undefined' && data.length > 0
                   && typeof window[$.trim(data)] !== 'undefined'
                   && $.isArray(window[$.trim(data)]) === true) {
                    opts['customValues'] = window[$.trim(data)];
                    $(o).removeAttr('data-customValues');
                }

                /*#endregion*/

                data = $(o).attr('data-startOfWeek');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['startOfWeek'] = $.trim(data);
                    $(o).removeAttr('data-startOfWeek');
                }

                data = $(o).attr('data-loadingTemplate');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                    opts['loadingTemplate'] = window[$.trim(data)]();
                    $(o).removeAttr('data-loadingTemplate');
                }

                data = $(o).attr('data-collision');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['collision'] = $.trim(data);
                    $(o).removeAttr('data-collision');
                }

                data = $(o).attr('data-direction');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['direction'] = $.trim(data);
                    $(o).removeAttr('data-direction');
                }

                data = $(o).attr('data-openAt');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['openAt'] = $.trim(data);
                    $(o).removeAttr('data-openAt');
                }

                data = $(o).attr('data-customTopBar');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['customTopBar'] = data;
                    $(o).removeAttr('data-customTopBar');
                }

                data = $(o).attr('data-container');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['container'] = $.trim(data);
                    $(o).removeAttr('data-container');
                }
                if (typeof opts['container'] === 'undefined'
                       || opts['container'] === null || opts['container'].length === 0)
                    opts['container'] = 'body';

                data = $(o).attr('data-btnPrev');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['btnPrev'] = $.trim(data);
                    $(o).removeAttr('data-btnPrev');
                }
                if (typeof opts['btnPrev'] === 'undefined'
                      || opts['btnPrev'] === null || opts['btnPrev'].length === 0)
                    opts['btnPrev'] = '&#171;';

                data = $(o).attr('data-btnNext');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['btnNext'] = $.trim(data);
                    $(o).removeAttr('data-btnNext');
                }
                if (typeof opts['btnNext'] === 'undefined'
                   || opts['btnNext'] === null || opts['btnNext'].length === 0)
                    opts['btnNext'] = '&#187;';

                data = $(o).attr('data-applyBtnClass');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['applyBtnClass'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                    $(o).removeAttr('data-applyBtnClass');
                }

                data = $(o).attr('data-onchange');
                if (typeof data !== 'undefined' && data.length > 0) {
                    opts['fireOnChange'] = $.trim(data);
                    $(o).removeAttr('data-onchange');
                }

                /*#endregion*/

                /*#region ajax*/

                var ajaxObj = undefined;
                data = $(o).attr('data-ajaxObjectSetting');
                if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'object') {
                    ajaxObj = window[$.trim(data)];
                    $(o).removeAttr('data-ajaxObjectSetting');
                }
                else {
                    data = $(o).attr('data-ajaxDataPath');
                    if (typeof data !== 'undefined' && data.length > 0) {
                        ajaxObj = {
                            url: $.trim(data)
                        };
                        $(o).removeAttr('data-ajaxDataPath');
                    }
                }

                var cache = undefined;
                if (typeof ajaxObj !== 'undefined') {

                    /*#region cache data*/

                    data = $(o).attr('data-ajaxCacheOnLoad');
                    var cacheSetting = {};
                    if (typeof data !== 'undefined' && data.length > 0) {
                        cacheSetting['cacheOnLoad'] = $.trim(data).toLowerCase() === 'true' ? true : false;
                        $(o).removeAttr('data-ajaxCacheOnLoad');
                    }
                    data = $(o).attr('data-ajaxCacheNumerKey');
                    if (typeof data !== 'undefined' && data.length > 0) {
                        cacheSetting['cacheNumerKey'] = parseInt($.trim(data)) || 0;
                        $(o).removeAttr('data-ajaxCacheNumerKey');
                    }

                    if ($.isEmptyObject(cacheSetting) === false) {
                        cacheSetting['id'] = $(o).attr('id');
                        var cacheLoad = cacheSetting.cacheOnLoad || false;
                        cache = BootStrap_NET.ExtraDateRangePicker.cacheObject.addCacheForElement(cacheSetting.id,
                               cacheSetting.cacheNumerKey || (cacheLoad === true ? 1 : -1), cacheLoad);
                    }

                    /*#endregion*/

                    data = $(o).attr('data-ajaxDataType');
                    if (typeof data !== 'undefined' && data.length > 0) {
                        ajaxObj['dataType'] = $.trim(data);
                        $(o).removeAttr('data-ajaDataType');
                    }

                    data = $(o).attr('data-ajaxMethodType');
                    if (typeof data !== 'undefined' && data.length > 0) {
                        ajaxObj['type'] = $.trim(data);
                        $(o).removeAttr('data-ajaxMethodType');
                    }

                    data = $(o).attr('data-ajaxContentType');
                    if (typeof data !== 'undefined' && data.length > 0) {
                        ajaxObj['contentType'] = $.trim(data);
                        $(o).removeAttr('data-ajaxContentType');
                    } else
                        ajaxObj['contentType'] = 'application/x-www-form-urlencoded; charset=UTF-8';



                    /*
                data = $(o).attr('data-ajaxBeforeSendFunction');
                if (typeof data !== 'undefined' && data.length > 0
                && typeof window[$.trim(data)] === 'function') {
                    ajaxObj['beforeSend'] = window[$.trim(data)];
                    $(o).removeAttr('data-ajaxBeforeSendFunction');
                }
                else {
                    ajaxObj['beforeSend'] = function (xhr, opts, cache) {
                        if (typeof cache !== 'undefined') {
                
                            //region get paging and keyword
                
                            var page = 1;
                            var ke = '';
                
                            if (typeof opts.data.page !== 'undefined')
                                page = typeof (opts.data.page) === 'number' ? opts.data.page : (parseInt(opts.data.page) || 1);
                            else {
                                try {
                                    var ob = JSON.parse(opts.data);
                                    page = typeof (ob.page) === 'number' ? ob.page : (parseInt(ob.page) || 1);
                                }
                                catch (ex) {
                                    //query string
                                    var p = BootStrap_NET.ExtraDropdown.commonFunction.getDataQuery('page', (opts.data.indexOf('?') === 0 ? '' : '?') + opts.data);
                                    if (p !== null && p.length > 0)
                                        page = parseInt(p) || 1;
                                }
                            }
                
                            if (typeof opts.data.keyword !== 'undefined')
                                ke = opts.data.keyword;
                            else if (typeof opts.data.q !== 'undefined')
                                ke = opts.data.q;
                            else {
                                try {
                                    var ob = JSON.parse(opts.data);
                                    ke = ob.keyword || (ob.q || '');
                                }
                                catch (ex) {
                                    //query string
                                    var p = BootStrap_NET.ExtraDropdown.commonFunction.getDataQuery('keyword', opts.data);
                                    if (p !== null && p.length > 0)
                                        ke = p;
                                    else {
                                        p = BootStrap_NET.ExtraDropdown.commonFunction.getDataQuery('q', opts.data);
                                        if (p !== null && p.length > 0)
                                            ke = p;
                                        else
                                            ke = '';
                                    }
                                }
                            }
                
                            //endregion
                
                            var fullKey = opts.url + '_' + ke + '_' + page;
                
                            if (cache.exist(fullKey)) {
                                var sel = $(o).data('select2');
                                if (typeof sel !== 'undefined') {
                                    //sel.results.loading = false;
                                    //sel.results.hideLoading();
                
                                    var cacheData = cache.get(fullKey);
                                    var newobj = opts.processResults(cacheData[0], cacheData[1]);
                                    if (page === 1) {
                                        sel.trigger('results:all', { data: newobj, query: cacheData[1] });
                                    }
                                    else
                                        sel.trigger('results:append', { data: newobj, query: cacheData[1] });
                                }
                                xhr.abort();
                
                                return false;
                            }
                        }
                        return true;
                    };
                }
                    */

                    data = $(o).attr('data-ajaxErrorFunction');
                    if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                        ajaxObj['error'] = window[$.trim(data)];
                        $(o).removeAttr('data-ajaxErrorFunction');
                    }

                    data = $(o).attr('data-ajaxSuccessFunction');
                    if (typeof data !== 'undefined' && data.length > 0
                    && typeof window[$.trim(data)] === 'function') {
                        ajaxObj['success'] = window[$.trim(data)];
                        $(o).removeAttr('data-ajaxSuccessFunction');
                    }

                    opts.ajaxObj = ajaxObj;
                }

                /*#endregion*/

                /*#endregion*/

                $(o).attr('isdeletekey', '1');
            }

            $(o).dateRangePicker(opts);

            var iconcalendar = $(o).parent().find('.input-group-addon');
            if (iconcalendar.length > 0) {
                iconcalendar.click(function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var inp = $(this).parent().find('.daterange-picker');
                    if (inp.length > 0)
                        inp.trigger('click.datepicker');
                });
            }

            if (typeof opts['fireOnChange'] !== 'undefined' && opts['fireOnChange'].length > 0) {

                $(o).bind('datepicker-open', function (e, obj) {
                    $(o).data('oldval', $(o).val());
                });

                if (typeof opts['onOpenedFunc'] !== 'undefined'
                    && opts['onOpenedFunc'].length > 0) {
                    $(o).bind('datepicker-opened', function (e, obj) {
                        opts['onOpenedFunc']();
                    });
                }

                $(o).bind('datepicker-change', function (event, obj) {
                    /* This event will be triggered when second date is selected */
                    //console.log(obj, moment(obj.date1).format(), moment(obj.date2).format());
                    // obj will be something like this:
                    // {
                    // 		date1: (Date object of the earlier date),
                    // 		date2: (Date object of the later date),
                    //	 	value: "2013-06-05 to 2013-06-07"
                    // }

                    //set hidden field
                    BootStrap_NET.ExtraDateRangePicker.mainFunction.setHiddenValue(o, obj);

                    if (typeof opts['lastDateSelectedFunc'] !== 'undefined'
                        && opts['lastDateSelectedFunc'].length > 0)
                        opts['lastDateSelectedFunc']();

                    if (typeof opts['inline'] !== 'undefined' && opts['inline'] === true) {
                        //setTimeout('__doPostBack(\'' + $(this).attr('name') + '\',\'\')', 0);
                        var oldval = $(o).data('oldval');
                        if (oldval === $(o).val()) {
                            return;
                        }
                        eval(opts['fireOnChange']);
                    }
                });

                $(o).bind('datepicker-closed', function (e, obj) {

                    if (typeof opts['onClosedFunc'] !== 'undefined'
                        && opts['onClosedFunc'].length > 0)
                        opts['onClosedFunc']();

                    if (typeof opts['inline'] !== 'undefined' && opts['inline'] === true) { }
                    else {
                        //setTimeout('__doPostBack(\'' + $(this).attr('name') + '\',\'\')', 0);
                        var oldval = $(o).data('oldval');
                        if (oldval === $(o).val()) {
                            return;
                        }
                        eval(opts['fireOnChange']);
                    }
                });

            }
            else {
                if (typeof opts['onOpenedFunc'] !== 'undefined'
                    && opts['onOpenedFunc'].length > 0) {
                    $(o).bind('datepicker-opened', function (e, obj) {
                        opts['onOpenedFunc']();
                    });
                }

                $(o).bind('datepicker-first-date-selected', function (event, obj) {
                    if (typeof opts['firstDateSelectedFunc'] !== 'undefined'
                            && opts['firstDateSelectedFunc'].length > 0) {
                        opts['firstDateSelectedFunc']();
                    }
                });

                $(o).bind('datepicker-change', function (event, obj) {

                    //set hidden field
                    BootStrap_NET.ExtraDateRangePicker.mainFunction.setHiddenValue(o, obj);

                    if (typeof opts['lastDateSelectedFunc'] !== 'undefined'
                           && opts['lastDateSelectedFunc'].length > 0) {
                        opts['firstDateSelectedFunc']();
                    }
                });

                $(o).bind('datepicker-closed', function (e, obj) {
                    if (typeof opts['onClosedFunc'] !== 'undefined'
                        && opts['onClosedFunc'].length > 0) {
                        opts['onClosedFunc']();
                    }
                });
            }

            data = $(o).attr('required');
            if (typeof data !== 'undefined') {
                $(o).on('datepicker-close', function (e) {
                    BootStrap_NET.ExtraDateRangePicker.mainFunction.CheckValid(o);
                });
            }

            if (typeof cache !== 'undefined' && cache._cacheOnLoad === true) {
                setTimeout(function () {
                    console.log('call cache');
                }, 150);
            }
        },
        setHiddenValue: function (o, obj) {
            var hdfid = $(o).attr('data-hdf');

            if (typeof obj !== 'undefined' && $.isEmptyObject(obj) === false) {
                var isSingleDate = true;
                var hasData = false;

                /*#region check dates*/

                if (Object.prototype.toString.call(obj.date1) === "[object Date]") {
                    // it is a date
                    if (isNaN(obj.date1.getTime())) {  // d.valueOf() could also work
                        // date is not valid
                    }
                    else {
                        // date is valid
                        hasData = true;
                    }
                }
                else {
                    // not a date
                }

                if (Object.prototype.toString.call(obj.date2) === "[object Date]") {
                    // it is a date
                    if (isNaN(obj.date1.getTime())) {  // d.valueOf() could also work
                        // date is not valid
                    }
                    else {
                        // date is valid
                        isSingleDate = false;
                    }
                }
                else {
                    // not a date
                }

                /*#endregion*/

                if (hasData === true) {
                    if (isSingleDate === true) {
                        var str = moment(obj.date1).format() + ',' + '|' +
                                   (o.tagName.toLowerCase() === 'input' ? $(o).val() : $(o).html());

                        $('#' + hdfid).val(str);
                    }
                    else {
                        var str2 = moment(obj.date1).format() + ',' +
                                 moment(obj.date2).format() + '|' +
                                 (o.tagName.toLowerCase() === 'input' ? $(o).val() : $(o).html());
                        $('#' + hdfid).val(str2);
                    }
                }

                if (o.tagName.toLowerCase() === 'input' &&
                        $(o).val().length === 0) {
                    if (typeof hdfid !== 'undefined' && hdfid.length > 0)
                        $('#' + hdfid).val(',|');
                }
                else if (o.tagName.toLowerCase() === 'span' &&
                        $(o).text().length === 0) {
                    if (typeof hdfid !== 'undefined' && hdfid.length > 0)
                        $('#' + hdfid).val(',|');
                }
            }
            else {
                if (typeof hdfid !== 'undefined' && hdfid.length > 0)
                    $('#' + hdfid).val(',|');
            }
        },
        CheckValid: function (el) {
            if (typeof $.fn.validate === 'function') {
                return $(el).valid();
            }
            else {
                var val = (el.tagName.toLowerCase() === 'input' ? $(el).val() : $(el).html());
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
        },
        ShowLoading: function (ddl, callback) {
            if (typeof ddl !== 'undefined') {
                var sel = $(ddl).data('dateRangePicker');
                if (typeof sel !== 'undefined')
                    sel.showLoading(callback);
                else
                    if (typeof callback === 'function')
                        callback();
            }
        },
        HideLoading: function (ddl) {
            if (typeof ddl !== 'undefined') {
                var sel = $(ddl).data('dateRangePicker');
                if (typeof sel !== 'undefined')
                    sel.hideLoading(callback);
                else
                    if (typeof callback === 'function')
                        callback();
            }
        }
    }
};

$(function () {
    BootStrap_NET.ExtraDateRangePicker.mainFunction.init();
    if (typeof Sys !== 'undefined' && typeof Sys.WebForms !== 'undefined')
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(BootStrap_NET.ExtraDateRangePicker.mainFunction.init);
});

$(window).load(function () {
    setTimeout(function () {
        BootStrap_NET.ExtraDateRangePicker.mainFunction.initWindowLoad();
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
