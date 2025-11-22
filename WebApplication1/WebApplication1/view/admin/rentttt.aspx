<%@ Page Title="Мои Аренды" Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="rentttt.aspx.cs" Inherits="WebApplication1.view.admin.rentttt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .table {
            background-color: #212529;
            color: #e0e0e0;
        }
        .table th {
            color: #00ffff;
        }
        .table-bordered {
            border: 1px solid #00ffff;
        }
        .table-bordered th, .table-bordered td {
            border: 1px solid #00ffff;
        }
        .table-hover tbody tr:hover {
            background-color: #2c3e50;
            color: #fff;
        }
        .btn-danger {
            background-color: #00ffff;
            border-color: #00ffff;
            color: #000;
        }
        .btn-danger:hover {
            background-color: #00e6e6;
            border-color: #00e6e6;
        }
        .img-thumbnail {
            background-color: #2c3e50;
            border: 1px solid #00ffff;
        }
    </style>
    <div class="container mt-4">
        <h1 class="text-center mb-4" style="color: #00ffff;">Мои Текущие Аренды</h1>

        <div class="row">
            <div class="col-12">
                <asp:GridView ID="GridViewRents" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered table-hover" 
                    EmptyDataText="У вас пока нет активных аренд."
                    OnRowCommand="GridViewRents_RowCommand"
                    OnRowDataBound="GridViewRents_RowDataBound">
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
                        <asp:TemplateField HeaderText="Действия">
                            <ItemTemplate>
                                <asp:Button ID="btnCancelRent" runat="server" 
                                    Text="Отменить аренду" 
                                    CssClass="btn btn-danger btn-sm" 
                                    CommandName="CancelRent" 
                                    CommandArgument='<%# Eval("RentId") %>'
                                    OnClientClick='<%# "return confirm(\"Вы уверены, что хотите отменить аренду?\");" %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-center mt-3" Visible="false"></asp:Label>
</asp:Content>
