// Check Jquery
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(['jquery'], factory);
        console.log("0");
    } else if (typeof module === 'object' && module.exports) {
        // Node/CommonJS
        module.exports = function (root, jQuery) {
            if (jQuery === undefined) {
                // require('jQuery') returns a factory that requires window to
                // build a jQuery instance, we normalize how we use modules
                // that require this pattern but the window provided is a noop
                // if it's defined (how jquery works)
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

        var Posts = {
            $el: null,

            options: {

                id: null,

                createNewPosts: function (success, error) {
                    success();
                },

                addPostAnswer: function (success, error) {
                    success();
                },

                getPost: function (success, error) {
                    success();
                },

                clickItem: function (success, error) {
                    success();
                },

                postPosts: function (success, error) { success(); },

                publishOrPrivatePosts: function (success, error) { success(); },

                putPost: function (success, error) { success(); },

                deletePost: function (success, error) { success(); },

                clickItemInListPosts: function (success, error) { success(); },

                refresh: function () { },

                showManager: function (success, error) { },

                clickLoadCoverImages: function(){},
                clickChooseImageCover: function(ev){},


                //setSelectedOption: function (success, error) { success(); }

                //create, add element 
                //createNode: function (element) {
                //    return document.createElement(element);
                //},
                //append: function (parent, el) {
                //    return parent.appendChild(el);
                //}
            },

            events: {
                'click .posts-manager .action-posts .create-new': 'createNewPosts',
                'click .posts-manager .menu-nav .search': 'searchPosts',
                'click .posts-manager .menu-nav .books': 'getBook',
                'click .posts-manager .action-posts .post': 'postPosts',
                'click .posts-manager .action-posts .answer': 'addPostAnswer',
                'click .posts-manager .action-posts .history': 'history',
                'click .posts-manager .action-posts .remove-trash': 'removeToTrashPosts',
                'click .posts-manager .action-posts .facebook': 'shareFacebookPosts',
                'click .posts-manager .action-posts .publish-or-private': 'publishOrPrivatePosts',
                'click .posts-manager .button.manager': 'showManager',
                'click .posts-manager .posts-list .list .item': 'clickItemInListPosts',
                'click .posts-manager .posts-list .posts-type .item': 'clickItemInPostsType',
                'click .posts-manager .tabular.menu .tab': 'setSelectedTabOption',
                //'click .posts-manager .list .ui.checkbox': 'clickCheckboxInList',
                'click .posts-manager .choose-items-checked': 'clickChooseItemsChecked',
                //'click .posts-manager .tabular.menu .tab.active .menu .item.selected': 'setSelectedOption'
                'click .posts-manager .actions .edit': 'clickEditQuestion',
                'click .posts-manager .btn-load-cover-images': 'clickLoadCoverImages',
                'click .posts-manager .btn-choose-image': 'clickChooseImageCover'
            },

            elms: {
                'editor': '#Editor',
                'coverImages': '#coverImgs',
                'iframeEditor': '#tinnyMCE',
                'mceToolbarGroup': '.mce-toolbar-grp',
                //'iframeEditor': '#mceu_18',
                'tags': '',
                'userInfo': '#UserInfo',
                'title': '#tinnyMCE .tile-post .input-title',
                'description': '#DescriptionPost textarea',
                'wrapDescription': '.posts-manager .wrap-description',
                'postType': '',
                'voteCount': null,
                'viewCount': null,

                'listPosts': '.posts-manager .posts-list .list',
                'body': '.body-content .body',
                'itemInListPosts': '.posts-manager .posts-list .item',

                'wrapTag': '#WrapTag',
                'listTag': '#WrapTag a',
                'dropdownBeforlistTag': '#WrapTag .dropdown.icon',

                'titleCompilation': '.posts-manager .manager-editor .input-title',
                'listCompilation': '.posts-manager .manager-editor .area-push-posts',
                'itemInListCompilation': '.posts-manager .manager-editor .area-push-posts .item',
                'itemLinkInListCompilation': '.posts-manager .manager-editor .area-push-posts .item a',
                'itemWrite': '.posts-manager .editor .write .item',
                'itemCompilation': '.posts-manager .editor .compilation .item',
                'itemTabEditor': '.posts-manager .tabular.menu .tab .item',
                'itemActiveTextInTabEditor': '.tabular.menu .item.tab.active .text',
                'itemPostTypeSelectedInPostsList': '.posts-manager .posts-list .posts-type .menu .item.selected',
                'itemInPostsType': '.posts-manager .posts-list .posts-type .item',
                'addPostAnswer': '.posts-manager .action-posts .answer',
                'postPost': '.posts-manager .action-posts .post',
                'publishOrPrivatePosts': '.posts-manager .action-posts .publish-or-private',
                'previewQuesion': '.posts-manager .post-preview .question',
                'previewRightAnswer': '.posts-manager .post-preview .answers .right-answer',
                'previewWrongAnswers': '.posts-manager .post-preview .answers .wrong-answers',
                'itemInPreviewAnswers': '.posts-manager .post-preview .answers .item',
                'postPreview': '.posts-manager .post-preview',
                'chooseItemsChecked': '.posts-manager .choose-items-checked',
                'itemCheckboxInList': '.posts-manager .list .ui.checkbox',
                'itemCheckboxCheckedInList': '.posts-manager .list .ui.checkbox.checked'
            },

            queries: {
                'userId': 'u_i',
                'postsListTypeId': 'po_lst_t',
                'postsTypeId': 'po_t',
                'parentId': 'pr_i',
                'postsId': 'po_i',
                'order': 'or_d',
                'sort': 'sor',
                'page': 'pg',
                'answer': 'ans'
            },

            // Initialization
            // ==============

            init: function (options, el) {
                var self = this;
                self.$el = $(el);
                self.undelegateEvents();
                self.delegateEvents();
                let postsTypeId = window.getParameterByName(self.queries.postsTypeId);
                let userId = window.getParameterByName(self.queries.userId);
                let postsId = window.getParameterByName(self.queries.postsId);
                let postListTypeId = window.getParameterByName(self.queries.postsListTypeId);;

                //add title post
                var $titlePostHtml = "<div class='tile-post'><input class='input-title' placeholder='Tiêu đề gây ấn tượng cho người đọc'><span class='message-info transition'><i class='ui notched circle loading icon green'></i>Đã lưu</span></div>";

                //sort list
                var actionOptions = {
                    valueNames: ['title', 'publish', 'update-time', 'created-time', 'view', 'like', 'comment']
                };


                // Detect mobile devices
                //(function (a) { (jQuery.browser = jQuery.browser || {}).mobile = /(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4)) })(navigator.userAgent || navigator.vendor || window.opera);
                //if ($.browser.mobile) this.$el.addClass('mobile');

                $.extend(this.options, options);

                //init posts
                //check title and bind editor by using interval
                var numberConditionBind = 0;
                //if (postsId) {
                var initEditorInterval = setInterval(function () {
                    var isInsertTitlePosts = false;
                    var titlePosts = self.getElementVal('title');

                    //insert title posts
                    if (titlePosts.length === 0) {
                        var $mceToolbarGroup = self.getElementVal('mceToolbarGroup');
                        if ($mceToolbarGroup && $mceToolbarGroup.length > 0) {
                            console.log('insert title posts');
                            $mceToolbarGroup.after($titlePostHtml);
                            isInsertTitlePosts = true;

                            //enter title down mouse to editor body
                            titlePosts.keyup(function (e) {
                                if (e.keyCode === 13) {
                                    console.log("enter");
                                    tinymce.execCommand('mceFocus', false, 'id_of_textarea');
                                }
                            });
                        }
                        numberConditionBind++;
                    }

                    //bind posts if posts exist
                    if (postsId > 0 && isInsertTitlePosts) {
                        console.log('bind posts if posts exist')
                        //init tag
                        self.bindTagsToEditor(postsId);
                        self.bindPostsToEditor(postsId, postsTypeId);
                        numberConditionBind++;
                    }

                    console.log(numberConditionBind);
                    if (isInsertTitlePosts) {
                        console.log('inited posts after checknum ', numberConditionBind);
                        clearInterval(initEditorInterval);
                    }
                }, 500);
                //}
                //init posts list
                let postsList = self.getPosts(postsTypeId, userId, 0);
                self.bindPostsToList(postsList);
                if (postListTypeId) {
                    // click post type in list
                    self.clickItemActiveList(postListTypeId);
                }

                //self.updateQueryVal('pg', 1);
            },

            delegateEvents: function () {
                this.bindEvents(false);
            },

            undelegateEvents: function () {
                this.bindEvents(true);
            },

            getElementVal: function (key) {
                return target = $(this.elms[key])
            },

            getQueryVal: function (key) {
                return target = $(this.query[key]);
            },

            getArrVal: function (items, atrrVal) {
                let arr = [];
                $.each(items, function (i, v) {
                    let dataValue = $(v).attr(atrrVal);
                    arr.push(dataValue);
                });
                return arr;
            },

            getArrTableContent: function (items) {
                let arr = [];
                $.each(items, function (i, v) {
                    let dataValue = $(v).attr('href');
                    arr.push(dataValue);
                });
                return arr;
            },

            getUrlUpdateQueryVal: function (key, value, url) {
                if (!url) url = window.location.href;
                var newUrl;
                var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
                var separator = url.indexOf('?') !== -1 ? "&" : "?";
                if (url.match(re)) {
                    newUrl = url.replace(re, '$1' + key + "=" + value + '$2');
                }
                else {
                    newUrl = url + separator + key + "=" + value;
                }
                return newUrl;
            },

            // push state when click item in list
            pushStateItem: function (ev, target) {
                ev.preventDefault();
                var targetUrl = $(target).attr('href'),
                    targetTitle = $(target).attr('title');

                window.history.pushState({ url: "" + targetUrl + "" }, targetTitle, targetUrl);
            },

            replaceState: function (ev, targetUrl, targetTitle) {
                ev.preventDefault();
                window.history.replaceState({ url: "" + targetUrl + "" }, targetTitle, targetUrl);
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

            //checkbox in list handler
            //click choose item checked 
            clickChooseItemsChecked: function (ev) {
                let self = this;
                let target = ev.target;
                let itemCheckboxCheckedInList = self.getElementVal('itemCheckboxCheckedInList');
                let ids = $(itemCheckboxCheckedInList).find('label').attr('.data-id');
                //console.log(ids);
                //console.log(itemCheckboxCheckedInList);
                $(target).text("");
            },
            //end checkbox in list handler


            //set view
            //POSTS = 1
            //QUESTION = 2
            //ANSWER = 3
            //EXAMINATION = 4
            //TEST = 5
            //TUTORIAL = 6
            //NOTEBOOK = 7
            setView: function () {
                let self = this;
                let postsId = ~~window.getParameterByName(self.queries.postsId) || 0; //if have child, this is parent posts
                let parentId = ~~window.getParameterByName(self.queries.parentId) || 0;
                let postTypeId = ~~window.getParameterByName(self.queries.postsTypeId);
                let postListTypeId = ~~window.getParameterByName(self.queries.postsListTypeId);
                let userId = ~~window.getParameterByName(self.queries.userId) || 0;
                let addPostAnswer = self.getElementVal('addPostAnswer');
                let itemInPreviewAnswers = self.getElementVal('itemInPreviewAnswers');
                let postPreview = self.getElementVal('postPreview');
                let postPost = self.getElementVal('postPost');
                let wrapDescription = self.getElementVal('wrapDescription');

                switch (postTypeId) {
                    case 1:
                        $(addPostAnswer).hide();
                        $(postPreview).hide();
                        $(postPost).text("Đăng bài viết");
                        $(wrapDescription).show();
                        break;
                    case 2:
                        //display button
                        $(addPostAnswer).show(300);
                        $(postPreview).show();
                        $(postPost).text("Đăng câu hỏi");
                        $(wrapDescription).hide();
                        break;
                    case 3:
                        //display button
                        $(addPostAnswer).show(300);
                        $(postPreview).show();
                        $(postPost).text("Hoàn thành câu hỏi");
                        $(wrapDescription).hide();
                        break;
                    case 4:
                        $(postPreview).hide();
                        $(addPostAnswer).hide(300);
                        $(postPost).text("Đăng bài thi tự luận");
                        $(wrapDescription).show();
                        break;
                    case 5:
                        $(postPreview).hide();
                        $(addPostAnswer).hide(300);
                        $(postPost).text("Đăng bài thi trắc nghiệm");
                        $(wrapDescription).show();
                        break;
                    case 6:
                        $(postPreview).hide();
                        $(addPostAnswer).hide(300);
                        $(postPost).text("Đăng hướng dẫn");
                        $(wrapDescription).show();
                        break;
                    case 7:
                        $(postPreview).hide();
                        $(postPost).text("Lưu ghi chú");
                        $(wrapDescription).show();
                        break;
                }
            },

            // function action
            //new posts
            createNewPosts: function (ev) {
                let self = this;
                let newUrl = self.getUrlUpdateQueryVal(self.queries.postsId, 0);
                let postsTypeId = window.getParameterByName(self.queries.postsTypeId);
                var success = function () {
                    //set posts id = 0
                    self.replaceState(ev, newUrl);
                    //clear all
                    self.bindPostsToEditor(0, postsTypeId);
                    self.bindTagsToEditor(0);
                };

                var error = function () {

                };

                this.options.createNewPosts(success, error);
            },

            addPostAnswer: function (ev) {
                let self = this;
                let target = ev.target;
                let postTypeId = ~~window.getParameterByName(self.queries.postsTypeId);
                let postId = ~~window.getParameterByName(self.queries.postsId);
                let postPost = self.getElementVal('postPost');
                let parentId;
                let newUrl;

                // if question, new post is answer: replace link pa_i=postId, po_i=0, po_t=3
                // else if answer, new post is still answer

                var success = function () {
                    //if post type is question have answer, add to preview

                    self.bindPostPreview(ev, postTypeId, postId);

                    //replace queries link
                    if (postTypeId === 2) {
                        parentId = postId;
                        newUrl = self.getUrlUpdateQueryVal(self.queries.parentId, parentId);
                        self.replaceState(ev, newUrl);
                    }
                    //new post
                    self.createNewPosts(ev);

                    // post in editor is new answer
                    postTypeId = 3;
                    //newUrl = self.getUrlUpdateQueryVal(self.queries.postsId, 0);
                    //self.replaceState(ev, newUrl);
                    newUrl = self.getUrlUpdateQueryVal(self.queries.postsTypeId, postTypeId);
                    self.replaceState(ev, newUrl);

                    //finish post
                    $(postPost).text("Hoàn thành các đáp án");
                };

                var error = function () {

                };

                this.options.addPostAnswer(success, error);
            },

            //add post to preview
            bindPostPreview: function (ev, postTypeId, postId) {
                let self = this;
                let htmlContent;
                let answer;
                let previewQuestion = self.getElementVal('previewQuesion');
                let previewRightAnswer = self.getElementVal('previewRightAnswer');
                let previewWrongAnswers = self.getElementVal('previewWrongAnswers');
                let itemInPreviewAnswers = self.getElementVal('itemInPreviewAnswers');
                let dataOrder = 0;

                if (postTypeId === 2) {
                    //get post
                    let post = self.getPost(postId);

                    if (post) {
                        //bind to preview
                        let question = "<div data-id='" + post.id + "' class='item padding-base'><div class='content'><a class='header'>" + post.title + "</a><div class='description'>" + post.headContent + "</div></div></div>";
                        $(previewQuestion).append(question);
                    }
                } else if (postTypeId === 3) {
                    if (itemInPreviewAnswers) {
                        let dataOrderLast = $(itemInPreviewAnswers).last().attr('data-order');
                        dataOrder = ~~dataOrderLast + 1;
                    }
                    if (dataOrder) {
                        htmlContent = tinyMCE.activeEditor.getContent({ format: 'raw' }) || '';
                        answer = "<div data-order='" + dataOrder + "' class='item answer padding-base border-bottom'><div class='actions'><i class='edit icon pointer'></i></div><div class='content'><div class='description'>" + htmlContent + "</div><div class=''><i class='edit icon'></i></div></div></div>";
                        if (dataOrder === 1) {
                            $(previewRightAnswer).append(answer);
                        } else {
                            $(previewWrongAnswers).append(answer);
                        }
                    }
                }
            },

            bindPostExistPreview: function (post) {
                let self = this;
                let previewQuestion = self.getElementVal('previewQuesion');
                let previewRightAnswer = self.getElementVal('previewRightAnswer');
                let previewWrongAnswers = self.getElementVal('previewWrongAnswers');
                let itemInPreviewAnswers = self.getElementVal('itemInPreviewAnswers');

                if (post) {
                    let postTypeId = ~~post.postTypeId;
                    if (postTypeId === 2) {
                        //bind to preview
                        let question = "<div data-id='" + post.id + "' class='item question container padding-base'><div class='actions'><i class='edit icon pointer'></i></div><div class='content'><a class='header'>" + post.title + "</a><div class='description'>" + post.headContent + "</div></div></div>";
                        $(previewQuestion).empty().append(question);
                    } else if (postTypeId === 3) {
                        let rightAnswer = '';
                        let wrongAnswers = '';
                        let htmlContent = post.htmlContent;
                        let answer2 = post.answer2;
                        let answer3 = post.answer3;
                        let answer4 = post.answer4;
                        let answer5 = post.answer5;
                        let dataOrder;
                        let arrAnswers = [answer2, answer3, answer4, answer5];

                        rightAnswer += "<div data-order='" + 1 + "' class='item answer padding-base'><div class='actions'><i class='edit icon pointer'></i></div><div class='content'><div class='description'>" + htmlContent + "</div></div></div>";
                        dataOrder = 2;
                        $.each(arrAnswers, function (i, v) {
                            wrongAnswers += "<div data-order='" + dataOrder + "' class='item answer padding-base border-bottom'><div class='actions'><i class='edit icon pointer'></i></div><div class='content'><div class='description'>" + v + "</div></div></div>";
                            dataOrder = dataOrder + 1;
                        });

                        $(previewRightAnswer).empty().append(rightAnswer);
                        $(previewWrongAnswers).empty().append(wrongAnswers);
                    } else {
                        //clear if normal question
                        $(previewRightAnswer).empty();
                        $(previewWrongAnswers).empty();
                    }

                } else {
                    //clear if normal question
                    $(previewRightAnswer).empty();
                    $(previewWrongAnswers).empty();
                }
            },

            // click edit question or answer
            clickEditQuestion: function (ev) {
                let self = this;
                let target = ev.target;
                let item = $(target).closest('.item');
                let title = self.getElementVal('title');
                let iframeEditor = self.getElementVal('iframeEditor');
                let editor = $(iframeEditor).find('iframe').contents().find('#tinymce');
                let htmlContent = tinyMCE.activeEditor.getContent({ format: 'raw' }) || '';
                let answerOrderQuery = ~~window.getParameterByName(self.queries.answer) || 0;

                let answerOrder = 0;
                let dataId = ~~item.attr('data-id');
                let dataOrder = ~~item.attr('data-order');
                let content = '';
                console.log(item);
                //let addPostAnswer = self.getElementVal('addPostAnswer');
                if (dataId) {
                    let postId = ~~window.getParameterByName(self.queries.postsId);
                    let previewQuestion = self.getElementVal('previewQuesion');
                    let question = "<div data-id='" + postId + "' class='item question container padding-base'><div class='actions'><i class='edit icon pointer'></i></div><div class='content'><a class='header'>" + title.val() + "</a><div class='description'>" + htmlContent + "</div></div></div>";
                    $(previewQuestion).empty().append(question);
                    //set view editor
                    answerOrder = 0;
                    title.show();
                } else if (dataOrder > 0) {
                    if (answerOrderQuery === 1) {
                        let rightAnswer = "<div data-order='" + answerOrderQuery + "' class='item answer padding-base'><div class='actions'><i class='edit icon pointer'></i></div><div class='content'><div class='description'>" + htmlContent + "</div></div></div>";
                        let previewRightAnswer = self.getElementVal('previewRightAnswer');
                        $(previewRightAnswer).empty().append(rightAnswer);
                    } else {
                        let wrongAnswers = "<div data-order='" + answerOrderQuery + "' class='item answer padding-base border-bottom'><div class='actions'><i class='edit icon pointer'></i></div><div class='content'><div class='description'>" + htmlContent + "</div></div></div>";
                        let previewWrongAnswers = self.getElementVal('previewWrongAnswers').find('.answer');
                        previewWrongAnswers.each(function (i, v) {
                            //console.log(v);
                            let editIcon = $(v).find('.edit');
                            let order = ~~$(v).attr('data-order')
                            editIcon.removeClass('red');
                            if (order === dataOrder) {
                                editIcon.addClass('red');
                            }
                            if (order === answerOrderQuery) {
                                let description = $(v).find('.description');
                                description.empty().append(htmlContent);
                            }
                        });
                    }
                    //setview editor
                    answerOrder = dataOrder;
                    title.hide();
                }

                //addPostAnswer.text("Hoàn thành chỉnh sửa");
                content = item.find('.content .description').html();
                let newUrl = self.getUrlUpdateQueryVal(self.queries.answer, answerOrder);
                self.replaceState(ev, newUrl);
                editor.empty().append(content);

                //check 
                //postPosts(ev);
            },

            clickLoadCoverImages: function (ev){
                let self = this;
                console.log('click load cover images to set choose images cover');
                var htmlContent = tinyMCE.activeEditor.getContent({ format: 'raw' }) || '';
                self.setChooseImagesCover(htmlContent);
            },

            //selected option post type in tab
            //setSelectedOption: function (ev) {
            //    let self = this;
            //    let target = ev.target;
            //    let postsTypeId = $(target).attr('data-value');
            //    let newUrl = self.getUrlUpdateQueryVal(self.queries.postsTypeId, postsTypeId);
            //    var success = function () {
            //        //replace postTypeId
            //        self.replaceState(ev, newUrl);
            //        //replace postsId = 0
            //        //self.createNewPosts(ev);
            //    };
            //    var error = function () {
            //        self.errorCallback("error");
            //    };
            //    this.options.setSelectedOption(success, error);
            //},

            setSelectedTabOption: function (ev) {
                let self = this;
                let target = ev.target;
                let itemSelected;
                if ($(target).hasClass('tab')) {
                    itemSelected = $(target).find('.item.selected');
                } else {
                    itemSelected = $(target).parent().find('.item.selected');
                }
                let postsTypeId = $(itemSelected).attr('data-value');

                let newUrl = self.getUrlUpdateQueryVal(self.queries.postsTypeId, postsTypeId);
                var success = function () {
                    //replace postTypeId
                    self.replaceState(ev, newUrl);

                    self.setView();
                    //replace postsId = 0
                    //self.createNewPosts(ev);
                };
                var error = function () {
                    self.errorCallback("error");
                };
                this.options.setSelectedOption(success, error);
            },

            // setDataPosts
            //normal all type is posts
            //POSTS = 1
            //QUESTION = 2
            //ANSWER = 3
            //EXAMINATION = 4
            //TEST = 5
            //TUTORIAL = 6
            //NOTEBOOK = 7
            setDataPosts() {
                let self = this;
                let postsId = ~~window.getParameterByName(self.queries.postsId) || 0; //if have child, this is parent posts
                let parentId = ~~window.getParameterByName(self.queries.parentId) || 0;
                let postTypeId = ~~window.getParameterByName(self.queries.postsTypeId);
                let userId = ~~window.getParameterByName(self.queries.userId) || 0;

                let title;
                let description;
                let htmlContent;
                let listTag;
                let tags;
                let coverImg;

                let relatedPosts;
                let listItem;
                let answer2;
                let answer3;
                let answer4;
                let answer5;

                let data = {};

                switch (postTypeId) {
                    case 1:
                        //init for a posts
                        title = self.getElementVal('title').val();
                        description = self.getElementVal('description').val();
                        htmlContent = tinyMCE.activeEditor.getContent({ format: 'raw' }) || '';
                        listTag = self.getElementVal('listTag');
                        tags = self.getArrVal(listTag, 'data-value').toString();
                        //coverImg = self.getRandomImageCover(htmlContent);

                        break;
                    case 2:
                        //init for a question
                        //set view
                        self.setView();

                        title = self.getElementVal('title').val();
                        description = self.getElementVal('description').val();
                        htmlContent = tinyMCE.activeEditor.getContent({ format: 'raw' }) || '';
                        listTag = self.getElementVal('listTag');
                        tags = self.getArrVal(listTag, 'data-value').toString();
                        //coverImg = self.getRandomImageCover(htmlContent);

                        let itemInPreviewAnswers = self.getElementVal('itemInPreviewAnswers');
                        //
                        $.each($(itemInPreviewAnswers), function (i, v) {
                            let dataOrder = ~~$(v).attr('data-order');
                            let content = $(v).find('.content .description').html();
                            if (dataOrder === 1) {
                                //right answer
                                htmlContent = content;
                            } else if (dataOrder === 2) {
                                answer2 = content;
                            } else if (dataOrder === 3) {
                                answer3 = content;
                            } else if (dataOrder === 4) {
                                answer4 = content;
                            } else if (dataOrder === 5) {
                                answer5 = content;
                            }
                        });

                        break;
                    case 3:
                        //init for an answer
                        description = self.getElementVal('description').val();
                        listTag = self.getElementVal('listTag');
                        tags = self.getArrVal(listTag, 'data-value').toString();
                        //coverImg = self.getRandomImageCover(htmlContent);
                        //
                        $.each($(itemInPreviewAnswers), function (i, v) {
                            let dataOrder = ~~$(v).attr('data-order');
                            let content = $(v).find('.content .description').html();
                            if (dataOrder === 1) {
                                //right answer
                                htmlContent = content;
                            } else if (dataOrder === 2) {
                                answer2 = content;
                            } else if (dataOrder === 3) {
                                answer3 = content;
                            } else if (dataOrder === 4) {
                                answer4 = content;
                            } else if (dataOrder === 5) {
                                answer5 = content;
                            }
                        });

                        break;
                    case 4:
                        //init for an exam
                        title = self.getElementVal('titleCompilation').val();
                        listItem = self.getElementVal('itemInListCompilation');
                        relatedPosts = self.getArrVal(listItem, 'data-id').toString();

                        break;
                    case 5:
                        //init for multil choise test
                        title = self.getElementVal('titleCompilation').val();
                        listItem = self.getElementVal('itemInListCompilation');
                        relatedPosts = self.getArrVal(listItem, 'data-id').toString();

                        break;
                    case 6:
                        //init for tutorial
                        title = self.getElementVal('titleCompilation').val();
                        listItem = self.getElementVal('itemInListCompilation');
                        relatedPosts = self.getArrVal(listItem, 'data-id').toString();

                        break;
                    case 7:
                        //init for note book
                        break;
                }

                data['Id'] = postsId;
                data['OwnerUserId'] = userId;
                data['PostTypeId'] = postTypeId;
                data['ParentId'] = parentId;
                data['Title'] = title;
                data['HtmlContent'] = htmlContent;
                data['Description'] = description;
                data['Tags'] = tags;
                data['CoverImg'] = null;
                data['RelatedPosts'] = relatedPosts;
                data['Answer2'] = answer2;
                data['Answer3'] = answer3;
                data['Answer4'] = answer4;
                data['Answer5'] = answer5;

                return data;
            },

            //post posts
            postPosts: function (ev) {
                let self = this;
                var data = self.setDataPosts();
                let postTypeId = ~~data.PostTypeId;
                let postsListTypeId = ~~window.getParameterByName(self.queries.postsListTypeId);
                let userId = ~~window.getParameterByName(self.queries.userId);
                let postsId = data.id || 0;

                // Did set choose cover images befor by setChooseCoverImages, all images is exist
                // Check image link or src, if src then use upload image, else save direct image link
                // Get cover image
                let coverImages = self.getElementVal('coverImages').find('img');
                let isImageLink = false;
                let coverSrc = null;
                let isUploadCoverImage = true;
                if(coverImages && coverImages.length > 0){
                    let imageChoosed = null;
                    coverImages.filter(function(index, value){
                        if($(value).hasClass('is-choosed'))
                            imageChoosed = value;
                    });

                    // If not choose then set default is image at 0
                    if(imageChoosed)
                        coverSrc = imageChoosed.src;
                    else
                        coverSrc = coverImages[0].src;

                    // Check if this is link image
                    isImageLink = coverSrc.includes("http");
                    if(isImageLink) 
                        isUploadCoverImage = false;
                    else 
                        isUploadCoverImage = true;
                }

                // Set direct link to save
                if(!isUploadCoverImage)
                    data.CoverImg = coverSrc;

                //console.log(data);
                var success = function () {
                    $.ajax({
                        url: "/api/posts/" + postsId,
                        type: 'POST',
                        dataType: 'json',
                        data: data,
                        async: false,
                        success: function (result) {
                            //ok
                            if (result.type === "msg") {
                                let postId = result.itemId;
                                let newUrl = self.getUrlUpdateQueryVal(self.queries.postsId, postId);
                                //replace postTypeId
                                self.replaceState(ev, newUrl);

                                if (postsListTypeId === postTypeId) {
                                    let postsList = self.getPosts(postTypeId, userId, 0);
                                    self.bindPostsToList(postsList);
                                }
                                window.showToast(result.type, result.message);
                            }

                            // Upload cover image if need
                            //console.log(isUploadCoverImage, coverSrc);
                            if(isUploadCoverImage){
                                self.uploadImageFile("coverImage", coverSrc, data.Id);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            self.errorCallback("postPosts " + errorThrown);
                        }
                    });

                };

                var error = function () {
                    self.errorCallback("postPosts " + errorThrown);
                };
                this.options.postPosts(success, error);
            },

            //get posts by user
            getPosts: function (postTypeId, userId, parentId) {
                let self = this;
                let data;
                $.ajax({
                    url: "/api/posts?u_i=" + userId + "&po_t=" + postTypeId + "&pr_i=" + parentId,
                    //crossDomain: true,
                    type: 'GET',
                    dataType: 'json',
                    async: false,
                    success: function (result, textStatus, jqXHR) {
                        data = result;
                        //self.bindPostsToList(result);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        // TODO: Cant check status code with unauthor
                        console.log(jqXHR, textStatus, errorThrown);
                        self.errorCallback("getPosts " + errorThrown);
                    }
                });
                return data;
            },

            // bind post to list
            bindPostsToList: function (data) {
                var self = this;
                let list = self.getElementVal('listPosts');
                var items = "";

                $.each(data, function (i, v) {
                    let description = v.headContent === null ? "" : v.headContent;
                    items += "<div data-id='" + v.id + "'  class='item cursor-pointer padding-right-base'>" +
                        "<div class='ui checkbox'><input type='checkbox'></div>" +
                        "<div  data-id='" + v.id + "' class='content'>" +
                        //"<div class='ui checkbox'><input type='checkbox'>" +
                        "<label class='ui big header green cursor-pointer'>" + v.title + "</label>" +
                        "<div class='description'>" + description + "</div>" +
                        "</div></div></div>";
                });
                $(list).empty().append(items);
                $('.ui.checkbox').checkbox();
            },

            //lick item in list posts
            clickItemInListPosts: function (ev) {
                let self = this;
                //get posts id
                let target = ev.target;
                let postsId = $(target).closest('.content').attr('data-id');

                //get post type id
                let itemPostTypeSelectedInPostsList = self.getElementVal('itemPostTypeSelectedInPostsList');
                let postsTypeId = $(itemPostTypeSelectedInPostsList).attr('data-value');
                let addPostAnswer = self.getElementVal('addPostAnswer');
                let newUrl;

                var success = function () {
                    // replace link
                    newUrl = self.getUrlUpdateQueryVal(self.queries.postsId, postsId);
                    self.replaceState(ev, newUrl);
                    // replace link
                    newUrl = self.getUrlUpdateQueryVal(self.queries.postsTypeId, postsTypeId);
                    newUrl = self.getUrlUpdateQueryVal(self.queries.answer, 0);
                    self.replaceState(ev, newUrl);

                    //bind
                    self.bindPostsToEditor(postsId, postsTypeId);
                    self.bindTagsToEditor(postsId);

                    //selected item posts type and active tab
                    self.clickItemActiveTab(postsTypeId);

                };

                var error = function () {
                    // sendButton.addClass('enabled');
                    console.log("error clickItemInListPosts");
                };
                this.options.clickItemInListPosts(success, error);
            },

            //clickItemInPostsType
            clickItemInPostsType: function (ev) {
                let self = this;
                var target = ev.target;
                let postsTypeId = $(target).attr('data-value');
                let userId = window.getParameterByName(self.queries.userId) || 0;
                let postsList = self.getPosts(postsTypeId, userId);
                //set posts list type 
                let newUrl = self.getUrlUpdateQueryVal(self.queries.postsListTypeId, postsTypeId);
                self.replaceState(ev, newUrl);
                self.bindPostsToList(postsList);

                //set view
                self.setView();
            },


            //active tab by posts type id
            clickItemActiveTab: function (postsTypeId) {
                let self = this;
                let items = self.getElementVal('itemTabEditor');
                let dataValue;
                $.each(items, function (e, v) {
                    dataValue = $(v).attr('data-value');
                    if (dataValue === postsTypeId) {
                        $(v).click();
                    }
                });

                //set view
                self.setView();
            },


            //click item post type in list
            clickItemActiveList: function (postTypeId) {
                let self = this;
                let items = self.getElementVal('itemInPostsType');
                let dataValue;
                $.each(items, function (e, v) {
                    dataValue = $(v).attr('data-value');
                    if (dataValue === postTypeId) {
                        $(v).click();
                    }
                });

                //set view
                self.setView();
            },

            //get a post
            getPost: function (id) {
                var self = this;
                var data;
                $.ajax({
                    url: "/api/posts/" + id,
                    //crossDomain: true,
                    type: 'GET',
                    dataType: 'json',
                    async: false,
                    success: function (result, textStatus, jqXHR) {
                        data = result;
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        self.errorCallback("getPost" + errorThrown);
                    }
                });
                return data;
            },


            //bind posts to editor write
            bindPostsToEditor: function (postsId, postsTypeId) {
                let self = this;
                if (postsTypeId === '1' || postsTypeId === '2') {
                    self.bindPostsToEditorWrite(postsId);
                } else if (postsTypeId === '4' || postsTypeId === '5' || postsTypeId === '6') {
                    self.bindPostsToEditorCompilation(postsId);
                }
            },

            //bind posts to editor write
            bindPostsToEditorWrite: function (postsId) {
                let self = this;
                let editor = self.getElementVal('editor');
                let title = self.getElementVal('title');
                let description = self.getElementVal('description');
                let iframeEditor = self.getElementVal('iframeEditor');
                let htmlContent = $(iframeEditor).find('iframe').contents().find('#tinymce');
                let publishOrPrivatePosts = self.getElementVal('publishOrPrivatePosts');
                if (postsId && postsId !== 0) {
                    console.log('bind post to editor to write');
                    let post = self.getPost(postsId);
                    //console.log(post);
                    if (post) {
                        let postTypeId = ~~post.postTypeId;

                        //bind preview question and answers
                        if (postTypeId === 2) {
                            //bind post question preview
                            self.bindPostExistPreview(post);

                            //bind answer preview
                            let parentId = postsId;
                            let postAnswer = self.getPosts(0, 0, parentId)[0];
                            self.bindPostExistPreview(postAnswer);
                        }
                        title.val(post.title);
                        htmlContent.empty().append(post.htmlContent);
                        description.val(post.description);

                        let lastActivityDate = post.lastActivityDate || null;
                        let isLock = lastActivityDate == null
                        let isUnlock = lastActivityDate !=null;
                        if(isLock){
                            $(publishOrPrivatePosts).removeClass('unlock').addClass('lock');
                        }else if(isUnlock){
                            $(publishOrPrivatePosts).removeClass('lock').addClass('unlock');
                        }

                        // click to get coverImage
                        self.clickLoadCoverImages();
                    }
                } else {
                    //$(editor).attr('data-id', '');
                    title.val('');
                    htmlContent.empty();
                    description.val('');
                }
            },



            //bind posts to editor compilation
            bindPostsToEditorCompilation: function (postsId) {
                let self = this;
                let items = "";
                let title = self.getElementVal('titleCompilation');
                let listCompilation = self.getElementVal('listCompilation');
                if (postsId && postsId !== 0) {
                    //TODO: getPostsByParent let postsChildList = self.getPosts
                    console.log('bind posts to editor compilation');
                    let post = self.getPost(postsId);
                    if (post.tableContent && post.relatedPosts) {
                        let relatedPosts = post.relatedPosts.split(',');
                        let tableContent = post.tableContent.split(',');
                        //console.log(post);
                        title.val(post.title);
                        let v1;
                        let v2;
                        for (let i = 0; i < relatedPosts.length; i++) {
                            //$.each(tableContent, function (e, v) {
                            v1 = relatedPosts[i];
                            v2 = tableContent[i];
                            items += "<div data-id='" + v1 + "' class='item padding-base'>" +
                                "<div class='content'><a class='header'>" + v2 + "</a>" +
                                //"<div class='description'>" + v.headContent + "</div>" +
                                "</div>" +
                                "</div>";
                            //});
                        }
                        console.log(items);
                        listCompilation.empty().append(items);
                    }
                } else {
                    let guide = "<p class='caption grey'>Để biên tập nội dung, bạn hãy kéo thả bài viết từ bên danh sách các bài viết vào đây.</p>";
                    title.val('');
                    listCompilation.empty().append(guide);
                }
            },


            //get tags
            getTagsByPosts: function (id) {
                var self = this;
                var data;
                $.ajax({
                    url: "/tag?po_i=" + id,
                    //crossDomain: true,
                    type: 'GET',
                    dataType: 'json',
                    async: false,
                    success: function (result, textStatus, jqXHR) {
                        data = result;
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        self.errorCallback("getTagsByPosts " + errorThrown);
                    }
                });
                return data;
            },

            bindTagsToEditor: function (postId) {
                var self = this;
                var tags = self.getTagsByPosts(postId);
                let dropdownBeforlistTag = $(self.getElementVal('dropdownBeforlistTag'));
                let listTag = self.getElementVal('listTag');
                let items = "";

                listTag.remove();
                if (tags && tags.length > 0) {
                    $.each(tags, function (i, v) {
                        items += "<a class='ui label transition visible' data-id='" + v.id + "' data-value='" + v.name + "' style='display: inline-block !important;'>" + v.name + "<i class='delete icon'></i></a>";
                    });
                    //console.log(items);
                    dropdownBeforlistTag.after(items);
                }
            },

            // post image
            postImgCover: function (imgId) {
                let htmlContent = tinyMCE.activeEditor.getContent({ format: 'raw' }) || '';
                let data = getRandomImageCover(htmlContent);
                $.ajax({
                    url: "/api/images/" + imgId,
                    type: 'POST',
                    dataType: 'json',
                    data: data,
                    async: false,
                    success: function (result) {
                        if (result.type === "msg") {
                            window.showToast(result.type, result.message);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        self.errorCallback("postPosts " + errorThrown);
                    }
                });
            },

            // Auto get image at 0 of list cover images
            // getRandomImageCover: function (htmlContent) {
            //     var self = this;
            //     document.getElementById("contentTemp").innerHTML = htmlContent;
            //     let images = document.getElementById("contentTemp").getElementsByTagName('img');
            //     let srcList = [];
            //     let img0;
            //     let localHostStr = "localhost";
            //     for (let i = 0; i < images.length; i++) {
            //         let img = images[i];
            //         console.log(img);
            //         let isError = false;
            //         var src = img.src;

            //         if(src.includes("blob:http")){
            //             isError = true;
            //         }

            //         if (src.indexOf(localHostStr) != -1) {
            //             isError = true;
            //         }

            //         if (!isError) {
            //             srcList.push(src);
            //         }
            //     }
            //     img0 = srcList[0];
            //     return img0;
            // },

            setChooseImagesCover: function (htmlContent) {
                var self = this;
                document.getElementById("contentTemp").innerHTML = htmlContent;
                let coverImages = self.getElementVal('coverImages');
                let images = document.getElementById("contentTemp").getElementsByTagName('img');
                let srcList = [];
                let coverImagesHtml = "";
                //let img0;
                let localHostStr = "localhost";
                for (let i = 0; i < images.length; i++) {
                    let img = images[i];
                    //console.log(img);
                    let isError = false;
                    var src = img.src;

                    if(src.includes("blob:http")){
                        isError = true;
                    }

                    if (src.indexOf(localHostStr) != -1) {
                        isError = true;
                    }

                    if (!isError) {
                        srcList.push(src);
                        coverImagesHtml += `<img class='btn-choose-image coverThumb' src='${src}' onclick='clickChooseImageCover()'/>`;
                    }
                }
                coverImages.empty().append(coverImagesHtml);
            },

            // Choose this image, set cache position to selected when submit 
            clickChooseImageCover : function (ev){
                var self = this;
                //console.log('choose this image', target);

                // All images cover
                let coverImages = self.getElementVal('coverImages');
                images = coverImages.find('img')

                // Reset all image have class is-choosed and set is-not-choose
                images.filter(function(index, value){
                    var image = $(value);
                    image.removeClass('is-choosed');
                    image.addClass('is-not-choosed');
                });

                // Add class is-choosed for this
                // console.log($(ev));
                $(ev.target).removeClass('is-not-choosed').addClass('is-choosed');
            },

            b64toBlob: function (b64Data, contentType, sliceSize) {
                contentType = contentType || '';
                sliceSize = sliceSize || 512;

                var byteCharacters = atob(b64Data);
                var byteArrays = [];

                for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                    var slice = byteCharacters.slice(offset, offset + sliceSize);

                    var byteNumbers = new Array(slice.length);
                    for (var i = 0; i < slice.length; i++) {
                        byteNumbers[i] = slice.charCodeAt(i);
                    }

                    var byteArray = new Uint8Array(byteNumbers);

                    byteArrays.push(byteArray);
                }

                var blob = new Blob(byteArrays, { type: contentType });
                return blob;
            },

            uploadImageFile: function (fileName, srcFile, postsId) {
                var self = this;

                if (!srcFile && postsId < 1) 
                    return;

                // Split the base64 string in data and contentType
                var block = srcFile.split(";");
                // Get the content type of the image
                var contentType = block[0].split(":")[1];// In this case "image/gif"
                // get the real base64 content of the file
                var realData = block[1].split(",")[1];// In this case "R0lGODlhPQBEAPeoAJosM...."

                // Convert it to a blob to upload
                var blob = self.b64toBlob(realData, contentType);
                //var file = input.files[0];
                blob.lastModifiedDate = new Date();
                blob.name = fileName;
                var file = blob;

                if (!file)
                    return;

                //reader.readAsDataURL(file);
                var formData = new FormData();
                formData.set("file", file, file.name);
                $.ajax({
                    url: '/api/images/posts/' + postsId + '/coverimg',
                    type: 'post',
                    data: formData,
                    contentType: false,
                    processData: false,
                    async: false,
                    success: function (response) {
                        var imageUrl = response.imageUrl;
                        if (imageUrl) {
                            console.log("upload succes ", imageUrl);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        self.errorCallback("uploadImageFile " + errorThrown);
                    }
                });
            },


            // search post
            searchPosts: function () {

            },

            //
            getBook: function () {

            },

            history: function () {

            },

            removeToTrashPosts: function () {

            },
            shareFacebookPosts: function () {

            },

            publishOrPrivatePosts: function (ev) {
                let self = this;
                let postsId = window.getParameterByName(self.queries.postsId);
                if(!postsId) return;

                let target = ev.target;
                let isLock = $(target).hasClass('lock');
                let isUnlock = $(target).hasClass('unlock');

                // If lock then unlock, otherwise
                if(isLock){
                    $(target).removeClass('lock').addClass('unlock');
                    console.log("unlock the post");
                }
                else if(isUnlock){
                    $(target).removeClass('unlock').addClass('lock');
                    console.log("lock the post");
                }

                //console.log(data);
                var success = function () {
                    $.ajax({
                        url: "/api/posts/activeOrDeacitve/" + postsId,
                        type: 'PUT',
                        dataType: 'json',
                        //data: data,
                        async: false,
                        success: function (result) {
                            if (result.type === "msg") {
                                window.showToast(result.type, result.message);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log("error then set lock or unlock agian");
                            self.errorCallback("postPosts " + errorThrown);
                            if(isLock){
                                $(target).removeClass('unlock').addClass('lock');
                            }
                            else if(isUnlock){
                                $(target).removeClass('lock').addClass('unlock');
                            }
                        }
                    });

                };

                var error = function () {
                    console.log("error then set lock or unlock agian");
                    if(isLock){
                        $(target).removeClass('unlock').addClass('lock');
                    }
                    else if(isUnlock){
                        $(target).removeClass('lock').addClass('unlock');
                    }
                };
                this.options.publishOrPrivatePosts(success, error);
            },

            //style
            showManager: function () {
                let self = this;
                let listPosts = self.getElementVal('listPosts');
                let body = self.getElementVal('body');

                var success = function () {
                    $(listPosts).css('display', 'block');
                    $(listPosts).show();
                    //$(body).attr('margirn-left');
                };
                var error = function () {
                    console.log("error");
                };
                this.options.showManager(success, error);
            },

            //update posts
            updatePosts: function (data) {

            },


            //msg callback 
            successCallback: function (msg) {
                window.showToast('success', msg);
                console.log('success: ' + msg);
            },

            errorCallback: function (msg) {
                window.showToast('error', msg);
                console.log('error: ' + msg);
            },

        }

        $.fn.posts = function (options) {
            return this.each(function () {
                var posts = Object.create(Posts);
                posts.init(options || {}, this);
                $.data(this, 'posts', posts);
            });
        };
    }));