<%@ Page Title="Manage Cars" Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="cars.aspx.cs" Inherits="WebApplication1.view.admin.cars" %>

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
                         <h3 class="text-danger">Manage Cars</h3>
                        <img src="../../assets/images/Слой 1.png" alt="Car Image" class="img-fluid"/>
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
                                <asp:ListItem Text="Available" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Booked" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <br />
                        <br />
                        <label id="ErrorMsg" runat="server"></label>
                        <asp:Button ID="Edit" runat="server" Text="Edit" CssClass="btn btn-danger"/>
                        <asp:Button ID="Save" runat="server" Text="Save" CssClass="btn btn-danger" OnClick="Save_Click"/>
                        <asp:Button ID="Delete" runat="server" Text="Delete" CssClass="btn btn-danger"/>
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <asp:GridView ID="carlist" runat="server" CssClass="table table-bordered table-hover text-center" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="CPlateNum" HeaderText="Licence #" />
        <asp:BoundField DataField="Brand" HeaderText="Brand" />
        <asp:BoundField DataField="Model" HeaderText="Model" />
        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C0}" />
        <asp:BoundField DataField="Color" HeaderText="Color" />
      <asp:TemplateField HeaderText="Status">
    <ItemTemplate>
        <asp:Label ID="lblStatus" runat="server" 
            Text='<%# Eval("Status").ToString() == "Available" ? "Available" : "Booked" %>' 
            CssClass='<%# Eval("Status").ToString() == "Available" ? "badge bg-success" : "badge bg-danger" %>'>
        </asp:Label>
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

