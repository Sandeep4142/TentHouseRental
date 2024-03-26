$(document).ready(function () {
    $('#loginBtn').click(function (event) {
        // Manually trigger form validation
        var form = $('form')[0];
        if (form.checkValidity() === false) {
            return;
        }
        // Prevent default form submission
        event.preventDefault();

        var email = $('#loginEmail').val();
        var password = $('#loginPassword').val();

        $.ajax({
            type: 'POST',
            url: 'http://localhost:5140/api/User/Login',
            contentType: 'application/json',
            data: JSON.stringify({ email: email, password: password }),
            success: function (response) {
                // Handle successful login
                localStorage.setItem('jwtToken', response.token);
                window.location.href = '/Product/product.html';
            },
            error: function (xhr, status, error) {
                if (xhr.status === 401) {
                    alert('Invalid password');
                } else if (xhr.status === 404) {
                    alert('User not found');
                } else {
                    alert('Error occurred while logging in');
                }
            }
        });
    });
});