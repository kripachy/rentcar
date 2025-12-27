<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="carlistt.aspx.cs"
    Inherits="WebApplication1.view.admin.carlistt" MasterPageFile="~/view/admin/usermaster.master" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="car-list-container">
            <div class="container">
                <div class="car-list-header">
                    <h1 class="car-list-title">Наш Автопарк</h1>
                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>

                <div class="custom-dropdown-wrapper">
                    <asp:DropDownList ID="ddlSort" runat="server" CssClass="custom-dropdown" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                        <asp:ListItem Text="По цене (по возрастанию)" Value="Price ASC" />
                        <asp:ListItem Text="По цене (по убыванию)" Value="Price DESC" />
                    </asp:DropDownList>
                </div>

                <div class="row">
                    <asp:Repeater ID="rptCars" runat="server">
                        <ItemTemplate>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-12 mb-4">
                                <a href='<%# GetCarDetailUrl(Eval("Plate")) %>' class="car-card">
                                    <div class="image-wrapper">
                                        <img src='<%# Eval("ImageUrl") %>'
                                            alt='<%# Eval("Brand") + " " + Eval("Model") %>'
                                            onerror='<%# GetOnErrorScript() %>' />
                                    </div>
                                    <div class="car-info">
                                        <div class="car-name">
                                            <%# Eval("Brand") + " " + Eval("Model") %>
                                        </div>
                                        <div class="car-price">
                                            <%# GetPrice(Eval("Price"), Eval("Status")) %>
                                        </div>
                                    </div>
                                    <div class="car-status <%# GetStatusClass(Eval(" Status")) %>">
                                        <%# GetStatusText(Eval("Status")) %>
                                    </div>
                                </a>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </asp:Content>