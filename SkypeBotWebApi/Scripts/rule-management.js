$(function () {
    $("#addRule").click(function () {
        var rule = {
            Name: $(".name").val(),
            Rule: $(".rule").val(),
            Value: $(".val").val()
        };
        $.ajax(
        {
            url: "/api/rules/",
            type: "POST",
            data: rule,
            success: function (result) {
                console.log(result);
                loadRules();
                clearFields();
            }
        });

    });

    var clearFields = function () {
        $(".name").val('');
        $(".rule").val('');
        $(".val").val('');
    };

    var loadRules = function () {
        $('#rules').html('');
        $.getJSON('/api/rules', function (rulesJsonPayload) {
            $(rulesJsonPayload).each(function (i, item) {
                $('#rules').append('<li>' + item.Name + '&nbsp;<button class="deleteRule" data-id="' + item.Id + '">Delete</button></li>');
            });

            $(".deleteRule").click(function () {
                $.ajax(
                {
                    url: "/api/rules/" + $(this).attr("data-id"),
                    type: "DELETE",
                    success: function (result) {
                        console.log(result);
                        loadRules();
                    }
                });

            });
        });
    };

    loadRules();
   
});

