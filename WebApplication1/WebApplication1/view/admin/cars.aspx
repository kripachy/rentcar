<%@ Page Title="Manage Cars" Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="cars.aspx.cs" Inherits="WebApplication1.view.admin.cars" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Manage Cars</h1>
    <div class="container">
        <div class="row">
            <div class="col-md-4">
                <div class="row mb-3">
                    <div class="col text-center">
                        <img src="https://via.placeholder.com/200" alt="Car Image" class="img-fluid"/>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <div class="form-group mb-3">
                            <label>Licence Number</label>
                            <asp:TextBox ID="txtLicence" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Brand</label>
                            <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Model</label>
                            <asp:TextBox ID="txtModel" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Price</label>
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Color</label>
                            <asp:TextBox ID="txtColor" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group mb-3">
                            <label>Available</label>
                            <asp:DropDownList ID="ddlAvailable" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary"/>
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