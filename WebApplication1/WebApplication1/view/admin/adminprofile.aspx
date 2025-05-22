<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="adminprofile.aspx.cs" Inherits="WebApplication1.view.admin.adminprofile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="text-danger text-center mb-4">Профиль Администратора</h2>

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