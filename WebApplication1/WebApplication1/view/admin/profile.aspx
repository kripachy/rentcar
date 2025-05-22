<%@ Page Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="WebApplication1.profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-8">
                <div class="card shadow">
                    <div class="card-header bg-danger text-white">
                        <h3 class="mb-0">Профиль пользователя</h3>
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label for="txtName" class="form-label">ФИО</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Введите ФИО (например: Иванов Иван Иванович)"></asp:TextBox>
                            <small class="text-muted">Формат: Фамилия Имя Отчество</small>
                        </div>
                        <div class="mb-3">
                            <label for="txtPhone" class="form-label">Телефон</label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Введите номер телефона (например: 80291234567)"></asp:TextBox>
                            <small class="text-muted">Формат: 80XXXXXXXXX</small>
                        </div>
                        <div class="mb-3">
                            <label for="ddlCity" class="form-label">Город</label>
                            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Выберите город" Value="" />
                                <asp:ListItem Text="Минск" Value="Минск" />
                                <asp:ListItem Text="Брест" Value="Брест" />
                                <asp:ListItem Text="Витебск" Value="Витебск" />
                                <asp:ListItem Text="Гомель" Value="Гомель" />
                                <asp:ListItem Text="Гродно" Value="Гродно" />
                                <asp:ListItem Text="Могилев" Value="Могилев" />
                                <asp:ListItem Text="Барановичи" Value="Барановичи" />
                                <asp:ListItem Text="Борисов" Value="Борисов" />
                                <asp:ListItem Text="Пинск" Value="Пинск" />
                                <asp:ListItem Text="Орша" Value="Орша" />
                                <asp:ListItem Text="Мозырь" Value="Мозырь" />
                                <asp:ListItem Text="Солигорск" Value="Солигорск" />
                                <asp:ListItem Text="Новополоцк" Value="Новополоцк" />
                                <asp:ListItem Text="Лида" Value="Лида" />
                                <asp:ListItem Text="Молодечно" Value="Молодечно" />
                                <asp:ListItem Text="Полоцк" Value="Полоцк" />
                                <asp:ListItem Text="Жлобин" Value="Жлобин" />
                                <asp:ListItem Text="Светлогорск" Value="Светлогорск" />
                                <asp:ListItem Text="Речица" Value="Речица" />
                                <asp:ListItem Text="Жодино" Value="Жодино" />
                                <asp:ListItem Text="Слуцк" Value="Слуцк" />
                                <asp:ListItem Text="Кобрин" Value="Кобрин" />
                                <asp:ListItem Text="Волковыск" Value="Волковыск" />
                                <asp:ListItem Text="Калинковичи" Value="Калинковичи" />
                                <asp:ListItem Text="Сморгонь" Value="Сморгонь" />
                                <asp:ListItem Text="Рогачев" Value="Рогачев" />
                                <asp:ListItem Text="Дзержинск" Value="Дзержинск" />
                                <asp:ListItem Text="Новогрудок" Value="Новогрудок" />
                                <asp:ListItem Text="Бобруйск" Value="Бобруйск" />
                            </asp:DropDownList>
                        </div>

                        <hr />

                        <h5>Изменить Email и Пароль</h5>

                        <div class="mb-3">
                            <label for="txtNewEmail" class="form-label">Новый Email (необязательно)</label>
                            <asp:TextBox ID="txtNewEmail" runat="server" CssClass="form-control" placeholder="Введите новую почту"></asp:TextBox>
                             <small class="text-muted">Оставьте пустым, чтобы не менять</small>
                        </div>

                         <div class="mb-3">
                            <label for="txtCurrentPassword" class="form-label">Текущий Пароль</label>
                            <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Введите текущий пароль"></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label for="txtNewPassword" class="form-label">Новый Пароль (необязательно)</label>
                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Введите новый пароль"></asp:TextBox>
                             <small class="text-muted">Оставьте пустым, чтобы не менять. Мин. 8 символов.</small>
                        </div>

                         <div class="mb-3">
                            <label for="txtConfirmNewPassword" class="form-label">Подтвердите Новый Пароль</label>
                            <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Повторите новый пароль"></asp:TextBox>
                        </div>

                         <asp:Label ID="lblPasswordChangeMsg" runat="server" Visible="false" CssClass="d-block text-center"></asp:Label>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnSave" runat="server" Text="Сохранить Изменения" CssClass="btn btn-danger" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
