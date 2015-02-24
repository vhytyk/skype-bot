$("#sendButton").click(function() {
    var message = {
        Conversation: $(".to").val(),
        Message: $(".msg").val()
    };
    $.ajax(
    {
        url: "/api/skype/SendMessagePost",
        type: "POST",
        data: message,
        success: function(result) {
            alert("successfully sent!");
        }
    });

});