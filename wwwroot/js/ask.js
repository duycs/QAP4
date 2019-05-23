// Check Jquery
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(['jquery'], factory);
    } else if (typeof module === 'object' && module.exports) {
        // Node/CommonJS
        module.exports = function (root, jQuery) {
            if (jQuery === undefined) {
             
                if (typeof window !== 'undefined') {
                    jQuery = require('jquery');
                }
                else {
                    jQuery = require('jquery')(root);
                }
            }
            factory(jQuery);
            return jQuery;
        };
    } else {
        // Browser globals
        factory(jQuery);
    }
}

    (function ($) {

        var PostsAsk = {
            $el: null,

            options: {
                postQuestion: function (data, success, error) { success(data); },
                cancelQuestion: function (success, error) { success(key) },
                searchQuestion: function (success, error) { success([]) }
            },

            events: {
                'keyup .ask-form .title-question': 'searchQuestion',
                'click .ask-form .button-post': 'postQuestion',
                'click .ask-form .button-cancel': 'cancelQuestion',
            },
            elms: {
                'alreadyQuestions': '.ask-form .already-questions',
                'titleQuestion': '.ask-form .title-question',
                'buttonPost': '.ask-form .button-post',
                'buttonCancel': '.ask-form .button-cancel',
                'listTag': '#WrapTag a',
                'itemPostTypeSelectedInPostsList': '.posts-type .menu .item.selected'
            },


            // Initialization
            // ==============

            init: function (options, el) {
                this.$el = $(el);
                this.undelegateEvents();
                this.delegateEvents();

                // Detect mobile devices
                (function (a) { (jQuery.browser = jQuery.browser || {}).mobile = /(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4)) })(navigator.userAgent || navigator.vendor || window.opera);
                if ($.browser.mobile) this.$el.addClass('mobile');
                $.extend(this.options, options);


            },

            delegateEvents: function () {
                this.bindEvents(false);
            },

            undelegateEvents: function () {
                this.bindEvents(true);
            },

            getElementVal: function (key) {
                return target = $(this.elms[key]);
            },

            getArrVal: function (items, atrrVal) {
                let arr = [];
                $.each(items, function (i, v) {
                    let dataValue = $(v).attr(atrrVal);
                    arr.push(dataValue);
                });
                return arr;
            },

            bindEvents: function (unbind) {
                var bindFunction = unbind ? 'off' : 'on';
                for (var key in this.events) {
                    var eventName = key.split(' ')[0];
                    var selector = key.split(' ').slice(1).join(' ');
                    var methodNames = this.events[key].split(' ');

                    for (var index in methodNames) {
                        if (methodNames.hasOwnProperty(index)) {
                            var method = this[methodNames[index]];

                            // Keep the context
                            method = $.proxy(method, this);

                            if (selector === '') {
                                this.$el[bindFunction](eventName, method);
                            } else {
                                this.$el[bindFunction](eventName, selector, method);
                            }
                        }
                    }
                }
            },


            //post posts question
            postQuestion: function (ev) {
                var self = this;

                //let userId = window.getParameterByName('u_i');
                let postTypeId = window.getParameterByName('po_t');
                let title = self.getElementVal('titleQuestion').val();
                let htmlContent = tinyMCE.activeEditor.getContent({ format: 'raw' }) || '';
                let listTag = self.getElementVal('listTag');
                let tags = self.getArrVal(listTag, 'data-value').toString();
                let postsTypeId = self.getElementVal('itemPostTypeSelectedInPostsList').attr('data-value');
                var data = {};

                data['Id'] = 0;
                //data['OwnerUserId'] = ~~userId;
                data['PostTypeId'] = postsTypeId;
                data['Title'] = title;
                data['HtmlContent'] = htmlContent;
                data['Tags'] = tags;

                console.log(data);
                var success = function () {
                    $.ajax({
                        url: "/api/posts/" + 0,
                        type: 'POST',
                        //contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: data,
                        success: function (result) {
                            let url = window.location.protocol + "//" + window.location.host + "/posts?po_t=" + postsTypeId;
                            window.location = url;
                            window.showToast(result.type, result.message);
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            self.errorCallback(errorThrown);
                        }
                    });
                };

                var error = function () {
                    // sendButton.addClass('enabled');
                    console.log("error");
                };

                this.options.postQuestion(data, success, error);

            },



            //search title questions
            searchQuestion: function (e) {
                var self = this;
                let title = self.getElementVal('titleQuestion');
                var q = title.val();
                var success = function (key) {
                    if (q !== "") {
                        //search tag
                        console.log(q);
                        $.ajax({
                            url: "/api/search/posts?pg=1&po_t=2&q=" + q,
                            //crossDomain: true,
                            type: 'GET',
                            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                            dataType: 'json',
                            success: function (result, textStatus, jqXHR) {
                                if (jqXHR.status === 204) {
                                    self.clearResultSearch();
                                }
                                else {
                                    self.bindResultSearch(result);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                self.clearResultSearch();
                                self.errorCallback(textStatus);
                            }
                        });
                    }
                };
                var error = function () {
                    // sendButton.addClass('enabled');
                    console.log("error");
                };
                this.options.searchQuestion(success, error);

            },

            //bind result
            bindResultSearch: function (data) {
                var self = this;
                let alreadyQuestions = self.getElementVal('alreadyQuestions');
                let items = "";
                $.each(data, function (i, v) {
                    //items += "<div class='item'><i></i><div class='content'><a class='header'>" + v.title + "</a><div class='description'>" + v.bodyContent + "</div></div></div>";
                    items += "<div class='item'><i></i><div class='content'><a href='/posts/" + v.id + "?po_t=2&u_i=" + v.ownerUserId + "' class='header'>" + v.title + "</a></div></div>";
                });
                alreadyQuestions.empty().append(items);
            },

            //clear result
            clearResultSearch: function () {
                let self = this;
                let alreadyQuestions = self.getElementVal('alreadyQuestions');
                alreadyQuestions.empty();
            },

            //error
            errorCallback: function (err) {
                //clearResultSearch();
                console.log(err);
            },

            cancelQuestion: function () {
                let self = this;
                self.clearResultSearch();

            },



        }

        $.fn.postsAsk = function (options) {
            return this.each(function () {
                var postsAsk = Object.create(PostsAsk);
                postsAsk.init(options || {}, this);
                $.data(this, 'postsAsk', postsAsk);
            });
        };
    }));