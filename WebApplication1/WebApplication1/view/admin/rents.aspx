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
            background-color: #ffffff;
            color: #000000;
        }

        .container {
            padding-top: 20px;
            padding-bottom: 20px;
        }

        h3.fw-bold {
            color: #000000;
            margin-bottom: 20px;
            font-weight: 700;
        }

        .card {
            border: 1px solid #dee2e6;
            border-radius: 15px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
            margin-bottom: 20px;
        }

        .card-header {
            background-color: #000000;
            color: #ffffff;
            border-bottom: 1px solid #00bcd4;
            font-weight: bold;
            border-radius: 15px 15px 0 0;
        }

        .card-body {
            background-color: #ffffff;
        }

        /* Styling for the GridView */
        .table thead th {
            background-color: #000000;
            color: #ffffff;
            border-color: #00bcd4;
            text-align: center;
        }

        .table tbody tr td {
            vertical-align: middle;
            text-align: center;
        }

        .table-striped tbody tr:nth-of-type(odd) {
            background-color: rgba(0, 188, 212, 0.05);
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

         /* Style for the car image */
        .img-fluid {
            max-width: 150px;
            height: auto;
            display: block;
            margin: 0 auto 20px auto;
        }

         /* Style for the action buttons, if they become visible */
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
