<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication1.view.admin.login" %>

    <!DOCTYPE html>
    <html>

    <head runat="server">
        <title>Вход - WheelDeal</title>
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
                border: none;
                font-weight: 500;
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
                border: 2px solid #dee2e6;
                border-radius: 8px;
                padding: 12px 15px;
                transition: all 0.3s ease;
            }

            .form-control:hover {
                border-color: #00bcd4;
            }

            .forgot-password {
                text-align: center;
                margin-top: 10px;
            }

            .forgot-password a {
                color: #6c757d;
                text-decoration: none;
                font-size: 14px;
                transition: color 0.3s ease;
            }

            .forgot-password a:hover {
                color: #00bcd4;
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
                                <a class="nav-link active" href="login.aspx">Войти</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="register.aspx">Регистрация</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>

            <div class="container-box">
                <h3 class="text-center">WheelDeal</h3>
                <h5 class="mt-3 text-center">Вход в аккаунт</h5>

                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mb-3" Placeholder="Email" />

                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control mb-2" TextMode="Password"
                    Placeholder="Пароль" oninput="validatePassword(this)" />
                <div id="passwordError" class="error-message">Кириллические символы не допускаются</div>

                <asp:Button ID="btnLogin" runat="server" Text="Войти" CssClass="btn btn-cyan w-100 mb-2"
                    OnClick="btnLogin_Click" />
                <asp:Label ID="lblMsg" runat="server" CssClass="text-danger d-block text-center" Visible="false" />

                <div class="forgot-password">
                    <a href="#" data-bs-toggle="modal" data-bs-target="#forgotPasswordModal"
                        onclick="clearRecoveryMessage()">Забыли пароль?</a>
                </div>

                <div class="text-center mt-3">
                    <a href="register.aspx" class="text-decoration-none text-cyan">Нет аккаунта? Зарегистрироваться</a>
                </div>
            </div>

            <!-- Модальное окно восстановления пароля -->
            <div class="modal fade" id="forgotPasswordModal" tabindex="-1" aria-labelledby="forgotPasswordModalLabel"
                aria-hidden="true" data-bs-backdrop="static">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="forgotPasswordModalLabel">Восстановление пароля</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Закрыть"
                                onclick="clearRecoveryMessage()"></button>
                        </div>
                        <div class="modal-body">
                            <p>Введите ваш email для получения временного пароля:</p>
                            <asp:TextBox ID="txtRecoveryEmail" runat="server" CssClass="form-control mb-3"
                                Placeholder="Ваш email" TextMode="Email" />
                            <asp:Button ID="btnSendPassword" runat="server" Text="Получить временный пароль"
                                CssClass="btn btn-cyan w-100" OnClick="btnSendPassword_Click" />
                            <asp:Label ID="lblRecoveryMessage" runat="server" CssClass="d-block text-center mt-2"
                                Visible="false"></asp:Label>
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

            function clearRecoveryMessage() {
                var lblRecoveryMessage = document.getElementById('<%= lblRecoveryMessage.ClientID %>');
                if (lblRecoveryMessage) {
                    lblRecoveryMessage.textContent = '';
                    lblRecoveryMessage.style.display = 'none';
                    lblRecoveryMessage.className = 'd-block text-center mt-2';
                }
            }

            // Открываем модальное окно после postback, если нужно
            window.addEventListener('DOMContentLoaded', function () {
                var modalElement = document.getElementById('forgotPasswordModal');
                var lblRecoveryMessage = document.getElementById('<%= lblRecoveryMessage.ClientID %>');

                // Проверяем, есть ли сообщение - если есть, открываем модальное окно
                if (modalElement && lblRecoveryMessage && lblRecoveryMessage.textContent.trim() !== '') {
                    var modal = bootstrap.Modal.getOrCreateInstance(modalElement);
                    modal.show();
                }

                // Очищаем сообщение при открытии модального окна через ссылку
                if (modalElement) {
                    modalElement.addEventListener('show.bs.modal', function () {
                        if (lblRecoveryMessage && !lblRecoveryMessage.textContent.includes('отправили')) {
                            lblRecoveryMessage.textContent = '';
                            lblRecoveryMessage.style.display = 'none';
                        }
                    });
                }
            });
        </script>
    </body>

    </html>