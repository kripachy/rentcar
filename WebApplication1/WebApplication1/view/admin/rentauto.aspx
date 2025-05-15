<%@ Page Title="Аренда авто" Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="rentauto.aspx.cs" Inherits="WebApplication1.view.admin.rentauto" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    WheelDeal - Аренда авто
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body { font-family: 'Segoe UI', sans-serif; }
        .card-custom { border-radius: 16px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }
        .car-image-wrapper {
            position: relative;
            max-width: 100%;
            max-height: 450px;
            margin-bottom: 1rem;
        }
        .car-image {
            width: 100%;
            height: auto;
            border-radius: 12px;
            object-fit: cover;
            max-height: 450px;
            display: block;
        }
        .arrow-btn {
            position: absolute;
            top: 50%;
            transform: translateY(-50%);
            background: transparent;
            border: 3px solid white;
            color: white;
            font-size: 2.5rem;
            font-weight: bold;
            width: 64px;
            height: 64px;
            border-radius: 50%;
            cursor: pointer;
            opacity: 0;
            transition: opacity 0.3s ease;
            user-select: none;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        .car-image-wrapper:hover .arrow-btn { opacity: 1; }
        .arrow-left { left: 10px; }
        .arrow-right { right: 10px; }
        .color-circle {
            width: 32px; height: 32px; border-radius: 50%; display: inline-block;
            border: 2px solid #ccc; cursor: pointer; transition: 0.2s;
            margin-right: 6px;
        }
        .color-circle:hover, .selected { border-color: #dc3545; }
        .form-label { font-weight: 600; }
        #carDetails { min-height: 40px; }
        #dotsContainer {
            position: absolute;
            bottom: 10px;
            left: 50%;
            transform: translateX(-50%);
            display: flex;
            gap: 8px;
        }
        .dot {
            width: 16px; height: 16px; border-radius: 50%;
            background-color: #ccc;
            cursor: pointer;
            border: none;
            outline: none;
            padding: 0;
        }
        .dot.active {
            background-color: #dc3545;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <div class="card-custom p-4 my-5">
                <div class="row align-items-center">
                    <!-- Блок параметров слева -->
                    <div class="col-lg-6">
                        <h3>Выберите параметры</h3>

                        <label class="form-label" for="ddlBrand">Марка:</label>
                        <asp:DropDownList ID="ddlBrand" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" CssClass="form-select" />
                        <br />

                        <label class="form-label">Модель:</label>
                        <asp:Label ID="lblModel" runat="server" CssClass="form-control-plaintext"></asp:Label>
                        <br />

                        <label class="form-label">Цена за аренду (в день):</label>
                        <asp:Label ID="lblPrice" runat="server" CssClass="form-control-plaintext text-danger fs-5"></asp:Label>
                        <br />

                        <label class="form-label">Цвет:</label>
                        <div>
                            <span class="color-circle" style="background-color: white;" onclick="setColor('white', this)"></span>
                            <span class="color-circle" style="background-color: black;" onclick="setColor('black', this)"></span>
                            <span class="color-circle" style="background-color: gray;" onclick="setColor('gray', this)"></span>
                            <span class="color-circle" style="background-color: red;" onclick="setColor('red', this)"></span>
                            <span class="color-circle" style="background-color: blue;" onclick="setColor('blue', this)"></span>
                            <span class="color-circle" style="background-color: yellow;" onclick="setColor('yellow', this)"></span>
                            <span class="color-circle" style="background-color: green;" onclick="setColor('green', this)"></span>
                        </div>
                        <asp:HiddenField ID="hfSelectedColor" runat="server" />

                        <br />

                        <label class="form-label" for="txtStartDate">Начало аренды (дата):</label>
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date" />
                        <br />

                        <label class="form-label" for="txtStartTime">Начало аренды (время):</label>
                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control" TextMode="Time" />
                        <br />

                        <label class="form-label" for="txtEndDate">Конец аренды (дата):</label>
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date" />
                        <br />

                        <label class="form-label" for="txtEndTime">Конец аренды (время):</label>
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" TextMode="Time" />
                        <br />

                        <asp:Button ID="btnRent" runat="server" Text="Арендовать" CssClass="btn btn-danger mt-3" OnClick="btnRent_Click" />

                        <div id="carDetails" runat="server" class="mt-3 text-danger"></div>

                        <asp:HiddenField ID="hfImageFiles" runat="server" />
                        <asp:HiddenField ID="hfCurrentImageIndex" runat="server" />
                    </div>

                    <!-- Блок с картинкой машины справа -->
                    <div class="col-lg-6">
                        <div class="car-image-wrapper">
                            <asp:Image ID="carImage" runat="server" CssClass="car-image" ImageUrl="~/assets/images/default-car.png" />
                            <asp:Button ID="btnPrevImage" runat="server" Text="&lt;" CssClass="arrow-btn arrow-left" OnClick="btnPrevImage_Click" />
                            <asp:Button ID="btnNextImage" runat="server" Text="&gt;" CssClass="arrow-btn arrow-right" OnClick="btnNextImage_Click" />
                            <div id="dotsContainer" runat="server"></div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function setColor(color, elem) {
            // Снять выделение со всех кружков
            var circles = document.querySelectorAll('.color-circle');
            circles.forEach(c => c.classList.remove('selected'));

            // Выделить выбранный кружок
            elem.classList.add('selected');

            // Записать в скрытое поле и вызвать постбэк
            var hf = document.getElementById('<%= hfSelectedColor.ClientID %>');
            hf.value = color;

            __doPostBack('<%= hfSelectedColor.UniqueID %>', ''); // вызвать постбэк, чтобы сервер узнал цвет и загрузил картинки
        }
    </script>
</asp:Content>
