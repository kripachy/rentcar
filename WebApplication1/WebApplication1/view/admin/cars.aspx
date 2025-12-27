<%@ Page Title="Управление автомобилями" Language="C#" MasterPageFile="~/view/admin/adminmaster.master"
    AutoEventWireup="true" CodeBehind="cars.aspx.cs" Inherits="WebApplication1.view.admin.cars" ValidateRequest="false"
    %>

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

            .btn-outline-danger {
                border-color: #00bcd4;
                color: #00bcd4;
                transition: all 0.3s ease;
            }

            .car-image {
                width: 200px;
                height: 200px;
                object-fit: contain;
            }

            .btn-outline-danger:hover {
                background-color: #00bcd4;
                color: white;
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

            .table th {
                background-color: #000000;
                color: #ffffff;
                font-weight: 600;
                border-color: #00bcd4;
            }

            .table-striped tbody tr:nth-of-type(odd) {
                background-color: rgba(0, 188, 212, 0.05);
            }

            .card {
                border: 1px solid #dee2e6;
                border-radius: 15px;
                box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
            }

            .card-body {
                background-color: #ffffff;
            }

            h3.text-danger {
                color: #000000 !important;
                font-weight: 700;
            }

            #ErrorMsg {
                transition: all 0.3s ease;
            }

            .badge.bg-success {
                background-color: #00bcd4 !important;
            }

            .btn-success {
                background-color: #00bcd4;
                border-color: #00bcd4;
            }

            .btn-success:hover {
                background-color: #0097a7;
                border-color: #0097a7;
            }
        </style>

        <div class="container">
            <div class="row">
                <div class="col-md-4">
                    <div class="row mb-3">
                        <div class="col text-center">
                            <h3 class="fw-bold text-center" style="color: #000000;">Управление автомобилями</h3>
                            <img id="carImage" runat="server" class="car-image" src="~/assets/images/default-car.png"
                                alt="Car Image" />
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-body">
                            <div class="form-group mb-3">
                                <label>Номер лицензии</label>
                                <asp:TextBox ID="txtLicence" runat="server" CssClass="form-control" autocomplete="off">
                                </asp:TextBox>
                            </div>
                            <div class="form-group mb-3">
                                <label>Марка</label>
                                <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control"
                                    placeholder="Любая марка"></asp:TextBox>
                            </div>
                            <div class="form-group mb-3">
                                <label>Модель</label>
                                <asp:TextBox ID="txtModel" runat="server" CssClass="form-control"
                                    placeholder="Любая модель"></asp:TextBox>
                            </div>
                            <div class="form-group mb-3">
                                <label>Цена</label>
                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" autocomplete="off">
                                </asp:TextBox>
                            </div>
                            <div class="form-group mb-3">
                                <label>Цвет</label>
                                <asp:DropDownList ID="ddlColor" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Выберите цвет" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Красный" Value="Red"></asp:ListItem>
                                    <asp:ListItem Text="Синий" Value="Blue"></asp:ListItem>
                                    <asp:ListItem Text="Зелёный" Value="Green"></asp:ListItem>
                                    <asp:ListItem Text="Чёрный" Value="Black"></asp:ListItem>
                                    <asp:ListItem Text="Белый" Value="White"></asp:ListItem>
                                    <asp:ListItem Text="Жёлтый" Value="Yellow"></asp:ListItem>
                                    <asp:ListItem Text="Серый" Value="Gray"></asp:ListItem>
                                    <asp:ListItem Text="Оранжевый" Value="Orange"></asp:ListItem>
                                    <asp:ListItem Text="Фиолетовый" Value="Purple"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group mb-3">
                                <label>Класс</label>
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" />
                            </div>
                            <div class="border rounded p-3 mb-3">
                                <strong>Главное фото</strong>
                                <div class="d-flex align-items-center gap-3 mt-2">
                                    <img id="imgMainPreview" runat="server" class="car-image"
                                        src="~/assets/images/Слой 1.png" alt="Main" />
                                    <button type="button" class="btn btn-primary btn-sm" data-bs-toggle="modal"
                                        data-mode="main" data-bs-target="#serverImagesModal">Выбрать
                                        изображение</button>
                                </div>
                            </div>

                            <div class="border rounded p-3 mb-3">
                                <strong>Галерея</strong>
                                <div class="mt-2">
                                    <button type="button" class="btn btn-outline-primary btn-sm" data-bs-toggle="modal"
                                        data-mode="gallery" data-bs-target="#serverImagesModal">Выбрать
                                        изображение</button>
                                </div>
                            </div>

                            <asp:HiddenField ID="hfSelectMode" runat="server" Value="main" />

                            <div class="modal fade" id="serverImagesModal" tabindex="-1" aria-hidden="true">
                                <div class="modal-dialog modal-lg modal-dialog-scrollable">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title">Фото на сервере</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal"
                                                aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            <asp:UpdatePanel ID="upImages" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="d-flex gap-2 mb-2">
                                                        <asp:FileUpload ID="fileUpload" runat="server"
                                                            CssClass="form-control form-control-sm" />
                                                        <asp:Button ID="btnUploadImage" runat="server" Text="Загрузить"
                                                            CssClass="btn btn-outline-secondary btn-sm"
                                                            OnClick="btnUploadImage_Click" />
                                                    </div>
                                                    <asp:Panel ID="panelNoImages" runat="server" Visible="false"
                                                        CssClass="alert alert-info">
                                                        Пока нет загруженных фото. Загрузите файл и попробуйте снова.
                                                    </asp:Panel>
                                                    <asp:Repeater ID="repImages" runat="server"
                                                        OnItemCommand="repImages_ItemCommand">
                                                        <HeaderTemplate>
                                                            <div class="row">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div class="col-6 col-md-4 mb-3">
                                                                <div class="border rounded p-2 h-100 text-center">
                                                                    <asp:HiddenField ID="hfFileId" runat="server"
                                                                        Value='<%# Eval("FileId") %>' />
                                                                    <img src='<%# Eval("PreviewUrl") %>'
                                                                        class="img-fluid mb-2"
                                                                        style="max-height:120px;object-fit:cover;" />
                                                                    <div class="form-check">
                                                                        <asp:RadioButton ID="rbMain" runat="server"
                                                                            GroupName="MainImage"
                                                                            CssClass="form-check-input" />
                                                                        <label class="form-check-label">Главное
                                                                            фото</label>
                                                                    </div>
                                                                    <div class="form-check">
                                                                        <asp:CheckBox ID="chkAttach" runat="server"
                                                                            CssClass="form-check-input" />
                                                                        <label class="form-check-label">Добавить в
                                                                            галерею</label>
                                                                    </div>
                                                                    <asp:LinkButton ID="btnDeleteImage" runat="server"
                                                                        CommandName="DeleteImage"
                                                                        CommandArgument='<%# Eval("FileId") %>'
                                                                        CssClass="btn btn-sm btn-link text-danger p-0">
                                                                        Удалить
                                                                    </asp:LinkButton>
                                                                    <small class="text-muted d-block"
                                                                        style="font-size: 0.75rem; word-break: break-all;">
                                                                        <%# Eval("OriginalName") %>
                                                                    </small>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                        </div>
                                        </FooterTemplate>
                                        </asp:Repeater>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnUploadImage" />
                                        </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary"
                                            data-bs-dismiss="modal">Закрыть</button>
                                        <button type="button" class="btn btn-primary"
                                            data-bs-dismiss="modal">Выбрать</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group mb-3">
                            <label>Статус</label>
                            <asp:DropDownList ID="ddlAvailable" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Доступен" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Забронирован" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <br />
                        <br />
                        <div class="form-group mb-2">
                            <label id="ErrorMsg" runat="server" class="text-danger d-block"
                                style="min-height: 24px;"></label>
                        </div>

                        <asp:Button ID="Edit" runat="server" Text="Редактировать" CssClass="btn btn-danger"
                            OnClick="Edit_Click" />
                        <asp:Button ID="Save" runat="server" Text="Сохранить" CssClass="btn btn-danger"
                            OnClick="Save_Click" />
                        <asp:Button ID="Delete" runat="server" Text="Удалить" CssClass="btn btn-danger"
                            OnClientClick="return confirm('Вы уверены, что хотите удалить этот автомобиль?');"
                            OnClick="Delete_Click" />
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <asp:GridView ID="carlist" runat="server" CssClass="table table-bordered table-striped"
                            AutoGenerateColumns="False" OnSelectedIndexChanged="carlist_SelectedIndexChanged"
                            DataKeyNames="CPlateNum,Price,CategoryId,CarId">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" ButtonType="Button"
                                    ControlStyle-CssClass="btn btn-sm btn-outline-danger" SelectText="Выбрать" />
                                <asp:BoundField DataField="CPlateNum" HeaderText="Номер лицензии" />
                                <asp:BoundField DataField="Brand" HeaderText="Марка" />
                                <asp:BoundField DataField="Model" HeaderText="Модель" />
                                <asp:BoundField DataField="Price" HeaderText="Цена" DataFormatString="{0:$#,0}" />
                                <asp:BoundField DataField="CategoryName" HeaderText="Класс" />
                                <asp:TemplateField HeaderText="Цвет">
                                    <ItemTemplate>
                                        <div style='width:20px; height:20px; border-radius:50%; background-color:<%# Eval("Color") %>; border:1px solid #ccc;'
                                            title='<%# Eval("Color") %>'></div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Статус">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server"
                                            Text='<%# Eval("Status").ToString() == "Available" ? "Доступен" : "Забронирован" %>'
                                            CssClass='<%# Eval("Status").ToString() == "Available" ? "badge bg-success" : "badge bg-danger" %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <asp:Button ID="btnExport" runat="server" Text="Экспорт в Excel"
                            CssClass="btn btn-success export-btn" OnClick="btnExport_Click" />
                    </div>
                </div>
            </div>
        </div>
        </div>
    </asp:Content>