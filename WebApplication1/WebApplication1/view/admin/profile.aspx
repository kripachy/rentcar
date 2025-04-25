<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="WebApplication1.profile" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Complete Profile - WheelDeal</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { background-color: #f8f9fa; padding-top: 40px; }
        .container-box {
            max-width: 450px;
            margin: 0 auto;
            padding: 30px;
            border-radius: 12px;
            background: #dc3545;
            color: white;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
        }
        .form-control {
            margin-bottom: 5px;
            padding: 12px 15px;
            border: none;
            border-radius: 8px;
        }
        .form-control:focus {
            border-color: #ffffff !important;
            box-shadow: 0 0 0 0.2rem rgba(255, 255, 255, 0.5);
        }
        .btn-save {
            background: white;
            color: #dc3545;
            font-weight: bold;
            padding: 12px;
            border: none;
            border-radius: 8px;
            width: 100%;
            margin-top: 15px;
            transition: all 0.3s;
        }
        .btn-save:disabled {
            opacity: 0.6;
            cursor: not-allowed;
        }
        .error-message {
            color: #ffcc00;
            font-size: 12px;
            margin-bottom: 15px;
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-box">
            <h2 class="text-center mb-4">Заполните профиль</h2>

            <div class="form-group">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control"
                    Placeholder="ФИО"
                    oninput="validateName(this, 'nameError')" />
                <div id="nameError" class="error-message"></div>
            </div>

            <div class="form-group">
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"
                    Placeholder="Телефон"
                    oninput="validatePhone(this, 'phoneError')" />
                <div id="phoneError" class="error-message"></div>
            </div>

            <div class="form-group">
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"
                    Placeholder="Адрес"
                    oninput="validateAddress(this, 'addressError')" />
                <div id="addressError" class="error-message"></div>
            </div>

            <asp:Button ID="btnSave" runat="server" Text="СОХРАНИТЬ ПРОФИЛЬ"
                CssClass="btn-save" OnClick="btnSave_Click" Enabled="false" />
        </div>

        <script>
            function validateInput(input, errorId, regex, errorMessage) {
                const errorElement = document.getElementById(errorId);
                const value = input.value;
                if (!regex.test(value) && value !== "") {
                    errorElement.textContent = errorMessage;
                    errorElement.style.display = "block";
                    return false;
                } else {
                    errorElement.style.display = "none";
                    return true;
                }
            }

            function validateName(input, errorId) {
                const isValid = validateInput(
                    input,
                    errorId,
                    /^[a-zA-Zа-яА-ЯёЁ\s'-]+$/g,
                    "Допускаются только буквы"
                );
                checkForm();
                return isValid;
            }

            function validatePhone(input, errorId) {
                const isValid = validateInput(
                    input,
                    errorId,
                    /^[0-9+\-()\s]{6,}$/g,
                    "Допускаются только цифры и символы + - ( )"
                );
                checkForm();
                return isValid;
            }

            function validateAddress(input, errorId) {
                const isValid = validateInput(
                    input,
                    errorId,
                    /^[a-zA-Zа-яА-ЯёЁ0-9\s,.-]+$/g,
                    "Недопустимые символы в адресе"
                );
                checkForm();
                return isValid;
            }

            function checkForm() {
                const nameValid = /^[a-zA-Zа-яА-ЯёЁ\s'-]+$/.test(document.getElementById('<%= txtName.ClientID %>').value);
                const phoneValid = /^[0-9+\-()\s]{6,}$/.test(document.getElementById('<%= txtPhone.ClientID %>').value);
                const addressValid = /^[a-zA-Zа-яА-ЯёЁ0-9\s,.-]+$/.test(document.getElementById('<%= txtAddress.ClientID %>').value);

                document.getElementById('<%= btnSave.ClientID %>').disabled = !(nameValid && phoneValid && addressValid);
            }

            document.addEventListener('DOMContentLoaded', function () {
                document.getElementById('<%= txtName.ClientID %>').addEventListener('input', checkForm);
                document.getElementById('<%= txtPhone.ClientID %>').addEventListener('input', checkForm);
                document.getElementById('<%= txtAddress.ClientID %>').addEventListener('input', checkForm);
                checkForm();
            });
        </script>
    </form>
</body>
</html>
