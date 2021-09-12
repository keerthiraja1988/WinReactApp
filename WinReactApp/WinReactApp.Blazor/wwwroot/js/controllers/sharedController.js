(
    function (publicMethod, $) {
        publicMethod.clearValidationSummary = function () {
            $(".validation-message").remove();
        }

        publicMethod.showLoadingIndicator = function () {
            $('#myNavLoadingIndicatorText').text('Processing, Please Wait...');
            document.getElementById("myNav").style.height = "100%";
        }

        publicMethod.showLoadingIndicator = function (customText) {
            $('#myNavLoadingIndicatorText').text(customText);
            document.getElementById("myNav").style.height = "100%";
        }

        publicMethod.showLoadingIndicatorAsync = async function () {
            $('#myNavLoadingIndicatorText').text('Processing, Please Wait...');
            document.getElementById("myNav").style.height = "100%";
        }

        publicMethod.hideLoadingIndicator = function () {
            setTimeout(
                function () {
                    document.getElementById("myNav").style.height = "0%";
                }, 100);

            setTimeout(
                function () {
                    $('#myNavLoadingIndicatorText').text('Processing, Please Wait...');
                }, 200);
        }

        publicMethod.hideLoadingIndicatorAsync = async function () {
            setTimeout(
                function () {
                    document.getElementById("myNav").style.height = "0%";
                }, 100);

            setTimeout(
                function () {
                    $('#myNavLoadingIndicatorText').text('Processing, Please Wait...');
                }, 200);
        }
    }(window.sharedController = window.sharedController || {}, jQuery)
);