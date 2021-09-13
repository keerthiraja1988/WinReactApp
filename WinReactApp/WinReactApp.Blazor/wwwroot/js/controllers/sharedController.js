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
                    $('#myNavLoadingIndicatorText').text('Processing, Please Wait...');
                    document.getElementById("myNav").style.height = "0%";
                }, 500);
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

        publicMethod.handleForbiddenHttpError = async function () {
            Swal.fire(
                {
                    title: '403 - Forbidden!',
                    text: "You don't have permission for this request",
                    icon: 'warning',
                    allowOutsideClick: false,
                }
            )
        }

        publicMethod.handleHttpInternalServerError = async function (requestId) {
            Swal.fire(
                {
                    title: '500 - Internal Server Error!',
                    text: "An error occured while processing you request",
                    icon: 'warning',
                    allowOutsideClick: false,
                    html:
                        '<h6>Request Id : <b>' + requestId + '</b></h6>',
                }
            )
        }
    }(window.sharedController = window.sharedController || {}, jQuery)
);