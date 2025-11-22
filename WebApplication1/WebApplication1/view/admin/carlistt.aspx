<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="carlistt.aspx.cs" Inherits="WebApplication1.view.admin.carlistt" MasterPageFile="~/view/admin/usermaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="my-4 text-center" style="color: #00ffff;">Наш Автопарк</h2>
         <div class="custom-dropdown-wrapper">
        <asp:DropDownList ID="ddlSort" runat="server" CssClass="custom-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
            <asp:ListItem Text="По цене (по возрастанию)" Value="Price ASC" />
            <asp:ListItem Text="По цене (по убыванию)" Value="Price DESC" />
        </asp:DropDownList>
    </div>
        <div class="row">
        

            <asp:Repeater ID="rptCars" runat="server">
             <ItemTemplate>
    <div class="col-md-4 col-sm-6 col-12">
        <a href='<%# GetCarDetailUrl(Eval("Brand"), Eval("Model")) %>' class="car-card">
            <div class="image-wrapper">
                <img src='<%# Eval("ImageUrl") %>'
                     alt='<%# Eval("Brand") + " " + Eval("Model") %>'
                     onerror="this.onerror=null; this.src=\'<%= ResolveClientUrl("~/images/default_car.png") %>\';" />
            </div>
            <div class="car-info">
                <div class="car-name"><%# Eval("Brand") + " " + Eval("Model") %></div>
                <div class="car-price"><%# GetPrice(Eval("Price"), Eval("Status")) %></div>
            </div>
            <div class="car-status <%# GetStatusClass(Eval("Status")) %>">
                <%# GetStatusText(Eval("Status")) %>
            </div>
        </a>
    </div>
</ItemTemplate>



            </asp:Repeater>
        </div>
    </div>

</asp:Content>
