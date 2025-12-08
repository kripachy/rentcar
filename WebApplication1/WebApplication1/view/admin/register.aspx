<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="WebApplication1.register" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Регистрация - WheelDeal</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content/custom.css?v=<%= DateTime.Now.Ticks %>" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.2.0/css/all.min.css" />
    <style>
        body {
            background-color: #ffffff;
            font-family: 'Segoe UI', sans-serif;
            color: #000000;
        }

        .container-box {
            max-width: 450px;
            margin: 60px auto;
            padding: 40px;
            border-radius: 16px;
            background: #ffffff;
            box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
            border: 1px solid #dee2e6;
        }

        .btn-cyan {
            background-color: #00bcd4;
            color: white;
            font-weight: 500;
            border: none;
            transition: all 0.3s ease;
        }

        .btn-cyan:hover {
            background-color: #0097a7;
            color: white;
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0, 188, 212, 0.3);
        }

        .error-message {
            color: #ff0000;
            font-size: 13px;
            margin-bottom: 10px;
            display: none;
        }

        .form-control:focus {
            border-color: #00bcd4 !important;
            box-shadow: 0 0 0 0.2rem rgba(0, 188, 212, 0.25);
        }

        .form-control {
            margin-bottom: 16px;
            border: 2px solid #dee2e6;
            border-radius: 8px;
            padding: 12px 15px;
            transition: all 0.3s ease;
        }

        .form-control:hover {
            border-color: #00bcd4;
        }

        .text-cyan {
            color: #00bcd4 !important;
        }
        .text-cyan:hover {
            color: #0097a7 !important;
        }

        h3.text-danger {
            color: #000000 !important;
            font-weight: 700;
            font-size: 2rem;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        h5 {
            color: #000000;
            font-weight: 600;
        }

        .navbar.bg-dark {
            background-color: #000000 !important;
        }
        .navbar-dark .nav-link {
            color: #ffffff !important;
        }
        .navbar-dark .nav-link:hover {
            color: #00bcd4 !important;
        }
        .navbar-dark .navbar-brand {
            color: #ffffff !important;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
            <div class="container-fluid">
                <a class="navbar-brand" href="userdashboard.aspx" style="font-weight: bold;">WheelDeal</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav"
                        aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link" href="userdashboard.aspx">Главная</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="login.aspx">Войти</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="register.aspx">Регистрация</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="container-box">
            <h3 class="text-center">WheelDeal</h3>
            <h5 class="mt-3 text-center">Создайте аккаунт</h5>

            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Электронная почта" />

            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"
                         Placeholder="Пароль" oninput="validatePassword(this)" />
            <div id="passwordError" class="error-message">Пароль не должен содержать кириллические символы</div>

            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Повторите пароль" />

            <asp:Button ID="btnRegister" runat="server" Text="Зарегистрироваться" CssClass="btn btn-cyan w-100 mb-2" OnClick="btnRegister_Click" />
            <asp:Label ID="lblMsg" runat="server" CssClass="text-danger d-block text-center" Visible="false" />

            <div class="text-center mt-3">
                <a href="login.aspx" class="text-decoration-none text-cyan">Уже есть аккаунт? Войти</a>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
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
