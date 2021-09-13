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

            publicMethod.onAuthValidationSuccess = function () {
                Swal.fire(
                    {
                        title: 'Success',
                        text: 'Your are authrized!',
                        icon: 'success',
                        timer: 5000,
                        allowOutsideClick: false,
                    })
            },
            publicMethod.onAuthValidationError = function () {
                Swal.fire(
                    {
                        title: 'Error',
                        text: 'Your are not authorized!',
                        icon: 'error',
                        timer: 5000,
                        allowOutsideClick: false,
                    })
            }

        publicMethod.onAuthUnAuthorizedError = function () {
            Swal.fire(
                {
                    title: 'Oops',
                    text: 'Email Address or Password is incorrect. Please try again.',
                    icon: 'error',
                    timer: 5000,
                    allowOutsideClick: false,
                })
        }
    }(window.authController = window.authController || {}, jQuery)
);