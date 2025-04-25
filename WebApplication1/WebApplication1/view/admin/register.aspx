<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="WebApplication1.register" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Register - WheelDeal</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f2f4f8;
            font-family: 'Segoe UI', sans-serif;
        }

        .container-box {
            max-width: 450px;
            margin: 60px auto;
            padding: 35px;
            border-radius: 16px;
            background: #ffffff;
            box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
        }

        .btn-red {
            background-color: #dc3545;
            color: white;
            font-weight: 500;
        }

        .btn-red:hover {
            background-color: #bb2d3b;
        }

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
            margin-bottom: 16px;
        }

        .text-danger {
            font-weight: 500;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-box">
            <h3 class="text-center text-danger">WheelDeal</h3>
            <h5 class="mt-3 text-center">Создайте аккаунт</h5>

            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Электронная почта" />

            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"
                         Placeholder="Пароль" oninput="validatePassword(this)" />
            <div id="passwordError" class="error-message">Пароль не должен содержать кириллические символы</div>

            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Повторите пароль" />

            <asp:Button ID="btnRegister" runat="server" Text="Зарегистрироваться" CssClass="btn btn-red w-100 mb-2" OnClick="btnRegister_Click" />
            <asp:Label ID="lblMsg" runat="server" CssClass="text-danger d-block text-center" Visible="false" />

            <div class="text-center mt-3">
                <a href="login.aspx" class="text-decoration-none text-danger">Уже есть аккаунт? Войти</a>
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
