<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="carlistt.aspx.cs" Inherits="WebApplication1.view.admin.carlistt" MasterPageFile="~/view/admin/usermaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<style>
    .car-card {
        border: 1px solid #e0e0e0;
        border-radius: 8px;
        padding: 15px;
        margin-bottom: 40px;
        text-align: center;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        background-color: #fff;
        height: 100%;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        transition: transform 0.2s ease, box-shadow 0.2s ease;
        cursor: pointer;
        text-decoration: none;
        color: inherit;
    }

    .car-card:hover {
        transform: translateY(-5px);
        box-shadow: 0 4px 8px rgba(0,0,0,0.15);
        text-decoration: none;
        color: inherit;
    }

    .image-wrapper {
        width: 250px;
        height: 180px;
        margin: 0 auto 10px auto;
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: #f9f9f9;
        overflow: hidden;
        border-radius: 8px;
    }

    .car-card img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        display: block;
        border-radius: 6px;
        transition: transform 0.3s ease;
    }

    .car-card img:hover {
        transform: scale(1.05);
    }

    .car-info {
        flex-grow: 1;
        display: flex;
        flex-direction: column;
        justify-content: center;
        padding-top: 10px;
    }

    .car-name {
        font-size: 1.1em;
        font-weight: bold;
        margin-bottom: 5px;
    }

    .car-price {
        font-size: 1.2em;
        color: #dc3545;
        margin-bottom: 10px;
    }

    .car-status {
        font-size: 1em;
        color: gray;
        min-height: 1.2em;
        margin-top: auto;
    }

    .car-status.unavailable {
        color: #dc3545;
        font-weight: bold;
    }
</style>
<style>
    .custom-dropdown-wrapper {
        display: flex;
        justify-content: center;
        margin-bottom: 30px;
        margin-top: 20px;
    }

    .custom-dropdown {
        padding: 10px 14px;
        font-size: 1rem;
        border-radius: 8px;
        border: 1px solid #ced4da;
        background-color: #fff;
        color: #495057;
        appearance: none;
        -webkit-appearance: none;
        -moz-appearance: none;
        background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='16' height='16' fill='gray' class='bi bi-caret-down-fill' viewBox='0 0 16 16'%3E%3Cpath d='M7.247 11.14l-4.796-5.481C2.108 5.345 2.522 4.5 3.25 4.5h9.5c.728 0 1.142.845.799 1.159l-4.796 5.48a1 1 0 0 1-1.506 0z'/%3E%3C/svg%3E");
        background-repeat: no-repeat;
        background-position: right 12px center;
        background-size: 16px 16px;
        min-width: 260px;
        box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        transition: border-color 0.3s ease;
    }

    .custom-dropdown:focus {
        border-color: #007bff;
        outline: none;
    }
</style>



    <div class="container">
        <h2 class="my-4 text-center">Наш Автопарк</h2>
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
