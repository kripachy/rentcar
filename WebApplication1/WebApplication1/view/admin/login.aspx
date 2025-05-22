<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication1.view.admin.login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Вход - WheelDeal</title>
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
        .forgot-password {
            text-align: center;
            margin-top: 10px;
        }
        .forgot-password a {
            color: #6c757d;
            text-decoration: none;
            font-size: 14px;
        }
        .forgot-password a:hover {
            color: #dc3545;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-box">
            <h3 class="text-center text-danger">WheelDeal</h3>
            <h5 class="mt-3 text-center">Вход в аккаунт</h5>

            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mb-3" Placeholder="Email" />

            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control mb-2" TextMode="Password"
                         Placeholder="Пароль" oninput="validatePassword(this)" />
            <div id="passwordError" class="error-message">Кириллические символы не допускаются</div>

            <asp:Button ID="btnLogin" runat="server" Text="Войти" CssClass="btn btn-red w-100 mb-2" OnClick="btnLogin_Click" />
            <asp:Label ID="lblMsg" runat="server" CssClass="text-danger d-block text-center" Visible="false" />

            <div class="forgot-password">
                <a href="#" data-bs-toggle="modal" data-bs-target="#forgotPasswordModal">Забыли пароль?</a>
            </div>

            <div class="text-center mt-3">
                <a href="register.aspx" class="text-decoration-none text-danger">Нет аккаунта? Зарегистрироваться</a>
            </div>
        </div>

        <!-- Модальное окно восстановления пароля -->
        <div class="modal fade" id="forgotPasswordModal" tabindex="-1" aria-labelledby="forgotPasswordModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="forgotPasswordModalLabel">Восстановление пароля</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"></button>
                    </div>
                    <div class="modal-body">
                        <p>Введите ваш email для получения временного пароля:</p>
                        <asp:TextBox ID="txtRecoveryEmail" runat="server" CssClass="form-control mb-3" Placeholder="Ваш email" TextMode="Email" />
                        <asp:Button ID="btnSendPassword" runat="server" Text="Получить временный пароль" 
                            CssClass="btn btn-red w-100" OnClick="btnSendPassword_Click" />
                        <asp:Label ID="lblRecoveryMessage" runat="server" CssClass="d-block text-center mt-2 text-danger" Visible="false"></asp:Label>
                    </div>
                </div>
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