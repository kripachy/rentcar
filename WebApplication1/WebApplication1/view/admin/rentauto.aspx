<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rentauto.aspx.cs" Inherits="WebApplication1.view.admin.rentauto" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Аренда автомобиля - WheelDeal</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { background-color: #f8f9fa; }
        .car-image {
            max-width: 100%;
            height: auto;
            object-fit: contain;
            margin-bottom: 10px;
        }
        .color-circle {
            width: 30px;
            height: 30px;
            border-radius: 50%;
            display: inline-block;
            margin: 5px;
            border: 2px solid #ddd;
            cursor: pointer;
        }
        .selected {
            border-color: #000;
        }
        .color-container {
            display: flex;
            flex-wrap: wrap;
            gap: 5px;
        }
        .btn-red { background-color: #dc3545; color: white; }
        .btn-red:hover { background-color: #bb2d3b; }
        .form-control:focus {
            border-color: #dc3545 !important;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <h3 class="text-danger text-center mb-4">Аренда автомобиля</h3>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group mb-3">
                        <label>Марка автомобиля</label>
                        <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged">
                            <asp:ListItem Text="Выберите марку" Value=""></asp:ListItem>
                            <asp:ListItem Text="Ford" Value="Ford"></asp:ListItem>
                            <asp:ListItem Text="Chevrolet" Value="Chevrolet"></asp:ListItem>
                            <asp:ListItem Text="Lamborghini" Value="Lamborghini"></asp:ListItem>
                            <asp:ListItem Text="Jaguar" Value="Jaguar"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group mb-3">
                        <label>Модель</label>
                        <asp:Label ID="lblModel" runat="server" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="form-group mb-3">
                        <label>Цвет автомобиля</label>
                        <div id="colorContainer" class="color-container">
                            <div class="color-circle" style="background-color: red;" onclick="selectColor('red')"></div>
                            <div class="color-circle" style="background-color: blue;" onclick="selectColor('blue')"></div>
                            <div class="color-circle" style="background-color: green;" onclick="selectColor('green')"></div>
                            <div class="color-circle" style="background-color: black;" onclick="selectColor('black')"></div>
                            <div class="color-circle" style="background-color: white;" onclick="selectColor('white')"></div>
                            <div class="color-circle" style="background-color: yellow;" onclick="selectColor('yellow')"></div>
                            <div class="color-circle" style="background-color: orange;" onclick="selectColor('orange')"></div>
                        </div>
                        <asp:HiddenField ID="hfSelectedColor" runat="server" />
                    </div>
                    <div class="form-group mb-3">
                        <label>Дата начала аренды</label>
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date" />
                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control mt-2" TextMode="Time" />
                    </div>
                    <div class="form-group mb-3">
                        <label>Дата окончания аренды</label>
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date" />
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control mt-2" TextMode="Time" />
                    </div>
                    <asp:Button ID="btnRent" runat="server" Text="Арендовать" CssClass="btn btn-red w-100" OnClick="btnRent_Click" />
                </div>
                <div class="col-md-6 text-center">
                    <asp:Image ID="carImage" runat="server" CssClass="car-image" ImageUrl="~/assets/images/default-car.png" />
                    <div class="mt-3">
                        <h5 id="carDetails" runat="server" class="text-danger"></h5>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script>
        function selectColor(color) {
            var circles = document.querySelectorAll('.color-circle');
            circles.forEach(circle => circle.classList.remove('selected'));
            var selectedCircle = Array.from(circles).find(c => c.style.backgroundColor === color);
            if (selectedCircle) selectedCircle.classList.add('selected');
            document.getElementById('<%= hfSelectedColor.ClientID %>').value = color;
        }
    </script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
