<%@ Page Title="Manage Cars" Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="customers.aspx.cs" Inherits="WebApplication1.view.admin.customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

     <style>
        .form-control:focus {
            border-color: #dc3545 !important;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25); 
        }

    </style>
   
    <div class="container">
        <div class="row">
            <div class="col-md-4">
                <div class="row mb-3">
                    <div class="col text-center">
                         <h3 class="text-danger">Manage Customers</h3>
                        <img src="" alt="Car Image" class="img-fluid"/>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <div class="form-group mb-3">
                            <label>Customer Name</label>
                            <asp:TextBox ID="txtLicence" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Customer Adress</label>
                            <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Customer Phone</label>
                            <asp:TextBox ID="txtModel" runat="server" CssClass="form-control"></asp:TextBox>
                        
                        <br />
                        <br />
                        <asp:Button ID="btnSubmit" runat="server" Text="Edit" CssClass="btn btn-danger"/>
                        <asp:Button ID="Button2" runat="server" Text="Add" CssClass="btn btn-danger"/>
                        <asp:Button ID="Button1" runat="server" Text="Delete" CssClass="btn btn-danger"/>
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <asp:GridView ID="gvCars" runat="server" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="LicenceNumber" HeaderText="Licence #" />
                                <asp:BoundField DataField="Brand" HeaderText="Brand" />
                                <asp:BoundField DataField="Model" HeaderText="Model" />
                                <asp:BoundField DataField="Price" HeaderText="Price" />
                                <asp:BoundField DataField="Color" HeaderText="Color" />
                                <asp:CheckBoxField DataField="Available" HeaderText="Available" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>