<%@ Page Title="Управление автомобилями" Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="cars.aspx.cs" Inherits="WebApplication1.view.admin.cars" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-control:focus {
            border-color: #dc3545 !important;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
        }

        .btn-outline-danger {
            border-color: #dc3545;
            color: #dc3545;
            transition: all 0.3s ease;
        }

        #carImage {
    width: 200px;
    height: 200px;
    object-fit: contain; 
}

        .btn-outline-danger:hover {
            background-color: #dc3545;
            color: white;
        }

        .table th {
            background-color: #f8f9fa;
            font-weight: 600;
        }

        .table-striped tbody tr:nth-of-type(odd) {
            background-color: rgba(220, 53, 69, 0.05);
        }
        #ErrorMsg {
    transition: all 0.3s ease;
}

    </style>

    <div class="container">
        <div class="row">
            <div class="col-md-4">
                <div class="row mb-3">
                    <div class="col text-center">
                        <h3 class="text-danger fw-bold text-center">Управление автомобилями</h3>
                        <img id="carImage" runat="server" src="../../assets/images/Слой 1.png" alt="Car Image" class="img-fluid" style="width: 200px; height: 150px; object-fit: contain;" />

                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <div class="form-group mb-3">
                            <label>Номер лицензии</label>
                            <asp:TextBox ID="txtLicence" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <div class="form-group mb-3">
    <label>Марка</label>
    <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" />
</div>

<div class="form-group mb-3">
    <label>Модель</label>
    <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-control" />
</div>

                        </div>
                        <div class="form-group mb-3">
                            <label>Цена</label>
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                       <div class="form-group mb-3">
    <label>Цвет</label>
    <asp:DropDownList ID="ddlColor" runat="server" CssClass="form-control">
        <asp:ListItem Text="Выберите цвет" Value=""></asp:ListItem>
        <asp:ListItem Text="Красный" Value="red"></asp:ListItem>
        <asp:ListItem Text="Жёлтый" Value="yellow"></asp:ListItem>
        <asp:ListItem Text="Чёрный" Value="black"></asp:ListItem>
        <asp:ListItem Text="Белый" Value="white"></asp:ListItem>
        <asp:ListItem Text="Синий" Value="blue"></asp:ListItem>
        <asp:ListItem Text="Оранжевый" Value="orange"></asp:ListItem>
    </asp:DropDownList>
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
    <label id="ErrorMsg" runat="server" class="text-danger d-block" style="min-height: 24px;"></label>
</div>

                        <asp:Button ID="Edit" runat="server" Text="Редактировать" CssClass="btn btn-danger" OnClick="Edit_Click"/>
                        <asp:Button ID="Save" runat="server" Text="Сохранить" CssClass="btn btn-danger" OnClick="Save_Click"/>
                        <asp:Button ID="Delete" runat="server" Text="Удалить" CssClass="btn btn-danger" OnClientClick="return confirm('Вы уверены, что хотите удалить этот автомобиль?');" OnClick="Delete_Click"/>
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <asp:GridView ID="carlist" runat="server" CssClass="table table-bordered table-striped"
    AutoGenerateColumns="False" OnSelectedIndexChanged="carlist_SelectedIndexChanged"
    DataKeyNames="CPlateNum,Price">
    <Columns>
        <asp:CommandField ShowSelectButton="True" ButtonType="Button" 
            ControlStyle-CssClass="btn btn-sm btn-outline-danger" SelectText="Выбрать" />
        <asp:BoundField DataField="CPlateNum" HeaderText="Номер лицензии" />
        <asp:BoundField DataField="Brand" HeaderText="Марка" />
        <asp:BoundField DataField="Model" HeaderText="Модель" />
        <asp:BoundField DataField="Price" HeaderText="Цена" DataFormatString="{0:$#,0}" />
        <asp:TemplateField HeaderText="Цвет">
    <ItemTemplate>
        <div style='width:20px; height:20px; border-radius:50%; background-color:<%# Eval("Color") %>; border:1px solid #ccc;' title='<%# Eval("Color") %>'></div>
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

                        <asp:Button ID="btnExport" runat="server" Text="Экспорт в Excel" CssClass="btn btn-success export-btn" OnClick="btnExport_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        document.getElementById('<%= ddlBrand.ClientID %>').addEventListener('change', function () {
            var brand = this.value;
            var imageMap = {
                'Aston Martin': '../../assets/images/logo/Aston-Martin-Logo.jpg',
                'Ford': '../../assets/images/logo/mustang.png',
                'Chevrolet': '../../assets/images/logo/chevrolet.jpg',
                'Lamborghini': '../../assets/images/logo/Lanborghini.jpg',
                'Jaguar': '../../assets/images/logo/jaguar.jpg',
                'Porsche': '../../assets/images/logo/porsche.png',
                'Maserati': '../../assets/images/logo/MASERATI.jpg',
                'Audi': '../../assets/images/logo/audi.jpg'
            };

            var carImage = document.getElementById('carImage');
            carImage.src = imageMap[brand] || '../../assets/images/Слой 1.png';
        });
    </script>
</asp:Content>
