if (typeof (SurveyApp) == 'undefined') {
    var SurveyApp = {};
}

SurveyApp.Index = (function () {
    var _config;

    return {

        init: function (config) {
            _config = config;
            alert("inside");


        }
    }
})();