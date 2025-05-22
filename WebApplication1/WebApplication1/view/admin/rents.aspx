<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" 
AutoEventWireup="true" CodeBehind="rents.aspx.cs" Inherits="WebApplication1.view.admin.rents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Add some general styles for better appearance */
        body {
            min-height: 100vh;
            display: flex;
            flex-direction: column;
            margin: 0;
        }

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
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">Список Аренд</div>
                    <div class="card-body">
                        <asp:GridView ID="gvCars" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="False"
                            DataKeyNames="RentId"
                            OnRowCancelingEdit="gvCars_RowCancelingEdit"
                            OnRowDeleting="gvCars_RowDeleting">

                            <Columns>
                                <asp:BoundField DataField="RentId" HeaderText="ID Аренды" ReadOnly="True" />
                                <asp:BoundField DataField="Car" HeaderText="Номер Машины" />
                                <asp:BoundField DataField="Customer" HeaderText="Клиент" ReadOnly="True" />
                                <asp:BoundField DataField="RentDate" HeaderText="Дата Начала" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="ReturnDate" HeaderText="Дата Окончания" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="Fees" HeaderText="Стоимость ($)" DataFormatString="{0:N2}" />
                                <asp:TemplateField HeaderText="Действия">
                                    <ItemTemplate>
                                        <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Удалить" 
                                            CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Вы уверены, что хотите удалить эту аренду?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
