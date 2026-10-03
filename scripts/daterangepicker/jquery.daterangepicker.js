/// <reference path="jquery-1.11.1-vsdoc.js" />

// daterangepicker.js
// version : 0.0.9
// author : Chunlong Liu
// last updated at: 2015-10-30
// license : MIT
// www.jszen.com

(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(['jquery', 'moment'], factory);
    } else if (typeof exports === 'object' && typeof module !== 'undefined') {
        // CommonJS. Register as a module
        module.exports = factory(require('jquery'), require('moment'));
    } else {
        // Browser globals
        factory(jQuery, moment);
    }
}(function ($, moment) {
    $.fn.dateRangePicker = function (opt) {
        if ($(this).length === 0)
            return;

        if (!opt) opt = {};

        var oriOpt = jQuery.extend(true, {}, opt);

        opt = $.extend(true,
		{
		    autoClose: false,
		    //DEV:
		    btnNext: '&gt;',
		    btnPrev: '&lt;',
		    calculateWraperWith: false,
		    showOtherMonth: false,
		    openAt: 'bottom',//top
		    direction: 'left',//left, right,center
		    useMomentShortDay: false,
		    overrideDataTooltip: false,
		    hideGap: false,
		    collision: 'fit flip',
		    onChangeMonthYear: false,
		    loadingTemplate: '<div class="dp-loading"><span class="fa fa-2x fa-spin fa-spinner" style="margin-top: 15%;"></span></div>',
		    defaultDate: false,
		    ajaxObj: false,
		    showDropdownMonth: false,
		    showMonthShort: false,
		    showDropdownYear: false,
		    showDifferenceFunc: undefined,
		    beforeShowDayFunc: undefined,
		    showClearButton: false,
		    clearBtnClass: '',

		    format: '',
		    separator: ' to ',
		    language: 'auto',
		    startOfWeek: 'sunday',// or monday
		    getValue: function () {
		        return $(this).val();
		    },
		    setValue: function (s, s1, s2, date1, date2) {
		        /*original
		        if (!$(this).attr('readonly') && !$(this).is(':disabled')
                    && s != $(this).val()) {
		            $(this).val(s);
		        }
                */
		        if (s !== $(this).val()) {
		            $(this).val(s);
		        }
		    },
		    startDate: false,
		    endDate: false,
		    time: {
		        enabled: false
		    },
		    minDays: 0,
		    maxDays: 0,
		    showShortcuts: false,
		    shortcuts:
			{
			    //'prev-days': [1,3,5,7],
			    // 'next-days': [3,5,7],
			    //'prev' : ['week','month','year'],
			    // 'next' : ['week','month','year']
			},
		    customShortcuts: [],
		    inline: false,
		    container: 'body',
		    alwaysOpen: false,
		    singleDate: false,
		    lookBehind: false,
		    batchMode: false,
		    duration: 200,
		    stickyMonths: false,
		    dayDivAttrs: [],
		    dayTdAttrs: [],
		    selectForward: false,
		    selectBackward: false,
		    applyBtnClass: '',
		    singleMonth: 'auto',
		    hoveringTooltip: function (days, startTime, hoveringTime) {
		        return days > 1 ? days + ' ' + lang('days') : '';
		    },
		    showTopbar: true,
		    swapTime: false,
		    showWeekNumbers: false,
		    getWeekNumber: function (date) //date will be the first day of a week
		    {
		        return moment(date).format('w');
		    }
		}, opt);

        opt.start = false;
        opt.end = false;

        opt.dataAjax = [];
        opt.startWeek = false;

        //detect a touch device
        opt.isTouchDevice = 'ontouchstart' in window || navigator.msMaxTouchPoints;
        var locale = moment.localeData('en');

        var lg = opt.language === 'auto' ? getFirstBrowserLanguage() : opt.language;
        var temp = moment.localeData(lg || 'en');
        if (typeof temp !== 'undefined' && temp !== null)
            locale = temp;

        //if it is a touch device, hide hovering tooltip
        if (opt.isTouchDevice) opt.hoveringTooltip = false;

        //show one month on mobile devices
        if (opt.singleMonth === 'auto') opt.singleMonth = $(window).width() < 480;
        if (opt.singleMonth) opt.stickyMonths = false;

        //DEV
        //if (opt.singleDate) opt.singleMonth = true;

        if (!opt.showTopbar) opt.autoClose = true;

        if (opt.startDate !== null && typeof opt.startDate === 'string')
            opt.startDate = ParseDate(opt.startDate);
        if (opt.endDate !== null && typeof opt.endDate === 'string')
            opt.endDate = ParseDate(opt.endDate);

        var langs = getLanguages();
        var box;
        var initiated = false;
        var self = this;
        var selfDom = $(self).get(0);
        var domChangeTimer;

        $(this).unbind('.datepicker').bind('click.datepicker', function (evt) {
            if (typeof box !== 'undefined') {
                var isOpen = box.is(':visible');
                if (!isOpen) open(opt.duration);
            }
        }).bind('change.datepicker', function (evt) {
            var hasSet = checkAndSetDefaultValue();
            if (hasSet === false)
                clearSelection();
        }).bind('keyup.datepicker', function (evt) {
            var ev = evt || window.event // IE support
            var kc = evt.which || ev.keyCode;
            var ctrlDown = ev.ctrlKey || ev.metaKey; // Mac support

            // Check for Alt+Gr (http://en.wikipedia.org/wiki/AltGr_key)
            if (ctrlDown && ev.altKey) return;

            if (ctrlDown) {
                if ((kc === 65 || kc === 97) /* 'A' or 'a'*/ || (kc === 67 || kc === 99)/* 'C' or 'c'*/) {
                    //console.log("controll pressed");
                    evt.preventDefault();
                    return;
                }
            }

            if ([37, 38, 39, 40, 16, 17, 18, 35, 36, 34, 33].indexOf(kc) != -1) { }
            else
            {
                delay(function () {
                    var hasSet = checkAndSetDefaultValue();
                    if (hasSet === false)
                        clearSelection();
                }, 2000);
            }
        });

        init_datepicker.call(this);

        if (opt.inline === false) {
            if (opt.alwaysOpen) {
                open(0);
            }
        }
        else {
            box.find('.gap').css({ display: 'block' });
            showGap();
        }

        // expose some api
        $(this).data('dateRangePicker',
		{
		    //DEV:
		    options: opt,
		    originalOpt: oriOpt,
		    setDateRange: function (d1, d2, silent) {
		        var da1, da2, sl;
		        if ($.isArray(d1) === true) {
		            if (d1.length > 0) {
		                if (typeof d1[0] === 'string')
		                    da1 = ParseDate(d1[0]);
		                else if (moment.isMoment(d1[0]))
		                    da1 = d1[0].toDate();
		                else
		                    da1 = d1[0];
		            }
		            if (d1.length > 1) {
		                if (typeof d1[1] === 'string')
		                    da2 = ParseDate(d1[1]);
		                else if (moment.isMoment(d1[1]))
		                    da2 = d1[1].toDate();
		                else
		                    da2 = d1[1];
		            }
		            sl = d2;
		        }
		        else {
		            sl = silent;
		            if (typeof d1 === 'string')
		                da1 = ParseDate(d1);
		            else if (moment.isMoment(d1))
		                da1 = d1.toDate();
		            else
		                da1 = d1;

		            if (typeof d2 === 'string')
		                da2 = ParseDate(d2);
		            else if (moment.isMoment(d2))
		                da2 = d2.toDate();
		            else
		                da2 = d2;
		        }

		        if (da1 && da2)
		            setDateRange(da1, da2, sl);
		    },
		    getDataValue: GetDataValue,
		    refresh: checkAndSetDefaultValue,
		    showMonth: showMonth,
		    clear: clearSelection,
		    close: closeDatePicker,
		    showLoading: ShowLoading,
		    hideLoading: HideLoading,
		    open: open,
		    replaceDiacritics: replaceDiacritics,
		    getAjaxDataRequestObject: GetAjaxDataRequestObject,
		    getDatePicker: getDatePicker,
		    destroy: function () {
		        var el = GetElement();
		        //console.log('begin destroy ', el);
		        $(el).data('date-picker-opened', null);
		        DestroyGlobalBind();
		        box.remove();
		        $(el).unbind('.datepicker');
		        $(el).unbind('datepicker-closed datepicker-change datepicker-first-date-selected');
		        $(el).data('dateRangePicker', '');
		    }
		});

        var delay = (function () {
            var timer = 0;
            return function (callback, ms) {
                clearTimeout(timer);
                timer = setTimeout(callback, ms);
            };
        })();

        $(window).on('resize.datepicker.' + ($(self).attr('id') || $(self).data('itime')), function () { delay(calcPosition, 500) });

        return this;

        function DestroyGlobalBind() {
            var id = $(self).attr('id');
            var key = (id || itime);
            //console.log('key : ', key);
            if (typeof id !== 'undefined' && id.length > 0) {
                var b = $('div[id^="dp_"][data-elid="' + id + '"]');
                if (b.length > 0)
                    b.remove();
            }

            if (typeof key !== 'undefined' && key.toString().length > 0) {
                var evlist = $._data(document, 'events');
                if (typeof evlist !== 'undefined' && $.isEmptyObject(evlist) === false) {
                    if ($.type(evlist.click) === 'array' && evlist.click.length > 0) {
                        $.each(evlist.click, function (i, o) {
                            if (o.namespace.indexOf(key.toString()) === 0) {
                                //console.log('destroy click ', o.namespace + '.' + o.type);
                                $(document).unbind('click.datepicker.' + key);
                                return false;
                            }
                        });
                    }
                }

                evlist = $._data(window, 'events');
                if (typeof evlist !== 'undefined' && $.isEmptyObject(evlist) === false) {
                    if ($.type(evlist.resize) === 'array' && evlist.resize.length > 0) {
                        $.each(evlist.resize, function (i, o) {
                            if (o.namespace.indexOf(key.toString()) === 0) {
                                //console.log('destroy resize ', o.namespace + '.' + o.type);
                                $(window).unbind('resize.datepicker.' + key);
                                return false;
                            }
                        });
                    }
                }
            }

        }

        function getTimeHTML() {
            return '<div>\
						<span>'+ lang('Time') + ': <span class="hour-val">00</span>:<span class="minute-val">00</span></span>\
					</div>\
					<div class="hour">\
						<label>'+ lang('Hour') + ': <input type="range" class="hour-range" name="hour" min="0" max="23"></label>\
					</div>\
					<div class="minute">\
						<label>'+ lang('Minute') + ': <input type="range" class="minute-range" name="minute" min="0" max="59"></label>\
					</div>';
        }

        function IsOwnDatePickerClicked(evt, selfObj) {
            var isjq = (selfObj instanceof jQuery);
            var el = isjq === true ? selfObj[0] : selfObj;

            return (evt.target == el || $(evt.target).data('itime') == $(el).data('itime')
                || (el.childNodes != undefined && $(el).has(evt.target).length > 0))
        }

        function getFormat() {
            var fm = opt.format;
            if (typeof fm === 'undefined' || fm.length === 0 || fm === null) {
                if (typeof opt.useMomentShortDay !== 'undefined'
                    && opt.useMomentShortDay !== null && opt.useMomentShortDay === true
                    && typeof locale._longDateFormat !== 'undefined')
                    fm = locale._longDateFormat.L;
            }
            if (typeof fm === 'undefined' || fm.length === 0 || fm === null)
                fm = 'DD/MM/YYYY';
            return fm;
        }

        function getFirstBrowserLanguage() {
            var nav = window.navigator,
            browserLanguagePropertyKeys = ['language', 'browserLanguage', 'systemLanguage', 'userLanguage'],
            i,
            language;

            // support for HTML 5.1 "navigator.languages"
            if (Array.isArray(nav.languages)) {
                for (i = 0; i < nav.languages.length; i++) {
                    language = nav.languages[i];
                    if (language && language.length) {
                        return language;
                    }
                }
            }

            // support for other well known properties in browsers
            for (i = 0; i < browserLanguagePropertyKeys.length; i++) {
                language = nav[browserLanguagePropertyKeys[i]];
                if (language && language.length) {
                    return language;
                }
            }

            return null;
        };

        function uniqId() {
            return Math.round(new Date().getTime() + (Math.random() * 100));
        }

        function GetElement() {
            var dataitime = $(self).data('itime');

            if (typeof dataitime === 'undefined')
                return self;
            var elem = $(":data(itime=" + dataitime + "):visible");
            return elem.length > 0 ? elem[0] : self;
        }

        function init_datepicker() {
            var selfclone = this;

            var isdelete = $(selfclone).attr('isdeletekey');
            if (typeof isdelete !== 'undefined' && isdelete.length > 0) {
                DestroyGlobalBind();
                $(selfclone).removeAttr('isdeletekey');
            }

            var iti = uniqId();
            $(selfclone).data('itime', iti);

            if ($(this).data('date-picker-opened')) {
                //console.log('call init close');
                closeDatePicker();
                return;
            }
            $(this).data('date-picker-opened', true);

            box = createDom();

            if (opt.inline === false)
                box.css('visibility', 'hidden');
            box.attr('id', 'dp_' + iti);
            var id = $(selfclone).attr('id');
            if (id)
                box.attr('data-elid', id);

            box.append('<div class="date-range-length-tip"></div>');
            box.delegate('.day', 'mouseleave', function () {
                box.find('.date-range-length-tip').hide();
            });

            $(opt.container).append(box);

            if (!opt.inline) {
                calcPosition();
            }
            else {
                box.addClass("inline-wrapper");
            }


            if (opt.alwaysOpen) {
                box.find('.apply-btn').hide();
            }

            if (opt.time.enabled) {
                if ((opt.startDate && opt.endDate) || (opt.start && opt.end)) {
                    showTime(ParseDate(opt.start || opt.startDate), 'time1');
                    showTime(ParseDate(opt.end || opt.endDate), 'time2');
                } else {
                    var defaultTime = ParseDate(opt.defaultDate, true);
                    if (opt.singleDate)
                        showTime(defaultTime, 'time1');
                    else {
                        showTime(defaultTime, 'time1');
                        showTime(defaultTime, 'time2');
                    }
                }
            }

            var hasSet = checkAndSetDefaultValue();
            if (hasSet === false)
                ShowDefault();

            //showSelectedInfo();


            var defaultTopText = '';
            if (opt.singleDate)
                defaultTopText = lang('default-single');
            else if (opt.minDays && opt.maxDays)
                defaultTopText = lang('default-range');
            else if (opt.minDays)
                defaultTopText = lang('default-more');
            else if (opt.maxDays)
                defaultTopText = lang('default-less');
            else
                defaultTopText = lang('default-default');

            box.find('.default-top').html(defaultTopText.replace(/\%d/, opt.minDays).replace(/\%d/, opt.maxDays));
            if (opt.singleMonth) {
                box.addClass('single-month');
            }
            else {
                box.addClass('two-months');
            }


            setTimeout(function () {
                updateCalendarWidth();
                initiated = true;
                if (opt.inline === false)
                    box.css({ 'visibility': '', 'display': 'none' });
            }, 0);

            box.click(function (evt) {
                evt.stopPropagation();
            });

            //if user click other place of the webpage, close date range picker window
            $(document).bind('click.datepicker.' + ($(self).attr('id') || $(self).data('itime')), function (evt) {
                var isinside = IsOwnDatePickerClicked(evt, selfclone);
                //console.log('isinside : ', isinside);
                if (isinside === false) {
                    if (box.is(':visible')) closeDatePicker();
                }
            });

            /*#region next and prev*/

            box.find('.next').click(function () {
                if (!opt.stickyMonths)
                    gotoNextMonth(this);
                else
                    gotoNextMonth_stickily(this);
            });

            function gotoNextMonth(selfel) {
                var tb = $(selfel).parents('table');
                var isMonth2 = tb.hasClass('month2');
                var month = isMonth2 ? opt.month2 : opt.month1;

                month = nextMonth(month);

                if (!opt.singleMonth && !opt.singleDate && !isMonth2 && compare_month(month, opt.month2) >= 0 || isMonthOutOfBounds(month)) return;
                if ($.isFunction(opt.onChangeMonthYear) === true) {
                    ShowLoading(function () {
                        var mm1 = moment(ParseDate(month));
                        opt.onChangeMonthYear(selfel, { m1: [mm1.month() + 1, mm1.year()] }, isMonth2,
                            function (dataRes) {
                                opt.dataAjax = dataRes;

                                showMonth(month, isMonth2 ? 'month2' : 'month1');
                                showGap();
                                updateCalendarWidth();
                                HideLoading();
                                calcPosition();
                            });
                    });
                }
                else {
                    showMonth(month, isMonth2 ? 'month2' : 'month1');
                    showGap();
                    updateCalendarWidth();
                    calcPosition();
                }
            }

            function gotoNextMonth_stickily(selfel) {
                var tb = $(selfel).parents('table');
                //var isMonth2 = tb.hasClass('month2');
                //var tb2 = tb.closest('.month-wrapper').find(isMonth2 ? '.month1' : '.month2');
                var nextMonth1 = nextMonth(opt.month1);
                var nextMonth2 = nextMonth(opt.month2);
                if (isMonthOutOfBounds(nextMonth2)) return;
                if (!opt.singleDate && compare_month(nextMonth1, nextMonth2) >= 0) return;
                if ($.isFunction(opt.onChangeMonthYear) === true) {
                    ShowLoading(function () {
                        var mm1 = moment(ParseDate(nextMonth1));
                        var mm2 = moment(ParseDate(nextMonth2));
                        opt.onChangeMonthYear(selfel,
                            { m1: [mm1.month() + 1, mm1.year()], m2: [mm2.month() + 1, mm2.year()] },
                            null,
                           function (dataRes) {
                               opt.dataAjax = dataRes;

                               showMonth(nextMonth1, 'month1');
                               showMonth(nextMonth2, 'month2');
                               showSelectedDays();
                               updateCalendarWidth();
                               HideLoading();
                               calcPosition();
                           });
                    });
                }
                else {
                    showMonth(nextMonth1, 'month1');
                    showMonth(nextMonth2, 'month2');
                    showSelectedDays();
                    updateCalendarWidth();
                    calcPosition();
                }
            }


            box.find('.prev').click(function () {
                if (!opt.stickyMonths)
                    gotoPrevMonth(this);
                else
                    gotoPrevMonth_stickily(this);
            });


            function gotoPrevMonth(selfel) {
                var tb = $(selfel).parents('table');
                var isMonth2 = tb.hasClass('month2');
                var month = isMonth2 ? opt.month2 : opt.month1;
                month = prevMonth(month);
                if (isMonth2 && compare_month(month, opt.month1) <= 0 || isMonthOutOfBounds(month)) return;
                if ($.isFunction(opt.onChangeMonthYear) === true) {
                    ShowLoading(function () {
                        var mm1 = moment(ParseDate(month));
                        opt.onChangeMonthYear(selfel, { m1: [mm1.month() + 1, mm1.year()] }, isMonth2,
                            function (dataRes) {
                                opt.dataAjax = dataRes;
                                showMonth(month, isMonth2 ? 'month2' : 'month1');
                                showGap();
                                updateCalendarWidth();
                                HideLoading();
                                calcPosition();
                            });
                    });
                }
                else {
                    showMonth(month, isMonth2 ? 'month2' : 'month1');
                    showGap();
                    updateCalendarWidth();
                    calcPosition();
                }
            }

            function gotoPrevMonth_stickily(selfel) {
                var tb = $(selfel).parents('table');
                //var isMonth2 = tb.hasClass('month2');
                //var tb2 = tb.closest('.month-wrapper').find(isMonth2 ? '.month1' : '.month2');
                var prevMonth1 = prevMonth(opt.month1);
                var prevMonth2 = prevMonth(opt.month2);
                if (isMonthOutOfBounds(prevMonth1)) return;
                if (!opt.singleDate && compare_month(prevMonth2, prevMonth1) <= 0) return;
                if ($.isFunction(opt.onChangeMonthYear) === true) {
                    ShowLoading(function () {
                        var mm1 = moment(ParseDate(prevMonth2));
                        var mm2 = moment(ParseDate(prevMonth1));
                        opt.onChangeMonthYear(selfel,
                            { m1: [mm1.month() + 1, mm1.year()], m2: [mm2.month() + 1, mm2.year()] },
                            null,
                            function (dataRes) {
                                opt.dataAjax = dataRes;

                                showMonth(prevMonth2, 'month2');
                                showMonth(prevMonth1, 'month1');
                                showSelectedDays();
                                updateCalendarWidth();
                                HideLoading();
                                calcPosition();
                            });
                    });
                }
                else {
                    showMonth(prevMonth2, 'month2');
                    showMonth(prevMonth1, 'month1');
                    showSelectedDays();
                    updateCalendarWidth();
                    calcPosition();
                }
            }

            /*#endregion*/

            /*#region bind other element*/

            box.delegate('.day', 'click', function (evt) {
                dayClicked($(this));
            });

            box.delegate('.day', 'mouseenter', function (evt) {
                dayHovering($(this));
            });

            box.delegate('th .sel-month', 'change', function (evt) {
                changeMonth(this, evt);
            });

            box.delegate('th .sel-year', 'change', function (evt) {
                changeYear(this, evt);
            });

            box.delegate('.week-number', 'click', function (evt) {
                weekNumberClicked($(this));
            });

            box.attr('unselectable', 'on')
            .css('user-select', 'none')
            .bind('selectstart', function (e) {
                e.preventDefault(); return false;
            });

            box.find('.apply-btn').click(function () {
                closeDatePicker();
                var dateRange = getDateString(new Date(opt.start)) + opt.separator + getDateString(new Date(opt.end));
                $(selfclone).trigger('datepicker-change',
                {
                    'value': dateRange,
                    'date1': new Date(opt.start),
                    'date2': new Date(opt.end)
                });
            });

            box.find('.clear-btn').click(function () {
                clearSelection();
            });

            box.find('[custom]').click(function () {
                var valueName = $(this).attr('custom');
                opt.start = false;
                opt.end = false;
                box.find('.day.checked').removeClass('checked');
                opt.setValue.call(selfDom, valueName);
                checkSelectionValid();
                showSelectedInfo(true);
                showSelectedDays();

                var data = GetDataValue();
                //console.log('data : ', data);
                if (data.length > 0) {
                    if (opt.singleDate === true) {
                        $(selfclone).trigger('datepicker-change',
                        {
                            'value': getDateString(data[0].toDate()),
                            'date1': data[0].toDate()
                        });
                    }
                    else {
                        var dateRange = getDateString(data[0].toDate()) + opt.separator + getDateString(data[1].toDate());
                        $(selfclone).trigger('datepicker-change',
                        {
                            'value': dateRange,
                            'date1': data[0].toDate(),
                            'date2': data[1].toDate()
                        });
                    }
                }

                if (opt.autoClose) closeDatePicker();
            });

            box.find('[shortcut]').click(function () {
                var shortcut = $(this).attr('shortcut');
                var end = new Date(), start = false;
                if (shortcut.indexOf('day') != -1) {
                    var day = parseInt(shortcut.split(',', 2)[1], 10);
                    start = new Date(new Date().getTime() + 86400000 * day);
                    end = new Date(end.getTime() + 86400000 * (day > 0 ? 1 : -1));
                }
                else if (shortcut.indexOf('week') != -1) {
                    var dir = shortcut.indexOf('prev,') != -1 ? -1 : 1;

                    if (dir == 1)
                        var stopDay = opt.startOfWeek == 'monday' ? 1 : 0;
                    else
                        var stopDay = opt.startOfWeek == 'monday' ? 0 : 6;

                    end = new Date(end.getTime() - 86400000);
                    while (end.getDay() != stopDay) end = new Date(end.getTime() + dir * 86400000);
                    start = new Date(end.getTime() + dir * 86400000 * 6);
                }
                else if (shortcut.indexOf('month') != -1) {
                    var dir = shortcut.indexOf('prev,') != -1 ? -1 : 1;
                    if (dir == 1)
                        start = nextMonth(end);
                    else
                        start = prevMonth(end);
                    start.setDate(1);
                    end = nextMonth(start);
                    end.setDate(1);
                    end = new Date(end.getTime() - 86400000);
                }
                else if (shortcut.indexOf('year') != -1) {
                    var dir = shortcut.indexOf('prev,') != -1 ? -1 : 1;
                    start = new Date();
                    start.setFullYear(end.getFullYear() + dir);
                    start.setMonth(0);
                    start.setDate(1);
                    end.setFullYear(end.getFullYear() + dir);
                    end.setMonth(11);
                    end.setDate(31);
                }
                else if (shortcut == 'custom') {
                    var name = $(this).html();
                    if (opt.customShortcuts && opt.customShortcuts.length > 0) {
                        for (var i = 0; i < opt.customShortcuts.length; i++) {
                            var sh = opt.customShortcuts[i];
                            if (sh.name === name) {
                                var data = [];
                                // try
                                // {
                                data = sh['dates'].call();
                                //}catch(e){}
                                if (data && data.length === 2) {
                                    start = data[0];
                                    end = data[1];
                                }

                                // if only one date is specified then just move calendars there
                                // move calendars to show this date's month and next months
                                if (data && data.length === 1) {
                                    var movetodate = data[0];
                                    showMonth(movetodate, 'month1');
                                    showMonth(nextMonth(movetodate), 'month2');
                                    showGap();
                                }

                                break;
                            }
                        }
                    }
                }
                if (start && end) {
                    setDateRange(start, end);
                    checkSelectionValid();
                }
            });

            box.find(".time1 input[type=range]").bind("change ", function (e) {
                var target = e.target,
                    hour = target.name == "hour" ? $(target).val().replace(/^(\d{1})$/, "0$1") : undefined,
                    min = target.name == "minute" ? $(target).val().replace(/^(\d{1})$/, "0$1") : undefined;
                setTime("time1", hour, min);
            });

            box.find(".time2 input[type=range]").bind("change ", function (e) {
                var target = e.target,
                    hour = target.name == "hour" ? $(target).val().replace(/^(\d{1})$/, "0$1") : undefined,
                    min = target.name == "minute" ? $(target).val().replace(/^(\d{1})$/, "0$1") : undefined;
                setTime("time2", hour, min);
            });

            /*#endregion*/

            //box.find('.gap').css('display','block');
        }

        function GetListMonth(lang, type) {
            var arrMonthName = [];
            var or = locale._abbr;
            moment.locale(lang);
            var m = moment();
            for (var i = 0; i < 12; i++) {
                arrMonthName.push({ m: (i + 1), n: m.month(i).format(type || 'MMMM') });
            }
            moment.locale(or);
            return arrMonthName;
        }

        function GetListWeek(lang, type) {
            var arrWeekName = [];
            var or = locale._abbr;
            moment.locale(lang);
            var m = moment();
            for (var i = 0; i < 7; i++) {
                arrWeekName.push({ d: (i + 1), n: m.day(i).format(type || 'dddd') });
            }
            moment.locale(or);
            return arrWeekName;
        }

        function TryParseDate(str, lang) {
            //console.log('TryParseDate : ', str, lang);

            if ($.type(str) === 'string' && str.length > 0) {

                moment.createFromInputFallback = function (config) {
                    // your favorite unreliable string magic, or
                    config._d = new Date(config._i);
                };

                var lg = lang || locale._abbr;
                if (lg === 'vi') {
                    var str2 = str.toLowerCase();
                    str2 = replaceDiacritics(str2);
                    var tp;

                    if (str2.indexOf('thang') >= 0 || str2.indexOf('thg') >= 0) {
                        //console.log('str2 : ', str2);

                        var lstMonthL = GetListMonth(lg, 'MMMM');
                        lstMonthL.sort(function (x, y) {
                            return y.n.length - x.n.length;
                        });
                        //var lstWeekL = GetListWeek(lg, 'dddd');

                        $.each(lstMonthL, function (i, o) {
                            str2 = str2.replace(new RegExp(replaceDiacritics(o.n), 'gi'), function (match) {
                                return 'THANG' + o.m;
                            });
                        });
                        /*
                        $.each(lstWeekL, function (i, o) {
                            str2 = str2.replace(replaceDiacritics(o.n).replace(/\s+/g,''), o.n);
                        });
                        */
                        //console.log('str2 : ', str2);

                        var tempFormat = [
                            'DD [THANG]MM YYYY HH:mm:ss',
                            'DD [THANG]MM YYYY HH:mm',
                            'DD [THANG]MM YYYY hh:mm',
                            'DD [THANG]MM YYYY h:mm',
                            'DD [THANG]MM YYYY H:mm',
                            'DD [THANG]MM YYYY',
                            '[THANG]MM DD YYYY HH:mm:ss',
                            '[THANG]MM DD YYYY HH:mm',
                            '[THANG]MM DD YYYY hh:mm',
                            '[THANG]MM DD YYYY h:mm',
                            '[THANG]MM DD YYYY H:mm',
                            '[THANG]MM DD YYYY',

                            'DD [TH]MM YYYY HH:mm:ss',
                            'DD [TH]MM YYYY HH:mm',
                            'DD [TH]MM YYYY hh:mm',
                            'DD [TH]MM YYYY h:mm',
                            'DD [TH]MM YYYY H:mm',
                            'DD [TH]MM YYYY',
                            '[TH]MM DD YYYY HH:mm:ss',
                            '[TH]MM DD YYYY HH:mm',
                            '[TH]MM DD YYYY hh:mm',
                            '[TH]MM DD YYYY h:mm',
                            '[TH]MM DD YYYY H:mm',
                            '[TH]MM DD YYYY',

                            'DD [THANG]M YYYY HH:mm:ss',
                            'DD [THANG]M YYYY HH:mm',
                            'DD [THANG]M YYYY hh:mm',
                            'DD [THANG]M YYYY h:mm',
                            'DD [THANG]M YYYY H:mm',
                            'DD [THANG]M YYYY',
                            '[THANG]M DD YYYY HH:mm:ss',
                            '[THANG]M DD YYYY HH:mm',
                            '[THANG]M DD YYYY hh:mm',
                            '[THANG]M DD YYYY h:mm',
                            '[THANG]M DD YYYY H:mm',
                            '[THANG]M DD YYYY',

                            'DD [TH]M YYYY HH:mm:ss',
                            'DD [TH]M YYYY HH:mm',
                            'DD [TH]M YYYY hh:mm',
                            'DD [TH]M YYYY h:mm',
                            'DD [TH]M YYYY H:mm',
                            'DD [TH]M YYYY',
                            '[TH]M DD YYYY HH:mm:ss',
                            '[TH]M DD YYYY HH:mm',
                            '[TH]M DD YYYY hh:mm',
                            '[TH]M DD YYYY h:mm',
                            '[TH]M DD YYYY H:mm',
                            '[TH]M DD YYYY'
                        ];

                        $.each(tempFormat, function (i, o) {
                            tp = moment(str2, o);
                            if (tp.isValid() === true) {
                                //console.log('ok : ', tp);
                                return false;
                            }
                        });
                    }

                    if (typeof tp === 'undefined' || tp.isValid() === false)
                        tp = moment(str, getFormat());
                    if (typeof tp === 'undefined' || tp.isValid() === false)
                        tp = moment(str);
                    return tp;
                }

                //console.log('getFormat() : ', getFormat());
                var tem = moment(str, getFormat());
                if (tem.isValid() === false)
                    tem = moment(str);
                return tem;
            }
        }

        function ParseDate(date, getDefault) {
            if (typeof date === 'boolean' || date === 'true' || date === 'false') {
                if (typeof getDefault !== 'undefined' && getDefault === true)
                    return new Date();
                else
                    return date;
            }

            var defaultDate = undefined;

            if (typeof date === 'string') {
                defaultDate = TryParseDate(date, locale._abbr);
                if (typeof defaultDate === 'undefined' || moment.isMoment(defaultDate) === false)
                    defaultDate = moment(date, getFormat()).toDate();
            }
            else if (typeof date === 'undefined' || date === null) {
                if (typeof getDefault !== 'undefined' && getDefault === true)
                    defaultDate = new Date();
            }
            else if (typeof date === 'object' && date.getTime)
                defaultDate = date;
            else {
                try {
                    defaultDate = moment(date).toDate();
                }
                catch (ex) {
                    defaultDate = undefined;
                }
            }

            if (typeof defaultDate === 'undefined' || defaultDate === null) {
                if (typeof getDefault !== 'undefined' && getDefault === true)
                    defaultDate = date;
            }

            return defaultDate;
        }

        function GetAjaxDataRequestObject() {
            var def = GetDataValue();
            if (def.length === 0) {
                var defaultDate = ParseDate(opt.defaultDate, true);
                if (opt.singleDate) {

                    if (opt.startDate && compare_month(defaultDate, opt.startDate) < 0)
                        defaultDate = ParseDate(opt.startDate);
                    if (opt.endDate && compare_month(defaultDate, opt.endDate) > 0)
                        defaultDate = ParseDate(opt.endDate);
                    var mm1 = moment(ParseDate(defaultDate));
                    return [self, { m1: [mm1.month() + 1, mm1.year()] }, false];

                }
                else {
                    if (opt.lookBehind) {
                        if (opt.startDate && compare_month(defaultDate, opt.startDate) < 0)
                            defaultDate = nextMonth(ParseDate(opt.startDate));
                        if (opt.endDate && compare_month(defaultDate, opt.endDate) > 0)
                            defaultDate = ParseDate(opt.endDate);

                        var mm1 = moment(prevMonth(ParseDate(defaultDate)));
                        var mm2 = moment(ParseDate(defaultDate));

                        return [self, {
                            m1: [mm1.month() + 1, mm1.year()],
                            m2: [mm2.month() + 1, mm2.year()]
                        }, null];
                    }
                    else {

                        if (opt.startDate && compare_month(defaultDate, opt.startDate) < 0)
                            defaultDate = ParseDate(opt.startDate);
                        if (opt.endDate && compare_month(nextMonth(defaultDate), opt.endDate) > 0)
                            defaultDate = prevMonth(ParseDate(opt.endDate));

                        var mm1 = moment(ParseDate(defaultDate));
                        var mm2 = moment(nextMonth(ParseDate(defaultDate)));

                        return [self, {
                            m1: [mm1.month() + 1, mm1.year()],
                            m2: [mm2.month() + 1, mm2.year()]
                        }, null];
                    }
                }
            }
            else {
                if (def.length === 1)
                    return [self, { m1: [def[0].month() + 1, def[0].year()] }, false];
                else if (def.length === 2)
                    return [self, {
                        m1: [def[0].month() + 1, def[0].year()],
                        m2: [def[1].month() + 1, def[1].year()]
                    }, null];
            }
        }

        function ShowDefault(force) {

            var defaultDate = ParseDate(opt.defaultDate, true);

            if (opt.singleDate) {
                if (opt.startDate && compare_month(defaultDate, opt.startDate) < 0)
                    defaultDate = ParseDate(opt.startDate);
                if (opt.endDate && compare_month(defaultDate, opt.endDate) > 0)
                    defaultDate = ParseDate(opt.endDate);
                if ((opt.inline === true && $.isFunction(opt.onChangeMonthYear) === true) ||
                    (typeof force !== 'undefined' && force === true && $.isFunction(opt.onChangeMonthYear) === true)) {
                    ShowLoading(function () {
                        //box.hide();
                        var mm1 = moment(ParseDate(defaultDate));
                        opt.onChangeMonthYear(self, { m1: [mm1.month() + 1, mm1.year()] }, false,
                            function (dataRes) {
                                opt.dataAjax = dataRes;

                                //box.show();
                                showMonth(defaultDate, 'month1');
                                updateCalendarWidth();
                                HideLoading();
                            });
                    });
                }
                else
                    showMonth(defaultDate, 'month1');
            }
            else {
                if (opt.lookBehind) {
                    if (opt.startDate && compare_month(defaultDate, opt.startDate) < 0)
                        defaultDate = nextMonth(ParseDate(opt.startDate));
                    if (opt.endDate && compare_month(defaultDate, opt.endDate) > 0)
                        defaultDate = ParseDate(opt.endDate);
                    if ((opt.inline === true && $.isFunction(opt.onChangeMonthYear) === true) ||
                    (typeof force !== 'undefined' && force === true && $.isFunction(opt.onChangeMonthYear) === true)) {
                        ShowLoading(function () {
                            //box.hide();
                            var mm1 = moment(prevMonth(ParseDate(defaultDate)));
                            var mm2 = moment(ParseDate(defaultDate));
                            opt.onChangeMonthYear(self, {
                                m1: [mm1.month() + 1, mm1.year()],
                                m2: [mm2.month() + 1, mm2.year()]
                            }, null, function (dataRes) {
                                opt.dataAjax = dataRes;

                                //box.show();
                                showMonth(prevMonth(defaultDate), 'month1');
                                showMonth(defaultDate, 'month2');
                                updateCalendarWidth();
                                HideLoading();
                            });
                        });
                    }
                    else {
                        showMonth(prevMonth(defaultDate), 'month1');
                        showMonth(defaultDate, 'month2');
                    }
                }
                else {

                    if (opt.startDate && compare_month(defaultDate, opt.startDate) < 0)
                        defaultDate = ParseDate(opt.startDate);
                    if (opt.endDate && compare_month(nextMonth(defaultDate), opt.endDate) > 0)
                        defaultDate = prevMonth(ParseDate(opt.endDate));
                    if ((opt.inline === true && $.isFunction(opt.onChangeMonthYear) === true) ||
                     (typeof force !== 'undefined' && force === true && $.isFunction(opt.onChangeMonthYear) === true)) {
                        ShowLoading(function () {
                            //box.hide();
                            var mm1 = moment(ParseDate(defaultDate));
                            var mm2 = moment(nextMonth(ParseDate(defaultDate)));
                            opt.onChangeMonthYear(self, {
                                m1: [mm1.month() + 1, mm1.year()],
                                m2: [mm2.month() + 1, mm2.year()]
                            }, null, function (dataRes) {
                                opt.dataAjax = dataRes;

                                //box.show();
                                showMonth(defaultDate, 'month1');
                                showMonth(nextMonth(defaultDate), 'month2');
                                updateCalendarWidth();
                                HideLoading();
                            });
                        });
                    }
                    else {
                        showMonth(defaultDate, 'month1');
                        showMonth(nextMonth(defaultDate), 'month2');
                    }
                }
            }

        }

        function ShowLoading(callback) {
            var ld = GetLoading();
            if (ld.length > 0)
                ld.show();
            if ($.isFunction(callback) === true)
                callback();
        }

        function HideLoading(callback) {
            var ld = GetLoading();
            if (ld.length > 0)
                ld.hide();
            if ($.isFunction(callback) === true)
                callback();
        }

        function GetLoading() {
            var divLoading = box.find('.month-wrapper .dp-loading');
            if (divLoading.length === 0) {
                try {
                    divLoading = $(opt.loadingTemplate);
                    divLoading.appendTo(box.find('.month-wrapper'));
                }
                catch (ex) {
                    divLoading = [];
                }
            }
            return divLoading;
        }

        function calcPosition() {
            if (!opt.inline) {
                var el = GetElement();

                //console.log('calcPosition :', el);

                var posMY = 'bottom';
                if (typeof opt.openAt !== 'undefined' && opt.openAt.length > 0)
                    posMY = opt.openAt;
                var posAT = 'left';
                if (typeof opt.direction !== 'undefined' && opt.direction.length > 0)
                    posAT = opt.direction;
                //console.log(box, self, posAT, posMY, opt.collision);

                var option = {
                    of: $(el), at: posAT + ' ' + posMY,
                    my: posAT + ' ' + (posMY === 'top' ? 'bottom' : (posMY === 'bottom' ? 'top' : posMY)),
                    collision: opt.collision
                };
                /*
                console.log('$("#' + box.attr('id') + '").position({' +
                    'of:$("#' + $(el).attr('id') + '"), at:"' + posAT + ' ' + posMY + '",my:"' +
                     posAT + ' ' + (posMY === 'top' ? 'bottom' : (posMY === 'bottom' ? 'top' : posMY)) +
                     '",collision: "' + opt.collision + '"})');
                */
                var isHide = box.is(':hidden');
                if (isHide === true)
                    box.show();
                box.position(option);
                if (isHide === true)
                    box.hide();
            }
        }

        function GetDataValue() {

            var __default_string = opt.getValue.call(GetElement());
            var defaults = __default_string ? __default_string.split(opt.separator) : '';

            if (defaults && ((defaults.length === 1 && opt.singleDate) || defaults.length >= 2)) {

                var ___format = getFormat();
                if (___format.match(/Do/)) {

                    ___format = ___format.replace(/Do/, 'D');
                    defaults[0] = defaults[0].replace(/(\d+)(th|nd|st)/, '$1');
                    if (defaults.length >= 2) {
                        defaults[1] = defaults[1].replace(/(\d+)(th|nd|st)/, '$1');
                    }
                }

                if (defaults.length >= 2) {
                    var mm1 = ParseDate(defaults[0], false);
                    var mm2 = ParseDate(defaults[1], false);
                    return [mm1, mm2];
                }
                else if (defaults.length === 1 && opt.singleDate) {
                    var mm1 = ParseDate(defaults[0], false);
                    return [mm1];
                }
            }
            return [];
        }

        function checkAndSetDefaultValue() {
            var hasSet = false;
            var __default_string = opt.getValue.call(GetElement());

            var defaults = __default_string ? __default_string.split(opt.separator) : '';

            if (defaults && ((defaults.length === 1 && opt.singleDate) || defaults.length >= 2)) {
                hasSet = true;

                var ___format = getFormat();
                if (___format.match(/Do/)) {
                    ___format = ___format.replace(/Do/, 'D');
                    defaults[0] = defaults[0].replace(/(\d+)(th|nd|st)/, '$1');
                    if (defaults.length >= 2) {
                        defaults[1] = defaults[1].replace(/(\d+)(th|nd|st)/, '$1');
                    }
                }

                //console.log(defaults, ___format, locale._abbr);

                // set initiated to avoid triggerring datepicker-change event
                initiated = false;
                if (defaults.length >= 2) {
                    var mm1, mm2;
                    mm1 = TryParseDate(defaults[0], locale._abbr);
                    mm2 = TryParseDate(defaults[1], locale._abbr);

                    if (typeof mm1 !== 'undefined' && mm1.isValid() === false)
                        mm1 = moment(defaults[0], ___format, locale._abbr);
                    if (typeof mm2 !== 'undefined' && mm2.isValid() === false)
                        mm2 = moment(defaults[1], ___format, locale._abbr);
                    if (typeof mm1 === 'undefined' || typeof mm2 === 'undefined')
                        return false;

                    if ($.isFunction(opt.onChangeMonthYear) === true) {
                        ShowLoading(function () {
                            opt.onChangeMonthYear(self,
                                { m1: [mm1.month() + 1, mm1.year()], m2: [mm2.month() + 1, mm2.year()] },
                                null,
                                function (dataRes) {
                                    opt.dataAjax = dataRes;

                                    setDateRange(mm1.toDate(), mm2.toDate());
                                    updateCalendarWidth();
                                    HideLoading();
                                });
                        });
                    }
                    else {
                        //console.log(mm1.isValid(), mm2.isValid(), mm1, mm2, defaults, ___format);
                        setDateRange(mm1.toDate(), mm2.toDate(), false);
                    }
                }
                else if (defaults.length === 1 && opt.singleDate) {
                    var mm1 = TryParseDate(defaults[0], locale._abbr);
                    //console.log('mm1 : ', mm1);
                    if (mm1.isValid() === false)
                        mm1 = moment(defaults[0], ___format, locale._abbr);
                    var c = opt.customValues;
                    if (c && mm1.parsingFlags().unusedInput.length > 0) {
                        var defaultDate = ParseDate(opt.defaultDate, true);
                        setSingleDate(defaultDate);
                        opt.setValue.call(selfDom, __default_string);
                    }
                    else {
                        if ($.isFunction(opt.onChangeMonthYear) === true) {
                            ShowLoading(function () {
                                opt.onChangeMonthYear(self, { m1: [mm1.month() + 1, mm1.year()] }, false,
                                    function (dataRes) {
                                        opt.dataAjax = dataRes;

                                        setSingleDate(mm1.toDate());

                                        updateCalendarWidth();
                                        HideLoading();
                                    });
                            });
                        }
                        else {
                            //console.log(mm1, defaults, ___format);
                            setSingleDate(mm1.toDate());
                        }
                    }
                }

                initiated = true;
            }
            else {
                //DEV
                //console.log(box, __default_string, defaults);
                box.find('.drp_top-bar').removeClass('normal');
            }

            return hasSet;
        }

        function updateCalendarWidth() {
            if (typeof opt.calculateWraperWith !== 'undefined' && opt.calculateWraperWith === true) {
                var isHide = box.is(':hidden');
                if (isHide === true)
                    box.show();
                //var gapMargin = box.find('.gap').css('margin-left');
                //if (gapMargin) gapMargin = parseInt(gapMargin);
                var thisB = box.find('.month-wrapper');
                var totalWidth = 0;
                /*
                totalWidth += parseInt(thisB.css("padding-left"), 10) + parseInt(thisB.css("padding-right"), 10); //Total Padding Width
                totalWidth += parseInt(thisB.css("margin-left"), 10) + parseInt(thisB.css("margin-right"), 10); //Total Margin Width
                totalWidth += parseInt(thisB.css("borderLeftWidth"), 10) + parseInt(thisB.css("borderRightWidth"), 10); //Total Border Width
                */
                var widbox = thisB.width(), fw = thisB.outerWidth(true);
                totalWidth = fw - widbox;

                var w1 = box.find('.w-table:eq(0)').outerWidth(true);
                //var w2 = box.find('.gap').width() + (gapMargin ? gapMargin * 2 : 0);
                var w2 = box.find('.gap').outerWidth(true);
                var w3 = box.find('.w-table:eq(1)').outerWidth(true);

                totalWidth += (w1 + w2 + w3);
                //console.log('updateCalendarWidth : ', totalWidth, w2);

                box.find('.month-wrapper').css('width', totalWidth + 'px');
                if (isHide === true)
                    box.hide();
            }
        }

        /*#region later mod*/

        function renderTime(name, date) {
            box.find("." + name + " input[type=range].hour-range").val(moment(ParseDate(date)).hours());
            box.find("." + name + " input[type=range].minute-range").val(moment(ParseDate(date)).minutes());
            setTime(name, moment(ParseDate(date)).format("HH"), moment(ParseDate(date)).format("mm"));
        }

        function changeTime(name, date) {
            opt[name] = parseInt(
				moment(parseInt(date))
					.startOf('day')
					.add(moment(opt[name + "Time"]).format("HH"), 'h')
					.add(moment(opt[name + "Time"]).format("mm"), 'm').valueOf()
				);
        }

        function swapTime() {
            renderTime("time1", opt.start);
            renderTime("time2", opt.end);
        }

        function setTime(name, hour, minute) {

            hour && (box.find("." + name + " .hour-val").text(hour));
            minute && (box.find("." + name + " .minute-val").text(minute));
            switch (name) {
                case "time1":
                    if (opt.start) {
                        setRange("start", moment(ParseDate(opt.start)));
                    }
                    setRange("startTime", moment(ParseDate(opt.startTime) || moment().valueOf()));
                    break;
                case "time2":
                    if (opt.end) {
                        setRange("end", moment(ParseDate(opt.end)));
                    }
                    setRange("endTime", moment(ParseDate(opt.endTime) || moment().valueOf()));
                    break;
            }
            function setRange(name, timePoint) {
                var h = timePoint.format("HH"),
					m = timePoint.format("mm");
                opt[name] = timePoint
					.startOf('day')
					.add(hour || h, "h")
					.add(minute || m, "m")
					.valueOf();
            }
            checkSelectionValid();
            showSelectedInfo();
            showSelectedDays();
        }

        function clearSelection() {
            opt.start = false;
            opt.end = false;
            box.find('.day.checked').removeClass('checked');
            box.find('.day.last-date-selected').removeClass('last-date-selected');
            box.find('.day.first-date-selected').removeClass('first-date-selected');
            opt.setValue.call(selfDom, '');
            checkSelectionValid();
            showSelectedInfo();
            showSelectedDays();
        }

        function handleStart(time) {
            var r = time;
            if (opt.batchMode === 'week-range') {
                if (opt.startOfWeek === 'monday') {
                    r = moment(parseInt(time)).startOf('isoweek').valueOf();
                }
                else {
                    r = moment(parseInt(time)).startOf('week').valueOf();
                }
            }
            else if (opt.batchMode === 'month-range') {
                r = moment(parseInt(time)).startOf('month').valueOf();
            }
            return r;
        }

        function handleEnd(time) {
            var r = time;
            if (opt.batchMode === 'week-range') {
                if (opt.startOfWeek === 'monday') {
                    r = moment(parseInt(time)).endOf('isoweek').valueOf();
                }
                else {
                    r = moment(parseInt(time)).endOf('week').valueOf();
                }
            }
            else if (opt.batchMode === 'month') {
                r = moment(parseInt(time)).endOf('month').valueOf();
            }
            return r;
        }

        function UpdateOtherMonth(isMonth2, month) {
            //console.log(isMonth2, month);
            var val, y;
            var wrapdiv = box.find('.month-wrapper');
            if (wrapdiv.length > 0) {
                if (isMonth2 === false) {
                    var tbmonth2 = wrapdiv.find('table.month2');
                    if (tbmonth2.length > 0) {
                        val = tbmonth2.find('th .sel-month').val();
                        //console.log(month, val);
                        y = moment(opt.month2).year();
                        if (tbmonth2.find('th .sel-year').length > 0)
                            y = tbmonth2.find('th .sel-year').val();
                        var month2 = moment({ day: 1, month: val, year: y }).toDate();
                        if (compare_month(month, month2) === 1) {
                            showMonth(month, 'month2');
                        }
                    }
                }
                else {
                    var tbmonth1 = wrapdiv.find('table.month1');
                    if (tbmonth1.length > 0) {
                        val = tbmonth1.find('th .sel-month').val();
                        //console.log(month, val);
                        y = moment(opt.month2).year();
                        if (tbmonth1.find('th .sel-year').length > 0)
                            y = tbmonth1.find('th .sel-year').val();
                        var month1 = moment({ day: 1, month: val, year: y }).toDate();
                        if (compare_month(month, month1) === -1) {
                            showMonth(month, 'month1');
                        }
                    }
                }
            }
        }

        function changeMonth(sel, e) {
            var tb = $(sel).parents('table');

            var isMonth2 = tb.hasClass('month2');
            var month = isMonth2 ? opt.month2 : opt.month1;

            var val = $(sel).val();
            //console.log(month, val);
            var y = moment(month).year();
            if (tb.find('th .sel-year').length > 0)
                y = tb.find('th .sel-year').val();
            month = moment({ day: 1, month: val, year: y }).toDate();
            //console.log(month, ', compare_month(month, opt.month2) : ', compare_month(month, opt.month2),
            //   ',isMonthOutOfBounds(month) : ', isMonthOutOfBounds(month));

            if (isMonthOutOfBounds(month) === true) {
                e.preventDefault();
                e.stopPropagation();
                return;
            }

            if ($.isFunction(opt.onChangeMonthYear) === true) {
                ShowLoading(function () {
                    var mm1 = moment(ParseDate(month));
                    opt.onChangeMonthYear(self, { m1: [mm1.month() + 1, mm1.year()] }, isMonth2,
                        function (dataRes) {
                            opt.dataAjax = dataRes;

                            showMonth(month, isMonth2 ? 'month2' : 'month1');
                            UpdateOtherMonth(isMonth2, month);

                            showGap();
                            updateCalendarWidth();
                            HideLoading();
                        });
                });
            }
            else {
                showMonth(month, isMonth2 ? 'month2' : 'month1');
                UpdateOtherMonth(isMonth2, month);

                showGap();
                updateCalendarWidth();
            }
        }

        function changeYear(sel, e) {
            var tb = $(sel).parents('table');

            var isMonth2 = tb.hasClass('month2');
            var month = isMonth2 ? opt.month2 : opt.month1;

            var val = $(sel).val();
            //console.log(month, val);
            var mon = moment(month).month();
            if (tb.find('th .sel-month').length > 0)
                mon = tb.find('th .sel-month').val();
            month = moment({ day: 1, month: mon, year: val }).toDate();
            //console.log(month, ', compare_month(month, opt.month2) : ', compare_month(month, opt.month2),
            //    ',isMonthOutOfBounds(month) : ', isMonthOutOfBounds(month));

            if (isMonthOutOfBounds(month) === true) {
                e.preventDefault();
                e.stopPropagation();
                return;
            }

            if ($.isFunction(opt.onChangeMonthYear) === true) {
                ShowLoading(function () {
                    var mm1 = moment(ParseDate(month));
                    opt.onChangeMonthYear(self, { m1: [mm1.month() + 1, mm1.year()] }, isMonth2,
                        function (dataRes) {
                            opt.dataAjax = dataRes;

                            showMonth(month, isMonth2 ? 'month2' : 'month1');
                            UpdateOtherMonth(isMonth2, month);

                            showGap();
                            updateCalendarWidth();
                            HideLoading();
                        });
                });
            }
            else {
                showMonth(month, isMonth2 ? 'month2' : 'month1');
                UpdateOtherMonth(isMonth2, month);

                showGap();
                updateCalendarWidth();
            }

        }

        function dayClicked(day) {
            if (day.hasClass('invalid')) return;

            var time = day.attr('time');
            day.addClass('checked');
            if (opt.singleDate) {
                opt.start = time;
                opt.end = false;
            }
            else if (opt.batchMode === 'week') {
                if (opt.startOfWeek === 'monday') {
                    opt.start = moment(parseInt(time)).startOf('isoweek').valueOf();
                    opt.end = moment(parseInt(time)).endOf('isoweek').valueOf();
                } else {
                    opt.end = moment(parseInt(time)).endOf('week').valueOf();
                    opt.start = moment(parseInt(time)).startOf('week').valueOf();
                }
            }
            else if (opt.batchMode === 'workweek') {
                opt.start = moment(parseInt(time)).day(1).valueOf();
                opt.end = moment(parseInt(time)).day(5).valueOf();
            }
            else if (opt.batchMode === 'weekend') {
                opt.start = moment(parseInt(time)).day(6).valueOf();
                opt.end = moment(parseInt(time)).day(7).valueOf();
            }
            else if (opt.batchMode === 'month') {
                opt.start = moment(parseInt(time)).startOf('month').valueOf();
                opt.end = moment(parseInt(time)).endOf('month').valueOf();
            }
            else if ((opt.start && opt.end) || (!opt.start && !opt.end)) {
                opt.start = handleStart(time);
                opt.end = false;
            }
            else if (opt.start) {
                //console.log('end', time);
                opt.end = handleEnd(time);
                if (opt.time.enabled) {
                    changeTime("end", opt.end);
                }
            }

            //Update time in case it is enabled and timestamps are available
            if (opt.time.enabled) {
                if (opt.start) {
                    changeTime("start", opt.start);
                }
                if (opt.end) {
                    changeTime("end", opt.end);
                }
            }

            //In case the start is after the end, swap the timestamps
            if (!opt.singleDate && opt.start && opt.end && opt.start > opt.end) {
                var tmp = opt.end;
                opt.end = handleEnd(opt.start);
                opt.start = handleStart(tmp);
                if (opt.time.enabled && opt.swapTime) {
                    swapTime();
                }
            }

            opt.start = parseInt(opt.start);
            opt.end = parseInt(opt.end);
            clearHovering();
            if (opt.start && !opt.end) {
                $(self).trigger('datepicker-first-date-selected',
				{
				    'date1': new Date(opt.start)
				});
                dayHovering(day);
            }

            updateSelectableRange(time);
            checkSelectionValid();
            showSelectedInfo();
            showSelectedDays();
            autoclose();

        }

        function weekNumberClicked(weekNumberDom) {
            var thisTime = parseInt(weekNumberDom.attr('data-start-time'), 10);
            if (!opt.startWeek) {
                opt.startWeek = thisTime;
                weekNumberDom.addClass('week-number-selected');
                var date1 = new Date(thisTime);
                opt.start = moment(date1).day(opt.startOfWeek == 'monday' ? 1 : 0).toDate();
                opt.end = moment(date1).day(opt.startOfWeek == 'monday' ? 7 : 6).toDate();
            }
            else {
                box.find('.week-number-selected').removeClass('week-number-selected');
                var date1 = new Date(thisTime < opt.startWeek ? thisTime : opt.startWeek);
                var date2 = new Date(thisTime < opt.startWeek ? opt.startWeek : thisTime);
                opt.startWeek = false;
                opt.start = moment(date1).day(opt.startOfWeek == 'monday' ? 1 : 0).toDate();
                opt.end = moment(date2).day(opt.startOfWeek == 'monday' ? 7 : 6).toDate();
            }
            updateSelectableRange();
            checkSelectionValid();
            showSelectedInfo();
            showSelectedDays();
            autoclose();
        }

        function isValidTime(time) {
            time = parseInt(time, 10);
            //console.log(moment(time).format('DD-MM-YYYY'));
            if (opt.startDate && compare_day(time, opt.startDate) < 0) return false;
            if (opt.endDate && compare_day(time, opt.endDate) > 0) return false;

            if (opt.start && !opt.end && !opt.singleDate) {
                //check maxDays and minDays setting
                //console.log(opt.maxDays, moment(time).format('DD-MM-YYYY'), moment(opt.start).format('DD-MM-YYYY'), countDays(time, opt.start));
                if (opt.maxDays > 0 && countDays(time, opt.start) > opt.maxDays) return false;
                //console.log(opt.minDays, moment(time).format('DD-MM-YYYY'), moment(opt.start).format('DD-MM-YYYY'), countDays(time, opt.start));
                if (opt.minDays > 0 && countDays(time, opt.start) < opt.minDays) return false;

                //check selectForward and selectBackward
                if (opt.selectForward && time < opt.start) return false;
                if (opt.selectBackward && time > opt.start) return false;

                //check disabled days
                if (opt.beforeShowDayFunc && typeof opt.beforeShowDayFunc === 'function') {
                    var valid = true;
                    var arr = opt.beforeShowDayFunc(new Date(time), opt.dataAjax);
                    if (!arr[0])
                        valid = false;

                    if (!valid) return false;
                }
            }
            return true;
        }

        function updateSelectableRange() {
            box.find('.day.invalid.tmp').removeClass('tmp invalid').addClass('valid');
            if (opt.start && !opt.end) {
                var isValid = false;
                var time;

                var arrBefore = [];
                var arrAfter = [];
                var hasMaxOrMin = opt.maxDays > 0 || opt.minDays > 0;

                var all = box.find('.day.toMonth');
                //console.log('all : ', all);

                var timeMain = parseInt(opt.start, 10);
                $.each(all, function (i, o) {
                    time = parseInt($(this).attr('time'), 10);
                    if (time > timeMain)
                        arrAfter.push(this);
                    else if (time < timeMain)
                        arrBefore.push(this);
                });

                if (arrBefore.length > 0) {
                    var flag = undefined;

                    $.each(arrBefore.reverse(), function () {

                        time = parseInt($(this).attr('time'), 10);

                        if ($(this).hasClass('invalid')) {
                            isValid = false;

                            if (hasMaxOrMin === false && opt.singleDate === false)
                                flag = time;
                        }
                        else {
                            if (typeof flag === 'undefined')
                                isValid = isValidTime(time);
                            else
                                isValid = false;
                            //console.log('bef isValid : ', isValid, ', time : ', time, ', moment(time): ', moment(time).format('DD-MM-YYYY'));
                            if (isValid === false) {
                                $(this).addClass('invalid tmp').removeClass('valid');

                                if (hasMaxOrMin === false && opt.singleDate === false)
                                    flag = time;
                            }
                            else
                                $(this).addClass('valid tmp').removeClass('invalid');
                        }
                    });
                }

                if (arrAfter.length > 0) {
                    var flag = undefined;
                    $(arrAfter).each(function () {

                        time = parseInt($(this).attr('time'), 10);

                        if ($(this).hasClass('invalid')) {
                            isValid = false;

                            if (hasMaxOrMin === false && opt.singleDate === false)
                                flag = time;
                        }
                        else {
                            if (typeof flag === 'undefined')
                                isValid = isValidTime(time);
                            else
                                isValid = false;
                            //console.log('aft isValid : ', isValid, ', time : ', time, ', moment(time): ', moment(time).format('DD-MM-YYYY'));
                            if (isValid === false) {
                                $(this).addClass('invalid tmp').removeClass('valid');

                                if (hasMaxOrMin === false && opt.singleDate === false)
                                    flag = time;
                            }
                            else
                                $(this).addClass('valid tmp').removeClass('invalid');
                        }
                    });
                }

            }

            return true;
        }

        function dayHovering(day) {
            var hoverTime = parseInt(day.attr('time'));
            var tooltip = '';

            if (day.hasClass('has-tooltip') && day.attr('data-tooltip')) {
                if (typeof opt.overrideDataTooltip !== 'undefined'
                    && opt.overrideDataTooltip === true) {
                    //console.log(opt.start, opt.end);
                    if (opt.start && !opt.end) {
                        var days = countDays(hoverTime, opt.start);
                        if (opt.hoveringTooltip) {
                            if (typeof opt.hoveringTooltip == 'function') {
                                tooltip = opt.hoveringTooltip(days, opt.start, hoverTime);
                            }
                            else if (opt.hoveringTooltip === true && days > 1) {
                                tooltip = days + ' ' + lang('days');
                            }
                        }
                    }
                    else
                        tooltip = '<span style="white-space:nowrap">' + day.attr('data-tooltip') + '</span>';
                }
                else
                    tooltip = '<span style="white-space:nowrap">' + day.attr('data-tooltip') + '</span>';
            }
            else if (!day.hasClass('invalid')) {
                if (opt.singleDate) {
                    box.find('.day.hovering').removeClass('hovering');
                    day.addClass('hovering');
                }
                else {
                    box.find('.day').each(function () {
                        var time = parseInt($(this).attr('time')),
							start = opt.start,
							end = opt.end;

                        if (time == hoverTime) {
                            $(this).addClass('hovering');
                        }
                        else {
                            $(this).removeClass('hovering');
                        }

                        if (
							(opt.start && !opt.end)
							&&
							(
								(opt.start < time && hoverTime >= time)
								||
								(opt.start > time && hoverTime <= time)
							)
						) {
                            $(this).addClass('hovering');
                        }
                        else {
                            $(this).removeClass('hovering');
                        }
                    });

                    if (opt.start && !opt.end) {
                        var days = countDays(hoverTime, opt.start);
                        if (opt.hoveringTooltip) {
                            if (typeof opt.hoveringTooltip == 'function') {
                                tooltip = opt.hoveringTooltip(days, opt.start, hoverTime);
                            }
                            else if (opt.hoveringTooltip === true && days > 1) {
                                tooltip = days + ' ' + lang('days');
                            }
                        }
                    }
                }
            }

            if (tooltip) {
                var posDay = day.offset();
                var posBox = box.offset();

                var _left = posDay.left - posBox.left;
                var _top = posDay.top - posBox.top;
                _left += day.width() / 2;


                var $tip = box.find('.date-range-length-tip');
                var w = $tip.css({ 'visibility': 'hidden', 'display': 'none' }).html(tooltip).width();
                var h = $tip.height();
                _left -= w / 2;
                _top -= h;
                setTimeout(function () {
                    $tip.css({ left: _left, top: _top, display: 'block', 'visibility': 'visible' });
                }, 10);
            }
            else {
                box.find('.date-range-length-tip').hide();
            }
        }

        function clearHovering() {
            box.find('.day.hovering').removeClass('hovering');
            box.find('.date-range-length-tip').hide();
        }

        function autoclose() {
            //console.log('autoclose');
            if (opt.singleDate === true) {
                if (initiated && opt.start) {
                    if (opt.autoClose) closeDatePicker();
                }
            } else {
                if (initiated && opt.start && opt.end) {
                    if (opt.autoClose) closeDatePicker();
                }
            }
        }

        function checkSelectionValid() {
            var days = Math.ceil((opt.end - opt.start) / 86400000) + 1;
            if (opt.singleDate) { // Validate if only start is there
                //console.log('opt.start : ', opt.start);
                if (opt.start && !opt.end)
                    box.find('.drp_top-bar').removeClass('error').addClass('normal');
                else
                    box.find('.drp_top-bar').removeClass('error').removeClass('normal');
            }
            else if (opt.maxDays && days > opt.maxDays) {
                opt.start = false;
                opt.end = false;
                box.find('.day').removeClass('checked');
                box.find('.drp_top-bar').removeClass('normal').addClass('error').find('.error-top').html(lang('less-than').replace('%d', opt.maxDays));
            }
            else if (opt.minDays && days < opt.minDays) {
                opt.start = false;
                opt.end = false;
                box.find('.day').removeClass('checked');
                box.find('.drp_top-bar').removeClass('normal').addClass('error').find('.error-top').html(lang('more-than').replace('%d', opt.minDays));
            }
            else {
                if (opt.start || opt.end)
                    box.find('.drp_top-bar').removeClass('error').addClass('normal');
                else
                    box.find('.drp_top-bar').removeClass('error').removeClass('normal');
            }

            if ((opt.singleDate && opt.start && !opt.end) || (!opt.singleDate && opt.start && opt.end)) {
                box.find('.apply-btn').removeClass('disabled');
            }
            else {
                box.find('.apply-btn').addClass('disabled');
            }

            if (opt.batchMode) {
                if ((opt.start && opt.startDate && compare_day(opt.start, opt.startDate) < 0)
					|| (opt.end && opt.endDate && compare_day(opt.end, opt.endDate) > 0)) {
                    opt.start = false;
                    opt.end = false;
                    box.find('.day').removeClass('checked');
                }
            }
        }

        function showSelectedInfo(forceValid, silent) {
            //console.log('forceValid : ', forceValid, ', silent : ', silent, opt.start, opt.end, new Date(opt.start), new Date(opt.end));
            box.find('.start-day').html('...');
            box.find('.end-day').html('...');
            box.find('.selected-days').hide();
            if (opt.start) {
                box.find('.start-day').html(getDateString(new Date(parseInt(opt.start))));
            }
            if (opt.end) {
                box.find('.end-day').html(getDateString(new Date(parseInt(opt.end))));
            }

            if (opt.start && opt.singleDate) {
                box.find('.apply-btn').removeClass('disabled');
                var dateRange = getDateString(new Date(opt.start));

                opt.setValue.call(selfDom, dateRange, getDateString(new Date(opt.start)), '', new Date(opt.start), null);

                if (initiated && !silent) {
                    $(self).trigger('datepicker-change',
					{
					    'value': dateRange,
					    'date1': new Date(opt.start)
					});
                }
            }
            else if (opt.start && opt.end) {
                var htmlcount = box.find('.selected-days').show();
                if (typeof opt.showDifferenceFunc === 'function')
                    opt.showDifferenceFunc(htmlcount, opt.start, opt.end, difference(new Date(opt.start), new Date(opt.end)));
                else
                    htmlcount.find('.selected-days-num').html(countDays(opt.end, opt.start));

                box.find('.apply-btn').removeClass('disabled');
                var dateRange = getDateString(new Date(opt.start)) + opt.separator + getDateString(new Date(opt.end));
                opt.setValue.call(selfDom, dateRange, getDateString(new Date(opt.start)),
                    getDateString(new Date(opt.end)),
                    new Date(opt.start), new Date(opt.end));
                //console.log('showSelectedInfo : ',opt.start, opt.end, dateRange, new Date(opt.start), new Date(opt.end));

                if (initiated && !silent) {
                    $(self).trigger('datepicker-change',
					{
					    'value': dateRange,
					    'date1': new Date(opt.start),
					    'date2': new Date(opt.end)
					});
                }
            }
            else if (forceValid) {
                box.find('.apply-btn').removeClass('disabled');
            }
            else {
                box.find('.apply-btn').addClass('disabled');
            }
        }

        function countDays(start, end) {
            return Math.abs(daysFrom1970(start) - daysFrom1970(end)) + 1;
        }

        /*#endregion*/

        function setDateRange(date1, date2, silent) {
            //console.log('setDateRange : ',date1, date2);
            if (date1.getTime() > date2.getTime()) {
                var tmp = date2;
                date2 = date1;
                date1 = tmp;
                tmp = null;
            }
            var valid = true;
            if (opt.startDate && compare_day(date1, opt.startDate) < 0) valid = false;
            if (opt.endDate && compare_day(date2, opt.endDate) > 0) valid = false;
            if (!valid) {
                showMonth(opt.startDate || new Date(), 'month1');
                showMonth(nextMonth(opt.startDate || new Date()), 'month2');
                showGap();
                calcPosition();
                return;
            }

            opt.start = date1.getTime();
            opt.end = date2.getTime();

            if (opt.time.enabled) {
                renderTime("time1", date1);
                renderTime("time2", date2);
            }

            if (opt.stickyMonths || (compare_day(date1, date2) > 0 && compare_month(date1, date2) === 0)) {
                if (opt.lookBehind) {
                    date1 = prevMonth(date2);
                } else {
                    date2 = nextMonth(date1);
                }
            }

            if (opt.stickyMonths && opt.endDate && compare_month(date2, opt.endDate) > 0) {
                date1 = prevMonth(date1);
                date2 = prevMonth(date2);
            }

            if (!opt.stickyMonths) {
                if (compare_month(date1, date2) == 0) {
                    if (opt.lookBehind) {
                        date1 = prevMonth(date2);
                    } else {
                        date2 = nextMonth(date1);
                    }
                }
            }

            showMonth(date1, 'month1');
            showMonth(date2, 'month2');
            showGap();
            checkSelectionValid();
            showSelectedInfo(false, silent);
            autoclose();
            calcPosition();
        }

        function setSingleDate(date1, silent) {
            //console.log('setSingleDate ', date1, silent);
            var valid = true;
            if (opt.startDate && compare_day(date1, opt.startDate) < 0) valid = false;
            if (opt.endDate && compare_day(date1, opt.endDate) > 0) valid = false;
            if (!valid) {
                showMonth(opt.startDate, 'month1');
                return;
            }

            opt.start = date1.getTime();


            if (opt.time.enabled) {
                renderTime("time1", date1);
            }


            showMonth(date1, 'month1');
            //showMonth(date2,'month2');
            showGap();

            checkSelectionValid();
            showSelectedInfo(false, silent);
            autoclose();
            calcPosition();
        }

        function showSelectedDays() {
            if (!opt.start && !opt.end) return;
            box.find('.day').each(function () {
                var time = parseInt($(this).attr('time')),
					start = ParseDate(opt.start),
					end = ParseDate(opt.end);
                if (opt.time.enabled) {
                    time = moment(time).startOf('day').valueOf();
                    start = moment(start || moment().valueOf()).startOf('day').valueOf();
                    end = moment(end || moment().valueOf()).startOf('day').valueOf();
                }
                if (
					(opt.start && opt.end && end >= time && start <= time)
					|| (opt.start && !opt.end && moment(start).format('YYYY-MM-DD') == moment(time).format('YYYY-MM-DD'))
				) {
                    $(this).addClass('checked');
                }
                else {
                    $(this).removeClass('checked');
                }

                //add first-date-selected class name to the first date selected
                if (opt.start && moment(start).format('YYYY-MM-DD') == moment(time).format('YYYY-MM-DD')) {
                    $(this).addClass('first-date-selected');
                }
                else {
                    $(this).removeClass('first-date-selected');
                }
                //add last-date-selected
                if (opt.end && moment(end).format('YYYY-MM-DD') == moment(time).format('YYYY-MM-DD')) {
                    $(this).addClass('last-date-selected');
                }
                else {
                    $(this).removeClass('last-date-selected');
                }
            });

            box.find('.week-number').each(function () {
                if ($(this).attr('data-start-time') == opt.startWeek) {
                    $(this).addClass('week-number-selected');
                }
            });
        }

        function showMonth(date, month) {
            if (typeof month === 'undefined' || month === null || month.length === 0)
                month = 'month1';
            //console.log('showMonth : ', month, date);
            var m = moment(date);
            date = m.toDate();
            var monthName = locale.months(m);
            var html = getHeadMonthAndYear(date.getFullYear());
            box.find('.' + month + ' .month-name').html(html);

            if (opt.showDropdownMonth === true && opt.showDropdownYear === true) {
                box.find('.' + month + ' .month-name .sel-month').val(date.getMonth());
                box.find('.' + month + ' .month-name .sel-year').val(date.getFullYear());
            }
            else if (opt.showDropdownMonth === false && opt.showDropdownYear === true) {
                box.find('.' + month + ' .month-name .lb-month').html(monthName);
                box.find('.' + month + ' .month-name .sel-year').val(date.getFullYear());
            }
            else if (opt.showDropdownMonth === true && opt.showDropdownYear === false) {
                box.find('.' + month + ' .month-name .sel-month').val(date.getMonth());
                box.find('.' + month + ' .month-name .lb-year').html(date.getFullYear());
            }
            else if (opt.showDropdownMonth === false && opt.showDropdownYear === false) {
                box.find('.' + month + ' .month-name .lb-month').html(monthName);
                box.find('.' + month + ' .month-name .lb-year').html(date.getFullYear());
            }

            box.find('.' + month + ' tbody').html(createMonthHTML(date));
            opt[month] = date;

            updateSelectableRange();
        }

        function showTime(date, name) {
            box.find('.' + name).append(getTimeHTML());
            renderTime(name, date);
        }

        function getDateString(d) {
            //console.log('typeof d : ',$.type(d));
            var s = moment(d).locale(locale._abbr).format(getFormat());
            return s;
        }

        function showGap() {
            showSelectedDays();
            var m1 = parseInt(moment(opt.month1).format('YYYYMM'));
            var m2 = parseInt(moment(opt.month2).format('YYYYMM'));
            var p = Math.abs(m1 - m2);
            var shouldShow = (p > 1 && p != 89);
            if (shouldShow) {
                box.addClass('has-gap').removeClass('no-gap').find('.gap').css({ 'visibility': 'visible' });
            }
            else {
                if (opt.singleDate === false)
                    box.removeClass('has-gap').addClass('no-gap').find('.gap').css('visibility', 'hidden');
            }
            var h1 = box.find('table.month1').height();
            var h2 = box.find('table.month2').height();
            box.find('.gap').height(Math.max(h1, h2) + 12);
        }

        function closeDatePicker() {
            if (opt.alwaysOpen) return;
            $(box).slideUp(opt.duration, function () {
                $(self).data('date-picker-opened', false);
                $(self).trigger('datepicker-closed', { relatedTarget: box, data: GetDataValue() });
            });
            //$(document).unbind('.datepicker');
            $(self).trigger('datepicker-close', { relatedTarget: box, data: GetDataValue() });
        }

        function compare_month(m1, m2) {
            //console.log('compare_month : ',m1, m2);
            var p = parseInt(moment(m1).format('YYYYMM')) - parseInt(moment(m2).format('YYYYMM'));
            if (p > 0) return 1;
            if (p == 0) return 0;
            return -1;
        }

        function compare_day(m1, m2) {
            var p = parseInt(moment(m1).format('YYYYMMDD')) - parseInt(moment(m2).format('YYYYMMDD'));
            if (p > 0) return 1;
            if (p === 0) return 0;
            return -1;
        }

        function nextMonth(month) {
            return moment(month).add(1, 'months').toDate();
        }

        function prevMonth(month) {
            return moment(month).add(-1, 'months').toDate();
        }

        function open(animationTime) {
            box.find('.gap').css('display', 'block');
            showGap();

            calcPosition();
            var hasSet = checkAndSetDefaultValue();
            if (hasSet === false) {
                if ($.isFunction(opt.onChangeMonthYear) === true) {
                    box.slideDown(animationTime, function () {
                        $(self).trigger('datepicker-opened', { relatedTarget: box });
                        clearSelection();
                        ShowDefault(true);
                        updateCalendarWidth();
                    });

                    $(self).trigger('datepicker-open', { relatedTarget: box });

                    return;
                }
            }

            box.slideDown(animationTime, function () {
                $(self).trigger('datepicker-opened', { relatedTarget: box });
                updateCalendarWidth();
            });
            $(self).trigger('datepicker-open', { relatedTarget: box });

            updateCalendarWidth();
        }

        function createDom() {
            var html = '<div class="date-picker-wrapper';
            if (opt.extraClass) html += ' ' + opt.extraClass + ' ';
            if (opt.singleDate) html += ' single-date ';
            if (!opt.showShortcuts) html += ' no-shortcuts ';
            if (!opt.showTopbar) html += ' no-topbar ';
            if (opt.customTopBar) html += ' custom-topbar ';
            html += '">';

            if (opt.showTopbar) {
                html += '<div class="drp_top-bar">';

                if (opt.customTopBar) {
                    if (typeof opt.customTopBar == 'function') opt.customTopBar = opt.customTopBar();
                    html += '<div class="custom-top">' + opt.customTopBar + '</div>';
                }
                else {
                    html += '<div class="normal-top">\
							<span style="color:#333">'+ lang('selected') + ' </span> <b class="start-day">...</b>';
                    if (!opt.singleDate) {
                        html += ' <span class="separator-day">' + opt.separator +
                            '</span> <b class="end-day">...</b> <i class="selected-days">(<span class="selected-days-num">3</span> ' + lang('days') + ')</i>'
                    }
                    html += '</div>';
                    html += '<div class="error-top">error</div>\
						<div class="default-top">default</div>';
                }
                html += '<div class="dp_btn">';
                html += '<input type="button" class="apply-btn disabled' + getApplyBtnClass() + '" value="' + lang('apply') + '" />';
                if (opt.showClearButton === true)
                    html += '<input type="button" class="clear-btn' + getClearBtnClass() + '" value="' + lang('clear') + '" />';
                html += '</div>';
                html += '</div>'
            }

            var _colspan = opt.showWeekNumbers ? 6 : 5;
            html += '<div class="month-wrapper">'
                + '<div class="w-table"><table class="month1" cellspacing="0" border="0" cellpadding="0"><thead><tr class="caption"><th class="prev" style="width:27px;"><span>' +
                opt.btnPrev + '</span></th><th colspan="' + _colspan + '" class="month-name"></th><th class="next" style="width:27px;">' +
                (opt.singleDate || !opt.stickyMonths ? '<span>' + opt.btnNext + '</span>' : '') + '</th></tr><tr class="week-name">'
                + getWeekHead() + '</thead><tbody></tbody></table></div>';

            if (hasMonth2()) {
                if (typeof opt.hideGap !== 'undefined' && opt.hideGap === true)
                { }
                else
                    html += '<div class="gap" style="display:none">' + getGapHTML() + '</div>';
                html += '<div class="w-table"><table class="month2' + (typeof opt.hideGap !== 'undefined' && opt.hideGap === true ? ' mg-lf' : '') +
                    '" cellspacing="0" border="0" cellpadding="0"><thead><tr class="caption"><th class="prev" style="width:27px;">' +
                    (!opt.stickyMonths ? '<span>' + opt.btnPrev + '</span>' : '') + '</th><th colspan="' + _colspan + '" class="month-name">'
                    + '</th><th class="next" style="width:27px;"><span>' +
                    opt.btnNext + '</span></th></tr><tr class="week-name">' + getWeekHead() + '</thead><tbody></tbody></table></div>'
            };

            //+'</div>'
            html += '<div style="clear:both;height:0;font-size:0;"></div>'
                + '<div class="time">'
                + '<div class="time1"></div>'
            if (!opt.singleDate) {
                html += '<div class="time2"></div>'
            };

            html += '</div>'
                + '<div style="clear:both;height:0;font-size:0;"></div>'
                + '</div>';

            html += '<div class="footer">';
            if (opt.showShortcuts) {
                html += '<div class="shortcuts"><b>' + lang('shortcuts') + '</b>';

                var data = opt.shortcuts;
                if (data) {
                    if (data['prev-days'] && data['prev-days'].length > 0) {
                        html += '&nbsp;<span class="prev-days">' + lang('past');
                        for (var i = 0; i < data['prev-days'].length; i++) {
                            var name = data['prev-days'][i];
                            name += (data['prev-days'][i] > 1) ? lang('days') : lang('day');
                            html += ' <a href="javascript:void(0);" shortcut="day,-' + data['prev-days'][i] + '">' + name + '</a>';
                        }
                        html += '</span>';
                    }

                    if (data['next-days'] && data['next-days'].length > 0) {
                        html += '&nbsp;<span class="next-days">' + lang('following');
                        for (var i = 0; i < data['next-days'].length; i++) {
                            var name = data['next-days'][i];
                            name += (data['next-days'][i] > 1) ? lang('days') : lang('day');
                            html += ' <a href="javascript:void(0);" shortcut="day,' + data['next-days'][i] + '">' + name + '</a>';
                        }
                        html += '</span>';
                    }

                    if (data['prev'] && data['prev'].length > 0) {
                        html += '&nbsp;<span class="prev-buttons">' + lang('previous');
                        for (var i = 0; i < data['prev'].length; i++) {
                            var name = lang('prev-' + data['prev'][i]);
                            html += ' <a href="javascript:void(0);" shortcut="prev,' + data['prev'][i] + '">' + name + '</a>';
                        }
                        html += '</span>';
                    }

                    if (data['next'] && data['next'].length > 0) {
                        html += '&nbsp;<span class="next-buttons">' + lang('next');
                        for (var i = 0; i < data['next'].length; i++) {
                            var name = lang('next-' + data['next'][i]);
                            html += ' <a href="javascript:void(0);" shortcut="next,' + data['next'][i] + '">' + name + '</a>';
                        }
                        html += '</span>';
                    }
                }

                if (opt.customShortcuts) {
                    for (var i = 0; i < opt.customShortcuts.length; i++) {
                        var sh = opt.customShortcuts[i];
                        html += '&nbsp;<span class="custom-shortcut"><a href="javascript:void(0);" shortcut="custom">' + sh.name + '</a></span>';
                    }
                }
                html += '</div>';
            }

            // Add Custom Values Dom
            if (opt.showCustomValues) {
                html += '<div class="customValues"><b>' + (opt.customValueLabel || lang('custom-values')) + '</b>';

                if (opt.customValues) {
                    for (var i = 0; i < opt.customValues.length; i++) {
                        var val = opt.customValues[i];
                        html += '&nbsp;<span class="custom-value"><a href="javascript:void(0);" custom="' + val.value + '">' + val.name + '</a></span>';
                    }
                }
            }

            html += '</div></div>';

            return $(html);
        }

        function getApplyBtnClass() {
            var klass = ''
            if (opt.autoClose === true) {
                klass += ' hide';
            }
            if (opt.applyBtnClass !== '') {
                klass += ' ' + opt.applyBtnClass;
            }
            return klass;
        }

        function getClearBtnClass() {
            var klass = ''
            if (opt.clearBtnClass !== '')
                klass += ' ' + opt.clearBtnClass;
            return klass;
        }

        function getWeekHead() {
            var prepend = opt.showWeekNumbers ? '<th>' + lang('week-number') + '</th>' : '';

            if (opt.startOfWeek == 'monday') {
                return prepend + '<th>' + locale._weekdaysShort[1] + '</th>' +
				'<th>' + locale._weekdaysShort[2] + '</th>' +
                '<th>' + locale._weekdaysShort[3] + '</th>' +
                '<th>' + locale._weekdaysShort[4] + '</th>' +
                '<th>' + locale._weekdaysShort[5] + '</th>' +
                '<th>' + locale._weekdaysShort[6] + '</th>' +
                '<th>' + locale._weekdaysShort[0] + '</th>';
            }
            else {
                return prepend + '<th>' + locale._weekdaysShort[0] + '</th>' +
					'<th>' + locale._weekdaysShort[1] + '</th>' +
					'<th>' + locale._weekdaysShort[2] + '</th>' +
					'<th>' + locale._weekdaysShort[3] + '</th>' +
					'<th>' + locale._weekdaysShort[4] + '</th>' +
					'<th>' + locale._weekdaysShort[5] + '</th>' +
					'<th>' + locale._weekdaysShort[6] + '</th>';
            }
        }

        function getHeadMonthAndYear(year) {
            year = year || moment().year();
            //console.log('year : ', year);

            var selectHtml = '';
            if (opt.showDropdownMonth === true) {
                //console.log(locale);
                var isShowShort = opt.showMonthShort;

                selectHtml += '<select class="sel-month' + (isShowShort === true ? ' half' : '') + '">';
                var disab = ' disabled="disabled"';
                var dd;
                for (var i = 0; i < 12; i++) {
                    dd = moment({ year: year, month: i, day: 1 });
                    if (opt.startDate && opt.endDate) {
                        selectHtml += '<option value="' + i + '"' +
                            ((compare_day(dd, opt.startDate) < 0 || compare_day(dd, opt.endDate) > 0) ? disab : '') + '>'
                            + (isShowShort ? locale.monthsShort(dd) : locale.months(dd));
                    }
                    else if (opt.startDate)
                        selectHtml += '<option value="' + i + '"' + (compare_day(dd, opt.startDate) < 0 ? disab : '') + '>'
                            + (isShowShort ? locale.monthsShort(dd) : locale.months(dd));
                    else if (opt.endDate)
                        selectHtml += '<option value="' + i + '"' + (compare_day(dd, opt.endDate) > 0 ? disab : '') + '>'
                            + (isShowShort ? locale.monthsShort(dd) : locale.months(dd));
                    else
                        selectHtml += '<option value="' + i + '"' + '>'
                            + (isShowShort ? locale.monthsShort(dd) : locale.months(dd));
                    + '</option>';
                }
                selectHtml += '</select>';
            }
            else
                selectHtml += '<span class="lb-month"></span>';

            if (opt.showDropdownYear === true) {
                //console.log(locale);
                selectHtml += '<select class="sel-year' + (isShowShort === true ? ' half' : '') + '">';
                for (var i = -25; i < 25; i++) {
                    selectHtml += '<option value="' + (year + i) + '"' + (i === 0 ? ' selected="selected"' : '')
                        + '>' + (year + i) + '</option>';
                }
                selectHtml += '</select>';
            }
            else
                selectHtml += '<span class="lb-year"></span>';

            return selectHtml;
        }

        function isMonthOutOfBounds(month) {
            var month = moment(month);
            if (opt.startDate && month.endOf('month').isBefore(ParseDate(opt.startDate))) {
                return true;
            }
            if (opt.endDate && month.startOf('month').isAfter(ParseDate(opt.endDate))) {
                return true;
            }
            return false;
        }

        function getGapHTML() {
            var html = ['<div class="gap-top-mask"></div><div class="gap-bottom-mask"></div><div class="gap-lines">'];
            for (var i = 0; i < 20; i++) {
                html.push('<div class="gap-line">\
					<div class="gap-1"></div>\
					<div class="gap-2"></div>\
					<div class="gap-3"></div>\
				</div>');
            }
            html.push('</div>');
            return html.join('');
        }

        function hasMonth2() {
            return (!opt.singleDate && !opt.singleMonth);
        }

        function attributesCallbacks(initialObject, callbacksArray, today) {
            var resultObject = jQuery.extend(true, {}, initialObject);

            jQuery.each(callbacksArray, function (cbAttrIndex, cbAttr) {
                var addAttributes = cbAttr(today);
                for (var attr in addAttributes) {
                    if (resultObject.hasOwnProperty(attr)) {
                        resultObject[attr] += addAttributes[attr];
                    } else {
                        resultObject[attr] = addAttributes[attr];
                    }
                }
            });

            var attrString = '';

            for (var attr in resultObject) {
                if (resultObject.hasOwnProperty(attr)) {
                    attrString += attr + '="' + replaceAllWhiteSpace(resultObject[attr], ' ', true) + '" ';
                }
            }
            //console.log('attrString : ', attrString);
            return attrString;
        }

        function daysFrom1970(t) {
            return Math.floor(toLocalTimestamp(t) / 86400000);
        }

        function toLocalTimestamp(t) {
            if (moment.isMoment(t)) t = t.toDate().getTime();
            if (typeof t == 'object' && t.getTime) t = t.getTime();
            if (typeof t == 'string' && !t.match(/\d{13}/)) t = ParseDate(t).getTime();
            t = parseInt(t, 10) - new Date().getTimezoneOffset() * 60 * 1000;
            return t;
        }

        function replaceAllWhiteSpace(stringToReplace, replaceWith, useTrim) {
            if (useTrim)
                return $.trim(stringToReplace).replace(/ {2,}/g, replaceWith).replace(/\s/g, replaceWith);
            else
                return stringToReplace.replace(/ {2,}/g, replaceWith).replace(/\s/g, replaceWith);
        }

        function createMonthHTML(d) {
            var days = [];
            d.setDate(1);
            var lastMonth = new Date(d.getTime() - 86400000);
            var now = new Date();

            var dayOfWeek = d.getDay();
            if ((dayOfWeek == 0) && (opt.startOfWeek == 'monday')) {
                // add one week
                dayOfWeek = 7;
            }

            if (dayOfWeek > 0) {
                for (var i = dayOfWeek; i > 0; i--) {
                    var day = new Date(d.getTime() - 86400000 * i);
                    var valid = isValidTime(day.getTime());
                    if (opt.startDate && compare_day(day, opt.startDate) < 0) valid = false;
                    if (opt.endDate && compare_day(day, opt.endDate) > 0) valid = false;
                    days.push({
                        date: day,
                        type: 'lastMonth',
                        day: day.getDate(),
                        time: day.getTime(),
                        valid: valid
                    });
                }
            }
            var toMonth = d.getMonth();
            for (var i = 0; i < 40; i++) {
                var today = moment(d).add(i, 'days').toDate();
                var valid = isValidTime(today.getTime());
                if (opt.startDate && compare_day(today, opt.startDate) < 0) valid = false;
                if (opt.endDate && compare_day(today, opt.endDate) > 0) valid = false;
                days.push({
                    date: today,
                    type: today.getMonth() == toMonth ? 'toMonth' : 'nextMonth',
                    day: today.getDate(),
                    time: today.getTime(),
                    valid: valid
                });
            }
            var html = [];
            for (var week = 0; week < 6; week++) {
                if (days[week * 7].type == 'nextMonth') break;
                html.push('<tr>');
                for (var day = 0; day < 7; day++) {
                    var _day = (opt.startOfWeek == 'monday') ? day + 1 : day;
                    var today = days[week * 7 + _day];
                    var highlightToday = moment(today.time).format('L') == moment(now).format('L');
                    today.extraClass = '';
                    today.tooltip = '';
                    today.highlightToday = highlightToday;
                    if (today.valid && opt.beforeShowDayFunc && typeof opt.beforeShowDayFunc == 'function') {
                        var _r = opt.beforeShowDayFunc(moment(today.time).toDate(), opt.dataAjax);
                        today.valid = _r[0];
                        today.extraClass = _r[1] || '';
                        today.tooltip = _r[2] || '';

                        if (_r.length > 3)
                            today.data = _r.splice(3);

                        if (today.tooltip != '') today.extraClass += ' has-tooltip ';
                    }

                    var todayDivAttr = {
                        time: today.time,
                        'data-tooltip': today.tooltip,
                        'class': 'day ' + today.type + ' ' + today.extraClass + ' ' + (today.valid ? 'valid' : 'invalid') + ' ' + (highlightToday ? 'real-today' : '')
                    };

                    var cltd = ((today.type === 'lastMonth' || today.type === 'nextMonth') ? 'dp-other-month ' : '');

                    if (cltd.length > 0)
                        cltd += (typeof opt.showOtherMonth !== 'undefined' && opt.showOtherMonth === true ? '' : ' dp-disabled ');

                    var tdAttr = {};
                    if (replaceAllWhiteSpace(cltd, ' ', true).length > 0)
                        tdAttr['class'] = cltd;

                    if (day == 0 && opt.showWeekNumbers) {
                        html.push('<td><div class="week-number" data-start-time="' + today.time + '">' + opt.getWeekNumber(today.date) + '</div></td>');
                    }

                    html.push('<td ' + attributesCallbacks(tdAttr, opt.dayTdAttrs, today) + '><div ' + attributesCallbacks(todayDivAttr, opt.dayDivAttrs, today) + '>' + showDayHTML(today) + '</div></td>');
                }
                html.push('</tr>');
            }
            return html.join('');
        }

        function showDayHTML(data) {
            if (opt.showDateFilterFunc && typeof opt.showDateFilterFunc == 'function') return opt.showDateFilterFunc(data);
            return data.day;
        }

        function getLanguages() {
            if (opt.language == 'auto') {
                var language = navigator.language ? navigator.language : navigator.browserLanguage;
                if (!language) return $.dateRangePickerLanguageTexts['default'];
                var language = language.toLowerCase();
                for (var key in $.dateRangePickerLanguageTexts) {
                    if (language.indexOf(key) != -1) {
                        return $.dateRangePickerLanguageTexts[key];
                    }
                }
                return $.dateRangePickerLanguageTexts['default'];
            }
            else if (opt.language && opt.language in $.dateRangePickerLanguageTexts) {
                return $.dateRangePickerLanguageTexts[opt.language];
            }
            else {
                var languageOpt = opt.language;
                if (typeof opt.language !== 'undefined') {
                    for (var key in $.dateRangePickerLanguageTexts) {
                        //console.log('key : ', key, ',languageOpt : ', languageOpt);

                        if (languageOpt.indexOf(key) != -1) {
                            return $.dateRangePickerLanguageTexts[key];
                        }
                    }
                }
                return $.dateRangePickerLanguageTexts['default'];
            }
        }

        function difference(dateFrom, dateTo) {
            if (dateFrom === null || dateFrom === undefined
                || dateFrom === false || dateFrom.toString() === 'Invalid Date')
                return {};
            if (Object.prototype.toString.call(dateFrom) !== "[object Date]")
                return {};

            var dt1 = dateTo || new Date();
            /*
             * setup 'empty' return object
             */
            var ret = { Days: 0, Months: 0, Years: 0, Hours: 0, Minutes: 0, Seconds: 0 };

            /*
             * If the dates are equal, return the 'empty' object
             */
            if (dateFrom == dt1) return ret;

            if (dateFrom > dt1) {
                var dtmp = dt1;
                dt1 = dateFrom;
                dateFrom = dtmp;
            }

            /*
             * First get the number of full years
             */

            var year1 = dateFrom.getFullYear();
            var year2 = dt1.getFullYear();

            var month1 = dateFrom.getMonth();
            var month2 = dt1.getMonth();

            var day1 = dateFrom.getDate();
            var day2 = dt1.getDate();

            var hour1 = dateFrom.getHours();
            var hour2 = dt1.getHours();

            var minute1 = dateFrom.getMinutes();
            var minute2 = dt1.getMinutes();

            var second1 = dateFrom.getSeconds();
            var second2 = dt1.getSeconds();

            /*
             * Set initial values bearing in mind the months or days may be negative
             */

            ret['Seconds'] = second2 - second1;

            var hasBorrow = false;
            if (ret['Seconds'] < 0) {
                hasBorrow = true;
                ret['Seconds'] = second2 + 60 - second1;
            }

            if (hasBorrow === true)
                minute1 += 1;

            ret['Minutes'] = minute2 - minute1;

            hasBorrow = false;
            if (ret['Minutes'] < 0) {
                hasBorrow = true;
                ret['Minutes'] = minute2 + 60 - minute1;
            }

            if (hasBorrow === true)
                hour1 += 1;

            ret['Hours'] = hour2 - hour1;

            hasBorrow = false;
            if (ret['Hours'] < 0) {
                hasBorrow = true;
                ret['Hours'] = hour2 + 24 - hour1;
            }

            if (hasBorrow === true)
                day1 += 1;

            ret['Days'] = day2 - day1;

            ret['Years'] = year2 - year1;
            ret['Months'] = month2 - month1;

            /*
             * First if the day difference is negative
             * eg dt2 = 13 oct, dt1 = 25 sept
             */

            if (ret['Days'] < 0) {
                /*
                 * Use temporary dates to get the number of days remaining in the month
                 */
                var dtmp1 = new Date(dt1.getFullYear(), dt1.getMonth() + 1, 1, 0, 0, -1);

                var numDays = dtmp1.getDate();

                ret['Months'] -= 1;
                ret['Days'] += numDays;

            }

            /*
             * Now if the month difference is negative
             */
            if (ret['Months'] < 0) {
                ret['Months'] += 12;
                ret['Years'] -= 1;
            }

            return ret;
        }

        /**
         * translate language string
         */
        function lang(t) {
            var _t = t.toLowerCase();
            var re = (t in langs) ? langs[t] : (_t in langs) ? langs[_t] : null;
            var defaultLanguage = $.dateRangePickerLanguageTexts['default'];
            if (re == null) re = (t in defaultLanguage) ? defaultLanguage[t] : (_t in defaultLanguage) ? defaultLanguage[_t] : '';
            return re;
        }

        // Return the date picker wrapper element
        function getDatePicker() {
            return box;
        }

        var alphabet = {
            a: /[\u0061\u24D0\uFF41\u1E9A\u00E0\u00E1\u00E2\u1EA7\u1EA5\u1EAB\u1EA9\u00E3\u0101\u0103\u1EB1\u1EAF\u1EB5\u1EB3\u0227\u01E1\u00E4\u01DF\u1EA3\u00E5\u01FB\u01CE\u0201\u0203\u1EA1\u1EAD\u1EB7\u1E01\u0105\u2C65\u0250]/ig,
            aa: /[\uA733]/ig,
            ae: /[\u00E6\u01FD\u01E3]/ig,
            ao: /[\uA735]/ig,
            au: /[\uA737]/ig,
            av: /[\uA739\uA73B]/ig,
            ay: /[\uA73D]/ig,
            b: /[\u0062\u24D1\uFF42\u1E03\u1E05\u1E07\u0180\u0183\u0253]/ig,
            c: /[\u0063\u24D2\uFF43\u0107\u0109\u010B\u010D\u00E7\u1E09\u0188\u023C\uA73F\u2184]/ig,
            d: /[\u0064\u24D3\uFF44\u1E0B\u010F\u1E0D\u1E11\u1E13\u1E0F\u0111\u018C\u0256\u0257\uA77A]/ig,
            dz: /[\u01F3\u01C6]/ig,
            e: /[\u0065\u24D4\uFF45\u00E8\u00E9\u00EA\u1EC1\u1EBF\u1EC5\u1EC3\u1EBD\u0113\u1E15\u1E17\u0115\u0117\u00EB\u1EBB\u011B\u0205\u0207\u1EB9\u1EC7\u0229\u1E1D\u0119\u1E19\u1E1B\u0247\u025B\u01DD]/ig,
            f: /[\u0066\u24D5\uFF46\u1E1F\u0192\uA77C]/ig,
            g: /[\u0067\u24D6\uFF47\u01F5\u011D\u1E21\u011F\u0121\u01E7\u0123\u01E5\u0260\uA7A1\u1D79\uA77F]/ig,
            h: /[\u0068\u24D7\uFF48\u0125\u1E23\u1E27\u021F\u1E25\u1E29\u1E2B\u1E96\u0127\u2C68\u2C76\u0265]/ig,
            hv: /[\u0195]/ig,
            i: /[\u0069\u24D8\uFF49\u00EC\u00ED\u00EE\u0129\u012B\u012D\u00EF\u1E2F\u1EC9\u01D0\u0209\u020B\u1ECB\u012F\u1E2D\u0268\u0131]/ig,
            j: /[\u006A\u24D9\uFF4A\u0135\u01F0\u0249]/ig,
            k: /[\u006B\u24DA\uFF4B\u1E31\u01E9\u1E33\u0137\u1E35\u0199\u2C6A\uA741\uA743\uA745\uA7A3]/ig,
            l: /[\u006C\u24DB\uFF4C\u0140\u013A\u013E\u1E37\u1E39\u013C\u1E3D\u1E3B\u017F\u0142\u019A\u026B\u2C61\uA749\uA781\uA747]/ig,
            lj: /[\u01C9]/ig,
            m: /[\u006D\u24DC\uFF4D\u1E3F\u1E41\u1E43\u0271\u026F]/ig,
            n: /[\u006E\u24DD\uFF4E\u01F9\u0144\u00F1\u1E45\u0148\u1E47\u0146\u1E4B\u1E49\u019E\u0272\u0149\uA791\uA7A5]/ig,
            nj: /[\u01CC]/ig,
            o: /[\xF4\u006F\u24DE\uFF4F\u00F2\u00F3\u00F4\u1ED3\u1ED1\u1ED7\u1ED5\u00F5\u1E4D\u022D\u1E4F\u014D\u1E51\u1E53\u014F\u022F\u0231\u00F6\u022B\u1ECF\u0151\u01D2\u020D\u020F\u01A1\u1EDD\u1EDB\u1EE1\u1EDF\u1EE3\u1ECD\u1ED9\u01EB\u01ED\u00F8\u01FF\u0254\uA74B\uA74D\u0275]/ig,
            oi: /[\u01A3]/ig,
            ou: /[\u0223]/ig,
            oo: /[\uA74F]/ig,
            oe: /[\u0153]/ig,
            p: /[\u0070\u24DF\uFF50\u1E55\u1E57\u01A5\u1D7D\uA751\uA753\uA755]/ig,
            q: /[\u0071\u24E0\uFF51\u024B\uA757\uA759]/ig,
            r: /[\u0072\u24E1\uFF52\u0155\u1E59\u0159\u0211\u0213\u1E5B\u1E5D\u0157\u1E5F\u024D\u027D\uA75B\uA7A7\uA783]/ig,
            s: /[\u0073\u24E2\uFF53\u015B\u1E65\u015D\u1E61\u0161\u1E67\u1E63\u1E69\u0219\u015F\u023F\uA7A9\uA785\u1E9B]/ig,
            ss: /[\u00DF\u1E9E]/ig,
            t: /[\u0074\u24E3\uFF54\u1E6B\u1E97\u0165\u1E6D\u021B\u0163\u1E71\u1E6F\u0167\u01AD\u0288\u2C66\uA787]/ig,
            tz: /[\uA729]/ig,
            u: /[\u0075\u24E4\uFF55\u00F9\u00FA\u00FB\u0169\u1E79\u016B\u1E7B\u016D\u00FC\u01DC\u01D8\u01D6\u01DA\u1EE7\u016F\u0171\u01D4\u0215\u0217\u01B0\u1EEB\u1EE9\u1EEF\u1EED\u1EF1\u1EE5\u1E73\u0173\u1E77\u1E75\u0289]/ig,
            v: /[\u0076\u24E5\uFF56\u1E7D\u1E7F\u028B\uA75F\u028C]/ig,
            vy: /[\uA761]/ig,
            w: /[\u0077\u24E6\uFF57\u1E81\u1E83\u0175\u1E87\u1E85\u1E98\u1E89\u2C73]/ig,
            x: /[\u0078\u24E7\uFF58\u1E8B\u1E8D]/ig,
            y: /[\u0079\u24E8\uFF59\u1EF3\u00FD\u0177\u1EF9\u0233\u1E8F\u00FF\u1EF7\u1E99\u1EF5\u01B4\u024F\u1EFF]/ig,
            z: /[\u007A\u24E9\uFF5A\u017A\u1E91\u017C\u017E\u1E93\u1E95\u01B6\u0225\u0240\u2C6C\uA763]/ig,
            '': /[\u0309\u0323\u0300\u0301\u0302\u0303\u0308]/ig
        };
        function replaceDiacritics(str) {
            for (var letter in alphabet) {
                str = str.replace(alphabet[letter], letter);
            }
            return str;
        };
    };

}));