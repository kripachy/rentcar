<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication1.login" %>

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
        .forgot-password { cursor: pointer; color: #dc3545; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-box">
            <h3 class="text-center text-danger">WheelDeal</h3>
            <h5 class="mt-3 text-center">Войдите в свой аккаунт</h5>

            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mb-3" Placeholder="Электронная почта" />

            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control mb-2" TextMode="Password"
                         Placeholder="Пароль" oninput="validatePassword(this)" />
            <div id="passwordError" class="error-message">Кириллические символы недопустимы</div>

            <asp:Button ID="btnLogin" runat="server" Text="Войти" CssClass="btn btn-red w-100 mb-2" OnClick="btnLogin_Click" />
            <asp:Label ID="lblMsg" runat="server" CssClass="text-danger d-block text-center" Visible="false" />

            <div class="text-center mt-3">
                <a href="register.aspx" class="text-decoration-none text-danger">Нет аккаунта? Зарегистрироваться</a><br />
                <span id="forgotPassword" class="forgot-password" onclick="showForgotPasswordModal()">Забыли пароль?</span>
            </div>
        </div>

        <!-- Модальное окно восстановления пароля -->
        <div class="modal fade" id="forgotPasswordModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Восстановление пароля</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"></button>
                    </div>
                    <div class="modal-body">
                        <p>Введите ваш email для получения временного пароля:</p>
                        <asp:TextBox ID="txtRecoveryEmail" runat="server" CssClass="form-control mb-3" Placeholder="Ваш email" />
                        <asp:Button ID="btnSendCode" runat="server" Text="Получить временный пароль" 
                            CssClass="btn btn-red w-100" OnClick="btnSendCode_Click" />
                        <asp:Label ID="Label1" runat="server" CssClass="d-block text-center mt-2" Visible="false" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Модальное окно сброса пароля -->
        <div class="modal fade" id="resetPasswordModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Сброс пароля</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"></button>
                    </div>
                    <div class="modal-body">
                        <asp:TextBox ID="txtResetCode" runat="server" CssClass="form-control mb-3" Placeholder="Код восстановления" />
                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control mb-3" Placeholder="Новый пароль" />
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control mb-3" Placeholder="Подтвердите пароль" />
                        <asp:Button ID="btnResetPassword" runat="server" Text="Сбросить пароль" CssClass="btn btn-red w-100" OnClick="btnResetPassword_Click" />
                        <asp:Label ID="lblResetMsg" runat="server" CssClass="text-danger d-block text-center mt-2" Visible="false" />
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

        function showForgotPasswordModal() {
            var modal = new bootstrap.Modal(document.getElementById('forgotPasswordModal'));
            modal.show();
        }

        function showResetPasswordModal() {
            var modal = new bootstrap.Modal(document.getElementById('resetPasswordModal'));
            modal.show();
        }
    </script>
</body>
</html>
