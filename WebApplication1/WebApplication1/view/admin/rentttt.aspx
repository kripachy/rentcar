<%@ Page Title="Мои Аренды" Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="rentttt.aspx.cs" Inherits="WebApplication1.view.admin.rentttt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h1 class="text-center text-danger mb-4">Мои Текущие Аренды</h1>

        <div class="row">
            <div class="col-12">
                <asp:GridView ID="GridViewRents" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered table-hover" 
                    EmptyDataText="У вас пока нет активных аренд." >
                    <Columns>
                        <asp:TemplateField HeaderText="Автомобиль">
                            <ItemTemplate>
                                <div class="d-flex align-items-center">
                                    <asp:Image ID="imgCar" runat="server" 
                                         ImageUrl='<%# GetCarImageUrl(Eval("Car")) %>' 
                                         AlternateText='<%# Eval("Car") %>' 
                                         CssClass="img-thumbnail" 
                                         Style="width: 100px; height: 60px; object-fit: cover; margin-right: 10px;" />
                                    <div>
                                        <asp:Label ID="lblCarPlate" runat="server" Text='<%# Eval("Car") %>' CssClass="fw-bold"></asp:Label>
                                        <br />
                                        <small class="text-muted"><%# GetCarDetails(Eval("Car")) %></small>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RentDate" HeaderText="Дата Начала" DataFormatString="{0:d}" SortExpression="RentDate" />
                        <asp:BoundField DataField="ReturnDate" HeaderText="Дата Окончания" DataFormatString="{0:d}" SortExpression="ReturnDate" />
                        <asp:BoundField DataField="Fees" HeaderText="Стоимость ($)" DataFormatString="{0:N2}" SortExpression="Fees" />
                        <asp:TemplateField HeaderText="Осталось">
                            <ItemTemplate>
                                <asp:Label ID="lblRemainingTime" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
