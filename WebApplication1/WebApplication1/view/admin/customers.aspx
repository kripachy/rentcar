<%@ Page Title="Manage Customers" Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="customers.aspx.cs" Inherits="WebApplication1.view.admin.customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-control:focus {
            border-color: #dc3545 !important;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
        }
        
        .export-btn {
            margin-top: 20px;
            margin-bottom: 30px;
        }
    </style>
   
    <div class="container">
        <div class="row">
            <div class="col-md-4">
                <div class="row mb-3">
                    <div class="col text-center">
                        <h3 class="text-danger">Manage Customers</h3>
                        <img src="../../assets/images/images.png" alt="CustomersImage" class="img-fluid"/>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <div class="form-group mb-3">
                            <label>Customer Name</label>
                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Customer Address</label>
                            <asp:TextBox ID="txtCustomerAdress" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Customer Phone</label>
                            <asp:TextBox ID="txtCustomerPhone" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Customer Password</label>
                            <asp:TextBox ID="txtCustomerPassword" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <br />
                        <br />
                        <asp:Button ID="Edit" runat="server" Text="Edit" CssClass="btn btn-danger" OnClick="Edit_Click"/>
                        <asp:Button ID="Add" runat="server" Text="Add" CssClass="btn btn-danger" OnClick="Add_Click"/>
                        <asp:Button ID="Delete" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="Delete_Click" OnClientClick="return confirm('Are you sure you want to delete this customer?');"/>
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
                                <asp:CommandField ShowSelectButton="True" 
                                    ButtonType="Button" 
                                    ControlStyle-CssClass="btn btn-sm btn-outline-danger" 
                                    SelectText="Select" />
                                <asp:BoundField DataField="CustId" HeaderText="ID"/>
                                <asp:BoundField DataField="CustName" HeaderText="Name" />
                                <asp:BoundField DataField="CustAdd" HeaderText="Address" />
                                <asp:BoundField DataField="CustPhone" HeaderText="Phone" />
                                 <asp:BoundField DataField="CustPassword" HeaderText="Password" />

                            </Columns>
                        </asp:GridView>

                        <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Click" CssClass="btn btn-success export-btn" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>