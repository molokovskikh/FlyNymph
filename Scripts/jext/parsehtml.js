/*
 @Author: Sergey Molokovskikh
 @Description: parse string html to object as struct HTML
*/

(function ($) {
    //Распарсить текст HTML по объектам
    $.get_object_html = function (html) {
        var head;
        var body;
        var links = [];
        var styles = [];
        var scripts = [];
        var evalCode;
        var cuts;
        var new_html;
        //Получаем содержимое тега по имени
        var OneNodeByTag = function (html, tag, no_tag, _cuts, _offset) {
            if (html && tag && html.length > 0 && tag.length > 0) {
                var res;
                var html_reverse = html.split("").reverse().join("");
                var tag_reverse = tag.split("").reverse().join("");
                var _tag_b = new RegExp('\\s*<\\s*' + tag + '[^\\>]*>\\s*').exec(html);
                var _tag_e = new RegExp('>\\s*' + tag_reverse + '\\s*\\/*\\s*<').exec(html_reverse);
                if (_tag_e && _tag_e.index >= 0) _tag_e = html.length - _tag_e.index;
                if (_tag_b && _tag_b.index >= 0) _tag_b = _tag_b.index;
                if (typeof _tag_e == 'number' && typeof _tag_b == 'number') {
                    res = html.substring(_tag_b, _tag_e);
                    //if (tag == 'script') {
                    //    debugger
                    //}
                    if (_cuts)// && !no_tag)
                    {
                        if (tag != 'body' && tag != 'head')
                            _cuts.push({
                                b: ((_offset && _offset.b && _offset.e) ? _offset.b : 0) + _tag_b
                              , e: ((_offset && _offset.b && _offset.e) ? _offset.b : 0) + _tag_e
                              , c: html.substring(_tag_b, _tag_e)
                            });
                    }

                    if (no_tag) {
                        _tag_b += />/.exec(res).index + 1;
                        _tag_e -= /</.exec(res.split("").reverse().join("")).index + 1;
                        res = html.substring(_tag_b, _tag_e);
                        if (tag == 'body' || tag == 'head')
                            _cuts.push({
                                b: ((_offset && _offset.b && _offset.e) ? _offset.b : 0) + _tag_b, e: ((_offset && _offset.b && _offset.e) ? _offset.b : 0) + _tag_e
                            , c: html.substring(_tag_b, _tag_e)
                            });
                    }
                }
                return res;
            }
        }

        var _tag_head;
        var _tag_body;

        _tag_head = [];
        head = OneNodeByTag(html, 'head', true, _tag_head);
        _tag_head = _tag_head.pop();

        _tag_body = [];
        body = OneNodeByTag(html, 'body', true, _tag_body);
        _tag_body = _tag_body.pop();

        cuts = []; //Массив для запоминания блоков вырезки

        //Если тэг head присутсвуют
        if (head) {

            //Поиск ссылок на стили 
            var _l;
            var _lexpression = /\<link\s*([^\>'"]*)(rel\=['|"]*([^\>'"]*)['|"]*)(href\=['|"]*([^\>'"]*)['|"]*)([^\>]*)\/*\>/gmi;
            while ((_l = _lexpression.exec(head)) != null) {
                cuts.push({
                    b: _tag_head.b + _l.index, e: _tag_head.b + _l.index + _l[0].length
                    , c: html.substring(_tag_head.b + _l.index, _tag_head.b + _l.index + _l[0].length)
                });
                links.push(_l[5]);
            }


            //Проход по тэгу head, и поиск скриптов
            var _s;
            var _s_pre = [];
            var _sexpression = /\<script([^\>]*)\/*\>/gim;
            while ((_s = _sexpression.exec(head)) != null) {
                // debugger
                _s_pre.push(
                    {
                        index: _s.index,
                        length: _s[0].length,
                        attr: _s[1]
                    });
            }
            for (var _is = 0; _is < _s_pre.length; _is++) {
                var _t_s =
               (_is < _s_pre.length - 1) ?
               head.substring(_s_pre[_is].index, _s_pre[_is + 1].index) :
               head.substr(_s_pre[_is].index, head.length - _s_pre[_is].index);


                var _a_t_s = /\<script[^\>]*src\=['|"]{0,1}([^\>'"]*)['|"]{0,1}[^\>]*\/*\>/gmi.exec(_t_s);
                if (_a_t_s && _a_t_s.length > 1) {
                    if (_a_t_s[1] && _a_t_s[1].length > 0) {
                        scripts.push(_a_t_s[1]);
                    }
                }

                var _ec = OneNodeByTag(_t_s, 'script', true, cuts, { b: _tag_head.b + _s_pre[_is].index, e: _tag_head.b + _s_pre[_is].index + _t_s.length });
                if (_ec) {
                    //      debugger;
                    evalCode = ((!evalCode) ? '' : evalCode) + _ec + '\n';
                }

            }
        }




        //Проход по секции body
        if (body) {
            //Поиск скриптов
            var _sb;
            var _sb_pre = [];
            var _sbexpression = /\<script([^\>]*)\/*\>/gim;
            while ((_sb = _sbexpression.exec(body)) != null) {
                // debugger
                _sb_pre.push(
                    {
                        index: _sb.index,
                        length: _sb[0].length,
                        attr: _sb[1]
                    });
            }


            for (var _is = 0; _is < _sb_pre.length; _is++) {
                var _t_s =
              (_is < _sb_pre.length - 1) ?
              body.substring(_sb_pre[_is].index, _sb_pre[_is + 1].index) :
              body.substr(_sb_pre[_is].index, body.length - _sb_pre[_is].index);

                var _a_t_s = /\<script[^\>]*src\=['|"]{0,1}([^\>'"]*)['|"]{0,1}[^\>]*\/*\>/gmi.exec(_t_s);
                if (_a_t_s && _a_t_s.length > 1) {
                    if (_a_t_s[1] && _a_t_s[1].length > 0) {
                        scripts.push(_a_t_s[1]);
                    }
                }

                var _ec = OneNodeByTag(_t_s, 'script', true, cuts, { b: _tag_body.b + _sb_pre[_is].index, e: _tag_body.b + _sb_pre[_is].index + _t_s.length });
                if (_ec) {
                    //     debugger;
                    evalCode = ((!evalCode) ? '' : evalCode) + _ec + '\n';
                }
            }
        }



        //Проход по тэгу html, и поиск стилей
        var _st;
        var _st_pre = [];
        //var _stexpression = /\<style[^\>]*(type\=["|']{0,1}[^"'\>]*["|']{0,1})\>/gim;
        var _stexpression = /\<style[^\>]*\>/gim;
        while ((_st = _stexpression.exec(html)) != null) {
            // debugger
            _st_pre.push(
                {
                    index: _st.index,
                    length: _st[0].length
                  // ,attr: _st[1]
                });
        }
        for (var _is = 0; _is < _st_pre.length; _is++) {
            var _t_s =
                (_is < _st_pre.length - 1) ?
                html.substring(_st_pre[_is].index, _st_pre[_is + 1].index) :
                html.substr(_st_pre[_is].index, html.length - _st_pre[_is].index);


            var _ec = OneNodeByTag(_t_s, 'style', true, cuts, { b: _st_pre[_is].index, e: _st_pre[_is].index + _t_s.length });
            if (_ec) {

                styles.push(_ec);
            }
        }

        for (var _cit = 0; _cit < cuts.length; _cit++) {
            var _be = cuts[_cit];
            cuts[_cit].c = html.substring(_be.b, _be.e);
        }

        var test_cuts_html = '';
        cuts = cuts.reverse();
        for (var _ih = _tag_body.b; _ih <= _tag_body.e;) {
            var _t = cuts.pop();
            if (_t && _t.b < _tag_body.b) continue;
            var _b = _ih;
            var _e = (_t) ? _t.b : _tag_body.e;
            if (_b < _e)
                new_html = ((!new_html) ? '' : new_html) + html.substring(_b, _e);
            if (!_t) break;
            _ih = _t.e;
        }
        return { links: links, styles: styles, scripts: scripts, evalCode: evalCode, html: new_html };
    }
})(jQuery)();
