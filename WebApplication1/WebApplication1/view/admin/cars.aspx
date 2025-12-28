<%@ Page Title="Управление автомобилями" Language="C#" MasterPageFile="~/view/admin/adminmaster.master"
    AutoEventWireup="true" CodeBehind="cars.aspx.cs" Inherits="WebApplication1.view.admin.cars" ValidateRequest="false"
    %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <div class="py-4 admin-cars-page">
            <div class="row g-4 mt-3">
                <div class="col-lg-4">
                    <div id="statusAlert" runat="server" class="alert d-none">
                        <i id="statusIcon" runat="server" class="fas me-2"></i>
                        <span id="ErrorMsg" runat="server"></span>
                    </div>

                    <div class="admin-form-card admin-card">
                        <div class="admin-card-header">
                            <div>
                                <div class="admin-card-title">
                                    <i class="fas fa-edit me-2"></i>Параметры автомобиля
                                </div>
                                <div class="admin-card-subtitle">Заполни поля и выбери действие</div>
                            </div>
                        </div>

                        <div class="admin-card-body">
                            <div class="mb-3">
                                <label class="form-label">Государственный Номер</label>
                                <asp:TextBox ID="txtLicence" runat="server" CssClass="form-control" placeholder="A123BC-7"
                                    autocomplete="off" />
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Марка</label>
                                    <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control" autocomplete="off"
                                        placeholder="Toyota" />
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Модель</label>
                                    <asp:TextBox ID="txtModel" runat="server" CssClass="form-control" autocomplete="off"
                                        placeholder="Camry" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Цена/День ($)</label>
                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" autocomplete="off"
                                        placeholder="50" />
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Цвет</label>
                                    <asp:DropDownList ID="ddlColor" runat="server" CssClass="form-select" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Категория</label>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" />
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Статус</label>
                                    <asp:DropDownList ID="ddlAvailable" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Доступен" Value="1" />
                                        <asp:ListItem Text="Забронирован" Value="0" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="d-grid gap-2">
                                <asp:Button ID="Save" runat="server" Text="Добавить Новый Автомобиль"
                                    CssClass="btn btn-primary" OnClick="Save_Click" />

                                <div class="row g-2">
                                    <div class="col-6">
                                        <asp:Button ID="Edit" runat="server" Text="Сохранить" CssClass="btn btn-info w-100"
                                            OnClick="Edit_Click" />
                                    </div>
                                    <div class="col-6">
                                        <asp:Button ID="Delete" runat="server" Text="Удалить"
                                            CssClass="btn btn-danger w-100"
                                            OnClientClick="return confirm('Вы уверены, что хотите удалить этот автомобиль?');"
                                            OnClick="Delete_Click" />
                                    </div>
                                </div>

                                <div class="row g-2">
                                    <div class="col-12">
                                        <button type="button" class="btn btn-outline-secondary w-100" data-bs-toggle="modal" data-bs-target="#serverImagesModal">
                                            <i class="fas fa-images me-1"></i>Фото
                                        </button>
                                    </div>
                                </div>

                                <div class="selected-images-preview mt-3" id="selectedImagesPreview" style="display: none;">
                                    <div class="d-flex justify-content-between align-items-center mb-2">
                                        <span class="small text-muted">Выбранные изображения:</span>
                                        <button type="button" class="btn btn-sm btn-outline-danger" onclick="clearSelectedImages()">
                                            <i class="fas fa-times"></i> Очистить
                                        </button>
                                    </div>
                                    <div class="selected-images-grid" id="selectedImagesGrid">
                                        <!-- Selected images will be displayed here -->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-8">
                    <div class="admin-table-container admin-card">
                        <div class="admin-table-header admin-card-header">
                            <h5><i class="fas fa-list-ul me-2"></i>Автопарк</h5>
                        </div>

                        <div class="admin-card-body p-0">
                            <div class="table-responsive">
                                <asp:GridView ID="carlist" runat="server" CssClass="admin-table table table-hover table-sm mb-0"
                                    AutoGenerateColumns="False" OnSelectedIndexChanged="carlist_SelectedIndexChanged"
                                    DataKeyNames="CPlateNum" GridLines="None"
                                    HeaderStyle-CssClass="admin-table-head"
                                    RowStyle-CssClass="admin-table-row"
                                    AlternatingRowStyle-CssClass="admin-table-row alt">
                                    <Columns>
                                        <asp:BoundField DataField="Brand" HeaderText="Марка" />
                                        <asp:BoundField DataField="Model" HeaderText="Модель" />
                                        <asp:BoundField DataField="CPlateNum" HeaderText="Номер" />
                                        <asp:TemplateField HeaderText="Цена/день">
                                            <ItemTemplate>
                                                <span class="admin-price">$<%# Eval("Price") %></span>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="text-end" />
                                            <HeaderStyle CssClass="text-end" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CategoryName" HeaderText="Класс" />
                                        <asp:TemplateField HeaderText="Статус">
                                            <ItemTemplate>
                                                <span
                                                    class='admin-status-badge <%# Eval("Status").ToString() == "Available" ? "available" : "booked" %>'>
                                                    <%# Eval("Status").ToString()=="Available" ? "Доступен" : "Забронирован" %>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Действие">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" CommandName="Select" CssClass="btn btn-sm btn-outline-secondary">
                                                    Выбрать
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div class="admin-table-footer">
                                <asp:Button ID="btnExport" runat="server" Text="Экспорт в Excel" CssClass="btn btn-outline-secondary"
                                    OnClick="btnExport_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Модальное окно -->
            <div class="modal fade" id="serverImagesModal" tabindex="-1">
                <div class="modal-dialog modal-xl">
                    <div class="modal-content">
                        <div class="modal-header bg-dark text-white">
                            <h5 class="modal-title"><i class="fas fa-images me-2"></i>Библиотека Изображений</h5>
                            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
                                aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="upImages" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="mb-4 p-3 border rounded bg-light">
                                        <div class="row g-3 align-items-center">
                                            <div class="col-md-8">
                                                <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control"
                                                    AllowMultiple="true" />
                                            </div>
                                            <div class="col-md-4">
                                                <asp:LinkButton ID="btnUploadImage" runat="server"
                                                    CssClass="btn btn-primary w-100" OnClick="btnUploadImage_Click">
                                                    <i class="fas fa-cloud-upload-alt me-1"></i> Загрузить Изображения
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="d-flex justify-content-between align-items-center mb-3">
                                        <h6 class="mb-0 text-muted">Доступные Изображения</h6>
                                        <span id="imgSummary" runat="server" class="badge bg-info text-dark p-2">Управление изображениями</span>
                                    </div>

                                    <div class="image-library-grid">
                                        <asp:Repeater ID="repImages" runat="server"
                                            OnItemCommand="repImages_ItemCommand">
                                            <ItemTemplate>
                                                <div class="image-item card shadow-sm">
                                                    <div class="image-thumb">
                                                        <img src='<%# Eval("PreviewUrl") %>' class="card-img-top" alt='<%# Eval("OriginalName") %>' />

                                                        <asp:LinkButton ID="btnDeleteImage" runat="server"
                                                            CommandName="DeleteImage"
                                                            CommandArgument='<%# Eval("FileId") %>'
                                                            CssClass="btn btn-sm btn-danger delete-btn"
                                                            OnClientClick="return confirm('Вы уверены, что хотите удалить это изображение?');">
                                                            <i class="fas fa-trash"></i>
                                                        </asp:LinkButton>

                                                        <div class="image-meta">
                                                            <div class="image-name" title='<%# Eval("OriginalName") %>'>
                                                                <%# Eval("OriginalName") %>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="card-body p-2">
                                                        <div class="image-controls">
                                                            <div class="form-check mb-2">
                                                                <asp:RadioButton ID="rbMain" runat="server" GroupName="MainImage" CssClass="form-check-input custom-radio-input" />
                                                                <label class="form-check-label custom-radio-label" for='<%# "rbMain_" + Container.ItemIndex %>'>
                                                                    <i class="fas fa-star me-1"></i>Главное
                                                                </label>
                                                            </div>
                                                            <div class="form-check">
                                                                <asp:CheckBox ID="chkAttach" runat="server" CssClass="form-check-input custom-checkbox-input" />
                                                                <label class="form-check-label custom-checkbox-label" for='<%# "chkAttach_" + Container.ItemIndex %>'>
                                                                    <i class="fas fa-images me-1"></i>Галерея
                                                                </label>
                                                            </div>
                                                        </div>

                                                        <asp:HiddenField ID="hfFileId" runat="server" Value='<%# Eval("FileId") %>' />
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>

                                    <asp:Panel ID="panelNoImages" runat="server" Visible="false"
                                        CssClass="text-center py-5">
                                        <i class="fas fa-image fa-4x mb-3 text-muted"></i>
                                        <p class="text-muted fs-5">Нет доступных изображений. Загрузите новые фотографии.</p>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnUploadImage" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer border-0">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                                <i class="fas fa-times me-2"></i>Закрыть
                            </button>
                            <button type="button" class="btn btn-primary px-4" data-bs-dismiss="modal">
                                <i class="fas fa-check me-2"></i>Готово
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>