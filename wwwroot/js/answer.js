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

        var PostsAnswer = {
            $el: null,

            options: {
                postAnswer: function (data, success, error) { success(data); },
                cancelAnswer: function (success, error) { success(); },
                voteUpAnswer: function (success, error) { success(); },
                voteDownAnswer: function (success, error) { success(); },
                voteUpQuestion: function (success, error) { success(); },
                voteDownQuestion: function (success, error) { success(); },
            },

            events: {
                'click .answer-form .button-post': 'postAnswer',
                'click .answer-form .button-cancel': 'cancelAnswer',
                'click .answer-form .answers .item .up-vote': 'voteUpAnswer',
                'click .answer-form .answers .item .down-vote': 'voteDownAnswer',
                'click .answer-form .question .up-vote': 'voteUpQuestion',
                'click .answer-form .question .down-vote': 'voteDownQuestion',
            },
            elms: {
                'alreadyQuestions': '.answer-form .already-questions',
                'titleAnswer': '.answer-form .title-question',
                'buttonPost': '.answer-form .button-post',
                'buttonCancel': '.answer-form .button-cancel',
                'answers': '.answer-form .answers',
                'answersItem': '.answer-form .answers .item',
                'tagList': '.answer-form .wrap-tags a'
            },


            // Initialization

            init: function (options, el) {
                this.$el = $(el);
                this.undelegateEvents();
                this.delegateEvents();

                // Detect mobile devices
                (function (a) { (jQuery.browser = jQuery.browser || {}).mobile = /(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4)) })(navigator.userAgent || navigator.vendor || window.opera);
                if ($.browser.mobile) this.$el.addClass('mobile');
                $.extend(this.options, options);

                //bind answers
                //let parentId = window.getLastSlugOfLink();
                //this.bindAnswers(0, parentId);

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


            //post posts answer (posts type = 3)
            postAnswer: function (ev) {
                console.log("posts answers");
                var self = this;

                let userId = window.getParameterByName('u_i');
                let parentId = window.getLastSlugOfLink();
                //let postTypeId = window.getParameterByName('po_t');
                let postTypeId = 3;
                let title = self.getElementVal('titleAnswer').val();
                let htmlContent = tinyMCE.activeEditor.getContent({ format: 'raw' }) || '';
                let listTag = self.getElementVal('listTag');
                let tags = self.getArrVal(listTag, 'data-value').toString();
                var data = {};

                data['Id'] = 0;
                //data['OwnerUserId'] = ~~userId;
                data['PostTypeId'] = ~~postTypeId;
                data['ParentId'] = ~~parentId;
                //data['Title'] = title;
                data['HtmlContent'] = htmlContent;
                data['Tags'] = tags;

                var success = function () {
                    $.ajax({
                        url: "/api/posts/" + 0,
                        type: 'POST',
                        //contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: data,
                        async: false,
                        success: function (result) {
                            if (result.type === "msg") {
                                let answer = self.getAnswers(result.itemId, result.parentId);
                                self.appendAnswer(answer);
                            }
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

                this.options.postAnswer(data, success, error);

            },

            // bind answer 
            bindAnswers: function () {
                let self = this;
                let data = self.getAnswers();
                let answers = self.getElementVal('answers');
                let items = "";
                $.each(data, function (i, v) {
                    items += "<div class='full-width item border-bottom padding-bottom-base padding-top-base'><div class='full-width left user-info'><a href='/users/" + v.ownerUserId + "' class='ui basic image label'> <img src='"+v.Avatar+"'>" + v.userDisplayName + "</a></div><div class='container answer'>" + v.htmlContent + "</div></div>";
                });
                answers.empty().append(items);
            },

            appendAnswer(v) {
                let self = this;
                let answersItem = self.getElementVal('answersItem');
                var item = "<div class='full-width item border-bottom padding-bottom-base padding-top-base'><div class='full-width left user-info'><a href='/users/" + v.ownerUserId + "' class='ui basic image label'> <img src='"+v.Avatar+"'>" + v.userDisplayName + "</a></div><div class='container answer'>" + v.htmlContent + "</div></div>";
                answersItem.last().after(item);
            },

            //get answers
            getAnswers: function (id, parentId) {
                let self = this;
                let url = "";
                if (id === undefined || id === 0) {
                    url = "/api/posts?pg=0&po_t=3&pa_po_i=" + parentId;
                } else {
                    url = "/api/posts/" + id;
                }
                let data;
                $.ajax({
                    url: url,
                    //crossDomain: true,
                    type: 'GET',
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    dataType: 'json',
                    async: false,
                    success: function (result, textStatus, jqXHR) {
                        if (jqXHR.status === 204) {
                            self.errorCallback(textStatus);
                        }
                        else {
                            data = result;
                        };
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        self.errorCallback(textStatus);
                    }
                });
                return data;
            },

            //vote function
            voteUpAnswer: function (ev) {
                let self = this;
                let curr = $(ev.currentTarget);
                let value = curr.find('.value');
                let answerId = curr.attr('data-id');
                let count = value.text();
                let ok = false;
                var success = function () {
                    ok = self.votePosts(1, true, answerId);
                    if (ok) {
                        count = ~~count + 1;
                        value.text(count);
                    };
                };
                var error = function () {
                    console.log("error");
                };
                this.options.voteUpAnswer(success, error);
            },

            voteDownAnswer: function (ev) {
                let self = this;
                let curr = $(ev.currentTarget);
                let value = curr.parent().find('.value');
                let answerId = curr.attr('data-id');
                let count = value.text();
                let ok = false;
                var success = function () {
                    ok = self.votePosts(1, false, answerId);
                    if (ok) {
                        count = ~~count - 1;
                        value.text(count);
                    };
                };
                var error = function () {
                    console.log("error");
                };
                this.options.voteDownAnswer(success, error);
            },

            voteUpQuestion: function (ev) {
                let self = this;
                let curr = $(ev.currentTarget);
                let value = curr.find('.value');
                let answerId = 0;
                let count = value.text();
                let ok = false;
                var success = function () {
                    ok = self.votePosts(1, true, answerId);
                    if (ok) {
                        count = ~~count + 1;
                        value.text(count);
                    };
                };
                var error = function () {
                    console.log("error");
                };
                this.options.voteUpQuestion(success, error);
            },

            voteDownQuestion: function (ev) {
                let self = this;
                let curr = $(ev.currentTarget);
                let value = curr.parent().find('.value');
                let answerId = 0;
                let count = value.text();
                let ok = false;
                var success = function () {
                    ok = self.votePosts(1, false, answerId);
                    if (ok) {
                        count = ~~count - 1;
                        value.text(count);
                    };
                };
                var error = function () {
                    console.log("error");
                };
                this.options.voteDownQuestion(success, error);
            },

            // post vote
            votePosts: function (type, isOn, answerId) {
                console.log("vote " + type);
                let success = false;
                let self = this;
                let postsId;
                let data = {};


                if (answerId !== 0) {
                    postsId = answerId;
                } else {
                    postsId = window.getLastSlugOfLink();
                }

                data["PostsId"] = ~~postsId;
                data["VoteTypeId"] = ~~type;
                data["IsOn"] = isOn;

                $.ajax({
                    url: "/api/votes",
                    type: 'POST',
                    //contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: data,
                    async: false,
                    success: function (result) {
                        console.log(result);
                        if (result.type === "msg") {
                            success = true;
                        }
                        showToast(result.type, result.message);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        showToast("err", textStatus);
                    }
                });
                return success;
            },


            //clear result
            //clearResultSearch: function () {
            //    let self = this;
            //    let alreadyQuestions = self.getElementVal('alreadyQuestions');
            //    alreadyQuestions.empty();
            //},

            //error
            errorCallback: function (err) {
                window.showToast("err", "err");
            },

            cancelAnswer: function () {
                console.log("cancle answer!");
                let self = this;
                //self.clearResultSearch();

            },
        }

        $.fn.postsAnswer = function (options) {
            return this.each(function () {
                var postsAnswer = Object.create(PostsAnswer);
                postsAnswer.init(options || {}, this);
                $.data(this, 'postsAnswer', postsAnswer);
            });
        };
    }));