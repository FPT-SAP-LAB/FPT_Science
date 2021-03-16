﻿$('#preview').click(function () {
    var content = $('.summernote').summernote('code');
    var address = $('#partner_address').val()
    var website = $('#partner_website').val()
    var imgInp = $('#imgInp').val();
    var temp = { content: content, website: website, address: address, imgInp: imgInp };
    $.ajax({
        url: '/Partner/pass_content',
        type: "POST",
        data: JSON.stringify(temp),
        contentType: "application/json;charset=utf-8",
        cache: false,
        success: function () {
            window.open('/Partner/Preview');
        },
        error: function () {

        }
    });
});