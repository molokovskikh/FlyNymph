(function ($) {
    var jsonSupport = function () {
        if (typeof window.JSON === 'undefined') {
            eval("var JSON;if(!JSON){JSON={}}(function(){function f(a){return a<10?\"0\"+a:a}function quote(a){escapable.lastIndex=0;return escapable.test(a)?'\"'+a.replace(escapable,function(a){var b=meta[a];return typeof b===\"string\"?b:\"\\\\u\"+(\"0000\"+a.charCodeAt(0).toString(16)).slice(-4)})+'\"':'\"'+a+'\"'}function str(a,b){var c,d,e,f,g=gap,h,i=b[a];if(i&&typeof i===\"object\"&&typeof i.toJSON===\"function\"){i=i.toJSON(a)}if(typeof rep===\"function\"){i=rep.call(b,a,i)}switch(typeof i){case\"string\":return quote(i);case\"number\":return isFinite(i)?String(i):\"null\";case\"boolean\":case\"null\":return String(i);case\"object\":if(!i){return\"null\"}gap+=indent;h=[];if(Object.prototype.toString.apply(i)===\"[object Array]\"){f=i.length;for(c=0;c<f;c+=1){h[c]=str(c,i)||\"null\"}e=h.length===0?\"[]\":gap?\"[\\n\"+gap+h.join(\",\\n\"+gap)+\"\\n\"+g+\"]\":\"[\"+h.join(\",\")+\"]\";gap=g;return e}if(rep&&typeof rep===\"object\"){f=rep.length;for(c=0;c<f;c+=1){if(typeof rep[c]===\"string\"){d=rep[c];e=str(d,i);if(e){h.push(quote(d)+(gap?\": \":\":\")+e)}}}}else{for(d in i){if(Object.prototype.hasOwnProperty.call(i,d)){e=str(d,i);if(e){h.push(quote(d)+(gap?\": \":\":\")+e)}}}}e=h.length===0?\"{}\":gap?\"{\\n\"+gap+h.join(\",\\n\"+gap)+\"\\n\"+g+\"}\":\"{\"+h.join(\",\")+\"}\";gap=g;return e}}\"use strict\";if(typeof Date.prototype.toJSON!==\"function\"){Date.prototype.toJSON=function(a){return isFinite(this.valueOf())?this.getUTCFullYear()+\"-\"+f(this.getUTCMonth()+1)+\"-\"+f(this.getUTCDate())+\"T\"+f(this.getUTCHours())+\":\"+f(this.getUTCMinutes())+\":\"+f(this.getUTCSeconds())+\"Z\":null};String.prototype.toJSON=Number.prototype.toJSON=Boolean.prototype.toJSON=function(a){return this.valueOf()}}var cx=/[\\u0000\\u00ad\\u0600-\\u0604\\u070f\\u17b4\\u17b5\\u200c-\\u200f\\u2028-\\u202f\\u2060-\\u206f\\ufeff\\ufff0-\\uffff]/g,escapable=/[\\\\\\\"\\x00-\\x1f\\x7f-\\x9f\\u00ad\\u0600-\\u0604\\u070f\\u17b4\\u17b5\\u200c-\\u200f\\u2028-\\u202f\\u2060-\\u206f\\ufeff\\ufff0-\\uffff]/g,gap,indent,meta={\"\\b\":\"\\\\b\",\"	\":\"\\\\t\",\"\\n\":\"\\\\n\",\"\\f\":\"\\\\f\",\"\\r\":\"\\\\r\",'\"':'\\\\\"',\"\\\\\":\"\\\\\\\\\"},rep;if(typeof JSON.stringify!==\"function\"){JSON.stringify=function(a,b,c){var d;gap=\"\";indent=\"\";if(typeof c===\"number\"){for(d=0;d<c;d+=1){indent+=\" \"}}else if(typeof c===\"string\"){indent=c}rep=b;if(b&&typeof b!==\"function\"&&(typeof b!==\"object\"||typeof b.length!==\"number\")){throw new Error(\"JSON.stringify\")}return str(\"\",{\"\":a})}}if(typeof JSON.parse!==\"function\"){JSON.parse=function(text,reviver){function walk(a,b){var c,d,e=a[b];if(e&&typeof e===\"object\"){for(c in e){if(Object.prototype.hasOwnProperty.call(e,c)){d=walk(e,c);if(d!==undefined){e[c]=d}else{delete e[c]}}}}return reviver.call(a,b,e)}var j;text=String(text);cx.lastIndex=0;if(cx.test(text)){text=text.replace(cx,function(a){return\"\\\\u\"+(\"0000\"+a.charCodeAt(0).toString(16)).slice(-4)})}if(/^[\\],:{}\\s]*$/.test(text.replace(/\\\\(?:[\"\\\\\\/bfnrt]|u[0-9a-fA-F]{4})/g,\"@\").replace(/\"[^\"\\\\\\n\\r]*\"|true|false|null|-?\\d+(?:\\.\\d*)?(?:[eE][+\\-]?\\d+)?/g,\"]\").replace(/(?:^|:|,)(?:\\s*\\[)+/g,\"\"))){j=eval(\"(\"+text+\")\");return typeof reviver===\"function\"?walk({\"\":j},\"\"):j}throw new SyntaxError(\"JSON.parse\")}}})()");
            window.JSON = JSON;
        }
    }

    var Objects = [];
    var initialize = false;
    var use_cookie = false;
    var use_ls = false;
    var use_ss = false;
    var updated = false;



    var testCookie = function () {
        var res = false;
        if ($.cookie) {
            $.cookie('test', 'testdata');
            res = $.cookie('test') == 'testdata';
            $.cookie('test', null);
        }
        return res;
    }


    var Exists = function (name) {
        var res = false;
        for (var o in Objects)
            if (Objects[o].name == name) {
                return true;
            }
        return res;
    }

    //Синхронизация с хранилищами на клиенте
    var syncWrite = function () {
        for (var o in Objects) {
            if (use_cookie) {
                $.cookie(Objects[o].name, JSON.stringify(Objects[o].data));
            }
            if (use_ls && localStorage) {
                localStorage.setItem(Objects[o].name, JSON.stringify(Objects[o].data))
            }
            if (use_ss && sessionStorage) {
                sessionStorage.setItem(Objects[o].name, JSON.stringify(Objects[o].data))
            }
        }
    }

    var syncRead = function () {
        if (!updated) {
            if (use_ls&&localStorage) {
                for (var f in localStorage) {
                    var val_f = eval('localStorage.getItem("' + f + '")');
                    if (!Exists(f)) {
                        try {
                            if (typeof val_f == 'string') {
                                jsonSupport();
                                Objects.push({ name: f, data: JSON.parse(val_f) });
                            }
                            else
                                Objects.push({ name: f, data: val_f });
                        }
                        catch (e) {
                            Objects.push({ name: f, data: val_f });
                        }
                    }
                }
            }

            if (use_ss && sessionStorage) {
                for (var f in sessionStorage) {
                    var val_f = eval('sessionStorage.getItem("' + f + '")');
                    if (!Exists(f)) {
                        try {
                            if (typeof val_f == 'string') {
                                jsonSupport();
                                Objects.push({ name: f, data: JSON.parse(val_f) });
                            }
                            else
                                Objects.push({ name: f, data: val_f });
                        }
                        catch (e) {
                            Objects.push({ name: f, data: val_f });
                        }
                    }
                }
            }

            if (use_cookie) {
                var cookies = document.cookie.split('; ');
                for (var i = 0, parts; (parts = cookies[i] && cookies[i].split('=')); i++) {
                    var key = decodeURIComponent(parts.shift().replace(/\+/g, ' '))
                    var val = $.cookie(key);
                    if (!Exists(key)) {
                        try {
                            if (typeof val == 'string') {
                                jsonSupport();
                                Objects.push({ name: f, data: JSON.parse(val) });
                            }
                            else
                                Objects.push({ name: f, data: val });
                        }
                        catch (e) {
                            Objects.push({ name: key, data: val });
                        }
                    }
                }
            }

            updated = true;
        }
    }




    //Инициализация
    var init = function () {
        //Подгрузка JSON, если не поддерживается клиентом на нативном уровне        
        if (!initialize) {
            if (!Objects) Objects = [];
            use_ls = typeof window.localStorage != 'undefined';
            use_ss = typeof window.sessionStorage != 'undefined';
            use_cookie = testCookie();
            initialize = true;
        }
    }

    //Методы над хранилищем
    var methods =
             {
                 add: function (name, obj) {
                     jsonSupport();
                     if (use_cookie)
                         $.cookie(name, JSON.stringify(obj));
                     if (use_ls&&localStorage)
                         localStorage.setItem(name, JSON.stringify(obj));
                     if (use_ss && sessionStorage)
                         sessionStorage.setItem(name, JSON.stringify(obj));
                     Objects.push({ name: name, data: obj });
                     return name;
                 },
                 remove: function (name) {
                     if (use_cookie)
                         $.cookie(name, null);
                     if (use_ls&&localStorage)
                         if (eval('localStorage.' + name))
                             localStorage.removeItem(name);
                     if (use_ss && sessionStorage)
                         if (eval('sessionStorage.' + name))
                             sessionStorage.removeItem(name);

                     for (var o in Objects)
                         if (Objects[o].name == name) {
                             Objects.splice(o, 1);
                             return name;
                         }
                 },
                 set: function (name, obj) {
                     if (!Exists(name)) return methods.add(name, obj);
                     jsonSupport();
                     if (use_cookie)
                         $.cookie(name, JSON.stringify(obj));
                     if (use_ls&&localStorage)
                         localStorage.setItem(name, JSON.stringify(obj));
                     if (use_ss && sessionStorage)
                         sessionStorage.setItem(name, JSON.stringify(obj));

                     for (var o in Objects)
                         if (Objects[o].name == name) {
                             Objects[o].data = obj;
                             return name;
                         }
                 },
                 get: function (name) {
                     var res = null;
                     //syncRead();                     
                     for (var o in Objects)
                         if (Objects[o].name == name) {
                             return Objects[o].data;
                         }
                     return res;
                 },
                 clear: function () {
                     //  syncRead();                   
                     for (var o in Objects) {
                         if (use_cookie) {
                             if ($.cookie(Objects[o].name))
                                 $.cookie(Objects[o].name, null);
                         }
                         if (use_ls&&localStorage) {
                             if (eval('localStorage.' + Objects[o].name))
                                 localStorage.removeItem(Objects[o].name);
                         }
                         if (use_ss && sessionStorage) {
                             if (eval('sessionStorage.' + Objects[o].name))
                                 sessionStorage.removeItem(Objects[o].name);
                         }
                     }
                     Objects.length = 0;
                 },
                 support: function () {
                     return { ls: use_ls, ss: use_ss, cookie: use_cookie };
                 }
             };
    $.Storage =
    $.fn.Storage =
        function (method, options, options2) {
            var res = null;
            init();
            if (method.toLowerCase() == 'support') {
                return methods[method.toLowerCase()].apply(this, []);
            }
            //Обработка опций           
            var set_option_in_args = false;
            if (arguments.length == 1) {
                if (options)
                    if (options.use_ls || options.use_ss || options.use_cookie) {
                        if (options.use_ls == null) options.use_ls = false;
                        if (options.use_ss == null) options.use_ss = false;
                        if (options.use_cookie == null) options.use_cookie = false;

                        if (options.use_ls === false)
                            if (use_ls === true) use_ls = options.use_ls;

                        if (options.use_ss === false)
                            if (use_ss === true) use_ss = options.use_ss;

                        if (options.use_cookie === false)
                            if (use_cookie === true) use_cookie = options.use_cookie;
                        set_option_in_args = true;
                    }
            }

            if (arguments.length == 3) {
                if (!options2)
                    if ($.StorageOptions) options2 = $.StorageOptions();

                if (options2)
                    if (options2.use_ls || options2.use_ss || options2.use_cookie) {
                        if (options2.use_ls == null) options2.use_ls = false;
                        if (options2.use_ss == null) options2.use_ss = false;
                        if (options2.use_cookie == null) options2.use_cookie = false;
                        if (options2.use_ls === false)
                            if (use_ls === true) use_ls = options2.use_ls;

                        if (options2.use_ss === false)
                            if (use_ss === true) use_ss = options2.use_ss;

                        if (options2.use_cookie === false)
                            if (use_cookie === true) use_cookie = options2.use_cookie;
                        set_option_in_args = true;
                    }
            }

            if ($.StorageOptions && (!set_option_in_args)) {
                if (typeof $.StorageOptions === 'function')
                    options = $.StorageOptions();
                else options = $.StorageOptions;
                if (options)
                    if (options.use_ls || options.use_ss || options.use_cookie) {
                        if (options.use_ls == null) options.use_ls = false;
                        if (options.use_ss == null) options.use_ss = false;
                        if (options.use_cookie == null) options.use_cookie = false;
                        if (options.use_ls === false)
                            if (use_ls === true) use_ls = options.use_ls;

                        if (options.use_ss === false)
                            if (use_ss === true) use_ss = options.use_ss;

                        if (options.use_cookie === false)
                            if (use_cookie === true) use_cookie = options.use_cookie;
                    }
            }
            //Обработка опций
            syncRead();

            var args = Array.prototype.slice.call(arguments, 1);

            if (method && methods[method.toLowerCase()]) {


                if (arguments.length < 2)
                    if (method.toLowerCase() == 'add' || method.toLowerCase() == 'set') {
                        $.error('Неправильно указанно количество параметров в методе "' + method + '" (jQuery.Storage)');
                    }
                if (method.toLowerCase() == 'remove') {
                    syncWrite(methods[method.toLowerCase()].apply(this, args));
                }
                else {
                    if (method.toLowerCase() == 'clear') {
                        methods[method.toLowerCase()].apply(this, args);
                    }
                    else
                        res = methods[method.toLowerCase()].apply(this, args);
                }
            }
            else {
                if (!method) {
                    return Objects;
                }
                else {
                    if (args.length == 1) { //Обмена содержимого двух объектов местами


                        if (typeof args[0] === 'string') {
                            var o = methods['get'].apply(this, [method]);
                            var o2 = methods['get'].apply(this, args);
                            if (o && o2) {
                                methods['set'].apply(this, [method, o2]);
                                methods['set'].apply(this, [args[0], o]);
                                return methods['get'].apply(this, [method]);
                                ;
                            }
                        }
                        else //Если второй параметр не строка, то обработаем как set метод
                        {

                            return methods[Exists(method) ? 'set' : 'add'].apply(this, [method, args[0]]);
                        }
                    }
                    //Возрат объекта
                    if (args.length == 0) {
                        var tg = methods['get'].apply(this, [method]);
                        if (tg) return tg;
                    }
                    $.error('Метод "' + method + '" не реализован в плагине jQuery.Storage');
                }
            }
            return res;
        };


})(jQuery);