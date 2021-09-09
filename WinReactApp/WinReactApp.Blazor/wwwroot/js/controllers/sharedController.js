(
    function (publicMethod, $) {
        publicMethod.clearValidationSummary = function () {
            $(".validation-message").remove();
        }
    }(window.sharedController = window.sharedController || {}, jQuery)
);

//window.clearValidationSummary = () => {
//    $(".validation-message").remove();
//};

//function showAlert(message) {
//    alert(message);
//}