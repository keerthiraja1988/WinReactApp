(
    function (publicMethod, $) {
        publicMethod.onSuccessRegistration = function (_sharedServiceObjRef) {
            Swal.fire(
                {
                    title: 'Thank you for your registration!',
                    text: 'Your account has been successfully created and redirecting to login page.',
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 5000,
                })

            setTimeout(
                function () {
                    _sharedServiceObjRef.invokeMethodAsync("NavigateToPageAsync", "login");
                }, 5000);
        },
            publicMethod.onSuccessUserLogin = function (_sharedServiceObjRef) {
                setTimeout(
                    function () {
                        _sharedServiceObjRef.invokeMethodAsync("NavigateToPageAsync", "");
                    }, 5000);
            }
    }(window.authController = window.authController || {}, jQuery)
);