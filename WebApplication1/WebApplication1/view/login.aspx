<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication1.view.login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WheelDeal - Вход</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <style>
        body {
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .login-container {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
            padding: 2rem;
            width: 100%;
            max-width: 400px;
        }

        .login-header {
            text-align: center;
            margin-bottom: 2rem;
        }

        .login-header h1 {
            color: #dc3545;
            font-weight: 700;
            font-size: 2rem;
            margin-bottom: 0.5rem;
        }

        .login-header p {
            color: #6c757d;
            margin: 0;
        }

        .form-group {
            margin-bottom: 1.5rem;
        }

        .form-control {
            border-radius: 8px;
            padding: 0.75rem 1rem;
            border: 1px solid #dee2e6;
            transition: all 0.3s ease;
        }

        .form-control:focus {
            border-color: #dc3545;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
        }

        .input-group-text {
            background: transparent;
            border: 1px solid #dee2e6;
            border-right: none;
        }

        .btn-login {
            background: linear-gradient(135deg, #dc3545 0%, #bb2d3b 100%);
            border: none;
            border-radius: 8px;
            padding: 0.75rem;
            font-weight: 600;
            width: 100%;
            margin-top: 1rem;
            transition: all 0.3s ease;
        }

        .btn-login:hover {
            background: linear-gradient(135deg, #bb2d3b 0%, #a52834 100%);
            transform: translateY(-1px);
        }

        .error-message {
            color: #dc3545;
            text-align: center;
            margin-top: 1rem;
            font-weight: 500;
        }

        .links {
            text-align: center;
            margin-top: 1.5rem;
        }

        .links a {
            color: #6c757d;
            text-decoration: none;
            transition: color 0.3s ease;
            display: block;
            margin: 0.5rem 0;
        }

        .links a:hover {
            color: #dc3545;
        }

        .divider {
            display: flex;
            align-items: center;
            text-align: center;
            margin: 1rem 0;
            color: #6c757d;
        }

        .divider::before,
        .divider::after {
            content: '';
            flex: 1;
            border-bottom: 1px solid #dee2e6;
        }

        .divider span {
            padding: 0 1rem;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-header">
                <h1><i class="fas fa-car-side me-2"></i>WheelDeal</h1>
                <p>Вход в систему</p>
            </div>

            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-text">
                        <i class="fas fa-envelope"></i>
                    </span>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email" TextMode="Email" required></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-text">
                        <i class="fas fa-lock"></i>
                    </span>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Пароль" TextMode="Password" required></asp:TextBox>
                </div>
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Войти" CssClass="btn btn-login" OnClick="btnLogin_Click" />
            
            <asp:Label ID="ErrorMsg" runat="server" CssClass="error-message" Visible="false"></asp:Label>

            <div class="links">
                <div class="divider">
                    <span>или</span>
                </div>
                <a href="~/view/register.aspx" runat="server">
                    <i class="fas fa-user-plus me-1"></i>Зарегистрироваться
                </a>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html> 