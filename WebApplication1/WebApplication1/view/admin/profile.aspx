<%@ Page Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true"
    CodeBehind="profile.aspx.cs" Inherits="WebApplication1.profile" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <style>
            .border-dashed {
                border: 2px dashed #dee2e6 !important;
            }

            .upload-area {
                transition: all 0.3s ease;
                cursor: pointer;
            }

            .upload-area:hover {
                background-color: #f1f3f5 !important;
                border-color: #00bcd4 !important;
            }

            .license-preview-container img {
                max-width: 100%;
                height: auto;
                transition: transform 0.3s ease;
            }

            .license-preview-container:hover img {
                transform: scale(1.02);
            }

            .font-weight-bold {
                font-weight: 600;
            }
        </style>
        <div class="container py-4">
            <div class="row justify-content-center">
                <div class="col-lg-10">
                    <div class="card shadow border-0">
                        <div class="card-header text-white"
                            style="background: linear-gradient(90deg, #000000, #00bcd4);">
                            <div class="d-flex align-items-center justify-content-between">
                                <div>
                                    <h3 class="mb-0">Профиль WheelDeal</h3>
                                    <small class="opacity-75">Используйте ник, который увидят в отзывах</small>
                                </div>
                                <i class="fas fa-user-astronaut fa-2x opacity-75"></i>
                            </div>
                        </div>
                        <div class="card-body bg-light">
                            <div class="row g-4">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtUsername" class="form-label">Имя пользователя (никнейм)</label>
                                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"
                                            placeholder="Например: speedster_22"></asp:TextBox>
                                        <small class="text-muted">Будет использовано в комментариях и отзывах.</small>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtName" class="form-label">ФИО</label>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"
                                            placeholder="Введите ФИО (например: Иванов Иван Иванович)"></asp:TextBox>
                                        <small class="text-muted">Формат: Фамилия Имя Отчество</small>
                                    </div>
                                </div>
                            </div>

                            <div class="row g-4">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtPhone" class="form-label">Телефон</label>
                                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"
                                            placeholder="Введите номер телефона (например: 80291234567)"></asp:TextBox>
                                        <small class="text-muted">Формат: 80XXXXXXXXX</small>
                                    </div>
                                </div>
                                <div class="col-md-6">
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
                                </div>
                            </div>

                            <div class="p-3 rounded border bg-white mb-4">
                                <h5 class="mb-3">Email и пароль</h5>
                                <div class="row g-3">
                                    <div class="col-md-6">
                                        <label for="txtNewEmail" class="form-label">Новый Email (необязательно)</label>
                                        <asp:TextBox ID="txtNewEmail" runat="server" CssClass="form-control"
                                            placeholder="Введите новую почту"></asp:TextBox>
                                        <small class="text-muted">Оставьте пустым, чтобы не менять</small>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="txtCurrentPassword" class="form-label">Текущий Пароль</label>
                                        <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password"
                                            CssClass="form-control" placeholder="Введите текущий пароль"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="txtNewPassword" class="form-label">Новый Пароль
                                            (необязательно)</label>
                                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"
                                            CssClass="form-control" placeholder="Введите новый пароль"></asp:TextBox>
                                        <small class="text-muted">Мин. 8 символов.</small>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="txtConfirmNewPassword" class="form-label">Подтвердите Новый
                                            Пароль</label>
                                        <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password"
                                            CssClass="form-control" placeholder="Повторите новый пароль"></asp:TextBox>
                                    </div>
                                </div>
                                <asp:Label ID="lblPasswordChangeMsg" runat="server" Visible="false"
                                    CssClass="d-block text-center mt-3"></asp:Label>
                            </div>

                            <div class="p-4 rounded border bg-white mb-4 shadow-sm">
                                <h5 class="mb-4 d-flex align-items-center">
                                    <i class="fas fa-id-card me-2 text-info"></i>
                                    Водительское удостоверение
                                </h5>

                                <asp:Panel ID="pnlLicensePreview" runat="server" Visible="false"
                                    CssClass="mb-4 text-center">
                                    <div class="license-preview-container position-relative d-inline-block">
                                        <asp:Image ID="imgLicensePreview" runat="server"
                                            CssClass="img-fluid rounded shadow"
                                            style="max-height: 250px; border: 3px solid #eee;" />
                                        <div class="mt-3">
                                            <asp:LinkButton ID="btnDeleteLicense" runat="server"
                                                CssClass="btn btn-sm btn-danger rounded-pill px-3"
                                                OnClick="btnDeleteLicense_Click"
                                                OnClientClick="return confirm('Вы уверены, что хотите удалить этот документ?');">
                                                <i class="fas fa-trash-alt me-1"></i> Удалить и загрузить заново
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="pnlUploadLicense" runat="server">
                                    <div class="row g-3">
                                        <div class="col-md-12">
                                            <label for="txtLicenseExpiryDate" class="form-label font-weight-bold">Срок
                                                действия удостоверения до</label>
                                            <asp:TextBox ID="txtLicenseExpiryDate" runat="server"
                                                CssClass="form-control form-control-lg" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="upload-area p-4 border-dashed rounded text-center bg-light mb-3"
                                                id="drop-area">
                                                <i class="fas fa-cloud-upload-alt fa-3x text-muted mb-3"></i>
                                                <p class="mb-2">Выберите скан или четкое фото удостоверения</p>
                                                <asp:FileUpload ID="fuLicense" runat="server" CssClass="form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <asp:Button ID="btnUploadLicense" runat="server"
                                                Text="Отправить на проверку"
                                                CssClass="btn btn-info btn-lg w-100 text-white font-weight-bold shadow-sm"
                                                OnClick="btnUploadLicense_Click" />
                                        </div>
                                    </div>
                                </asp:Panel>

                                <div class="mt-3 p-3 rounded bg-light border-start border-4 border-info">
                                    <asp:Label ID="lblLicenseStatus" runat="server" CssClass="d-block h6 mb-0">
                                    </asp:Label>
                                </div>
                            </div>

                            <div class="d-grid gap-2">
                                <asp:Button ID="btnSave" runat="server" Text="Сохранить изменения"
                                    CssClass="btn btn-dark" OnClick="btnSave_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>