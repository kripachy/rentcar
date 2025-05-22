<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" 
AutoEventWireup="true" CodeBehind="rents.aspx.cs" Inherits="WebApplication1.view.admin.rents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Add some general styles for better appearance */
        .container {
            padding-top: 20px;
            padding-bottom: 20px;
        }

        h3.fw-bold {
            color: #dc3545; /* Danger color for headers */
            margin-bottom: 20px;
        }

        .card {
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            margin-bottom: 20px; /* Add margin below cards */
        }

        .card-header {
            background-color: #f8f9fa; /* Light background for header */
            border-bottom: 1px solid #e0e0e0;
            font-weight: bold;
        }

        /* Styling for the GridView */
        .table thead th {
            background-color: #dc3545;
            color: white;
            border-color: #dc3545;
            text-align: center;
        }

        .table tbody tr td {
            vertical-align: middle;
            text-align: center;
        }

        .table-striped tbody tr:nth-of-type(odd) {
            background-color: rgba(0, 0, 0, 0.05);
        }

         .form-control:focus {
            border-color: #dc3545 !important;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
        }

         /* Style for the car image */
        .img-fluid {
            max-width: 150px; /* Adjust as needed */
            height: auto;
            display: block;
            margin: 0 auto 20px auto; /* Center and add space below */
        }

         /* Style for the action buttons, if they become visible */
        .btn-danger {
            background-color: #dc3545;
            border-color: #dc3545;
        }

        .btn-danger:hover {
            background-color: #bb2d3b;
            border-color: #bb2d3b;
        }

    </style>

    <div class="container">
        <h3 class="fw-bold text-center">Управление Арендами</h3>
        <div class="row">
            <div class="col-md-4">
                <div class="card">
                    <div class="card-header">Действия</div>
                    <div class="card-body">
                         <img src="../../assets/images/car-rental.png" alt="Rents Image" class="img-fluid"/>
                        <asp:Label ID="ErrorMsg" runat="server" CssClass="text-danger" Visible="false"></asp:Label>

                        <div class="form-group mb-3">
                            <label>Выбрать Автомобиль</label>
                            <asp:DropDownList ID="ddlCarPlate" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <%-- These buttons are currently not visible based on the original code --%>
                        <asp:Button ID="Edit" runat="server" Text="Редактировать" CssClass="btn btn-danger w-100 mb-2" Visible="false" />
                        <asp:Button ID="Add" runat="server" Text="Добавить" CssClass="btn btn-danger w-100 mb-2" Visible="false" />
                        <asp:Button ID="Delete" runat="server" Text="Удалить" CssClass="btn btn-danger w-100" Visible="false" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="card">
                    <div class="card-header">Список Аренд</div>
                    <div class="card-body">
                        <asp:GridView ID="gvCars" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="False"
                            DataKeyNames="RentId"
                            OnRowEditing="gvCars_RowEditing"
                            OnRowUpdating="gvCars_RowUpdating"
                            OnRowCancelingEdit="gvCars_RowCancelingEdit"
                            OnRowDeleting="gvCars_RowDeleting">

                            <Columns>
                                <asp:BoundField DataField="RentId" HeaderText="ID Аренды" ReadOnly="True" />
                                <asp:BoundField DataField="Car" HeaderText="Номер Машины" />
                                <asp:BoundField DataField="Customer" HeaderText="Клиент" ReadOnly="True" />
                                <asp:BoundField DataField="RentDate" HeaderText="Дата Начала" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="ReturnDate" HeaderText="Дата Окончания" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="Fees" HeaderText="Стоимость ($)" DataFormatString="{0:N2}" />
                                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" HeaderText="Действия" ButtonType="Button" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
