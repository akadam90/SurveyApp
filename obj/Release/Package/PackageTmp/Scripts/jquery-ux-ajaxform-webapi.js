$(function () {
    $.fn.ajaxsubmitwebapi = function (options) {

        var _mapFormData = function (form) {
            var data = {};

            $(form).serializeArray().map(function (x) {
                    data[x.name] = x.value;                
            });

            return data;
        };
            

        $(this).submit(function (event) {
            if (typeof (options.onBeforeSubmit) == 'function') {
                options.onBeforeSubmit(event);
            }

            event.preventDefault();

            data = _mapFormData(this);

            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                dataType: 'json',
                //data: $.toJSON(data),
                contentType: 'application/json',
                success: function (response) {
                    if (typeof (options.onSuccess) == 'function') {
                        options.onSuccess(response);
                    }
                },
                error: function (xhr, status, error) {
                    if (typeof (options.onError) == 'function') {
                        options.onError(xhr, status, error);
                    }
                }
            });
        });
    }
    });