
var screeWidth = $(window).width();
var screenHeight = $(window).height();
//var mToolbar = 'tag|movePost|insertfile undo redo | styleselect |forecolor backcolor| bold italic | alignleft aligncenter alignjustify | bullist | link image |print media';
var mToolbar = 'styleselect fontsizeselect fontselect image codesample mathSymbols code link';

if (screeWidth <= 767) {
    mToolbar = 'styleselect fontsizeselect image codesample  mathSymbols code link';
}

//init
tinymce.init({
    selector: 'textarea.main-editor',
    //language_url: '/wwwroot/lib/TinnyMCE/language/vi_VN.js',
    theme: 'modern',
    menubar: false,
    statusbar: true,
    skin: "lightgray",
    min_height: screenHeight - screenHeight * 0.3,
    max_width: 720,
    //content_css: '/wwwroot/css/myTinnyMCE.css',
    visual: false,
    //add plugins
    plugins: ['link image  wordcount codesample mathSymbols code'],
    //toolbar
    toolbar: mToolbar,

    block_formats: 'Paragraph=p;Header 1=h1;Header 2=h2;Header 3=h3',
    fontsize_formats: '8pt 10pt 12pt 14pt 18pt 24pt 36pt',
    browser_spellcheck: false,
    font_formats:
        "Andale Mono=andale mono,times;" +
        "Arial=arial,helvetica,sans-serif;" +
        "Arial Black=arial black,avant garde;" +
        "Book Antiqua=book antiqua,palatino;" +
        "Comic Sans MS=comic sans ms,sans-serif;" +
        "Courier New=courier new,courier;" +
        "Georgia=georgia,palatino;" +
        "Helvetica=helvetica;" +
        "Impact=impact,chicago;" +
        "Symbol=symbol;" +
        "Tahoma=tahoma,arial,helvetica,sans-serif;" +
        "Terminal=terminal,monaco;" +
        "Times New Roman=times new roman,times;" +
        "Trebuchet MS=trebuchet ms,geneva;" +
        "Verdana=verdana,geneva;" +
        "Webdings=webdings;" +
        "Wingdings=wingdings,zapf dingbats",
    codesample_dialog_width: '400',
    codesample_dialog_height: '400',
    codesample_languages: [
        {text: 'HTML/XML', value: 'markup'},
        {text: 'JavaScript', value: 'javascript'},
        {text: 'CSS', value: 'css'},
        {text: 'PHP', value: 'php'},
        {text: 'Ruby', value: 'ruby'},
        {text: 'Python', value: 'python'},
        {text: 'Java', value: 'java'},
        {text: 'C', value: 'c'},
        {text: 'C#', value: 'csharp'},
        {text: 'C++', value: 'cpp'}
    ],
    external_plugins: {'mathSymbols': '/lib/tinymce/plugins/mathsymbols/plugin.js'}, // Add plugin to Tinymce

    //set defaut font size, font type
    setup: function (ed) {
        ed.on('init', function () {
            this.getDoc().body.style.fontSize = '18px';
            this.getDoc().body.style.fontFamily = 'Arial';
        });
    },

    //custom buttom
    //upload file button
    paste_data_images: true,
    file_picker_callback: function (callback, value, meta) {
        if (meta.filetype == 'image') {
            $('#upload').trigger('click');
            $('#upload').on('change', function () {
                var file = this.files[0];
                var reader = new FileReader();
                reader.onload = function (e) {
                    callback(e.target.result, {
                        alt: ''
                    });
                };
                reader.readAsDataURL(file);
            });
        }
    },
});