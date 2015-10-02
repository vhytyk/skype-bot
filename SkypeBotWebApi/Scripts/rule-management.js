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
                $('#rules').append('<tr><td>' + item.Name + '</td><td>'+item.Rule+'</td><td>'+item.Value+'</td><td><button class="deleteRule" data-id="' + item.Id + '">Delete</button></td></tr>');
            });

            $(".deleteRule").click(function () {
                $.ajax(
                {
                    url: "/api/rules/delete/" + $(this).attr("data-id"),
                    type: "GET",
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

