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
                        <asp:Label ID="ErrorMsg" runat="server" CssClass="text-danger" Visible="false"></asp:Label>

                        <div class="form-group mb-3">
                            <label>Select Car</label>
                            <asp:DropDownList ID="ddlCarPlate" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <asp:Button ID="Edit" runat="server" Text="Edit" CssClass="btn btn-danger" Visible="false" />
                        <asp:Button ID="Add" runat="server" Text="Add" CssClass="btn btn-danger" Visible="false" />
                        <asp:Button ID="Delete" runat="server" Text="Delete" CssClass="btn btn-danger" Visible="false" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <asp:GridView ID="gvCars" runat="server" CssClass="table table-striped"
                            AutoGenerateColumns="False"
                            DataKeyNames="RentId"
                            OnRowEditing="gvCars_RowEditing"
                            OnRowUpdating="gvCars_RowUpdating"
                            OnRowCancelingEdit="gvCars_RowCancelingEdit"
                            OnRowDeleting="gvCars_RowDeleting">

                            <Columns>
                                <asp:BoundField DataField="RentId" HeaderText="Rent ID" ReadOnly="True" />
                                <asp:BoundField DataField="Car" HeaderText="Car Plate Number" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer Name" ReadOnly="True" />
                                <asp:BoundField DataField="RentDate" HeaderText="Rent Date" />
                                <asp:BoundField DataField="ReturnDate" HeaderText="Return Date" />
                                <asp:BoundField DataField="Fees" HeaderText="Fees" />
                                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
