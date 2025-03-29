<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" 
AutoEventWireup="true" CodeBehind="rents.aspx.cs" Inherits="WebApplication1.view.admin.rents" %>



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
                         <h3 class="text-danger">Rented Cars</h3>
                        <img src="../../assets/images/car-rental.png" alt="CustomersImage" class="img-fluid"/>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <div class="form-group mb-3">
                            <label>Customer Name</label>
                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Customer Adress</label>
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
                        <asp:Button ID="Edit" runat="server" Text="Edit" CssClass="btn btn-danger"/>
                        <asp:Button ID="Add" runat="server" Text="Add" CssClass="btn btn-danger"/>
                        <asp:Button ID="Delete" runat="server" Text="Delete" CssClass="btn btn-danger"/>
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
