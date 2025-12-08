<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="adminprofile.aspx.cs" Inherits="WebApplication1.view.admin.adminprofile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body {
            background-color: #ffffff;
            color: #000000;
        }

        .form-control:focus {
            border-color: #00bcd4 !important;
            box-shadow: 0 0 0 0.2rem rgba(0, 188, 212, 0.25);
        }

        .form-control {
            border: 2px solid #dee2e6;
            border-radius: 8px;
            transition: all 0.3s ease;
        }

        .form-control:hover {
            border-color: #00bcd4;
        }

        .btn-danger {
            background-color: #00bcd4;
            border-color: #00bcd4;
            color: white;
            transition: all 0.3s ease;
        }

        .btn-danger:hover {
            background-color: #0097a7;
            border-color: #0097a7;
            color: white;
        }

        .card {
            border: 1px solid #dee2e6;
            border-radius: 15px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
        }

        .card-body {
            background-color: #ffffff;
        }

        h2.text-danger {
            color: #000000 !important;
            font-weight: 700;
        }
    </style>

    <div class="container mt-4">
        <h2 class="text-center mb-4" style="color: #000000; font-weight: 700;">Профиль Администратора</h2>

        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-body">
                        <div class="mb-3">
                            <label for="txtCurrentEmail" class="form-label">Текущий Email</label>
                            <asp:TextBox ID="txtCurrentEmail" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtNewEmail" class="form-label">Новый Email (необязательно)</label>
                            <asp:TextBox ID="txtNewEmail" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <hr />
                        <div class="mb-3">
                            <label for="txtCurrentPassword" class="form-label">Текущий Пароль</label>
                            <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtNewPassword" class="form-label">Новый Пароль (необязательно)</label>
                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                             <small class="form-text text-muted">Оставьте пустым, чтобы не менять. Мин. 8 символов.</small>
                        </div>
                         <div class="mb-3">
                            <label for="txtConfirmNewPassword" class="form-label">Подтвердите Новый Пароль</label>
                            <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                        </div>

                         <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="d-block text-center mt-3"></asp:Label>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnSave" runat="server" Text="Сохранить Изменения" CssClass="btn btn-danger" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 