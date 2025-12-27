<%@ Page Title="Управление автомобилями" Language="C#" MasterPageFile="~/view/admin/adminmaster.master"
    AutoEventWireup="true" CodeBehind="cars.aspx.cs" Inherits="WebApplication1.view.admin.cars" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid py-4">
        <h3 class="admin-page-title text-center">Управление автомобилями</h3>
        <div class="row">
            <!-- Форма управления автомобилями -->
            <div class="col-lg-4 mb-4">
                <div class="admin-card">
                    <div class="card-body">
                        <div class="text-center mb-4">
                            <asp:Image ID="carImage" runat="server" CssClass="admin-form-image" ImageUrl="~/assets/images/default-car.png" AlternateText="Car Image" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Номер лицензии</label>
                            <asp:TextBox ID="txtLicence" runat="server" CssClass="form-control form-control-admin" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Марка</label>
                            <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control form-control-admin" placeholder="Любая марка"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Модель</label>
                            <asp:TextBox ID="txtModel" runat="server" CssClass="form-control form-control-admin" placeholder="Любая модель"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Цена</label>
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control form-control-admin" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Цвет</label>
                            <asp:DropDownList ID="ddlColor" runat="server" CssClass="form-select form-control-admin">
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
                        <div class="mb-3">
                            <label class="form-label">Класс</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select form-control-admin" />
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
                        <div class="mb-3">
                            <label class="form-label">Статус</label>
                            <asp:DropDownList ID="ddlAvailable" runat="server" CssClass="form-select form-control-admin">
                                <asp:ListItem Text="Доступен" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Забронирован" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <!-- Кнопки управления -->
                        <div class="d-grid gap-2">
                             <asp:Button ID="Save" runat="server" Text="Сохранить" CssClass="btn btn-admin-primary" OnClick="Save_Click" />
                            <asp:Button ID="Edit" runat="server" Text="Редактировать" CssClass="btn btn-admin-secondary" OnClick="Edit_Click" />
                            <asp:Button ID="Delete" runat="server" Text="Удалить" CssClass="btn btn-admin-danger" OnClientClick="return confirm('Вы уверены, что хотите удалить этот автомобиль?');" OnClick="Delete_Click" />
                        </div>

                        <div class="mt-3">
                            <label id="ErrorMsg" runat="server" class="text-danger d-block" style="min-height: 24px;"></label>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Список автомобилей -->
            <div class="col-lg-8">
                <div class="admin-card">
                    <div class="card-body">
                        <div class="d-flex justify-content-end mb-3">
                             <asp:Button ID="btnExport" runat="server" Text="Экспорт в Excel" CssClass="btn btn-admin-primary export-btn" OnClick="btnExport_Click" />
                        </div>
                        <asp:GridView ID="carlist" runat="server" CssClass="table table-hover admin-table"
                            AutoGenerateColumns="False" OnSelectedIndexChanged="carlist_SelectedIndexChanged"
                            DataKeyNames="CPlateNum,Price,CategoryId,CarId" GridLines="None">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-sm btn-select" SelectText="Выбрать" />
                                <asp:BoundField DataField="CPlateNum" HeaderText="Номер лицензии" />
                                <asp:BoundField DataField="Brand" HeaderText="Марка" />
                                <asp:BoundField DataField="Model" HeaderText="Модель" />
                                <asp:BoundField DataField="Price" HeaderText="Цена" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="CategoryName" HeaderText="Класс" />
                                <asp:TemplateField HeaderText="Цвет">
                                    <ItemTemplate>
                                        <div class="color-swatch" style='background-color:<%# Eval("Color") %>;' title='<%# Eval("Color") %>'></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Статус">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server"
                                            Text='<%# Eval("Status").ToString() == "Available" ? "Доступен" : "Забронирован" %>'
                                            CssClass='<%# Eval("Status").ToString() == "Available" ? "badge badge-available" : "badge badge-booked" %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <HeaderStyle CssClass="border-bottom" />
                            <RowStyle CssClass="align-middle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
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
</asp:Content>
