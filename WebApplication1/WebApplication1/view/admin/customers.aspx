<%@ Page Title="Управление клиентами" Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="customers.aspx.cs" Inherits="WebApplication1.view.admin.customers" %>

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

        .export-btn {
            margin-top: 20px;
            margin-bottom: 30px;
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
                        <h3 class="fw-bold text-center" style="color: #000000;">Управление клиентами</h3>

                        <img src="../../assets/images/images.png" alt="CustomersImage" class="img-fluid"/>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <div class="form-group mb-3">
                            <label>Имя клиента</label>
                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Адрес клиента</label>
                            <asp:TextBox ID="txtCustomerAdress" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Телефон клиента</label>
                            <asp:TextBox ID="txtCustomerPhone" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Пароль клиента</label>
                            <asp:TextBox ID="txtCustomerPassword" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <br />
                        <br />
                        <asp:Button ID="Edit" runat="server" Text="Редактировать" CssClass="btn btn-danger" OnClick="Edit_Click"/>
                        <asp:Button ID="Delete" runat="server" Text="Удалить" CssClass="btn btn-danger" OnClick="Delete_Click" OnClientClick="return confirm('Вы уверены, что хотите удалить этого клиента?');"/>
                        <asp:Label ID="ErrorMsg" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <asp:GridView ID="gvCustomers" runat="server" CssClass="table table-striped" 
                            AutoGenerateColumns="False" OnSelectedIndexChanged="gvCustomers_SelectedIndexChanged" 
                            DataKeyNames="CustId">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-sm btn-outline-danger" SelectText="Выбрать" />
                                <asp:BoundField DataField="CustId" HeaderText="ID"/>
                                <asp:BoundField DataField="CustName" HeaderText="Имя" />
                                <asp:BoundField DataField="CustAdd" HeaderText="Адрес" />
                                <asp:BoundField DataField="CustPhone" HeaderText="Телефон" />
                                <asp:BoundField DataField="CustPassword" HeaderText="Пароль" />
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnExport" runat="server" Text="Экспорт в Excel" OnClick="btnExport_Click" CssClass="btn btn-success export-btn" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
