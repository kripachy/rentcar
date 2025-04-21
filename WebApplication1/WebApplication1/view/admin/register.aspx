<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="WebApplication1.register" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Register - WheelDeal</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { background-color: #f8f9fa; }
        .container-box {
            max-width: 450px; margin: 60px auto; padding: 30px;
            border-radius: 12px; background: white;
            box-shadow: 0 0 15px rgba(0,0,0,0.15);
        }
        .btn-red { background-color: #dc3545; color: white; }
        .btn-red:hover { background-color: #bb2d3b; }
        .error-message {
            color: #ff0000;
            font-size: 13px;
            margin-bottom: 10px;
            display: none;
        }
        .form-control:focus {
            border-color: #dc3545 !important;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
        }
        .form-control {
            margin-bottom: 15px; /* Добавляем отступы для полей ввода */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-box">
            <h3 class="text-center text-danger">WheelDeal</h3>
            <h5 class="mt-3 text-center">Create Your Account</h5>

            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Email" />

            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"
                         Placeholder="Password" oninput="validatePassword(this)" />
            <div id="passwordError" class="error-message">Cyrillic characters are not allowed</div>

            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Confirm Password" />

            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-red w-100 mb-2" OnClick="btnRegister_Click" />
            <asp:Label ID="lblMsg" runat="server" CssClass="text-danger d-block text-center" Visible="false" />

            <div class="text-center mt-3">
                <a href="login.aspx" class="text-decoration-none text-danger">Already have an account? Log in</a>
            </div>
        </div>
    </form>

    <script>
        function validatePassword(input) {
            const errorDiv = document.getElementById('passwordError');
            const containsCyrillic = /[а-яА-ЯёЁ]/.test(input.value);

            if (containsCyrillic) {
                errorDiv.style.display = "block";
            } else {
                errorDiv.style.display = "none";
            }
        }
    </script>
</body>
</html>
